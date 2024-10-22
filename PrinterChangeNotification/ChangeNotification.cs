﻿using System;
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
    public class ChangeNotification : WaitHandle, IChangeNotification
    {
        private static readonly UInt32 PRINTER_NOTIFY_OPTIONS_REFRESH = 0x01;
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        private IntPtr _printerHandle = IntPtr.Zero;
        private IntPtr _changeHandle = INVALID_HANDLE_VALUE;
        private bool disposed;

        public WaitHandle WaitHandle => this;

        private ChangeNotification()
        {
        }


        public static IChangeNotification Create(PRINTER_CHANGE changes,
                                                    string printerName = null,
                                                    PRINTER_NOTIFY_CATEGORY category = PRINTER_NOTIFY_CATEGORY.PRINTER_NOTIFY_CATEGORY_ALL,
                                                    NotifyOptions options = null)
        {
            var notification = new ChangeNotification
            {
                _printerHandle = OpenPrinter(printerName)
            };

            var ptrNotifyOptions = IntPtr.Zero;

            try
            {
                if (options != null)
                {
                    var size = SizeOfNotifyOptions(options);
                    ptrNotifyOptions = Marshal.AllocHGlobal(size);
                    ToPtr(ptrNotifyOptions, options);
                }

                notification._changeHandle = NativeMethods.FindFirstPrinterChangeNotification(
                    notification._printerHandle,
                    (UInt32) changes,
                    (UInt32) category,
                    ptrNotifyOptions);
                if (notification._changeHandle == INVALID_HANDLE_VALUE)
                {
                    throw new Win32Exception();
                }

                // Don't let SafeWaitHandle own the handle as it can't close it
                notification.SafeWaitHandle = new SafeWaitHandle(notification._changeHandle, false);
            }
            catch
            {
                NativeMethods.ClosePrinter(notification._printerHandle);
                throw;
            }
            finally
            {
                Marshal.FreeHGlobal(ptrNotifyOptions);
            }

            return notification;
        }

        public NotifyInfo FindNextPrinterChangeNotification(bool refresh)
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

                if (!NativeMethods.FindNextPrinterChangeNotification(_changeHandle, out var change, pOptions, ppPrinterNotifyInfo))
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

        private static IntPtr OpenPrinter(string printerName)
        {
            if (!NativeMethods.OpenPrinter(printerName, out var printerHandle, IntPtr.Zero))
            {
                throw new Win32Exception();
            }

            return printerHandle;
        }

        private static NotifyInfo CreatePrinterNotifyInfo(uint change, IntPtr ptr)
        {
            return Environment.Is64BitProcess ?
                CreatePrinterNotifyInfo64(change, ptr):
                CreatePrinterNotifyInfo32(change, ptr);
        }

        private static NotifyInfo CreatePrinterNotifyInfo32(uint change, IntPtr ptr)
        {
            var result = new NotifyInfo
            {
                Change = (PRINTER_CHANGE) change
            };

            if (ptr == IntPtr.Zero)
            {
                return result;
            }

            var notifyInfo = Marshal.PtrToStructure<PRINTER_NOTIFY_INFO_32>(ptr);

            var pos = ptr + 12; // pointer to aData

            for (int i = 0; i < notifyInfo.Count; i++)
            {
                var info = CreatePrinterNotifyInfoData(pos);
                result.Data.Add(info);

                pos += Marshal.SizeOf<PRINTER_NOTIFY_INFO_DATA_32>();
            }

            result.Flags = notifyInfo.Flags;

            return result;
        }
        private static NotifyInfo CreatePrinterNotifyInfo64(uint change, IntPtr ptr)
        {
            var result = new NotifyInfo
            {
                Change = (PRINTER_CHANGE) change
            };

            if (ptr == IntPtr.Zero)
            {
                return result;
            }

            var notifyInfo = Marshal.PtrToStructure<PRINTER_NOTIFY_INFO_64>(ptr);

            var pos = ptr + 16; // pointer to aData

            for (int i = 0; i < notifyInfo.Count; i++)
            {
                var info = CreatePrinterNotifyInfoData(pos);
                result.Data.Add(info);

                pos += Marshal.SizeOf<PRINTER_NOTIFY_INFO_DATA_64>();
            }

            result.Flags = notifyInfo.Flags;

            return result;
        }

        private static NotifyInfoData CreatePrinterNotifyInfoData(IntPtr ptr)
        {
            return Environment.Is64BitProcess ? CreatePrinterNotifyInfoData64(ptr) : CreatePrinterNotifyInfoData32(ptr);
        }

        private static NotifyInfoData CreatePrinterNotifyInfoData32(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return new NotifyInfoData();
            }

            var data = Marshal.PtrToStructure<PRINTER_NOTIFY_INFO_DATA_32>(ptr);

            return new NotifyInfoData
            {
                Type = data.Type,
                Field = data.Field,
                Id = data.Id,
                Value = GetNotifyFieldValue(data),
            };
        }

        private static NotifyInfoData CreatePrinterNotifyInfoData64(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return new NotifyInfoData();
            }

            var data = Marshal.PtrToStructure<PRINTER_NOTIFY_INFO_DATA_64>(ptr);

            return new NotifyInfoData
            {
                Type = data.Type,
                Field = data.Field,
                Id = data.Id,
                Value = GetNotifyFieldValue(data),
            };
        }

        private static dynamic GetNotifyFieldValue(PRINTER_NOTIFY_INFO_DATA_32 data)
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

        private static dynamic GetNotifyFieldValue(PRINTER_NOTIFY_INFO_DATA_64 data)
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

        private static dynamic GetJobNotifyFieldValue(PRINTER_NOTIFY_INFO_DATA_32 data)
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

        private static dynamic GetJobNotifyFieldValue(PRINTER_NOTIFY_INFO_DATA_64 data)
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

        private static dynamic GetPrinterNotifyFieldValue(PRINTER_NOTIFY_INFO_DATA_32 data)
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

        private static dynamic GetPrinterNotifyFieldValue(PRINTER_NOTIFY_INFO_DATA_64 data)
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


        private static void ToPtr(IntPtr pos, NotifyOptions options)
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
                    Marshal.WriteInt32(fieldsPos, field);
                    fieldsPos += Marshal.SizeOf(field.GetType());
                }
            }
        }

        private static int SizeOfNotifyOptions(NotifyOptions options)
        {
            var size = Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS>();
            foreach (var printerNotifyOptionsType in options.Types)
            {
                size += Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS_TYPE>();
                size += printerNotifyOptionsType.Fields.Count * Marshal.SizeOf<UInt32>();
            }

            return size;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // No managed resources to dispose of.
                }

                // Dispose of unmanaged resources
                if (_changeHandle != INVALID_HANDLE_VALUE)
                {
                    NativeMethods.FindClosePrinterChangeNotification(_changeHandle);
                }

                if (_printerHandle != IntPtr.Zero)
                {
                    NativeMethods.ClosePrinter(_printerHandle);
                }

                disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}