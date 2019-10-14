using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PrinterChangeNotification;
using PrinterChangeNotification.enums;
using CommandLine;
// ReSharper disable InconsistentNaming

namespace Monitor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Monitor(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void Monitor(IEnumerable<string> args)
        {
            NotifyOptions printerNotifyOptions = null;

            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                if (o.JobNotifyFields.Any() || o.PrinterNotifyFields.Any())
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
                            Fields = o.JobNotifyFields.Cast<UInt16>().ToList(),
                        });
                    }

                    if (o.PrinterNotifyFields.Any())
                    {
                        printerNotifyOptions.Types.Add(new NotifyOptionsType
                        {
                            Type = NOTIFY_TYPE.PRINTER_NOTIFY_TYPE,
                            Fields = o.PrinterNotifyFields.Cast<UInt16>().ToList(),
                        });
                    }
                }

                MonitorPrinter(o.PrinterName, printerNotifyOptions, o.PrinterChange);
            });
        }

        private static void MonitorPrinter(string printerName, NotifyOptions printerNotifyOptions, PRINTER_CHANGE change)
        {
            if (printerNotifyOptions == null && change == 0)
            {
                throw new Exception("Either or both of printer changes or fields to monitor must be set.");
            }

            using var printerChangeNotification = ChangeNotification.Create(printerName,
                                                change,
                                                PRINTER_NOTIFY_CATEGORY.PRINTER_NOTIFY_CATEGORY_ALL,
                                                printerNotifyOptions);
            
            while (true)
            {
                printerChangeNotification.WaitHandle.WaitOne();
                NotifyInfo printerNotifyInfo;
                bool refresh = false;

                do
                {
                    printerNotifyInfo = printerChangeNotification.FindNextPrinterChangeNotification(refresh);
                    WriteToConsole(printerNotifyInfo);

                    refresh = true; // For next iteration if data overflowed
                } while ((printerNotifyInfo.Flags & NotifyInfo.PRINTER_NOTIFY_INFO_DISCARDED) != 0);
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
