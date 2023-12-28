using concurrentDesignPatterns.DesignPatterns.Active_Object_Pattern;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ActiveObjectPatternExample
{
    class Program
    {
        //static void Main()
        //{
        //    // Creating an instance of the TaskScheduler
        //    TaskScheduler taskScheduler = new TaskScheduler();

        //    // Submit various tasks asynchronously
        //    Task.Run(() => SubmitTask(taskScheduler, "Urgent Task", TaskPriority.High));
        //    Task.Run(() => SubmitTask(taskScheduler, "Important Task", TaskPriority.Normal));
        //    Task.Run(() => SubmitTask(taskScheduler, "Regular Task", TaskPriority.Normal));
        //    Task.Run(() => SubmitTask(taskScheduler, "Background Task", TaskPriority.Low));

        //    Console.ReadLine(); // Keep the application running for asynchronous tasks to complete
        //}

        // Method to submit tasks with specified priorities
        private static void SubmitTask(TaskScheduler taskScheduler, string taskName, TaskPriority priority)
        {
            for (int i = 0; i < 5; i++)
            {
                // Submit multiple tasks with specified priority
                string task = $"{taskName} - Iteration {i + 1}";
                taskScheduler.Enqueue(task, priority);
                Console.WriteLine($"Task submitted: {task} (Priority: {priority})");
                Thread.Sleep(2000); // Simulate time between task submissions
            }
        }
    }

    // TaskScheduler class implementing the Active Object pattern
    internal class TaskScheduler
    {
        // Concurrent collection to store tasks
        private readonly BlockingCollection<TaskItem> tasks = new BlockingCollection<TaskItem>();

        // Method to enqueue a task with specified priority
        public void Enqueue(string task, TaskPriority priority)
        {
            tasks.Add(new TaskItem { Task = task, Priority = priority });
        }

        // Method to process tasks based on priority
        private void ProcessTasks()
        {
            foreach (var task in tasks.GetConsumingEnumerable())
            {
                Console.WriteLine($"Processing task: {task.Task} (Priority: {task.Priority})");
                Thread.Sleep(3000); // Simulate task processing time
            }
        }

        // Constructor to start the task processing loop
        public TaskScheduler()
        {
            Task.Run(() => ProcessTasks());
        }
    }
}
