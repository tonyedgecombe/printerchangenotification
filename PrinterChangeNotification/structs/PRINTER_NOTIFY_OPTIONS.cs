using System;
using System.Runtime.InteropServices;

// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification.structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PRINTER_NOTIFY_OPTIONS
    {
        public UInt32 Version;
        public UInt32 Flags;
        public UInt32 Count;

        public IntPtr pTypes;   // *PRINTER_NOTIFY_OPTIONS_TYPE
    }
}