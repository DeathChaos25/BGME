using Ryo.Definitions.Structs;
using System.Runtime.InteropServices;

namespace BGME.Framework.P3R.SMT5V.Types;

[StructLayout(LayoutKind.Explicit, Size = 0xE0)]
public unsafe struct USoundAtomCue
{
    [FieldOffset(0x0038)] public USoundAtomCueSheet* CueSheet;
    [FieldOffset(0x0040)] public FString CueName;
}