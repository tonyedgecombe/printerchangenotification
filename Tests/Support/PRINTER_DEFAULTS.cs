using System;
using System.Runtime.InteropServices;

namespace Tests.Support
{
    [Flags]
    public enum PRINTER_ACCESS_MASK : uint
    {
        SERVER_ACCESS_ADMINISTER = 0x00000001,
        SERVER_ACCESS_ENUMERATE = 0x00000002,

        PRINTER_ACCESS_ADMINISTER = 0x00000004,
        PRINTER_ACCESS_USE = 0x00000008,
        PRINTER_ACCESS_MANAGE_LIMITED = 0x00000040,
        PRINTER_ALL_ACCESS = 0x000F000C,
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct PRINTER_DEFAULTS
    {
        public string pDatatype;

        public IntPtr pDevMode;

        public PRINTER_ACCESS_MASK DesiredPrinterAccess;
    }
}