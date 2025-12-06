using System;
using System.Collections;
using Sandbox.ModAPI.Ingame;

namespace IngameScript.Core.Scheduler
{
    public class WaitService : IWaitService
    {
        private readonly MyGridProgram _gridProgram;

        private static WaitService _instance = null;

        public static WaitService Waiter 
        { 
            get 
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("WaitService has not been initialized. Please initialize it before use.");
                }
                return _instance;
            }
        }

        public WaitService(MyGridProgram gridProgram)
        {
            _gridProgram = gridProgram;
            _instance = this;
        }

        protected WaitService(MyGridProgram gridProgram, WaitService instance)
        {
            _gridProgram = gridProgram;
            _instance = instance;
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
