using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using Monitor;
using PrinterChangeNotification.enums;

namespace PrinterChangeNotification
{
    public class PrinterChangeNotification : SafeHandleZeroOrMinusOneIsInvalid
    {
        private class PrinterChangeNotificationWaitHandle : WaitHandle
        {
            public PrinterChangeNotificationWaitHandle(IntPtr handle)
            {
                SafeWaitHandle = new SafeWaitHandle(handle, false);
            }
        }

        public WaitHandle WaitHandle => new PrinterChangeNotificationWaitHandle(handle);

        public PrinterChangeNotification(Printer printer, 
                                         PRINTER_CHANGE changes, 
                                         PRINTER_NOTIFY_CATEGORY category,
                                         PrinterNotifyOptions options) : base(true)
        {
            Console.WriteLine("Starting monitoring");

            var hGlobalPtr = IntPtr.Zero;

            if (options != null)
            {
                var size = options.SizeOf();
                hGlobalPtr = Marshal.AllocHGlobal(size);
                options.ToPtr(hGlobalPtr);
            }

            try
            {
                handle = NativeMethods.FindFirstPrinterChangeNotification(printer.DangerousGetHandle(),
                    (UInt32) changes,
                    (UInt32) category,
                    hGlobalPtr);
                if (handle == new IntPtr(-1))
                {
                    throw new Win32Exception();
                }
            }
            finally
            {
                Marshal.FreeHGlobal(hGlobalPtr);
            }
        }

        public uint FindNextPrinterChangeNotification()
        {
            if (!NativeMethods.FindNextPrinterChangeNotification(handle, out var change, IntPtr.Zero, IntPtr.Zero))
            {
                throw new Win32Exception();
            }

            return change;
        }


        protected override bool ReleaseHandle()
        {
            Console.WriteLine("Ending monitoring");
            return NativeMethods.FindClosePrinterChangeNotification(handle);
        }
    }
}