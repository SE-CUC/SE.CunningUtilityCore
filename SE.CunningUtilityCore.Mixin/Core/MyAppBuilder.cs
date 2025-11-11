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
            Services.AddSingleton(_program.GridTerminalSystem);
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
            var services = builder.Services;
            var echo = services.GetService<Action<string>>();

            var ini = new MyIni();
            var iniParsed = ini.TryParse(program.Me.CustomData);

            if (!iniParsed && !string.IsNullOrWhiteSpace(program.Me.CustomData))
            {
                echo("[ERROR] Failed to parse CustomData configuration.");
                LoggerConfig.WriteDefault(ini);
                program.Me.CustomData = ini.ToString();
                echo("[INFO] Default configuration has been written to CustomData.");
            }

            var loggerConfig = LoggerConfig.Read(ini);
            services.AddSingleton(loggerConfig);

            var surfaceLogger = new SurfaceLogger(loggerConfig, program.Me.GetSurface(0));
            var compositeLogger = new CompositeLogger(surfaceLogger);
            services.AddSingleton<ILogger>(compositeLogger);

            builder.OnError((ex, s) => s.GetService<ILogger>()?.Error(ex, "An unhandled exception occurred in the main loop."));
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