using BGME.Framework.Music;
using Reloaded.Hooks.Definitions.X64;
using Timer = System.Timers.Timer;

namespace BGME.Framework.P5R;

internal unsafe class BgmService : BaseBgm
{
    [Function(CallingConventions.Microsoft)]
    private delegate void PlayBgmFunction(int cueId);
    private readonly SHFunction<PlayBgmFunction> playBgm;

    private readonly Timer holdupBgmBuffer = new(TimeSpan.FromMilliseconds(1000)) { AutoReset = false };
    private bool holdupBgmQueued;

    public BgmService(MusicService music)
        : base(music)
    {
        this.holdupBgmBuffer.Elapsed += (sender, args) =>
        {
            this.PlayBgm(341);
            this.holdupBgmQueued = false;
        };
        
        playBgm = new(PlayBgm, "40 53 48 83 EC 30 89 CB");
    }
    
    protected override int VictoryBgmId { get; } = 340;

    protected override void PlayBgm(int cueId)
    {
        var currentBgmId = this.GetGlobalBgmId(cueId);
        if (currentBgmId == null)
        {
            return;
        }

        // Buffer playing hold up music so it doesn't
        // interrupt battle BGM if quick AOA.
        if (cueId == 341 && this.holdupBgmQueued == false)
        {
            this.holdupBgmBuffer.Start();
            this.holdupBgmQueued = true;
            return;
        }
        else
        {
            this.holdupBgmBuffer.Stop();
            this.holdupBgmQueued = false;

            Log.Debug($"Playing BGM ID: {currentBgmId}");
            this.playBgm.OriginalFunction((int)currentBgmId);
        }
    }
}
