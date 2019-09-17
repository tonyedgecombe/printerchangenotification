using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PrinterChangeNotification.enums;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace PrinterChangeNotification
{
    public class PrinterNotifyOptionsType
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct PRINTER_NOTIFY_OPTIONS_TYPE
        {
            public UInt16 Type;
            private UInt16 Reserved0;
            private UInt32 Reserved1;
            private UInt32 Reserved2;
            public UInt32 Count;
            public IntPtr pFields; // *DWORD
        }

        public NOTIFY_TYPE Type;
        public List<UInt32> Fields = new List<UInt32>();

        public int SizeOf()
        {
            return Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS_TYPE>() + Fields.Count * Marshal.SizeOf<UInt32>();
        }

        public void ToPtr(IntPtr ptr)
        {
            var pos = ptr + Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS_TYPE>();

            PRINTER_NOTIFY_OPTIONS_TYPE optionsType = default;
            optionsType.Type = (UInt16) Type;
            optionsType.Count = (uint) Fields.Count;
            optionsType.pFields = pos;

            Marshal.StructureToPtr(optionsType, ptr, false);

            foreach (var field in Fields)
            {
                Marshal.WriteInt32(pos, (int) field);
                pos += Marshal.SizeOf<Int32>();
            }
        }

    }
}