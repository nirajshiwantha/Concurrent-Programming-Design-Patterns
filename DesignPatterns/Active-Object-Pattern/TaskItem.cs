using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace concurrentDesignPatterns.DesignPatterns.Active_Object_Pattern;

// Nested class representing a task
public class TaskItem
{
    public string Task { get; set; }
    public TaskPriority Priority { get; set; }
}