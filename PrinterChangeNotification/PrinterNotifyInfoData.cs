using System;
using System.Runtime.InteropServices;
using PrinterChangeNotification.enums;
using PrinterChangeNotification.structs;

// ReSharper disable InconsistentNaming
#pragma warning disable 612

namespace PrinterChangeNotification
{
    public class PrinterNotifyInfoData
    {

        public UInt16 Type { get; }
        public UInt16 Field { get; }

        public UInt32 Id { get; }

        public dynamic Value { get; set; }

        public PrinterNotifyInfoData(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return;
            }

            var data = Marshal.PtrToStructure<PRINTER_NOTIFY_INFO_DATA>(ptr);
            Type = data.Type;
            Field = data.Field;
            Id = data.Id;

            switch ((NOTIFY_TYPE)data.Type)
            {
                case NOTIFY_TYPE.PRINTER_NOTIFY_TYPE:
                    switch ((PRINTER_NOTIFY_FIELD)data.Field)
                    {
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SERVER_NAME:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRINTER_NAME:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SHARE_NAME:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PORT_NAME:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DRIVER_NAME:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_COMMENT:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_LOCATION:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DEVMODE:
                            Value = data.pBuf;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SEPFILE:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRINT_PROCESSOR:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PARAMETERS:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DATATYPE:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SECURITY_DESCRIPTOR:
                            Value = data.pBuf;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_ATTRIBUTES:
                            Value = data.adwData0;
                            break;

                        // In my tests although priority is monitored it's value isn't available via this field
                        // It should be possible to pick it up in the devmode though.
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRIORITY:
                            Value = data.adwData0;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DEFAULT_PRIORITY:
                            Value = data.adwData0;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_START_TIME:
                            Value = data.adwData0;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_UNTIL_TIME:
                            Value = data.adwData0;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_STATUS:
                            Value = data.adwData0;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_STATUS_STRING:
                            // Not supported
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_CJOBS:
                            Value = data.adwData0;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_AVERAGE_PPM:
                            Value = data.adwData0;
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
                            Value = true;
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_FRIENDLY_NAME:
                            // Windows Vista and later
                            // Not particularly well documented
                            Value = true; // Probably should be a string
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
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_MACHINE_NAME:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PORT_NAME:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_USER_NAME:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_NOTIFY_NAME:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DATATYPE:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRINT_PROCESSOR:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PARAMETERS:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DRIVER_NAME:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DEVMODE:
                            Value = data.pBuf;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_STATUS:
                            Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_STATUS_STRING:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_SECURITY_DESCRIPTOR:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DOCUMENT:
                            Value = Marshal.PtrToStringUni(data.pBuf);
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRIORITY:
                            Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_POSITION:
                            Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_SUBMITTED:
                            Value = data.pBuf;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_START_TIME:
                            Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_UNTIL_TIME:
                            Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TIME:
                            Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TOTAL_PAGES:
                            Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PAGES_PRINTED:
                            Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TOTAL_BYTES:
                            Value = data.adwData0;
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_BYTES_PRINTED:
                            Value = data.adwData0;
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
        }

        public static int SizeOf()
        {
            return Marshal.SizeOf<PRINTER_NOTIFY_INFO_DATA>();
        }
    }
}