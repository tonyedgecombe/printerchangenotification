using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PrinterChangeNotification;
using PrinterChangeNotification.enums;
using Tests.Support;

namespace Tests
{
    [TestFixture]
    class PrinterNotifyOptionsTest
    {
        public void Comment()
        {
            // Use a random comment then we don't have t worry about it being set from a previous run.
            var randomComment = DateTime.Now.Ticks.ToString();

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

            using var changeNotification = ChangeNotification.Create(0, NameConstants.PrinterName, PRINTER_NOTIFY_CATEGORY.PRINTER_NOTIFY_CATEGORY_ALL, options);

            // If we don't close the printer handle before calling WaitOne then WaitOne can block indefinitely
            using (var printer = new SafePrinter(NameConstants.PrinterName))
            {
                // This should trigger a change
                var pi2 = printer.GetPrinter();
                pi2.pComment = randomComment;
                pi2.pSecurityDescriptor = IntPtr.Zero;
                printer.SetPrinter(pi2, 0);
            }

            changeNotification.WaitHandle.WaitOne();

            var change = changeNotification.FindNextPrinterChangeNotification(false);
            
            Assert.That(change.Change == 0); // We didn't request change monitoring

            Assert.That(change.Data.Count, Is.EqualTo(1));
            Assert.That(change.Data[0].Value, Is.EqualTo(randomComment));
        }

        [Test]
        public void MultipleFields()
        {
            // Use a random comment then we don't have t worry about it being set from a previous run.
            var randomComment = DateTime.Now.Ticks.ToString();
            uint priority;

            var options = new NotifyOptions
            {
                Types = new List<NotifyOptionsType>
                {
                    new NotifyOptionsType()
                    {
                        Fields = new List<ushort>
                        {
                            (ushort) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_COMMENT,
                            (ushort) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_LOCATION,
                            (ushort) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRIORITY,
                        },
                        Type = NOTIFY_TYPE.PRINTER_NOTIFY_TYPE,
                    }
                }
            };

            using var changeNotification = ChangeNotification.Create(0, NameConstants.PrinterName, PRINTER_NOTIFY_CATEGORY.PRINTER_NOTIFY_CATEGORY_ALL, options);

            // If we don't close the printer handle before calling WaitOne then WaitOne can block indefinitely
            using (var printer = new SafePrinter(NameConstants.PrinterName))
            {
                // This should trigger a change
                var pi2 = printer.GetPrinter();
                pi2.pComment = randomComment + "Comment";
                pi2.pLocation = randomComment + "Location";
                priority = ++pi2.Priority & 0xFF;
                printer.SetPrinter(pi2, 0);
            }

            changeNotification.WaitHandle.WaitOne();

            var change = changeNotification.FindNextPrinterChangeNotification(true);
            
            Assert.That(change.Change == 0); // We didn't request change monitoring

            Assert.That(change.Data.Count, Is.EqualTo(3));

            var comment = change.Data.First(d => d.Field == (int) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_COMMENT).Value;
            Assert.That(comment, Is.EqualTo(randomComment + "Comment"));

            var location = change.Data.First(d => d.Field == (int) PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_LOCATION).Value;
            Assert.That(location, Is.EqualTo(randomComment + "Location"));

            var newPriority = change.Data.First(d => d.Field == (int)PRINTER_NOTIFY_FIELD.PRINTER_NOTIFY_FIELD_PRIORITY).Value;
            Assert.That(newPriority, Is.EqualTo(priority));
        }
    }
}
