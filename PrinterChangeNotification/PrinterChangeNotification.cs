using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using PrinterChangeNotification.enums;
using PrinterChangeNotification.structs;

// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification
{
    public class PrinterChangeNotification : SafeHandleZeroOrMinusOneIsInvalid
    {
        public static UInt32 PRINTER_NOTIFY_OPTIONS_REFRESH = 0x01;

        private class PrinterChangeNotificationWaitHandle : WaitHandle
        {
            public PrinterChangeNotificationWaitHandle(IntPtr handle)
            {
                SafeWaitHandle = new SafeWaitHandle(handle, false);
            }
        }

        public WaitHandle WaitHandle => new PrinterChangeNotificationWaitHandle(handle);

        public PrinterChangeNotification(Printer printer, 
                                         PRINTER_CHANGE changes, 
                                         PRINTER_NOTIFY_CATEGORY category,
                                         PrinterNotifyOptions options) : base(true)
        {
            var hGlobalPtr = IntPtr.Zero;

            if (options != null)
            {
                var size = SizeOf(options);
                hGlobalPtr = Marshal.AllocHGlobal(size);
                ToPtr(hGlobalPtr, options);
            }

            try
            {
                handle = NativeMethods.FindFirstPrinterChangeNotification(printer.DangerousGetHandle(),
                    (UInt32) changes,
                    (UInt32) category,
                    hGlobalPtr);
                if (handle == new IntPtr(-1))
                {
                    throw new Win32Exception();
                }
            }
            finally
            {
                Marshal.FreeHGlobal(hGlobalPtr);
            }
        }

        private static void ToPtr(IntPtr pos, PrinterNotifyOptions options)
        {
            PRINTER_NOTIFY_OPTIONS st;
            st.Flags = options.Flags;
            st.Count = (uint) options.Types.Count;
            st.Version = 2;
            st.pTypes = pos + Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS>();

            Marshal.StructureToPtr(st, pos, false);
            pos += Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS>();

            var fieldsPos = (IntPtr) pos + (options.Types.Count * Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS_TYPE>());

            foreach (var optionsType in options.Types)
            {
                PRINTER_NOTIFY_OPTIONS_TYPE str = default;
                str.Type = (UInt16) optionsType.Type;
                str.Count = (UInt32) optionsType.Fields.Count;
                str.pFields = fieldsPos;

                Marshal.StructureToPtr(str, pos, false);
                pos += Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS_TYPE>();

                foreach (var field in optionsType.Fields)
                {
                    Marshal.WriteInt32(fieldsPos, (Int32) field);
                    fieldsPos += Marshal.SizeOf<Int32>();
                }
            }
        }

        private static int SizeOf(PrinterNotifyOptions options)
        {
            var size = Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS>();
            foreach (var printerNotifyOptionsType in options.Types)
            {
                size += Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS_TYPE>();
                size += printerNotifyOptionsType.Fields.Count * Marshal.SizeOf<UInt32>();
            }

            return size;
        }

        public PrinterNotifyInfo FindNextPrinterChangeNotification(bool refresh)
        {
            var ppPrinterNotifyInfo = IntPtr.Zero;
            var pOptions = IntPtr.Zero;
            var pPrinterNotifyInfo = IntPtr.Zero;

            try
            {
                ppPrinterNotifyInfo = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>());

                PRINTER_NOTIFY_OPTIONS options = default;
                options.Flags = refresh ? PRINTER_NOTIFY_OPTIONS_REFRESH : 0x00;

                pOptions = Marshal.AllocHGlobal(Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS>());
                Marshal.StructureToPtr(options, pOptions, false);

                if (!NativeMethods.FindNextPrinterChangeNotification(handle, out var change, pOptions, ppPrinterNotifyInfo))
                {
                    throw new Win32Exception();
                }

                pPrinterNotifyInfo = Marshal.ReadIntPtr(ppPrinterNotifyInfo);

                return new PrinterNotifyInfo((PRINTER_CHANGE) change, pPrinterNotifyInfo);
            }
            finally
            {
                Marshal.FreeHGlobal(pOptions);
                Marshal.FreeHGlobal(ppPrinterNotifyInfo);

                if (pPrinterNotifyInfo != IntPtr.Zero)
                {
                    NativeMethods.FreePrinterNotifyInfo(pPrinterNotifyInfo);
                }
            }
        }


        protected override bool ReleaseHandle()
        {
            return NativeMethods.FindClosePrinterChangeNotification(handle);
        }
    }
}