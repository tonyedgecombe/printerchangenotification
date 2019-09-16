using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification
{

    public class PrinterNotifyOptions
    {
        // Used purely for marshalling
        [StructLayout(LayoutKind.Sequential)]
        private struct PRINTER_NOTIFY_OPTIONS
        {
            public UInt32 Version;
            public UInt32 Flags;
            public UInt32 Count;

            public IntPtr pTypes;   // *PRINTER_NOTIFY_OPTIONS_TYPE
        }

        public UInt32 Flags;   
        public static UInt32 PRINTER_NOTIFY_OPTIONS_REFRESH = 0x01;

        public List<PrinterNotifyOptionsType> Types = new List<PrinterNotifyOptionsType>();

        public int SizeOf()
        {
            return Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS>() + Types.Sum(optionsType => optionsType.SizeOf());
        }

        public void ToPtr(IntPtr ptr)
        {
            var pos = ptr + Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS>(); // Point to memory right after the structure;

            PRINTER_NOTIFY_OPTIONS options;
            options.Version = 2;
            options.Flags = Flags;
            options.Count = (uint) Types.Count;
            options.pTypes = pos;

            Marshal.StructureToPtr(options, ptr, false);

            foreach (var optionsType in Types)
            {
                optionsType.ToPtr(pos);
                pos += optionsType.SizeOf();
            }
        }
    }
}