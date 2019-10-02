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
            var result = new PrinterNotifyInfoData();

            if (ptr == IntPtr.Zero)
            {
                return result;
            }

            var data = Marshal.PtrToStructure<PRINTER_NOTIFY_INFO_DATA>(ptr);
            result.Type = data.Type;
            result.Field = data.Field;
            result.Id = data.Id;

            switch ((NOTIFY_TYPE)data.Type)
            {
                case NOTIFY_TYPE.PRINTER_NOTIFY_TYPE:
                    switch ((PRINTER_NOTIFY_FIELD)data.Field)
                    {
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SERVER_NAME:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRINTER_NAME:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SHARE_NAME:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PORT_NAME:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DRIVER_NAME:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_COMMENT:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_LOCATION:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DEVMODE:
                            result.Value = data.pBuf;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SEPFILE:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRINT_PROCESSOR:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PARAMETERS:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DATATYPE:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SECURITY_DESCRIPTOR:
                            result.Value = data.pBuf;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_ATTRIBUTES:
                            result.Value = data.adwData0;
                            break;

                        // In my tests although priority is monitored it's value isn't available via this field
                        // It should be possible to pick it up in the devmode though.
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRIORITY:
                            result.Value = data.adwData0;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DEFAULT_PRIORITY:
                            result.Value = data.adwData0;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_START_TIME:
                            result.Value = data.adwData0;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_UNTIL_TIME:
                            result.Value = data.adwData0;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_STATUS:
                            result.Value = data.adwData0;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_STATUS_STRING:
                            // Not supported
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_CJOBS:
                            result.Value = data.adwData0;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_AVERAGE_PPM:
                            result.Value = data.adwData0;
                            break;
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
                            result.Value = true;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_FRIENDLY_NAME:
                            // Windows Vista and later
                            // Not particularly well documented
                            result.Value = true; // Probably should be a string
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_BRANCH_OFFICE_PRINTING:
                            // Windows 8 and later
                            // Not documented at all
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case NOTIFY_TYPE.JOB_NOTIFY_TYPE:
                    switch ((JOB_NOTIFY_FIELD)data.Field)
                    {
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRINTER_NAME:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_MACHINE_NAME:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PORT_NAME:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_USER_NAME:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_NOTIFY_NAME:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DATATYPE:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRINT_PROCESSOR:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PARAMETERS:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DRIVER_NAME:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DEVMODE:
                            result.Value = data.pBuf;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_STATUS:
                            result.Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_STATUS_STRING:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_SECURITY_DESCRIPTOR:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DOCUMENT:
                            result.Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRIORITY:
                            result.Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_POSITION:
                            result.Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_SUBMITTED:
                            result.Value = data.pBuf;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_START_TIME:
                            result.Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_UNTIL_TIME:
                            result.Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TIME:
                            result.Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TOTAL_PAGES:
                            result.Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PAGES_PRINTED:
                            result.Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TOTAL_BYTES:
                            result.Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_BYTES_PRINTED:
                            result.Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_REMOTE_JOB_ID:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }
    }
}