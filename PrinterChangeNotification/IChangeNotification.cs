using System;
using System.Threading;

namespace PrinterChangeNotification
{
    public interface IChangeNotification : IDisposable
    {
        WaitHandle WaitHandle { get; }
        NotifyInfo FindNextPrinterChangeNotification(bool refresh);
    }
}
