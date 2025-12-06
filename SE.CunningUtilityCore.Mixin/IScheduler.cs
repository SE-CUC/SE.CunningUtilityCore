using System;
using System.Collections;

namespace IngameScript.Core.Scheduler
{
    public interface IScheduler
    {
        void AddTask(IEnumerator task, TaskPriority priority = TaskPriority.Normal);
        void AddSequentialTasks(IEnumerator[] tasks, TaskPriority priority = TaskPriority.Normal);
        void AddParallelTasks(IEnumerator[] tasks, TaskPriority priority = TaskPriority.Normal);
        void AddRepeatingTask(IEnumerator task, int repeatDelayTicks, TaskPriority priority = TaskPriority.Normal);
        void Update();
    }
}
