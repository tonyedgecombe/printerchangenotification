using System;
using System.Collections.Generic;

namespace PrinterChangeNotification
{
    public class NotifyOptions
    {
        public UInt32 Flags;   

        public List<NotifyOptionsType> Types = new List<NotifyOptionsType>();
    }
}