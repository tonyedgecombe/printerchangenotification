using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PRINTER_NOTIFY_OPTIONS
    {
        public UInt32 Version;
        public UInt32 Flags;
        public UInt32 Count;

        public IntPtr pTypes;   // *PRINTER_NOTIFY_OPTIONS_TYPE
    }
}