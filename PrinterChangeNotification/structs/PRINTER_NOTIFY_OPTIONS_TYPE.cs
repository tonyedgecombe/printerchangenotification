using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace PrinterChangeNotification.structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PRINTER_NOTIFY_OPTIONS_TYPE
    {
        private Int32 Type;
        private Int32 Reserved0;
        private UInt32 Reserved1;
        private UInt32 Reserved2;
        private UInt32 Count;
        private IntPtr pFields; // *DWORD
    }
}