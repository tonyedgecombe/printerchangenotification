using System;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace PrinterChangeNotification.enums
{
    // Note: These flags are different from the documentation
    // A pull request has been sent to MS 
    // https://github.com/MicrosoftDocs/win32/pull/45

    
    [Flags]
    public enum PRINTER_NOTIFY_CATEGORY : UInt32
    {
        PRINTER_NOTIFY_CATEGORY_2D  = 0x000000, // This is not defined in winspool.h but pulled from the documentation
        PRINTER_NOTIFY_CATEGORY_ALL = 0x001000,
        PRINTER_NOTIFY_CATEGORY_3D  = 0x002000,
    }
}