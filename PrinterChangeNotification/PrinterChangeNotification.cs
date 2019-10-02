using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using PrinterChangeNotification.enums;
using PrinterChangeNotification.structs;
#pragma warning disable 612

// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification
{
    public class PrinterChangeNotification : SafeHandleZeroOrMinusOneIsInvalid
    {
        public static UInt32 PRINTER_NOTIFY_OPTIONS_REFRESH = 0x01;

        public WaitHandle WaitHandle => new PrinterChangeNotificationWaitHandle(handle);

        private IntPtr _printerHandle;

        public PrinterChangeNotification(string printerName, 
                                         PRINTER_CHANGE changes, 
                                         PRINTER_NOTIFY_CATEGORY category,
                                         PrinterNotifyOptions options) : base(true)
        {
            var hGlobalPtr = IntPtr.Zero;

            OpenPrinter(printerName);

            try
            {
                if (options != null)
                {
                    var size = SizeOfNotifyOptions(options);
                    hGlobalPtr = Marshal.AllocHGlobal(size);
                    ToPtr(hGlobalPtr, options);
                }

                handle = NativeMethods.FindFirstPrinterChangeNotification(_printerHandle,
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

        private void OpenPrinter(string printerName)
        {
            if (!NativeMethods.OpenPrinter(printerName, out _printerHandle, IntPtr.Zero))
            {
                throw new Win32Exception();
            }
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

                return CreatePrinterNotifyInfo(change, pPrinterNotifyInfo);
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

        private static PrinterNotifyInfo CreatePrinterNotifyInfo(uint change, IntPtr ptr)
        {
            var result = new PrinterNotifyInfo
            {
                Change = (PRINTER_CHANGE) change
            };

            if (ptr == IntPtr.Zero)
            {
                return result;
            }

            var notifyInfo = Marshal.PtrToStructure<PRINTER_NOTIFY_INFO>(ptr);

            var pos = ptr + 12; // pointer to aData

            for (int i = 0; i < notifyInfo.Count; i++)
            {
                var info = CreatePrinterNotifyInfoData(pos);
                result.Data.Add(info);

                pos += Marshal.SizeOf<PRINTER_NOTIFY_INFO_DATA>();
            }

            result.Flags = notifyInfo.Flags;

            return result;
        }

        private static PrinterNotifyInfoData CreatePrinterNotifyInfoData(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return new PrinterNotifyInfoData();
            }

            var data = Marshal.PtrToStructure<PRINTER_NOTIFY_INFO_DATA>(ptr);

            return new PrinterNotifyInfoData
            {
                Type = data.Type,
                Field = data.Field,
                Id = data.Id,
                Value = GetNotifyFieldValue(data),
            };
        }

        private static dynamic GetNotifyFieldValue(PRINTER_NOTIFY_INFO_DATA data)
        {
            switch ((NOTIFY_TYPE)data.Type)
            {
                case NOTIFY_TYPE.PRINTER_NOTIFY_TYPE:
                    return GetPrinterNotifyFieldValue(data);
                case NOTIFY_TYPE.JOB_NOTIFY_TYPE:
                    return GetJobNotifyFieldValue(data);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static dynamic GetJobNotifyFieldValue(PRINTER_NOTIFY_INFO_DATA data)
        {
            switch ((JOB_NOTIFY_FIELD)data.Field)
            {
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRINTER_NAME:
                    return Marshal.PtrToStringUni(data.pBuf);
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_MACHINE_NAME:
                    return Marshal.PtrToStringUni(data.pBuf);
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PORT_NAME:
                    return Marshal.PtrToStringUni(data.pBuf);
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_USER_NAME:
                    return Marshal.PtrToStringUni(data.pBuf);
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_NOTIFY_NAME:
                    return Marshal.PtrToStringUni(data.pBuf);
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DATATYPE:
                    return Marshal.PtrToStringUni(data.pBuf);
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRINT_PROCESSOR:
                    return Marshal.PtrToStringUni(data.pBuf);
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PARAMETERS:
                    return Marshal.PtrToStringUni(data.pBuf);
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DRIVER_NAME:
                    return Marshal.PtrToStringUni(data.pBuf);
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DEVMODE:
                    return data.pBuf;
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_STATUS:
                    return data.adwData0;
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_STATUS_STRING:
                    return Marshal.PtrToStringUni(data.pBuf);
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_SECURITY_DESCRIPTOR:
                    break;
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DOCUMENT:
                    return Marshal.PtrToStringUni(data.pBuf);
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRIORITY:
                    return data.adwData0;
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_POSITION:
                    return data.adwData0;
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_SUBMITTED:
                    return data.pBuf;
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_START_TIME:
                    return data.adwData0;
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_UNTIL_TIME:
                    return data.adwData0;
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TIME:
                    return data.adwData0;
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TOTAL_PAGES:
                    return data.adwData0;
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PAGES_PRINTED:
                    return data.adwData0;
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TOTAL_BYTES:
                    return data.adwData0;
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_BYTES_PRINTED:
                    return data.adwData0;
                case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_REMOTE_JOB_ID:
                    break;
                default:
                    return null;
            }

            return null;
        }

        private static dynamic GetPrinterNotifyFieldValue(PRINTER_NOTIFY_INFO_DATA data)
        {
            switch ((PRINTER_NOTIFY_FIELD)data.Field)
            {
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SERVER_NAME:
                    return Marshal.PtrToStringUni(data.pBuf);
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRINTER_NAME:
                    return Marshal.PtrToStringUni(data.pBuf);
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SHARE_NAME:
                    return Marshal.PtrToStringUni(data.pBuf);
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PORT_NAME:
                    return Marshal.PtrToStringUni(data.pBuf);
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DRIVER_NAME:
                    return Marshal.PtrToStringUni(data.pBuf);
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_COMMENT:
                    return Marshal.PtrToStringUni(data.pBuf);
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_LOCATION:
                    return Marshal.PtrToStringUni(data.pBuf);
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DEVMODE:
                    return data.pBuf;
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SEPFILE:
                    return Marshal.PtrToStringUni(data.pBuf);
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRINT_PROCESSOR:
                    return Marshal.PtrToStringUni(data.pBuf);
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PARAMETERS:
                    return Marshal.PtrToStringUni(data.pBuf);
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DATATYPE:
                    return Marshal.PtrToStringUni(data.pBuf);
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SECURITY_DESCRIPTOR:
                    return data.pBuf;
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_ATTRIBUTES:
                    return data.adwData0;

                // In my tests although priority is monitored it's value isn't available via this field
                // It should be possible to pick it up in the devmode though.
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRIORITY:
                    return data.adwData0;
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DEFAULT_PRIORITY:
                    return data.adwData0;
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_START_TIME:
                    return data.adwData0;
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_UNTIL_TIME:
                    return data.adwData0;
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_STATUS:
                    return data.adwData0;
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_STATUS_STRING:
                    // Not supported
                    break;
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_CJOBS:
                    return data.adwData0;
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_AVERAGE_PPM:
                    return data.adwData0;
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_TOTAL_PAGES:
                    // Not supported
                    break;
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PAGES_PRINTED:
                    // Not supported
                    break;
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_TOTAL_BYTES:
                    // Not supported
                    break;
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_BYTES_PRINTED:
                    // Not supported
                    break;
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_OBJECT_GUID:
                    // Not particularly well documented
                    return true;
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_FRIENDLY_NAME:
                    // Windows Vista and later
                    // Not particularly well documented
                    return true; // Probably should be a string
                case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_BRANCH_OFFICE_PRINTING:
                    // Windows 8 and later
                    // Not documented at all
                    break;
                default:
                    return null;
            }

            return null;
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

            var fieldsPos = pos + (options.Types.Count * Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS_TYPE>());

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

        private static int SizeOfNotifyOptions(PrinterNotifyOptions options)
        {
            var size = Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS>();
            foreach (var printerNotifyOptionsType in options.Types)
            {
                size += Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS_TYPE>();
                size += printerNotifyOptionsType.Fields.Count * Marshal.SizeOf<UInt32>();
            }

            return size;
        }


        protected override bool ReleaseHandle()
        {
            return NativeMethods.FindClosePrinterChangeNotification(handle) && NativeMethods.ClosePrinter(_printerHandle);
        }
    }
}