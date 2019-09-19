using System;
using System.Collections.Generic;

namespace PrinterChangeNotification
{
    public class PrinterNotifyOptions
    {
        public UInt32 Flags;   
        public static UInt32 PRINTER_NOTIFY_OPTIONS_REFRESH = 0x01;

        public List<PrinterNotifyOptionsType> Types = new List<PrinterNotifyOptionsType>();
    }
}