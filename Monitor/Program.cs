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
                        Type = NOTIFY_TYPE.PRINTER_NOTIFY_TYPE,
                        Fields =
                        {
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRINTER_NAME,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_COMMENT,
                        }
                    },
                    //new PrinterNotifyOptionsType
                    //{
                    //    Type = NOTIFY_TYPE.JOB_NOTIFY_TYPE,
                    //    Fields =
                    //    {
                    //        (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRINTER_NAME,
                    //        (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TOTAL_PAGES,
                    //    }
                    //},
                }
            };

            using (var printer = new Printer(null))
            using (var printerChangeNotification = new PrinterChangeNotification.PrinterChangeNotification(printer, 
                        PRINTER_CHANGE.PRINTER_CHANGE_PRINTER,
                        PRINTER_NOTIFY_CATEGORY.PRINTER_NOTIFY_CATEGORY_ALL,
                        options))
            using (var waitHandle = printerChangeNotification.WaitHandle)
            {
                while (true)
                {
                    Console.WriteLine("\nWaiting");

                    waitHandle.WaitOne();
                    var change = printerChangeNotification.FindNextPrinterChangeNotification();
                    Console.WriteLine(change);

                    Console.WriteLine("Notification received");
                }
            }
        }
    }
}
