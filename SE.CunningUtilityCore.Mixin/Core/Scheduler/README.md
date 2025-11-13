# Scheduler

The `Scheduler` module provides a simple cooperative multitasking scheduler for running background tasks.

## IScheduler

The `IScheduler` interface provides the following methods:

- `void AddTask(IEnumerator task)`: Adds a new `ysync` task to the scheduler.
- `void Update()`: Executes one step of each scheduled task.

## Usage

To use the scheduler, first get an instance of `IScheduler` from the `IInjector` container. Then, add your `ysync` tasks to the scheduler using the `AddTask` method. Finally, call the `Update` method in your main loop to process the tasks.

### Example

```csharp
public class MyApp : IMyApp
{
    private readonly IScheduler _scheduler;

    public MyApp(IInjector injector)
    {
        _scheduler = injector.GetService<IScheduler>();
        _scheduler.AddTask(MyBackgroundTask());
    }

    public void Main(string argument, UpdateType updateSource)
    {
        _scheduler.Update();
    }

    private IEnumerator MyBackgroundTask()
    {
        // Do some work here
        yield return null;
        // Do some more work here
    }
}
```
