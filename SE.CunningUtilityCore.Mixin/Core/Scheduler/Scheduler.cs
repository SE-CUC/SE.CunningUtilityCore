using System;
using System.Collections;
using System.Collections.Generic;

namespace IngameScript.Core.Scheduler
{
    public class Scheduler : IScheduler
    {
        private class RepeatingTask
        {
            public IEnumerator Task;
            public int RepeatDelayTicks;
            public int TicksUntilRepeat;
        }

        private readonly List<IEnumerator> _normalPriorityTasks = new List<IEnumerator>();
        private readonly List<IEnumerator> _highPriorityTasks = new List<IEnumerator>();
        private IEnumerator _immediateTask;
        private IEnumerator _currentTask;
        private readonly List<RepeatingTask> _repeatingTasks = new List<RepeatingTask>();

        public void AddTask(IEnumerator task, TaskPriority priority = TaskPriority.Normal)
        {
            switch (priority)
            {
                case TaskPriority.Immediate:
                    _immediateTask = task;
                    break;
                case TaskPriority.High:
                    _highPriorityTasks.Insert(0, task);
                    break;
                case TaskPriority.Normal:
                default:
                    _normalPriorityTasks.Add(task);
                    break;
            }
        }

        public void AddSequentialTasks(IEnumerator[] tasks, TaskPriority priority = TaskPriority.Normal)
        {
            AddTask(RunSequence(tasks), priority);
        }

        public void AddParallelTasks(IEnumerator[] tasks, TaskPriority priority = TaskPriority.Normal)
        {
            AddTask(RunParallel(tasks), priority);
        }

        public void AddRepeatingTask(IEnumerator task, int repeatDelayTicks, TaskPriority priority = TaskPriority.Normal)
        {
            _repeatingTasks.Add(new RepeatingTask { Task = task, RepeatDelayTicks = repeatDelayTicks, TicksUntilRepeat = 0 });
        }

        public void Update()
        {
            for (int i = 0; i < _repeatingTasks.Count; i++)
            {
                var repeatingTask = _repeatingTasks[i];
                repeatingTask.TicksUntilRepeat--;
                if (repeatingTask.TicksUntilRepeat <= 0)
                {
                    AddTask(repeatingTask.Task);
                    repeatingTask.TicksUntilRepeat = repeatingTask.RepeatDelayTicks;
                }
            }

            if (_immediateTask != null)
            {
                _currentTask = _immediateTask;
                _immediateTask = null;
            }

            if (_currentTask == null)
            {
                if (_highPriorityTasks.Count > 0)
                {
                    _currentTask = _highPriorityTasks[0];
                    _highPriorityTasks.RemoveAt(0);
                }
                else if (_normalPriorityTasks.Count > 0)
                {
                    _currentTask = _normalPriorityTasks[0];
                    _normalPriorityTasks.RemoveAt(0);
                }
            }

            if (_currentTask != null)
            {
                if (!_currentTask.MoveNext())
                {
                    _currentTask = null;
                }
            }
        }

        private IEnumerator RunSequence(IEnumerator[] tasks)
        {
            for(int i = 0; i < tasks.Length; i++)
            {
                yield return tasks[i];
            }
        }

        private IEnumerator RunParallel(IEnumerator[] tasks)
        {
            var activeTasks = new List<IEnumerator>(tasks);
            while (activeTasks.Count > 0)
            {
                for (int i = activeTasks.Count - 1; i >= 0; i--)
                {
                    if (!activeTasks[i].MoveNext())
                    {
                        activeTasks.RemoveAt(i);
                    }
                }
                yield return null;
            }
        }
    }
}
