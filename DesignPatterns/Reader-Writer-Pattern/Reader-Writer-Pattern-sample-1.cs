using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace concurrentDesignPatterns.DesignPatterns.Reader_Writer_Pattern;

internal class Reader_Writer_Pattern_sample_1
{
    static int sharedData = 0;
    static readonly object lockObject = new object();
    static void Reader()
    {
        for (int i = 0; i < 3; i++)
        {
            lock (lockObject)
            {
                Console.WriteLine($"Reader: {sharedData}");
            }
            Thread.Sleep(100); // Simulating reading time
        }
    }
    static void Writer()
    {
        for (int i = 0; i < 3; i++)
        {
            lock (lockObject)
            {
                sharedData++;
                Console.WriteLine($"Writer: {sharedData}");
            }
            Thread.Sleep(150); // Simulating writing time
        }
    }
    //static void Main()
    //{
    //    Thread readerThread1 = new Thread(Reader);
    //    Thread readerThread2 = new Thread(Reader);
    //    Thread writerThread = new Thread(Writer);
    //    readerThread1.Start();
    //    readerThread2.Start();
    //    writerThread.Start();
    //    readerThread1.Join();
    //    readerThread2.Join();
    //    writerThread.Join();
    //}
}
