using System;
using PrinterChangeNotification.enums;

namespace Monitor
{
    class Program
    {
        static void Main()
        {
            using (var printer = new Printer(null))
            using (var printerChangeNotification = new PrinterChangeNotification.PrinterChangeNotification(printer, PRINTER_CHANGE.PRINTER_CHANGE_DELETE_PRINTER))
            using (var waitHandle = printerChangeNotification.WaitHandle)
            {
                while (true)
                {
                    Console.WriteLine("Waiting");

                    waitHandle.WaitOne();
                    printerChangeNotification.FindNextPrinterChangeNotification();

                    Console.WriteLine("Notification received");
                }
            }
        }
    }
}
