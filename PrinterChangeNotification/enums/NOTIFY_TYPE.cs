using System;
// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification.enums
{
    [Flags]
    public enum NOTIFY_TYPE : UInt16
    {
        PRINTER_NOTIFY_TYPE = 0x00,
        JOB_NOTIFY_TYPE     = 0x01,

        [Obsolete(message: "Not available", error: true)]
        SERVER_NOTIFY_TYPE  = 0x02,
    }
}