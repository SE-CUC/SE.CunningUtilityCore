using System;
using System.Collections;

namespace IngameScript
{
    public interface IWaitService
    {
        IEnumerator Wait(TimeSpan duration);
        IEnumerator WaitGameTime(TimeSpan duration);
        IEnumerator WaitUntil(DateTime targetTime);
    }
}
