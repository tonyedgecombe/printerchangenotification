using System;
// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification.enums
{
    // Note: These flags are different from the documentation
    // TODO Verify which is correct

    [Flags]
    public enum PRINTER_NOTIFY_CATEGORY : UInt32
    {
        PRINTER_NOTIFY_CATEGORY_ALL = 0x001000,
        PRINTER_NOTIFY_CATEGORY_3D  = 0x002000,
    }
}