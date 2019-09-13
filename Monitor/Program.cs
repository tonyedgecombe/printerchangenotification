using System;

namespace Monitor
{
    class Program
    {
        static void Main()
        {
            using (var printer = new Printer(null))
            using (var printerChangeNotification = new PrinterChangeNotification.PrinterChangeNotification(printer))
            using (var waitHandle = printerChangeNotification.WaitHandle)
            {
                while (true)
                {
                    Console.WriteLine("Waiting");

                    waitHandle.WaitOne();
                    printerChangeNotification.FindNextChangeNotification();

                    Console.WriteLine("Notification received");
                }
            }
        }
    }
}
