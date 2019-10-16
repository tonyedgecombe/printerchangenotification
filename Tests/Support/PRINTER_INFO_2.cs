using System;
using System.Runtime.InteropServices;

namespace Tests.Support
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct PRINTER_INFO_2
    {
        [MarshalAs(UnmanagedType.LPTStr)] private string pServerName;

        [MarshalAs(UnmanagedType.LPTStr)] public string pPrinterName;

        [MarshalAs(UnmanagedType.LPTStr)] private string pShareName;

        [MarshalAs(UnmanagedType.LPTStr)] public string pPortName;

        [MarshalAs(UnmanagedType.LPTStr)] public string pDriverName;

        [MarshalAs(UnmanagedType.LPTStr)] private string pComment;

        [MarshalAs(UnmanagedType.LPTStr)] private string pLocation;

        public IntPtr pDevMode;

        [MarshalAs(UnmanagedType.LPTStr)] private string pSepFile;

        [MarshalAs(UnmanagedType.LPTStr)] private string pPrintProcessor;

        [MarshalAs(UnmanagedType.LPTStr)] private string pDatatype;

        [MarshalAs(UnmanagedType.LPTStr)] private string pParameters;

        public IntPtr pSecurityDescriptor;

        public uint Attributes;

        public uint Priority;

        public uint DefaultPriority;

        public uint StartTime;

        public uint UntilTime;

        public uint Status;

        public uint cJobs;

        public uint AveragePPM;
    }
}