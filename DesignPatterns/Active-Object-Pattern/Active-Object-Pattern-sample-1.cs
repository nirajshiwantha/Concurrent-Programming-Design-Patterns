using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace concurrentDesignPatterns.DesignPatterns.Active_Object_Pattern;

internal class Active_Object_Pattern_sample_1
{
    //static void Main()
    //{
    //    Kitchen kitchen = new Kitchen();

    //    // Place orders asynchronously
    //    Task.Run(() => kitchen.PlaceOrder("Pasta"));
    //    Task.Run(() => kitchen.PlaceOrder("Burger"));
    //    Task.Run(() => kitchen.PlaceOrder("Salad"));
    //    Console.ReadLine(); // Keep the application running for the asynchronous orders to complete
    //}
}
class Kitchen
{
    private Queue<string> orderQueue = new Queue<string>();
    private object lockObject = new object();
    public void PlaceOrder(string item)
    {
        lock (lockObject)
        {
            orderQueue.Enqueue(item);
            Console.WriteLine($"Order placed: {item}");
        }
        // Schedule the order processing asynchronously
        Task.Run(() => ProcessOrders());
    }
    private void ProcessOrders()
    {
        string order;
        lock (lockObject)
        {
            // Dequeue the next order if available
            order = orderQueue.Count > 0 ? orderQueue.Dequeue() : null;
        }
        if (order != null)
        {
            // Simulate order processing time
            Thread.Sleep(2000);
            Console.WriteLine($"Order completed: {order}");
        }
    }
}
