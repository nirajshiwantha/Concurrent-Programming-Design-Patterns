namespace concurrentDesignPatterns.DesignPatterns;
public class Producer_Consumer_Pattern_sample_1
{
    static Queue<int> sharedQueue = new Queue<int>();
    static object lockObject = new object();
    static void Producer()
    {
        for (int i = 0; i < 5; i++)
        {
            lock (lockObject)
            {
                sharedQueue.Enqueue(i);
                Console.WriteLine($"Produced: {i}");
            }
            Thread.Sleep(100);
        }
    }
    static void Consumer()
    {
        for (int i = 0; i < 5; i++)
        {
            lock (lockObject)
            {
                if (sharedQueue.Count > 0)
                {
                    int item = sharedQueue.Dequeue();
                    Console.WriteLine($"Consumed: {item}");
                }
            }
            Thread.Sleep(150);
        }
    }

    //static void Main()
    //{
    //    Thread producerThread = new Thread(Producer);
    //    Thread consumerThread = new Thread(Consumer);
    //    producerThread.Start();
    //    consumerThread.Start();
    //    producerThread.Join();
    //    consumerThread.Join();
    //}
}
