using System;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace PrinterChangeNotification
{
    internal class PrinterChangeNotificationWaitHandle : WaitHandle
    {
        public PrinterChangeNotificationWaitHandle(IntPtr handle)
        {
            SafeWaitHandle = new SafeWaitHandle(handle, false);
        }
    }
}