using NUnit.Framework;
using PrinterChangeNotification;
using PrinterChangeNotification.enums;
using Tests.Support;

namespace Tests
{
    [TestFixture]
    internal class PrinterChangeTests
    {
        [Test]
        public void SetPrinter()
        {
            using var changeNotification = ChangeNotification.Create(NameConstants.PrinterName, PRINTER_CHANGE.PRINTER_CHANGE_PRINTER, PRINTER_NOTIFY_CATEGORY.PRINTER_NOTIFY_CATEGORY_ALL, null);

            // Make sure changeNotification isn't signaled before we start
            Assert.That(changeNotification.WaitHandle.WaitOne(0), Is.False);
            
            // If we don't close the printer handle before calling WaitOne then WaitOne can block indefinitely
            using (var printer = new SafePrinter(NameConstants.PrinterName))
            {
                // This should trigger a change
                var pi2 = printer.GetPrinter();
                pi2.pComment = "A printer comment";
                printer.SetPrinter(pi2, 0);
            }

            changeNotification.WaitHandle.WaitOne();

            var change = changeNotification.FindNextPrinterChangeNotification(false);
            Assert.That(change.Change == PRINTER_CHANGE.PRINTER_CHANGE_SET_PRINTER);
        }
    }
}
