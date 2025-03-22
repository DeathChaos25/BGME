using BGME.Framework.Interfaces;
using BGME.Framework.Music;
using BGME.Framework.P3R.Configuration;
using BGME.Framework.P3R.Template;
using PersonaMusicScript.Types;
using PersonaMusicScript.Types.Games;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Ryo.Interfaces;
using System.Diagnostics;
using System.Drawing;
using Unreal.ObjectsEmitter.Interfaces;

namespace BGME.Framework.P3R;

public class Mod : ModBase
{
    private readonly IModLoader modLoader;
    private readonly IReloadedHooks hooks;
    private readonly ILogger log;
    private readonly IMod owner;

    private Config config;
    private readonly IModConfig modConfig;

    private readonly IRyoApi ryo;
    private readonly IBgmeApi bgmeApi;
    private readonly IBgmeService bgme;
    private bool foundDisableVictoryMod;

    public Mod(ModContext context)
    {
        this.modLoader = context.ModLoader;
        this.hooks = context.Hooks!;
        this.log = context.Logger;
        this.owner = context.Owner;
        this.config = context.Configuration;
        this.modConfig = context.ModConfig;

#if DEBUG
        Debugger.Launch();
#endif

        Project.Initialize(this.modConfig, this.modLoader, this.log, Color.LightBlue);
        Log.LogLevel = this.config.LogLevel;

        this.modLoader.GetController<IBgmeApi>().TryGetTarget(out this.bgmeApi!);
        this.modLoader.GetController<IRyoApi>().TryGetTarget(out this.ryo!);
        this.modLoader.GetController<ICriAtomEx>().TryGetTarget(out var criAtomEx);
        this.modLoader.GetController<IUnreal>().TryGetTarget(out var unreal);
        this.modLoader.GetController<IStartupScanner>().TryGetTarget(out var scanner);
        this.modLoader.GetController<IDataTables>().TryGetTarget(out var dt);

        var appId = this.modLoader.GetAppConfig().AppId;
        var modDir = this.modLoader.GetDirectoryForModId(this.modConfig.ModId);
        var game = GetGame(appId);

        var musicResources = new MusicResources(game, modDir);
        var music = new MusicService(musicResources, this.bgmeApi, null, false);

        // Register music from BGME mods.
        this.bgmeApi!.BgmeModLoading += this.OnBgmeModLoading;
        foreach (var mod in this.bgmeApi.GetLoadedMods())
        {
            this.OnBgmeModLoading(mod);
        }

        switch (game)
        {
            case Game.P3R_PC:
                this.bgme = new P3R.BgmeService(criAtomEx!, music);
                break;
            case Game.SMT5V:
                var smt5v = new SMT5V.BgmeService(criAtomEx!, music);
                this.bgme = smt5v;

                smt5v.SetConfig(this.config);
                break;
            default:
                throw new Exception($"Missing BGME service for game {game}.");
        }

        this.ApplyConfig();
    }

    private void OnBgmeModLoading(BgmeMod mod)
    {
        var bgmeMusicDirP3R = Path.Join(mod.ModDir, "bgme", "p3r");
        if (Directory.Exists(bgmeMusicDirP3R))
        {
            this.ryo.AddAudioPath(bgmeMusicDirP3R, new() { CategoryIds = [0, 13] });
        }

        var bgmeMusicDirSMT5 = Path.Join(mod.ModDir, "bgme", "smt5v");
        if (Directory.Exists(bgmeMusicDirSMT5))
        {
            this.ryo.AddAudioPath(bgmeMusicDirSMT5, new() { AcbName = "BGM", CategoryIds = [0, 4, 9, 40, 11, 35, 50] });
        }

        if (mod.ModId == "BGME.DisableVictoryTheme")
        {
            this.foundDisableVictoryMod = true;
            this.bgme.SetVictoryDisabled(true);
        }
    }

    private void ApplyConfig()
    {
        Log.LogLevel = this.config.LogLevel;
        if (this.config.DisableVictoryBgm || this.foundDisableVictoryMod)
        {
            this.bgme.SetVictoryDisabled(true);
        }
        else
        {
            this.bgme.SetVictoryDisabled(false);
        }
    }

    private static Game GetGame(string appId)
    {
        if (appId.Contains("p3r", StringComparison.OrdinalIgnoreCase)) return Game.P3R_PC;
        if (appId.Contains("smt5v", StringComparison.OrdinalIgnoreCase)) return Game.SMT5V;

        throw new Exception($"Unknown app: {appId}");
    }

    #region Standard Overrides
    public override void ConfigurationUpdated(Config configuration)
    {
        // Apply settings from configuration.
        // ... your code here.
        config = configuration;
        log.WriteLine($"[{modConfig.ModId}] Config Updated: Applying");
        this.ApplyConfig();
    }
    #endregion

    #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Mod() { }
#pragma warning restore CS8618
    #endregion
}