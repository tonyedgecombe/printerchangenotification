using System;
using System.Runtime.InteropServices;

namespace Tests.Support
{
    internal static class NativeMethods
    {
        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int AddPortEx(string server, int level, [MarshalAs(UnmanagedType.Struct)] ref PORT_INFO_1 pi, string monitorName);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int DeletePort(string server, IntPtr hwnd, string portName);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int OpenPrinter(string pPrinterName, out IntPtr phPrinter, ref PRINTER_DEFAULTS pDefault);

        [DllImport("winspool.drv", SetLastError = true)]
        internal static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int DeletePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern IntPtr AddPrinter(string pName, uint level, [MarshalAs(UnmanagedType.Struct)] ref PRINTER_INFO_2 pi2);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int EnumPorts(string pName, uint level, IntPtr pPorts, uint cbBuf, ref uint pcbNeeded, ref uint pcReturned);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int EnumPrinters(PrinterEnumFlags flags, string name, uint level, IntPtr pPrinterEnum, uint cbBuf, ref uint pcbNeeded, ref uint pcReturned);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int GetPrinter(IntPtr hPrinter, Int32 dwLevel, IntPtr pPrinter, Int32 dwBuf, out Int32 dwNeeded);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int SetPrinter(IntPtr hPrinter, Int32 dwLevel, IntPtr pPrinter, Int32 uCommand);
    }
}