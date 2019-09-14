using System;
using System.ComponentModel;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using Monitor;
using PrinterChangeNotification.enums;

namespace PrinterChangeNotification
{
    internal class PrinterChangeNotificationWaitHandle : WaitHandle
    {
        public PrinterChangeNotificationWaitHandle(IntPtr handle)
        {
            SafeWaitHandle = new SafeWaitHandle(handle, false);
        }
    }

    public class PrinterChangeNotification : SafeHandleZeroOrMinusOneIsInvalid
    {
        public WaitHandle WaitHandle => new PrinterChangeNotificationWaitHandle(handle);

        public PrinterChangeNotification(Printer printer, PRINTER_CHANGE changes) : base(true)
        {
            Console.WriteLine("Starting monitoring");

            handle = NativeMethods.FindFirstPrinterChangeNotification(printer.DangerousGetHandle(), (UInt32)changes, 0, IntPtr.Zero);
            if (handle == new IntPtr(-1))
            {
                throw new Win32Exception();
            }
        }

        public void FindNextPrinterChangeNotification()
        {
            if (!NativeMethods.FindNextPrinterChangeNotification(handle, out var change, IntPtr.Zero, IntPtr.Zero))
            {
                throw new Win32Exception();
            }

            Console.WriteLine($"Change: {change}");
        }


        protected override bool ReleaseHandle()
        {
            Console.WriteLine("Ending monitoring");
            return NativeMethods.FindClosePrinterChangeNotification(handle);
        }
    }
}