using System;
using System.Collections.Generic;

namespace PrinterChangeNotification
{
    public class PrinterNotifyOptions
    {
        public UInt32 Flags;   

        public List<PrinterNotifyOptionsType> Types = new List<PrinterNotifyOptionsType>();
    }
}