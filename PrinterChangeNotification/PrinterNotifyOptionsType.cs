using System;
using System.Collections.Generic;
using PrinterChangeNotification.enums;

namespace PrinterChangeNotification
{
    public class PrinterNotifyOptionsType
    {
        public NOTIFY_TYPE Type;
        public List<UInt32> Fields = new List<UInt32>();
    }
}