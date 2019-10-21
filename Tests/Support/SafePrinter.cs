using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Tests.Support
{
    internal class SafePrinter : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafePrinter(string printerName) : base(true)
        {
            PRINTER_DEFAULTS defaults = default;
            defaults.DesiredPrinterAccess = PRINTER_ACCESS_MASK.PRINTER_ALL_ACCESS;

            if (NativeMethods.OpenPrinter(printerName, out var printerHandle, ref defaults) == 0)
            {
                throw new Win32Exception();
            }

            handle = printerHandle;
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.ClosePrinter(handle);
        }

        public void Delete()
        {
            if (NativeMethods.DeletePrinter(handle) == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        internal static void AddPrinter(string printerName, string portName, string driverName)
        {
            var pi2 = new PRINTER_INFO_2
            {
                pPrinterName = printerName,
                pPortName = portName,
                pDriverName = driverName,
            };

            var handle = NativeMethods.AddPrinter(null, 2, ref pi2);
            if (handle == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            NativeMethods.ClosePrinter(handle);
        }

        public static void AddPort(string portName)
        {
            var pi = new PORT_INFO_1 { Name = portName };

            if (NativeMethods.AddPortEx(null, 1, ref pi, "Local Port") == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            Debug.WriteLine($"Port '{portName}' added.");
        }

        public static void DeletePort(string portName)
        {
            if (NativeMethods.DeletePort(null, IntPtr.Zero, portName) == 0)
            {
                throw new Win32Exception();
            }

            Debug.WriteLine($"Port '{portName}' deleted.");
        }

        public PRINTER_INFO_2 GetPrinter()
        {
            if (NativeMethods.GetPrinter(handle, 2, IntPtr.Zero, 0, out var cbNeeded) != 0)
            {
                throw new ApplicationException("Unexpected success in GetPrinter");
            }

            //ERROR_INSUFFICIENT_BUFFER = 122 expected, if not -> Exception
            if (Marshal.GetLastWin32Error() != 122)
            {
                throw new Win32Exception();
            }

            var address = Marshal.AllocHGlobal(cbNeeded);
            try
            {
                if (NativeMethods.GetPrinter(handle, 2, address, cbNeeded, out cbNeeded) == 0)
                {
                    throw new Win32Exception();
                }

                var result = Marshal.PtrToStructure<PRINTER_INFO_2>(address);

                // As we don't retain the allocated memory remove the pointers to that memory.
                result.pSecurityDescriptor = IntPtr.Zero;
                result.pDevMode = IntPtr.Zero;

                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(address);
            }
        }

        public void SetPrinter(PRINTER_INFO_2 printerInfo2, int command)
        {
            var size = Marshal.SizeOf<PRINTER_INFO_2>();
            var address = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.StructureToPtr(printerInfo2, address, false);

                if (NativeMethods.SetPrinter(handle, 2, address, command) == 0)
                {
                    throw new Win32Exception();
                }
            }
            finally
            {
                Marshal.FreeHGlobal(address);
            }
        }


        public static IEnumerable<PRINTER_INFO_2> EnumPrinters()
        {
            uint needed = 0;
            uint returned = 0;

            const PrinterEnumFlags flags = PrinterEnumFlags.PRINTER_ENUM_LOCAL;

            if (NativeMethods.EnumPrinters(flags, null, 2, IntPtr.Zero, 0, ref needed, ref returned) != 0)
            {
                // This call to EnumPrinters should only succeed if there are no printers defined
                yield break;
            }

            //ERROR_INSUFFICIENT_BUFFER = 122 expected, if not -> Exception
            if (Marshal.GetLastWin32Error() != 122)
            {
                throw new Win32Exception();
            }

            var ptr = Marshal.AllocHGlobal((int)needed);
            try
            {
                if (NativeMethods.EnumPrinters(flags, null, 2, ptr, needed, ref needed, ref returned) == 0)
                {
                    throw new Win32Exception();
                }

                var offset = ptr;

                for (int i = 0; i < returned; i++)
                {
                    yield return Marshal.PtrToStructure<PRINTER_INFO_2>(offset);
                    offset += Marshal.SizeOf<PRINTER_INFO_2>();
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static IEnumerable<PORT_INFO_1> EnumPorts()
        {
            uint needed = 0;
            uint returned = 0;

            if (NativeMethods.EnumPorts(null, 1, IntPtr.Zero, 0, ref needed, ref returned) != 0)
            {
                throw new ApplicationException("Unexpected success enumerating ports, are there really no ports defined");
            }

            //ERROR_INSUFFICIENT_BUFFER = 122 expected, if not -> Exception
            if (Marshal.GetLastWin32Error() != 122)
            {
                throw new Win32Exception();
            }

            var ptr = Marshal.AllocHGlobal((int)needed);
            try
            {
                if (NativeMethods.EnumPorts(null, 1, ptr, needed, ref needed, ref returned) == 0)
                {
                    throw new Win32Exception();
                }

                var offset = ptr;
                var type = typeof(PORT_INFO_1);

                for (int i = 0; i < returned; i++)
                {
                    yield return (PORT_INFO_1)Marshal.PtrToStructure(offset, type);
                    offset += Marshal.SizeOf(type);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}