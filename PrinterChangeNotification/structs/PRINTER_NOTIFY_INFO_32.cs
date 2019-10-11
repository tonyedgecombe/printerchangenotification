using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification.structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PRINTER_NOTIFY_INFO_32
    {
        public readonly UInt32 Version;
        public readonly UInt32 Flags;
        public readonly UInt32 Count;
        private readonly PRINTER_NOTIFY_INFO_DATA_32 aData;
    }
}