using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PrinterChangeNotification.enums;
// ReSharper disable InconsistentNaming

namespace PrinterChangeNotification
{
    public class PrinterNotifyInfoData
    {
        [StructLayout(LayoutKind.Explicit)]
        private struct PRINTER_NOTIFY_INFO_DATA
        {
            [FieldOffset(0)]
            public readonly UInt16 Type;

            [FieldOffset(2)]
            public readonly UInt16 Field;

            [FieldOffset(4)]
            private readonly UInt32 Reserved;

            [FieldOffset(8)]
            public readonly UInt32 Id;

            [FieldOffset(12)]
            private readonly UInt32 adwData0;

            [FieldOffset(16)]
            private readonly UInt32 adwData1;

            [FieldOffset(12)]
            private readonly UInt32 cbBuf;

            [FieldOffset(16)]
            private readonly IntPtr pBuf;
        }

        public UInt16 Type { get; }
        public UInt16 Field { get; }

        public UInt32 Id { get; }

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
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRINTER_NAME:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SHARE_NAME:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PORT_NAME:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DRIVER_NAME:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_COMMENT:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_LOCATION:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DEVMODE:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SEPFILE:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRINT_PROCESSOR:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PARAMETERS:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DATATYPE:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_SECURITY_DESCRIPTOR:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_ATTRIBUTES:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRIORITY:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_DEFAULT_PRIORITY:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_START_TIME:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_UNTIL_TIME:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_STATUS:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_STATUS_STRING:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_CJOBS:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_AVERAGE_PPM:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_TOTAL_PAGES:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PAGES_PRINTED:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_TOTAL_BYTES:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_BYTES_PRINTED:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_OBJECT_GUID:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_FRIENDLY_NAME:
                            break;
                        case PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_BRANCH_OFFICE_PRINTING:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case NOTIFY_TYPE.JOB_NOTIFY_TYPE:
                    switch ((JOB_NOTIFY_FIELD)data.Field)
                    {
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRINTER_NAME:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_MACHINE_NAME:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PORT_NAME:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_USER_NAME:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_NOTIFY_NAME:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DATATYPE:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRINT_PROCESSOR:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PARAMETERS:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DRIVER_NAME:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DEVMODE:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_STATUS:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_STATUS_STRING:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_SECURITY_DESCRIPTOR:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_DOCUMENT:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRIORITY:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_POSITION:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_SUBMITTED:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_START_TIME:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_UNTIL_TIME:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TIME:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TOTAL_PAGES:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PAGES_PRINTED:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_TOTAL_BYTES:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_BYTES_PRINTED:
                            break;
                        case JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_REMOTE_JOB_ID:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case NOTIFY_TYPE.SERVER_NOTIFY_TYPE:
                    throw new NotImplementedException("Server notify type in header file but not documented so I haven't implemented it.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static int SizeOf()
        {
            return Marshal.SizeOf<PRINTER_NOTIFY_INFO_DATA>();
        }
    }
    public class PrinterNotifyInfo
    {
        public PRINTER_CHANGE Change { get; }

        public List<PrinterNotifyInfoData> Data { get; } = new List<PrinterNotifyInfoData>();

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
                    Data.Add(new PrinterNotifyInfoData(pos));

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