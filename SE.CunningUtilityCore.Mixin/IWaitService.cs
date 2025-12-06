using System;
using System.Collections;

namespace IngameScript.Core.Scheduler
{
    public interface IWaitService
    {
        IEnumerator Wait(TimeSpan duration);
        IEnumerator WaitGameTime(TimeSpan duration);
        IEnumerator WaitUntil(DateTime targetTime);
    }
}
