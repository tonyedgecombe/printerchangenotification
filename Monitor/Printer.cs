using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Monitor
{
    class Printer : SafeHandleZeroOrMinusOneIsInvalid
    {
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool OpenPrinter(string pPrinterName, out IntPtr phPrinter, IntPtr pDefault);

        [DllImport("winspool.drv", SetLastError = true)]
        private static extern bool ClosePrinter(IntPtr hPrinter);


        public Printer(string printerName) : base(true)
        {
            Console.WriteLine("Opening printer");
            if (!OpenPrinter(printerName, out handle, IntPtr.Zero))
            {
                throw new Win32Exception();
            }
        }

        protected override bool ReleaseHandle()
        {
            Console.WriteLine("Closing printer");
            return ClosePrinter(handle);
        }
    }
}