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

        public Dictionary<UInt16, PrinterNotifyInfoData> Data { get; } = new Dictionary<UInt16, PrinterNotifyInfoData>();

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
                Console.WriteLine($"Notify: Version: {notifyInfo.Version}, Flags: {notifyInfo.Flags}, Count: {notifyInfo.Count}");

                var pos = ptr + 12; // pointer to aData

                for (int i = 0; i < notifyInfo.Count; i++)
                {
                    var info = new PrinterNotifyInfoData(pos);
                    Data[info.Field] = info;

                    pos += PrinterNotifyInfoData.SizeOf();
                }
            }
        }

        public override string ToString()
        {
            return $"Change {Change}";
        }
    }
}