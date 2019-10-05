using System;
using System.Collections.Generic;
using System.Linq;
using PrinterChangeNotification;
using PrinterChangeNotification.enums;
using CommandLine;
// ReSharper disable InconsistentNaming

namespace Monitor
{
    class Program
    {
        private const UInt32 PRINTER_NOTIFY_INFO_DISCARDED = 0x01;

        static void Main(string[] args)
        {
            NotifyOptions printerNotifyOptions = null;

            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                if (o.JobNotifyFields.Any() || o.JobNotifyFields.Any())
                {
                    printerNotifyOptions = new NotifyOptions
                    {
                        Types = new List<NotifyOptionsType>()
                    };

                    if (o.JobNotifyFields.Any())
                    {
                        printerNotifyOptions.Types.Add(new NotifyOptionsType
                        {
                            Type = NOTIFY_TYPE.JOB_NOTIFY_TYPE,
                            Fields = o.JobNotifyFields.Cast<UInt32>().ToList(),
                        });
                    }

                    if (o.PrinterNotifyFields.Any())
                    {
                        printerNotifyOptions.Types.Add(new NotifyOptionsType
                        {
                            Type = NOTIFY_TYPE.PRINTER_NOTIFY_TYPE,
                            Fields = o.PrinterNotifyFields.Cast<UInt32>().ToList(),
                        });
                    }
                }
                MonitorPrinter(o.PrinterName, printerNotifyOptions, o.PrinterChange);
            });
        }

        private static void MonitorPrinter(string printerName, NotifyOptions printerNotifyOptions, PRINTER_CHANGE change)
        {
            using (var printerChangeNotification = new ChangeNotification(printerName,
                    change,
                    PRINTER_NOTIFY_CATEGORY.PRINTER_NOTIFY_CATEGORY_ALL,
                    printerNotifyOptions))
            {
                while (true)
                {
                    printerChangeNotification.WaitOne();
                    NotifyInfo printerNotifyInfo;
                    bool refresh = false;

                    do
                    {
                        printerNotifyInfo = printerChangeNotification.FindNextPrinterChangeNotification(refresh);
                        WriteToConsole(printerNotifyInfo);

                        refresh = true; // For next iteration if data overflowed
                    } while ((printerNotifyInfo.Flags & PRINTER_NOTIFY_INFO_DISCARDED) != 0);
                }
            }
        }

        private static void WriteToConsole(NotifyInfo printerNotifyInfo)
        {
            Console.WriteLine($"Change: {printerNotifyInfo.Change}");

            var printerNotifyData = printerNotifyInfo.Data.Where(data => data.Type == (int) NOTIFY_TYPE.PRINTER_NOTIFY_TYPE);
            foreach (var printerNotifyInfoData in printerNotifyData)
            {
                Console.WriteLine($"{(PRINTER_NOTIFY_FIELD) printerNotifyInfoData.Field} = {printerNotifyInfoData.Value}");
            }

            var jobNotifyData = printerNotifyInfo.Data.Where(data => data.Type == (int) NOTIFY_TYPE.JOB_NOTIFY_TYPE);
            foreach (var pair in jobNotifyData)
            {
                Console.WriteLine($"{pair.Id}:{(JOB_NOTIFY_FIELD) pair.Field} = {pair.Value}");
            }

            Console.WriteLine();
        }
    }
}
