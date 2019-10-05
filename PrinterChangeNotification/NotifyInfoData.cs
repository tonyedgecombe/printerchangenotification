using System;

// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification
{
    public struct NotifyInfoData
    {
        public UInt16 Type { get; set; }
        public UInt16 Field { get; set; }

        public UInt32 Id { get; set; }

        public dynamic Value { get; set; }
    }
}