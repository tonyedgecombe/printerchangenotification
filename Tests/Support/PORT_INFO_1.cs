using System.Runtime.InteropServices;

namespace Tests.Support
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PORT_INFO_1
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;
    }
}