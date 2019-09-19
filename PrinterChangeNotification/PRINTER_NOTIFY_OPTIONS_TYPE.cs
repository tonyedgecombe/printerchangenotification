using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PRINTER_NOTIFY_OPTIONS_TYPE
    {
        public UInt16 Type;
        public UInt16 Reserved0;
        public UInt32 Reserved1;
        public UInt32 Reserved2;
        public UInt32 Count;
        public IntPtr pFields; // *DWORD
    }
}