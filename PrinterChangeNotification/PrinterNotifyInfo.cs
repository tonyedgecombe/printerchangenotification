using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PrinterChangeNotification.enums;
using PrinterChangeNotification.structs;

#pragma warning disable 612

// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification
{
    public class PrinterNotifyInfo
    {
        public PRINTER_CHANGE Change { get; }

        public List<PrinterNotifyInfoData> Data { get; } = new List<PrinterNotifyInfoData>();

        public UInt32 Flags { get; }


        public PrinterNotifyInfo(PRINTER_CHANGE change, IntPtr ptr)
        {
            Change = change;

            if (ptr != IntPtr.Zero)
            {
                var notifyInfo = Marshal.PtrToStructure<PRINTER_NOTIFY_INFO>(ptr);

                var pos = ptr + 12; // pointer to aData

                for (int i = 0; i < notifyInfo.Count; i++)
                {
                    var info = CreatePrinterNotifyInfoData(pos);
                    Data.Add(info);

                    pos += Marshal.SizeOf<PRINTER_NOTIFY_INFO_DATA>();
                }

                Flags = notifyInfo.Flags;
            }
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
            switch ((PRINTER_NOTIFY_FIELD) data.Field)
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
    }
}