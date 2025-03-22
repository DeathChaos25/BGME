using System.Runtime.InteropServices;

namespace BGME.Framework.P3R.SMT5V.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x20)]
public unsafe struct FBGMRequest
{
    [FieldOffset(0x0000)] public bool bPlayRequest;
    [FieldOffset(0x0001)] public bool bStopRequest;
    [FieldOffset(0x0008)] public USoundAtomCue* Cue;
    [FieldOffset(0x0010)] public EFadeType FadeType;
    [FieldOffset(0x0014)] public float FadeTime;
    [FieldOffset(0x0018)] public float StartPosition;
    [FieldOffset(0x001C)] public bool bForceReplay;
}
