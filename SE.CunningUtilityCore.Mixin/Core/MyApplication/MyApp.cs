using Sandbox.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using IngameScript.Core.DI;
using IngameScript.Core.Scheduler;

namespace IngameScript
{
    public class MyApp : IMyApp
    {
        private readonly IInjector _injector;
        private readonly IScheduler _scheduler;

        private readonly List<Action<string, UpdateType, IInjector>> _onFirstMainTriggerStart;
        private readonly List<Action<string, UpdateType, IInjector>> _onFirstMainTriggerEnd;
        private readonly List<Action<IInjector>> _onSave;
        private readonly List<Action<Exception, IInjector>> _onError;
        private readonly List<Action<string, UpdateType, IInjector>> _onTerminalAction;
        private bool _isFirstMainTrigger = true;

        public MyApp(
            IInjector injector,
            List<Action<string, UpdateType, IInjector>> onFirstMainTriggerStart,
            List<Action<string, UpdateType, IInjector>> onFirstMainTriggerEnd,
            List<Action<IInjector>> onSave,
            List<Action<Exception, IInjector>> onError,
            List<Action<string, UpdateType, IInjector>> onTerminalAction)
        {
            _injector = injector;
            _scheduler = _injector.GetService<IScheduler>();
            _onFirstMainTriggerStart = onFirstMainTriggerStart;
            _onFirstMainTriggerEnd = onFirstMainTriggerEnd;
            _onSave = onSave;
            _onError = onError;
            _onTerminalAction = onTerminalAction;
        }

        public virtual void Main(string argument, UpdateType updateSource)
        {
            try
            {
                if (_isFirstMainTrigger)
                {
                    for (int i = 0; i < _onFirstMainTriggerStart.Count; i++)
                    {
                        _onFirstMainTriggerStart[i](argument, updateSource, _injector);
                    }
                }

                if (updateSource.HasFlag(UpdateType.Terminal) || updateSource.HasFlag(UpdateType.Trigger))
                {
                    for (int i = 0; i < _onTerminalAction.Count; i++)
                    {
                        _onTerminalAction[i](argument, updateSource, _injector);
                    }
                }

                _scheduler.Update();

                if (_isFirstMainTrigger)
                {
                    for (int i = 0; i < _onFirstMainTriggerEnd.Count; i++)
                    {
                        _onFirstMainTriggerEnd[i](argument, updateSource, _injector);
                    }
                    _isFirstMainTrigger = false;
                }
            }
            catch (Exception ex)
            {
                for (int i = 0; i < _onError.Count; i++)
                {
                    _onError[i](ex, _injector);
                }
            }
        }

        public void Save()
        {
            try
            {
                for (int i = 0; i < _onSave.Count; i++)
                {
                    _onSave[i](_injector);
                }
            }
            catch (Exception ex)
            {
                for (int i = 0; i < _onError.Count; i++)
                {
                    _onError[i](ex, _injector);
                }
            }
        }
    }
}
