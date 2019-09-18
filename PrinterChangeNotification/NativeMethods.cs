using System;
using System.Runtime.InteropServices;

namespace PrinterChangeNotification
{
    internal class NativeMethods
    {
        //HANDLE WINAPI FindFirstPrinterChangeNotification(
        //    __in HANDLE hPrinter,
        //    DWORD fdwFilter,
        //    DWORD fdwOptions,
        //    __in_opt PVOID  pPrinterNotifyOptions
        //);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindFirstPrinterChangeNotification(IntPtr hPrinter, UInt32 fdwPrinterChange, UInt32 fdwOptions, IntPtr pPrinterNotifyOptions);

        //BOOL WINAPI FindNextPrinterChangeNotification(
        //    __in HANDLE hChange,
        //    __out_opt PDWORD pdwChange,
        //    __in_opt LPVOID pvReserved,
        //    __out_opt LPVOID *ppPrinterNotifyInfo
        //);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool FindNextPrinterChangeNotification(IntPtr hChange, out UInt32 pdwChange, IntPtr pvReserved, IntPtr ppPrinterNotifyInfo);

        //BOOL WINAPI FreePrinterNotifyInfo(
        //    __in PPRINTER_NOTIFY_INFO pPrinterNotifyInfo
        //);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool FreePrinterNotifyInfo(IntPtr pPrinterNotifyInfo);

        //BOOL WINAPI FindClosePrinterChangeNotification(
        //    __in HANDLE hChange
        //);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool FindClosePrinterChangeNotification(IntPtr hChange);

        //BOOL WINAPI OpenPrinterW(
        //    _In_opt_ LPWSTR             pPrinterName,
        //    _Out_ LPHANDLE            phPrinter,
        //    _In_opt_ LPPRINTER_DEFAULTSW pDefault
        //);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool OpenPrinter(string pPrinterName, out IntPtr phPrinter, IntPtr pDefault);

        //BOOL WINAPI ClosePrinter(
        //    _In_ HANDLE hPrinter
        //);

        [DllImport("winspool.drv", SetLastError = true)]
        public static extern bool ClosePrinter(IntPtr hPrinter);
    }
}
