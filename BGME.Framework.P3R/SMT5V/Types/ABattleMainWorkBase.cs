using System.Runtime.InteropServices;

namespace BGME.Framework.P3R.SMT5V.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x6C0)]
public unsafe struct ABattleMainWorkBase
{
    [FieldOffset(0x03B0)] public FBtlDescData m_DescData;
}