using System.Collections.Generic;
using System.Linq;
using CommandLine;
using PrinterChangeNotification.enums;

namespace Monitor
{
    public class Options
    {
        [Option('p', "printername", Default = null, Required = false)]
        public string PrinterName { get; set; }

        [Option('c', "printerchanges", Required = false, Separator = ',', HelpText = "Comma separated list of printer changes to monitor.")]
        public IEnumerable<PRINTER_CHANGE> PrinterChanges { get; set; }

        [Option('j', "jobnotifyfields", Required = false, Separator = ',', HelpText = "Comma separated list of job fields to monitor.")]
        public IEnumerable<JOB_NOTIFY_FIELD> JobNotifyFields { get; set; }

        [Option('f', "printernotifyfields", Required = false, Separator = ',', HelpText = "Comma separated list of printer fields to monitor.")]
        public IEnumerable<PRINTER_NOTIFY_FIELD> PrinterNotifyFields { get; set; }

        public PRINTER_CHANGE PrinterChange => PrinterChanges.Aggregate((PRINTER_CHANGE) 0, (o, c) => o | c);
    }
}