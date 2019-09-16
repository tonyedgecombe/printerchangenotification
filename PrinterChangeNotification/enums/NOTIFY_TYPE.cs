using System;
// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification.enums
{
    [Flags]
    public enum NOTIFY_TYPE : UInt32
    {
        PRINTER_NOTIFY_TYPE = 0x00,
        JOB_NOTIFY_TYPE     = 0x01,
        SERVER_NOTIFY_TYPE  = 0x02,
    }
}