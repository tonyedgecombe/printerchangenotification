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
            NotifyOptions notifyOptions = null;

            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                if (o.JobNotifyFields.Any() || o.JobNotifyFields.Any())
                {
                    notifyOptions = new NotifyOptions
                    {
                        Types = new List<NotifyOptionsType>()
                    };

                    if (o.JobNotifyFields.Any())
                    {
                        notifyOptions.Types.Add(new NotifyOptionsType
                        {
                            Type = NOTIFY_TYPE.JOB_NOTIFY_TYPE,
                            Fields = o.JobNotifyFields.Cast<UInt32>().ToList(),
                        });
                    }

                    if (o.PrinterNotifyFields.Any())
                    {
                        notifyOptions.Types.Add(new NotifyOptionsType
                        {
                            Type = NOTIFY_TYPE.PRINTER_NOTIFY_TYPE,
                            Fields = o.PrinterNotifyFields.Cast<UInt32>().ToList(),
                        });
                    }
                }
                MonitorPrinter(o.PrinterName, notifyOptions, o.PrinterChange);
            });
        }

        private static void MonitorPrinter(string printerName, NotifyOptions notifyOptions, PRINTER_CHANGE change)
        {
            using var changeNotification = new ChangeNotification(printerName,
                                            change,
                                            PRINTER_NOTIFY_CATEGORY.PRINTER_NOTIFY_CATEGORY_ALL,
                                            notifyOptions);

            while (true)
            {
                changeNotification.WaitOne();
                NotifyInfo notifyInfo;
                bool refresh = false;

                do
                {
                    notifyInfo = changeNotification.FindNextPrinterChangeNotification(refresh);
                    WriteToConsole(notifyInfo);

                    refresh = true; // For next iteration if data overflowed
                } while ((notifyInfo.Flags & PRINTER_NOTIFY_INFO_DISCARDED) != 0);
            }
        }

        private static void WriteToConsole(NotifyInfo notifyInfo)
        {
            Console.WriteLine($"Change: {notifyInfo.Change}");

            var printerNotifyData = notifyInfo.Data.Where(data => data.Type == (int) NOTIFY_TYPE.PRINTER_NOTIFY_TYPE);
            foreach (var printerNotifyInfoData in printerNotifyData)
            {
                Console.WriteLine($"{(PRINTER_NOTIFY_FIELD) printerNotifyInfoData.Field} = {printerNotifyInfoData.Value}");
            }

            var jobNotifyData = notifyInfo.Data.Where(data => data.Type == (int) NOTIFY_TYPE.JOB_NOTIFY_TYPE);
            foreach (var pair in jobNotifyData)
            {
                Console.WriteLine($"{pair.Id}:{(JOB_NOTIFY_FIELD) pair.Field} = {pair.Value}");
            }

            Console.WriteLine();
        }
    }
}
