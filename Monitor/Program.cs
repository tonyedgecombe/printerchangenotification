using System;
using System.Collections.Generic;
using System.Linq;
using PrinterChangeNotification;
using PrinterChangeNotification.enums;
using CommandLine;
// ReSharper disable InconsistentNaming

namespace Monitor
{
    public class Options
    {
        [Option('p', "printername", Default = null, Required = false)]
        public string PrinterName { get; set; }

        [Option('c', "printerchanges", Required = false, Separator = ',', HelpText = "Comma separated list of changes to monitor.")]
        public IEnumerable<PRINTER_CHANGE> PrinterChanges { get; set; }

        [Option('j', "jobnotifyfields", Required = false, Separator = ',')]
        public IEnumerable<JOB_NOTIFY_FIELD> JobNotifyFields { get; set; }

        [Option('f', "printernotifyfields", Required = false, Separator = ',')]
        public IEnumerable<PRINTER_NOTIFY_FIELD> PrinterNotifyFields { get; set; }

        public PRINTER_CHANGE PrinterChange => PrinterChanges.Aggregate((PRINTER_CHANGE) 0, (o, c) => o | c);
    }

    class Program
    {
        private const UInt32 PRINTER_NOTIFY_INFO_DISCARDED = 0x01;

        static void Main(string[] args)
        {
            PrinterNotifyOptions printerNotifyOptions = null;

            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                if (o.JobNotifyFields.Any() || o.JobNotifyFields.Any())
                {
                    printerNotifyOptions = new PrinterNotifyOptions
                    {
                        Types = new List<PrinterNotifyOptionsType>()
                    };

                    if (o.JobNotifyFields.Any())
                    {
                        printerNotifyOptions.Types.Add(new PrinterNotifyOptionsType
                        {
                            Type = NOTIFY_TYPE.JOB_NOTIFY_TYPE,
                            Fields = o.JobNotifyFields.Cast<UInt32>().ToList(),
                        });
                    }

                    if (o.PrinterNotifyFields.Any())
                    {
                        printerNotifyOptions.Types.Add(new PrinterNotifyOptionsType
                        {
                            Type = NOTIFY_TYPE.PRINTER_NOTIFY_TYPE,
                            Fields = o.PrinterNotifyFields.Cast<UInt32>().ToList(),
                        });
                    }
                }
                MonitorPrinter(o.PrinterName, printerNotifyOptions, o.PrinterChange);
            });
        }

        private static void MonitorPrinter(string printerName, PrinterNotifyOptions printerNotifyOptions, PRINTER_CHANGE change)
        {
            using (var printerChangeNotification = new PrinterChangeNotification.PrinterChangeNotification(printerName,
                change,
                PRINTER_NOTIFY_CATEGORY.PRINTER_NOTIFY_CATEGORY_ALL,
                printerNotifyOptions))
            using (var waitHandle = printerChangeNotification.WaitHandle)
            {
                while (true)
                {
                    waitHandle.WaitOne();
                    PrinterNotifyInfo printerNotifyInfo;
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

        private static void WriteToConsole(PrinterNotifyInfo printerNotifyInfo)
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
