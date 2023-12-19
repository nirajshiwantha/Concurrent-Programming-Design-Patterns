using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace concurrentDesignPatterns.DesignPatterns.Fork_Join_Pattern;

internal class Fork_Join_Pattern_sample_2
{
    //static void Main()
    //{
    //    // Creating financial data for 20 branches
    //    List<BranchFinancials> branchFinancialsList = GenerateBranchFinancials(20);

    //    // Calculating total revenue using the Fork-Join pattern
    //    long totalRevenue = CalculateTotalRevenue(branchFinancialsList);

    //    Console.WriteLine($"Total Revenue: {totalRevenue}");
    //}

    static List<BranchFinancials> GenerateBranchFinancials(int numBranches)
    {
        List<BranchFinancials> branchFinancialsList = new List<BranchFinancials>();

        // Manually specifying monthly revenue for each fixed branch
        for (int i = 1; i <= numBranches; i++)
        {
            long monthlyRevenue = i * 10000; // Replace with your specific monthly revenue values
            branchFinancialsList.Add(new BranchFinancials { BranchId = i, MonthlyRevenue = monthlyRevenue });
        }

        return branchFinancialsList;
    }

    static long CalculateTotalRevenue(List<BranchFinancials> branchFinancialsList)
    {
        // Creating a concurrent accumulator for total revenue
        ConcurrentLong totalRevenue = new ConcurrentLong();

        // Dividing the branch financials into segments for parallel processing
        int segmentSize = Math.Max(1, branchFinancialsList.Count / Environment.ProcessorCount);
        Parallel.ForEach(Partitioner.Create(0, branchFinancialsList.Count, segmentSize),
            range =>
            {
                // Local accumulator for each task
                long localSum = 0;

                for (int i = range.Item1; i < range.Item2; i++)
                {
                    // Accumulating the monthly revenue for each branch
                    localSum += branchFinancialsList[i].MonthlyRevenue;
                }

                // Adding the local sum to the total revenue accumulator
                totalRevenue.Add(localSum);

                // Printing the local sum for each task
                int taskId = Task.CurrentId ?? -1;
                Console.WriteLine($"Task {taskId}: Local Sum = {localSum}");
            });

        return totalRevenue.Value;
    }

}

class BranchFinancials
{
    public int BranchId { get; set; }
    public long MonthlyRevenue { get; set; }
}

class ConcurrentLong
{
    private long value = 0;
    private readonly object lockObject = new object();

    public void Add(long amount)
    {
        lock (lockObject)
        {
            value += amount;
        }
    }

    public long Value
    {
        get
        {
            lock (lockObject)
            {
                return value;
            }
        }
    }
}