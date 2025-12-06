using System;
using System.Collections;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class WaitService : IWaitService
    {
        private readonly MyGridProgram _gridProgram;

        public WaitService(MyGridProgram gridProgram)
        {
            _gridProgram = gridProgram;
        }

        public IEnumerator Wait(TimeSpan duration)
        {
            var waitUntil = DateTime.Now + duration;
            while (DateTime.Now < waitUntil)
            {
                yield return null;
            }
        }

        public IEnumerator WaitGameTime(TimeSpan duration)
        {
            var remaining = duration;
            while (remaining.TotalSeconds > 0)
            {
                yield return null;
                remaining -= _gridProgram.Runtime.TimeSinceLastRun;
            }
        }

        public IEnumerator WaitUntil(DateTime targetTime)
        {
            while (DateTime.Now < targetTime)
            {
                yield return null;
            }
        }
    }
}
