using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;

namespace IngameScript
{
    public class MyApp : IMyApp
    {
        private readonly IMyAppServices _services;

        private readonly List<Action<string, UpdateType, IMyAppServices>> _onFirstMainTriggerStart;
        private readonly List<Action<string, UpdateType, IMyAppServices>> _onFirstMainTriggerEnd;
        private readonly List<Action<IMyAppServices>> _onSave;
        private readonly List<Action<Exception, IMyAppServices>> _onError;
        private readonly List<Action<string, UpdateType, IMyAppServices>> _onTerminalAction;
        private bool _isFirstMainTrigger = true;

        public MyApp(
            IMyAppServices services,
            List<Action<string, UpdateType, IMyAppServices>> onFirstMainTriggerStart,
            List<Action<string, UpdateType, IMyAppServices>> onFirstMainTriggerEnd,
            List<Action<IMyAppServices>> onSave,
            List<Action<Exception, IMyAppServices>> onError,
            List<Action<string, UpdateType, IMyAppServices>> onTerminalAction)
        {
            _services = services;
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
                    _onFirstMainTriggerStart.ForEach(action => action(argument, updateSource, _services));
                }

                if (updateSource.HasFlag(UpdateType.Terminal) || updateSource.HasFlag(UpdateType.Trigger))
                {
                    _onTerminalAction.ForEach(action => action(argument, updateSource, _services));
                }

                if (_isFirstMainTrigger)
                {
                    _onFirstMainTriggerEnd.ForEach(action => action(argument, updateSource, _services));
                    _isFirstMainTrigger = false;
                }
            }
            catch (Exception ex)
            {
                _onError.ForEach(action => action(ex, _services));
            }
        }

        public void Save()
        {
            try
            {
                _onSave.ForEach(action => action(_services));
            }
            catch (Exception ex)
            {
                _onError.ForEach(action => action(ex, _services));
            }
        }
    }
}