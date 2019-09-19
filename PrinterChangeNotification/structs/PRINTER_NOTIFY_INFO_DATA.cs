using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification.structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct PRINTER_NOTIFY_INFO_DATA
    {
        [FieldOffset(0)]
        public readonly UInt16 Type;

        [FieldOffset(2)]
        public readonly UInt16 Field;

        [FieldOffset(4)]
        private readonly UInt32 Reserved;

        [FieldOffset(8)]
        public readonly UInt32 Id;

        [FieldOffset(12)]
        public readonly UInt32 adwData0;

        [FieldOffset(16)]
        private readonly UInt32 adwData1;

        [FieldOffset(12)]
        private readonly UInt32 cbBuf;

        [FieldOffset(16)] public readonly IntPtr pBuf;
    }
}