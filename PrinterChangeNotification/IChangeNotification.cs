using System;

namespace PrinterChangeNotification
{
    public interface IChangeNotification : IDisposable
    {
        bool WaitOne();
        NotifyInfo FindNextPrinterChangeNotification(bool refresh);
    }
}
