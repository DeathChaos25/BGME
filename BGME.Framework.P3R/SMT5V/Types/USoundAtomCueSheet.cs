using System.Runtime.InteropServices;

namespace BGME.Framework.P3R.SMT5V.Types;

[StructLayout(LayoutKind.Explicit, Size = 0xF0)]
public unsafe struct USoundAtomCueSheet
{
    [FieldOffset(0x00B8)] public nint acbHn;
}