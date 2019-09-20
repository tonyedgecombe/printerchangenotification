using System;
using System.ComponentModel;
using Microsoft.Win32.SafeHandles;

namespace PrinterChangeNotification
{
    public class Printer : SafeHandleZeroOrMinusOneIsInvalid
    {
        public Printer(string printerName) : base(true)
        {
            if (!NativeMethods.OpenPrinter(printerName, out handle, IntPtr.Zero))
            {
                throw new Win32Exception();
            }
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.ClosePrinter(handle);
        }
    }
}