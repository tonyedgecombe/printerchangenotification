using System;
using System.ComponentModel;
using PrinterChangeNotification;
using PrinterChangeNotification.enums;

namespace Monitor
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Monitoring print server");

            using (var printer = new Printer(null))
            {
                var changeHandle = NativeMethods.FindFirstPrinterChangeNotification(printer.DangerousGetHandle(), (UInt32) PRINTER_CHANGE.PRINTER_CHANGE_ALL, 0, IntPtr.Zero);
                if (changeHandle == new IntPtr(-1))
                {
                    throw new Win32Exception();
                }

                try
                {
                    while (true)
                    {
                        Console.WriteLine("Waiting for change");

                        break;
                    }
                }
                finally
                {
                    Console.WriteLine("Ending monitoring");
                    NativeMethods.FindClosePrinterChangeNotification(changeHandle);
                }
            }
        }
    }
}
