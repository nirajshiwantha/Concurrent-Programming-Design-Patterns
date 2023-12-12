namespace concurrentDesignPatterns.DesignPatterns;

public class Producer_Consumer_Pattern_sample_2
{
    static Queue<string> logQueue = new Queue<string>();
    static object lockObject = new object();
    static bool isLoggingCompleted = false;

    //static async Task Main()
    //{
    //    // Start a logger consumer asynchronously
    //    Task loggerTask = Task.Run(() => Logger());

    //    // Start multiple producers asynchronously
    //    List<Task> producerTasks = new List<Task>();
    //    for (int i = 1; i <= 3; i++)
    //    {
    //        int producerId = i;
    //        producerTasks.Add(Task.Run(() => Producer(producerId)));
    //    }

    //    // Wait for all producer tasks to complete
    //    await Task.WhenAll(producerTasks);

    //    // Signal that logging is completed
    //    lock (lockObject)
    //    {
    //        isLoggingCompleted = true;
    //    }

    //    // Wait for the logger to complete
    //    await loggerTask;
    //}

    static void Producer(int producerId)
    {
        for (int i = 1; i <= 5; i++)
        {
            string logMessage = $"Producer {producerId}: Log message {i}";
            lock (lockObject)
            {
                logQueue.Enqueue(logMessage);
                Console.WriteLine($"Enqueued: {logMessage}");
            }
            Thread.Sleep(100000);
        }
    }

    static async Task Logger()
    {
        using (StreamWriter writer = new StreamWriter("log.txt"))
        {
            while (true)
            {
                string logMessage;
                lock (lockObject)
                {
                    if (logQueue.Count > 0)
                    {
                        logMessage = logQueue.Dequeue();
                    }
                    else if (isLoggingCompleted)
                    {
                        break; // Exit the loop when logging is completed and the queue is empty
                    }
                    else
                    {
                        continue; // Continue to check the queue if it's not empty
                    }
                }

                // Simulate logging to a file
                await writer.WriteLineAsync(logMessage);
                Console.WriteLine($"Logged: {logMessage}");

                // Simulate varying processing time for different log messages
                Thread.Sleep(150);
            }
        }
    }
}
