using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NUnit.Framework;
using PrinterChangeNotification.structs;

namespace Tests
{
    [TestFixture]
    public class StructSizes
    {
        [SetUp]
        public void SetUp()
        {
            Debug.Print(Environment.Is64BitProcess ? "64 Bit" : "32 Bit");
        }

        [Test]
        public void IntPtr()
        {
            var size = Marshal.SizeOf<IntPtr>();
            Assert.That(size, Is.EqualTo(Environment.Is64BitProcess ? 8 : 4));
        }

        [Test]
        [Platform(Include = "32-Bit")]
        public void PRINTER_NOTIFY_INFO_32()
        {
            var size = Marshal.SizeOf<PRINTER_NOTIFY_INFO_32>();
            Assert.That(size, Is.EqualTo(Environment.Is64BitProcess ? 48 : 32));
        }

        [Test]
        [Platform(Include = "64-Bit")]
        public void PRINTER_NOTIFY_INFO_64()
        {
            var size = Marshal.SizeOf<PRINTER_NOTIFY_INFO_64>();
            Assert.That(size, Is.EqualTo(48));
        }

        [Test]
        [Platform(Include = "32-Bit")]
        public void PRINTER_NOTIFY_INFO_DATA_32()
        {
            var size = Marshal.SizeOf<PRINTER_NOTIFY_INFO_DATA_32>();
            Assert.That(size, Is.EqualTo(20));
        }

        [Test]
        [Platform(Include = "64-Bit")]
        public void PRINTER_NOTIFY_INFO_DATA_64()
        {
            var size = Marshal.SizeOf<PRINTER_NOTIFY_INFO_DATA_64>();
            Assert.That(size, Is.EqualTo(32));
        }

        [Test]
        public void PRINTER_NOTIFY_OPTIONS()
        {
            var size = Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS>();
            Assert.That(size, Is.EqualTo(Environment.Is64BitProcess ? 24 : 16));
        }

        [Test]
        public void PRINTER_NOTIFY_OPTIONS_TYPE()
        {
            var size = Marshal.SizeOf<PRINTER_NOTIFY_OPTIONS_TYPE>();
            Assert.That(size, Is.EqualTo(Environment.Is64BitProcess ? 24 : 20));
        }
    }
}
