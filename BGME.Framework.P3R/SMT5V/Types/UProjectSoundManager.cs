using System.Runtime.InteropServices;

namespace BGME.Framework.P3R.SMT5V.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x4D0)]
public unsafe struct UProjectSoundManager
{
    [FieldOffset(0x0250)] public FBGMRequest BGMRequest;
}
