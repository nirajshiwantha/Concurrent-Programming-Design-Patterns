using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace concurrentDesignPatterns.DesignPatterns.Reader_Writer_Pattern;
internal class Reader_Writer_Pattern_sample_2
{
    //static async Task Main()
    //{
    //    DataRepository repository = new DataRepository();

    //    Task readerTask1 = ReaderAsync(repository);
    //    Task readerTask2 = ReaderAsync(repository);
    //    Task writerTask = WriterAsync(repository);

    //    // Wait for all tasks to complete
    //    await Task.WhenAll(readerTask1, readerTask2, writerTask);

    //    // Display the final database state
    //    // Using a separate read operation to showcase the repository's encapsulation
    //    await repository.ReadDataAsync(); 
    //}

    static async Task ReaderAsync(DataRepository repository)
    {
        for (int i = 0; i < 3; i++)
        {
            await repository.ReadDataAsync();
            await Task.Delay(100); // Simulating reading time asynchronously
        }
    }

    static async Task WriterAsync(DataRepository repository)
    {
        for (int i = 0; i < 3; i++)
        {
            int dataToAdd = i + 1;
            await repository.WriteDataAsync(dataToAdd);
            await Task.Delay(150); // Simulating writing time asynchronously
        }
    }
}
