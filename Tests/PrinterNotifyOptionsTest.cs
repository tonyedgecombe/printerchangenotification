using System;
using System.Collections.Generic;
using NUnit.Framework;
using PrinterChangeNotification;
using PrinterChangeNotification.enums;
using Tests.Support;

namespace Tests
{
    [TestFixture()]
    class PrinterNotifyOptionsTest
    {
        [Test]
        public void Comment()
        {
            // Use a random comment then we don't have t worry about it being set from a previous run.
            var randomComment = new Random((int) new DateTime().Ticks).ToString();

            var options = new NotifyOptions
            {
                Types = new List<NotifyOptionsType>
                {
                    new NotifyOptionsType()
                    {
                        Fields = new List<ushort> {(ushort) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_COMMENT},
                        Type = NOTIFY_TYPE.PRINTER_NOTIFY_TYPE,
                    }
                }
            };

            using var changeNotification = ChangeNotification.Create(NameConstants.PrinterName, 0, PRINTER_NOTIFY_CATEGORY.PRINTER_NOTIFY_CATEGORY_ALL, options);

            // If we don't close the printer handle before calling WaitOne then WaitOne can block indefinitely
            using (var printer = new SafePrinter(NameConstants.PrinterName))
            {
                // This should trigger a change
                var pi2 = printer.GetPrinter();
                pi2.pComment = randomComment;
                printer.SetPrinter(pi2, 0);
            }

            changeNotification.WaitHandle.WaitOne();

            var change = changeNotification.FindNextPrinterChangeNotification(false);
            
            Assert.That(change.Change == 0); // We didn't request change monitoring

            Assert.That(change.Data.Count, Is.EqualTo(1));
            Assert.That(change.Data[0].Value, Is.EqualTo(randomComment));
        }
    }
}
