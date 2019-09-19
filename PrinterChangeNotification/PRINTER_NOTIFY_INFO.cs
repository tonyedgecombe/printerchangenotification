using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PRINTER_NOTIFY_INFO
    {
        public readonly UInt32 Version;
        public readonly UInt32 Flags;
        public readonly UInt32 Count;
        private readonly IntPtr aData;
    }
}