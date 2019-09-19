using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using PrinterChangeNotification;

namespace Monitor
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