using System;
using System.Collections.Generic;
using PrinterChangeNotification;
using PrinterChangeNotification.enums;

namespace Monitor
{
    class Program
    {
        static void Main()
        {
            var options = new PrinterNotifyOptions
            {
                Types = new List<PrinterNotifyOptionsType>
                {
                    new PrinterNotifyOptionsType
                    {
                        Type = NOTIFY_TYPE.JOB_NOTIFY_TYPE,
                        Fields =
                        {
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TOTAL_PAGES,
                        }
                    },
                    //new PrinterNotifyOptionsType()
                    //{
                    //    Type = NOTIFY_TYPE.PRINTER_NOTIFY_TYPE,
                    //    Fields =
                    //    {
                    //        (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRINTER_NAME,
                    //    }
                    //},
                }
            };

            using (var printer = new Printer(null))
            using (var printerChangeNotification = new PrinterChangeNotification.PrinterChangeNotification(printer, 
                        PRINTER_CHANGE.PRINTER_CHANGE_ALL,
                        PRINTER_NOTIFY_CATEGORY.PRINTER_NOTIFY_CATEGORY_ALL,
                        options))
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
