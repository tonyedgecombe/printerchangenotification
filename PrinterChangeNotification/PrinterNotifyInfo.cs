using System;
using System.Collections.Generic;
using PrinterChangeNotification.enums;


// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification
{
    public class PrinterNotifyInfo
    {
        public PRINTER_CHANGE Change { get; set; }

        public List<PrinterNotifyInfoData> Data { get; } = new List<PrinterNotifyInfoData>();

        public UInt32 Flags { get; set; }
    }
}