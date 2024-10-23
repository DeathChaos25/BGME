using BGME.Framework.Music;
using BGME.Framework.P5R.Rhythm;
using p5rpc.lib.interfaces;

namespace BGME.Framework.P5R;

internal class BgmeService : IBgmeService
{
    private readonly IP5RLib p5rLib;
    private readonly BgmService bgm;
    private readonly EncounterBgm encounterBgm;

    private readonly RhythmGame? rhythmGame;

    public BgmeService(IP5RLib p5rLib, MusicService music)
    {
        this.p5rLib = p5rLib;
        this.bgm = new(music);
        this.encounterBgm = new(music);
    }

    public void SetVictoryDisabled(bool isDisabled)
    {
        Log.Debug($"Disable Victory BGM: {isDisabled}");
        this.bgm.SetVictoryDisabled(isDisabled);
        this.encounterBgm.SetVictoryDisabled(isDisabled);
    }
}
