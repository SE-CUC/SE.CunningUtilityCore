using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;

namespace IngameScript
 {
    public partial class MyAppBuilder : IMyAppBuilder
    {
        public IMyAppServices Services { get; private set; }

        private readonly AutoConfigFeatures _features;
        private readonly MyGridProgram _program;

        private MyAppBuilder(MyGridProgram program, AutoConfigFeatures features)
        {
            _program = program;
            _features = features;
            Services = new MyAppServices();
            Services.AddSingleton(_program);
            Services.AddSingleton(_program.GridTerminalSystem);
            Services.AddSingleton(_program.Me);
            Services.AddSingleton(_program.IGC);
            Services.AddSingleton(_program.Runtime);
            Services.AddSingleton<Action<string>>(_program.Echo);
        }

        public static IMyAppBuilder Create(MyGridProgram program, AutoConfigFeatures features = AutoConfigFeatures.All)
        {
            var builder = new MyAppBuilder(program, features);
            PreInitSystem(builder);
            return builder;
        }

        private static void PreInitSystem(MyAppBuilder builder)
        {
            // TODO:
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
            _beforeBuild.ForEach(action => action(Services));

            // TODO: Replace with actual implementation
            // if ((_features & AutoConfigFeatures.Scheduler) != 0) builder = builder.WithScheduler();
            // if ((_features & AutoConfigFeatures.Commands) != 0) builder = builder.WithCommands();
            // if ((_features & AutoConfigFeatures.IGC) != 0) builder = builder.WithIGC();

            var app = new MyApp(Services, _onFirstMainTriggerStart, _onFirstMainTriggerEnd, _onSave, _onError, _onTerminalAction);

            _afterBuild.ForEach(action => action(Services));
            return app;
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

        public IMyAppBuilder BeforeBuild(Action<IMyAppServices> action) { _beforeBuild.Add(action); return this; }
        public IMyAppBuilder AfterBuild(Action<IMyAppServices> action) { _afterBuild.Add(action); return this; }
        public IMyAppBuilder OnFirstMainTriggerStart(Action<string, UpdateType, IMyAppServices> action) { _onFirstMainTriggerStart.Add(action); return this; }
        public IMyAppBuilder OnFirstMainTriggerEnd(Action<string, UpdateType, IMyAppServices> action) { _onFirstMainTriggerEnd.Add(action); return this; }
        public IMyAppBuilder OnSave(Action<IMyAppServices> action) { _onSave.Add(action); return this; }
        public IMyAppBuilder OnError(Action<Exception, IMyAppServices> action) { _onError.Add(action); return this; }
        public IMyAppBuilder OnTerminalAction(Action<string, UpdateType, IMyAppServices> action) { _onTerminalAction.Add(action); return this; }
    }
}