using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        private readonly IMyApp _app;

        public Program()
        {
            var builder = MyAppBuilder.Create(this);
        }

        public void Main(string argument, UpdateType updateSource) => _app.Main(argument, updateSource);

        public void Save() => _app.Save();
    }

    public partial class MyAppBuilder: IMyAppBuilder
    {
        public IMyAppServices Services { get; private set; } = new IMyAppServices();

        private readonly IMyGridTerminalSystem _gridTerminalSystem;

        private readonly AutoConfigFeatures _features;

        public MyAppBuilder(IMyGridTerminalSystem gridTerminalSystem)
        {
            _gridTerminalSystem = gridTerminalSystem;
            Services.AddSingleton<MyGridProgram>(_gridTerminalSystem);
        }

        public static IMyAppBuilder Create(IMyGridTerminalSystem gridTerminalSystem, AutoConfigFeatures features = AutoConfigFeatures.All)
        {
            var builder = new MyAppBuilder(gridTerminalSystem);

            PreInitSystem(builder);    

            return builder;
        }

        private static void PreInitSystem(MyAppBuilder builder)
        {
            //logger, DI, configuration are on by default
            //logger use ProgramBlock.SurfacePanel
            //configuration use ProgramBlock.CustomData

            //firstly we init EchoLogger
            //after trying to read config from CustomData and init LoggerConfig
            //If successful, we init logger from config
            //if not stop executing with error message on Echo
            //finally we init DI container with logger and config services.
        }

        public IMyApp Build() 
        {
            if ((features & AutoConfigFeatures.Scheduler) != 0) builder = builder.WithScheduler();
            if ((features & AutoConfigFeatures.Commands) != 0) builder = builder.WithCommands();
            if ((features & AutoConfigFeatures.IGC) != 0) builder = builder.WithIGC();
        }

    }

    public partial class MyAppBuilder
    {
        private readonly List<Action<IMyAppServices>> _beforeBuild = new List<Action<IMyAppServices>>();
        private readonly List<Action<IMyAppServices>> _afterBuild = new List<Action<IMyAppServices>>();
        private readonly List<Action<string, UpdateType, IMyAppServices>> _onFirstMainTriggerStart = new List<Action<string, UpdateType, IMyAppServices>>();
        private readonly List<Action<string, UpdateType, IMyAppServices>> _onFirstMainTriggerEnd = new List<Action<string, UpdateType, IMyAppServices>>();
        private readonly List<Action<IMyAppServices>> _onSave = new List<Action<IMyAppServices>>();
        private readonly List<Action<Exception, IMyAppServices>> _onError = new List<Action<Exception, IMyAppServices>>();
        private readonly List<Action<string, UpdateType, IMyAppServices>> _onTerminalAction = new List<Action<string, UpdateType, IMyAppServices>>();

        public IMyAppBuilder BeforeBuild(Action<IMyAppServices> action) { }
        public IMyAppBuilder AfterBuild(Action<IMyAppServices> action) { }
        public IMyAppBuilder OnFirstMainTriggerStart(Action<string, UpdateType, IMyAppServices> action) { }
        public IMyAppBuilder OnFirstMainTriggerEnd(Action<string, UpdateType, IMyAppServices> action) { }
        public IMyAppBuilder OnSave(Action<IMyAppServices> action) { }
        public IMyAppBuilder OnError(Action<Exception, IMyAppServices> action) { }
        public IMyAppBuilder OnTerminalAction(Action<string, UpdateType, IMyAppServices> action) { }
    }

    public class MyApp:IMyApp
    {
        private readonly IMyAppServices _services;

        private readonly List<Action<string, UpdateType, IMyAppServices>> _onFirstMainTriggerStart = new List<Action<string, UpdateType, IMyAppServices>>();
        private readonly List<Action<string, UpdateType, IMyAppServices>> _onFirstMainTriggerEnd = new List<Action<string, UpdateType, IMyAppServices>>();
        private readonly List<Action<IMyAppServices>> _onSave = new List<Action<IMyAppServices>>();
        private readonly List<Action<Exception, IMyAppServices>> _onError = new List<Action<Exception, IMyAppServices>>();
        private readonly List<Action<string, UpdateType, IMyAppServices>> _onTerminalAction = new List<Action<string, UpdateType, IMyAppServices>>();
        private bool _isFirstMainTrigger = true;

        public virtual void Main(string argument, UpdateType updateSource)
        {
            try
            {
                FirstLaunchStart(argument, updateSource);

                OnTerminalAction(argument, updateSource);

                FirstLaunchEnd(argument, updateSource);
            }
            catch (Exception ex)
            {
                _onError.ForEach(action => action(ex, _services));
            }
        }

        private void OnTerminalAction(string argument, UpdateType updateSource)
        {
            if (updateSource.HasFlag(UpdateType.Terminal))
            {
                foreach (var action in _onTerminalAction)
                {
                    action(argument, updateSource, _services);
                }
            }
        }

        private void FirstLaunchEnd(string argument, UpdateType updateSource)
        {
            if (_isFirstMainTrigger)
            {
                _onFirstMainTriggerEnd.ForEach(action => action(argument, updateSource, _services));
                _isFirstMainTrigger = false;
            }
        }

        private void FirstLaunchStart(string argument, UpdateType updateSource)
        {
            if (_isFirstMainTrigger)
                _onFirstMainTriggerStart.ForEach(action => action(argument, updateSource, _services));
        }

        public void Save()
        {
            try
            {
                foreach (var action in _onSave)
                {
                    action(_services);
                }
            }
            catch(Exception ex)
            {
                _onError.ForEach(action => action(ex, _services));
            }
        }
    }

    public enum AutoConfigFeatures
    {
        None = 0,
        Logging = 1 << 0,
        Scheduler = 1 << 1,
        Configuration = 1 << 2,
        Commands = 1 << 3,
        IGC = 1 << 4,
        All = Logging | Scheduler | Configuration | Commands | IGC
    }
}
