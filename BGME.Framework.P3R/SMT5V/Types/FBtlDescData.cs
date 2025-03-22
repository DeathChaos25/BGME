using System.Runtime.InteropServices;

namespace BGME.Framework.P3R.SMT5V.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x90)]
public unsafe struct FBtlDescData
{
    [FieldOffset(0x0000)] public int m_EvtID;
    [FieldOffset(0x0004)] public E_BTL_SYMBOL_ENCOUNT m_SymbolEncountType;
    [FieldOffset(0x0018)] public int m_BGMNo;
    [FieldOffset(0x001C)] public int m_EncID;
}
