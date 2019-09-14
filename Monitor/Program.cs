using System;
using PrinterChangeNotification.enums;

namespace Monitor
{
    class Program
    {
        static void Main()
        {
            using (var printer = new Printer(null))
            using (var printerChangeNotification = new PrinterChangeNotification.PrinterChangeNotification(printer, 
                        PRINTER_CHANGE.PRINTER_CHANGE_DELETE_PRINTER,
                        PRINTER_NOTIFY_CATEGORY.PRINTER_NOTIFY_CATEGORY_ALL))
            using (var waitHandle = printerChangeNotification.WaitHandle)
            {
                while (true)
                {
                    Console.WriteLine("Waiting");

                    waitHandle.WaitOne();
                    var change = printerChangeNotification.FindNextPrinterChangeNotification();
                    Console.WriteLine($"Change: {change}");

                    Console.WriteLine("Notification received");
                }
            }
        }
    }
}
