using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PrinterChangeNotification.enums;
// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification
{
    public class PrinterNotifyInfo
    {
        public PRINTER_CHANGE Change { get; }

        public List<PrinterNotifyInfoData> Data { get; } = new List<PrinterNotifyInfoData>();

        public UInt32 Flags { get; }

        [StructLayout(LayoutKind.Sequential)]
        private struct PRINTER_NOTIFY_INFO
        {
            public readonly UInt32 Version;
            public readonly UInt32 Flags;
            public readonly UInt32 Count;
            private readonly IntPtr aData;
        }

        public PrinterNotifyInfo(PRINTER_CHANGE change, IntPtr ptr)
        {
            Change = change;

            if (ptr != IntPtr.Zero)
            {
                var notifyInfo = Marshal.PtrToStructure<PRINTER_NOTIFY_INFO>(ptr);

                var pos = ptr + 12; // pointer to aData

                for (int i = 0; i < notifyInfo.Count; i++)
                {
                    var info = new PrinterNotifyInfoData(pos);
                    Data.Add(info);

                    pos += PrinterNotifyInfoData.SizeOf();
                }

                Flags = notifyInfo.Flags;
            }
        }
    }
}