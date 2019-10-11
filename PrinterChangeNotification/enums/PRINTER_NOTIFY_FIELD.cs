using System;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification.enums
{
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public enum PRINTER_NOTIFY_FIELD : UInt16

    {
        [Obsolete] PRINTER_NOTIFY_FIELD_SERVER_NAME         = 0x00, // Not supported according to the docs
        PRINTER_NOTIFY_FIELD_PRINTER_NAME                   = 0x01,
        PRINTER_NOTIFY_FIELD_SHARE_NAME                     = 0x02,
        PRINTER_NOTIFY_FIELD_PORT_NAME                      = 0x03,
        PRINTER_NOTIFY_FIELD_DRIVER_NAME                    = 0x04,
        PRINTER_NOTIFY_FIELD_COMMENT                        = 0x05,
        PRINTER_NOTIFY_FIELD_LOCATION                       = 0x06,
        PRINTER_NOTIFY_FIELD_DEVMODE                        = 0x07,
        PRINTER_NOTIFY_FIELD_SEPFILE                        = 0x08,
        PRINTER_NOTIFY_FIELD_PRINT_PROCESSOR                = 0x09,
        PRINTER_NOTIFY_FIELD_PARAMETERS                     = 0x0A,
        PRINTER_NOTIFY_FIELD_DATATYPE                       = 0x0B,
        PRINTER_NOTIFY_FIELD_SECURITY_DESCRIPTOR            = 0x0C,
        PRINTER_NOTIFY_FIELD_ATTRIBUTES                     = 0x0D,
        PRINTER_NOTIFY_FIELD_PRIORITY                       = 0x0E,
        PRINTER_NOTIFY_FIELD_DEFAULT_PRIORITY               = 0x0F,
        PRINTER_NOTIFY_FIELD_START_TIME                     = 0x10,
        PRINTER_NOTIFY_FIELD_UNTIL_TIME                     = 0x11,
        PRINTER_NOTIFY_FIELD_STATUS                         = 0x12,
        [Obsolete] PRINTER_NOTIFY_FIELD_STATUS_STRING       = 0x13, // Not supported according to the docs
        PRINTER_NOTIFY_FIELD_CJOBS                          = 0x14,
        PRINTER_NOTIFY_FIELD_AVERAGE_PPM                    = 0x15,
        [Obsolete] PRINTER_NOTIFY_FIELD_TOTAL_PAGES         = 0x16, // Not supported according to the docs
        [Obsolete] PRINTER_NOTIFY_FIELD_PAGES_PRINTED       = 0x17, // Not supported according to the docs
        [Obsolete] PRINTER_NOTIFY_FIELD_TOTAL_BYTES         = 0x18, // Not supported according to the docs
        [Obsolete] PRINTER_NOTIFY_FIELD_BYTES_PRINTED       = 0x19, // Not supported according to the docs
        PRINTER_NOTIFY_FIELD_OBJECT_GUID                    = 0x1A,
        PRINTER_NOTIFY_FIELD_FRIENDLY_NAME                  = 0x1B,
        PRINTER_NOTIFY_FIELD_BRANCH_OFFICE_PRINTING         = 0x1C,
    }
}