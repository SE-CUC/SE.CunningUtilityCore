
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public partial class MyAppBuilder : IMyAppBuilder
    {
        public IInjector Injector { get; private set; }

        private readonly AutoConfigFeatures _features;
        private readonly MyGridProgram _program;

        private MyAppBuilder(MyGridProgram program, AutoConfigFeatures features)
        {
            _program = program;
            _features = features;
            Injector = new Injector();
            Injector.AddSingleton(_program.GridTerminalSystem);
            Injector.AddSingleton(_program.Echo);
        }

        public static IMyAppBuilder Create(MyGridProgram program, AutoConfigFeatures features = AutoConfigFeatures.All)
        {
            var builder = new MyAppBuilder(program, features);
            PreInitSystem(builder);
            return builder;
        }

        private static void PreInitSystem(MyAppBuilder builder)
        {
            var program = builder._program;
            var injector = builder.Injector;
            var echo = injector.GetService<Action<string>>();

            var configManager = new ConfigManager(program, echo);
            var loggerConfig = configManager.Load();
            injector.AddSingleton(loggerConfig);

            var surfaceLogger = new SurfaceLogger(loggerConfig, program.Me.GetSurface(0));
            var compositeLogger = new CompositeLogger(surfaceLogger);
            injector.AddSingleton<ILogger>(compositeLogger);

            var scheduler = new Scheduler();
            injector.AddSingleton<IScheduler>(scheduler);

            var waitService = new WaitService(program);
            injector.AddSingleton<IWaitService>(waitService);

            builder.OnError((ex, s) => s.GetService<ILogger>()?.Error(ex, "An unhandled exception occurred in the main loop."));
            echo("Logger, config, DI was succesfully inited");
            surfaceLogger.Info("Logger, config, DI was succesfully inited");
        }

        public IMyApp Build()
        {
            _beforeBuild.ForEach(action => action(Injector));

            // TODO: Replace with actual implementation
            // if ((_features & AutoConfigFeatures.Scheduler) != 0) builder = builder.WithScheduler();
            // if ((_features & AutoConfigFeatures.Commands) != 0) builder = builder.WithCommands();
            // if ((_features & AutoConfigFeatures.IGC) != 0) builder = builder.WithIGC();

            var app = new MyApp(Injector, _onFirstMainTriggerStart, _onFirstMainTriggerEnd, _onSave, _onError, _onTerminalAction);

            _afterBuild.ForEach(action => action(Injector));
            return app;
        }
    }

    public partial class MyAppBuilder
    {
        private readonly List<Action<IInjector>> _beforeBuild = new List<Action<IInjector>>();
        private readonly List<Action<IInjector>> _afterBuild = new List<Action<IInjector>>();
        private readonly List<Action<string, UpdateType, IInjector>> _onFirstMainTriggerStart = new List<Action<string, UpdateType, IInjector>>();
        private readonly List<Action<string, UpdateType, IInjector>> _onFirstMainTriggerEnd = new List<Action<string, UpdateType, IInjector>>();
        private readonly List<Action<IInjector>> _onSave = new List<Action<IInjector>>();
        private readonly List<Action<Exception, IInjector>> _onError = new List<Action<Exception, IInjector>>();
        private readonly List<Action<string, UpdateType, IInjector>> _onTerminalAction = new List<Action<string, UpdateType, IInjector>>();

        public IMyAppBuilder BeforeBuild(Action<IInjector> action) { _beforeBuild.Add(action); return this; }
        public IMyAppBuilder AfterBuild(Action<IInjector> action) { _afterBuild.Add(action); return this; }
        public IMyAppBuilder OnFirstMainTriggerStart(Action<string, UpdateType, IInjector> action) { _onFirstMainTriggerStart.Add(action); return this; }
        public IMyAppBuilder OnFirstMainTriggerEnd(Action<string, UpdateType, IInjector> action) { _onFirstMainTriggerEnd.Add(action); return this; }
        public IMyAppBuilder OnSave(Action<IInjector> action) { _onSave.Add(action); return this; }
        public IMyAppBuilder OnError(Action<Exception, IInjector> action) { _onError.Add(action); return this; }
        public IMyAppBuilder OnTerminalAction(Action<string, UpdateType, IInjector> action) { _onTerminalAction.Add(action); return this; }
    }
}