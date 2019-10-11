using System;
using System.Collections.Generic;
using PrinterChangeNotification.enums;

namespace PrinterChangeNotification
{
    public class NotifyOptionsType
    {
        public NOTIFY_TYPE Type;
        public List<UInt16> Fields = new List<UInt16>();
    }
}