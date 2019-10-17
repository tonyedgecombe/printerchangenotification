using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using NUnit.Framework;
using PrinterChangeNotification;
using PrinterChangeNotification.enums;

namespace Tests
{
    [TestFixture]
    internal class JobNotifyOptionsTest
    {
        [Test]
        public void AddJob()
        {
            var options = new NotifyOptions
            {
                Types = new List<NotifyOptionsType>
                {
                    new NotifyOptionsType
                    {
                        Fields = new List<ushort> {(ushort) JOB_NOTIFY_FIELD.JOB_NOTIFY_FIELD_PRINTER_NAME},
                        Type = NOTIFY_TYPE.JOB_NOTIFY_TYPE,
                    }
                }
            };

            using var changeNotification = ChangeNotification.Create(NameConstants.PrinterName, 0, PRINTER_NOTIFY_CATEGORY.PRINTER_NOTIFY_CATEGORY_ALL, options);

            using var localPrintServer = new LocalPrintServer();
            var defaultPrintQueue = localPrintServer.GetPrintQueue(NameConstants.PrinterName);

            var myPrintJob = defaultPrintQueue.AddJob();

            // Write a Byte buffer to the JobStream and close the stream
            var myByteBuffer = Encoding.Unicode.GetBytes("This is a test string for the print job stream.");

            using var myStream = myPrintJob.JobStream;
            myStream.Write(myByteBuffer, 0, myByteBuffer.Length);
            myStream.Close();

            using var myStream2 = myPrintJob.JobStream;
            myStream2.Write(myByteBuffer, 0, myByteBuffer.Length);
            myStream2.Close();

            changeNotification.WaitHandle.WaitOne();

            var changes = changeNotification.FindNextPrinterChangeNotification(false);

            Assert.That(changes.Data.Count, Is.EqualTo(1));
            Assert.That(changes.Data.First().Value, Is.EqualTo(NameConstants.PrinterName));
        }
    }
}