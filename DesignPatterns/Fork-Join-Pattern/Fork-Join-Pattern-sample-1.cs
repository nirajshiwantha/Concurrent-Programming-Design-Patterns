using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace concurrentDesignPatterns.DesignPatterns.Fork_Join_Pattern;

internal class Fork_Join_Pattern_sample_1
{
    private static object lockObject = new object();
    //static void Main()
    //{
    //    int[] numbers = Enumerable.Range(1, 1000000).ToArray();
    //    long totalSum = ParallelSum(numbers);
    //    Console.WriteLine($"Total Sum: {totalSum}");
    //}
    static long ParallelSum(int[] numbers)
    {
        long totalSum = 0;
        Parallel.ForEach(numbers, new ParallelOptions { MaxDegreeOfParallelism = 4 }, () => 0L, (num, loopState, localSum) =>
        {
            
            localSum += num;
            return localSum;
        }, localSum =>
        {
            lock (lockObject)
            {
                int threadId = Task.CurrentId ?? -1;
                Console.WriteLine($"Thread {threadId}: Local Sum = {localSum}");
                totalSum += localSum;
            }
        });
        return totalSum;
    }
}
