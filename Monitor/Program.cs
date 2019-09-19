using System;
using System.Collections.Generic;
using System.Linq;
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
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SHARE_NAME,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PORT_NAME,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DRIVER_NAME,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_COMMENT,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_LOCATION,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DEVMODE,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SEPFILE,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRINT_PROCESSOR,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PARAMETERS,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DATATYPE,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SECURITY_DESCRIPTOR,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_ATTRIBUTES,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRIORITY,              // Priority doesn't seem to be monitored specifically, rather as part of the dev mode
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DEFAULT_PRIORITY,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_START_TIME,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_UNTIL_TIME,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_STATUS,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_CJOBS,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_AVERAGE_PPM,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_OBJECT_GUID,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_FRIENDLY_NAME,
                            (uint) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_BRANCH_OFFICE_PRINTING,
                        }
                    },
                    new PrinterNotifyOptionsType
                    {
                        Type = NOTIFY_TYPE.JOB_NOTIFY_TYPE,
                        Fields =
                        {
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRINTER_NAME                ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_MACHINE_NAME                ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PORT_NAME                   ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_USER_NAME                   ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_NOTIFY_NAME                 ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DATATYPE                    ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRINT_PROCESSOR             ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PARAMETERS                  ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DRIVER_NAME                 ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DEVMODE                     ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_STATUS                      ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_STATUS_STRING               ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DOCUMENT                    ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRIORITY                    ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_POSITION                    ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_SUBMITTED                   ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_START_TIME                  ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_UNTIL_TIME                  ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TIME                        ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TOTAL_PAGES                 ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PAGES_PRINTED               ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TOTAL_BYTES                 ,
                            (uint) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_BYTES_PRINTED               ,
                        }
                    },
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
                    waitHandle.WaitOne();
                    PrinterNotifyInfo info = printerChangeNotification.FindNextPrinterChangeNotification();
                    Console.WriteLine($"Change: {info.Change}");

                    var printerNotifyData = info.Data.Where(pair => pair.Type == (int) NOTIFY_TYPE.PRINTER_NOTIFY_TYPE);
                    foreach (var printerNotifyInfoData in printerNotifyData)
                    {
                        Console.WriteLine($"{(PRINTER_NOTIFY_FIELD)printerNotifyInfoData.Field} = {printerNotifyInfoData.Value}");
                    }

                    var jobNotifyData = info.Data.Where(pair => pair.Type == (int) NOTIFY_TYPE.JOB_NOTIFY_TYPE);
                    foreach (var pair in jobNotifyData)
                    {
                        Console.WriteLine($"{(JOB_NOTIFY_FIELD)pair.Field} = {pair.Value}");
                    }
                }
            }
        }
    }
}
