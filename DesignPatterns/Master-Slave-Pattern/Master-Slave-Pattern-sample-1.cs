using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace concurrentDesignPatterns.DesignPatterns.Master_Slave_Pattern;
internal class Master_Slave_Pattern_sample_1
{
    //static void Main()
    //{
    //    List<int> dataset = GenerateLargeDataset();
    //    MasterProcess(dataset);
    //}
    static List<int> GenerateLargeDataset()
    {
        // Simulating a large dataset generation
        List<int> dataset = new List<int>();
        for (int i = 1; i <= 1000000; i++)
        {
            dataset.Add(i);
        }
        return dataset;
    }
    static void MasterProcess(List<int> dataset)
    {
        // Divide the dataset into segments
        int segmentSize = dataset.Count / Environment.ProcessorCount;
        List<Task<long>> tasks = new List<Task<long>>();
        for (int i = 0; i < Environment.ProcessorCount; i++)
        {
            int start = i * segmentSize;
            int end = (i == Environment.ProcessorCount - 1) ? dataset.Count : (i + 1) * segmentSize;
            List<int> segment = dataset.GetRange(start, end - start);
            // Assign each segment to a slave for parallel processing
            tasks.Add(Task.Run(() => SlaveProcess(segment)));
        }
        // Wait for all tasks to complete and aggregate the results
        Task.WaitAll(tasks.ToArray());
        long totalResult = tasks.Sum(t => t.Result);
        Console.WriteLine($"Master Process Result: {totalResult}");
    }
    static long SlaveProcess(List<int> segment)
    {
        // Simulate parallel processing of a dataset segment
        long result = segment.Sum(x => (long)x);
        Console.WriteLine($"Slave Process Result: {result}");
        return result;
    }
}
