using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace concurrentDesignPatterns.DesignPatterns.Reader_Writer_Pattern;
public class DataRepository
{
    private readonly List<int> database = new List<int>();
    private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

    public async Task ReadDataAsync()
    {
        await Task.Run(() =>
        {
            lock (database)
            {
                Console.WriteLine($"Reader Thread {Thread.CurrentThread.ManagedThreadId}: Reading from database - {string.Join(", ", database)}");
            }
        });
    }

    public async Task WriteDataAsync(int dataToAdd)
    {
        await semaphore.WaitAsync();
        try
        {
            await Task.Run(() =>
            {
                lock (database)
                {
                    database.Add(dataToAdd);
                    Console.WriteLine($"Writer Thread {Thread.CurrentThread.ManagedThreadId}: Writing to database - Added {dataToAdd}");
                }
            });
        }
        finally
        {
            semaphore.Release();
        }
    }
}
