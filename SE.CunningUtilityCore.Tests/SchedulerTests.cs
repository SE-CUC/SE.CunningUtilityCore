using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace IngameScript.Tests
{
    [TestFixture]
    public class SchedulerTests
    {
        private IScheduler _scheduler;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new Scheduler();
        }

        private IEnumerator SimpleTask(List<int> executionOrder, int id)
        {
            executionOrder.Add(id);
            yield return null;
            executionOrder.Add(id);
        }

        [Test]
        public void NormalPriorityTasks_RunInFIFOOrder()
        {
            var executionOrder = new List<int>();
            _scheduler.AddTask(SimpleTask(executionOrder, 1));
            _scheduler.AddTask(SimpleTask(executionOrder, 2));

            _scheduler.Update(); // Task 1, step 1
            Assert.AreEqual(new List<int> { 1 }, executionOrder);

            _scheduler.Update(); // Task 1, step 2
            Assert.AreEqual(new List<int> { 1, 1 }, executionOrder);

            _scheduler.Update(); // Task 2, step 1
            Assert.AreEqual(new List<int> { 1, 1, 2 }, executionOrder);

            _scheduler.Update(); // Task 2, step 2
            Assert.AreEqual(new List<int> { 1, 1, 2, 2 }, executionOrder);
        }

        [Test]
        public void HighPriorityTask_RunsBeforeNormal()
        {
            var executionOrder = new List<int>();
            _scheduler.AddTask(SimpleTask(executionOrder, 1), TaskPriority.Normal);
            _scheduler.AddTask(SimpleTask(executionOrder, 2), TaskPriority.High);

            _scheduler.Update(); // Task 2, step 1 (High priority)
            Assert.AreEqual(new List<int> { 2 }, executionOrder);

            _scheduler.Update(); // Task 2, step 2
            Assert.AreEqual(new List<int> { 2, 2 }, executionOrder);

            _scheduler.Update(); // Task 1, step 1 (Normal priority)
            Assert.AreEqual(new List<int> { 2, 2, 1 }, executionOrder);

            _scheduler.Update(); // Task 1, step 2
            Assert.AreEqual(new List<int> { 2, 2, 1, 1 }, executionOrder);
        }

        [Test]
        public void ImmediatePriorityTask_PreemptsCurrentTask()
        {
            var executionOrder = new List<int>();
            _scheduler.AddTask(SimpleTask(executionOrder, 1), TaskPriority.Normal);

            _scheduler.Update(); // Task 1, step 1
            Assert.AreEqual(new List<int> { 1 }, executionOrder);

            _scheduler.AddTask(SimpleTask(executionOrder, 2), TaskPriority.Immediate);

            _scheduler.Update(); // Immediate Task 2, step 1
            Assert.AreEqual(new List<int> { 1, 2 }, executionOrder);

            _scheduler.Update(); // Immediate Task 2, step 2
            Assert.AreEqual(new List<int> { 1, 2, 2 }, executionOrder);

            _scheduler.Update(); // Current task is now null, scheduler picks up original task 1 again
            Assert.AreEqual(new List<int> { 1, 2, 2, 1 }, executionOrder);
        }

        [Test]
        public void SequentialTasks_RunInOrder()
        {
            var executionOrder = new List<int>();
            var tasks = new[] { SimpleTask(executionOrder, 1), SimpleTask(executionOrder, 2) };
            _scheduler.AddSequentialTasks(tasks);

            _scheduler.Update(); // Task 1, step 1
            _scheduler.Update(); // Task 1, step 2
            _scheduler.Update(); // Task 2, step 1
            _scheduler.Update(); // Task 2, step 2

            Assert.AreEqual(new List<int> { 1, 1, 2, 2 }, executionOrder);
        }

        [Test]
        public void ParallelTasks_RunConcurrently()
        {
            var executionOrder = new List<int>();
            var tasks = new[] { SimpleTask(executionOrder, 1), SimpleTask(executionOrder, 2) };
            _scheduler.AddParallelTasks(tasks);

            _scheduler.Update(); // Both tasks, step 1
            CollectionAssert.AreEquivalent(new List<int> { 1, 2 }, executionOrder);

            _scheduler.Update(); // Both tasks, step 2
            CollectionAssert.AreEquivalent(new List<int> { 1, 2, 1, 2 }, executionOrder);
        }

        [Test]
        public void RepeatingTask_RunsAtInterval()
        {
            var executionOrder = new List<int>();
            _scheduler.AddRepeatingTask(() => SimpleTask(executionOrder, 1), 3);

            _scheduler.Update(); // Tick 1 - Task runs
            Assert.AreEqual(1, executionOrder.Count);

            _scheduler.Update(); // Tick 2 - Task finishes, delay starts
            _scheduler.Update(); // Tick 3 - Delay
            _scheduler.Update(); // Tick 4 - Delay

            _scheduler.Update(); // Tick 5 - Task runs again
            Assert.AreEqual(3, executionOrder.Count);
        }
    }
}
