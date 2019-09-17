using System;
// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification.enums
{
    public enum JOB_NOTIFY_FIELD : UInt32
    { 
        JOB_NOTIFY_FIELD_PRINTER_NAME                = 0x00,
        JOB_NOTIFY_FIELD_MACHINE_NAME                = 0x01,
        JOB_NOTIFY_FIELD_PORT_NAME                   = 0x02,
        JOB_NOTIFY_FIELD_USER_NAME                   = 0x03,
        JOB_NOTIFY_FIELD_NOTIFY_NAME                 = 0x04,
        JOB_NOTIFY_FIELD_DATATYPE                    = 0x05,
        JOB_NOTIFY_FIELD_PRINT_PROCESSOR             = 0x06,
        JOB_NOTIFY_FIELD_PARAMETERS                  = 0x07,
        JOB_NOTIFY_FIELD_DRIVER_NAME                 = 0x08,
        JOB_NOTIFY_FIELD_DEVMODE                     = 0x09,
        JOB_NOTIFY_FIELD_STATUS                      = 0x0A,
        JOB_NOTIFY_FIELD_STATUS_STRING               = 0x0B,
        JOB_NOTIFY_FIELD_SECURITY_DESCRIPTOR         = 0x0C,
        JOB_NOTIFY_FIELD_DOCUMENT                    = 0x0D,
        JOB_NOTIFY_FIELD_PRIORITY                    = 0x0E,
        JOB_NOTIFY_FIELD_POSITION                    = 0x0F,
        JOB_NOTIFY_FIELD_SUBMITTED                   = 0x10,
        JOB_NOTIFY_FIELD_START_TIME                  = 0x11,
        JOB_NOTIFY_FIELD_UNTIL_TIME                  = 0x12,
        JOB_NOTIFY_FIELD_TIME                        = 0x13,
        JOB_NOTIFY_FIELD_TOTAL_PAGES                 = 0x14,
        JOB_NOTIFY_FIELD_PAGES_PRINTED               = 0x15,
        JOB_NOTIFY_FIELD_TOTAL_BYTES                 = 0x16,
        JOB_NOTIFY_FIELD_BYTES_PRINTED               = 0x17,
        JOB_NOTIFY_FIELD_REMOTE_JOB_ID               = 0x18,
    }
}