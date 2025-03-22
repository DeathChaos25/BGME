using BGME.Framework.Music;
using BGME.Framework.P3R.Configuration;
using Ryo.Interfaces;

namespace BGME.Framework.P3R.SMT5V;

internal class BgmeService : IBgmeService
{
    private readonly BgmService _bgm;
    private readonly EncounterBgm _encounterBgm;

    public BgmeService(ICriAtomEx criAtomEx, MusicService music)
    {
        _encounterBgm = new EncounterBgm(music);
        _bgm = new BgmService(criAtomEx, music, _encounterBgm);
    }

    public void SetVictoryDisabled(bool isDisabled)
    {
        _bgm.SetVictoryDisabled(isDisabled);
        _encounterBgm.SetVictoryDisabled(isDisabled);
    }

    public void SetConfig(Config config)
    {
        _bgm.SetVolumeFix(config.SMT5V_UseVolumeFix);
    }
}
