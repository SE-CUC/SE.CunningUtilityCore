using System;
using IngameScript.Core.DI;

namespace IngameScript
{
    public interface IMyAppBuilder
    {
        IInjector Injector { get; }
        IMyAppBuilder BeforeBuild(Action<IInjector> action);
        IMyAppBuilder AfterBuild(Action<IInjector> action);
        IMyAppBuilder OnFirstMainTriggerStart(Action<string, Sandbox.ModAPI.Ingame.UpdateType, IInjector> action);
        IMyAppBuilder OnFirstMainTriggerEnd(Action<string, Sandbox.ModAPI.Ingame.UpdateType, IInjector> action);
        IMyAppBuilder OnSave(Action<IInjector> action);
        IMyAppBuilder OnError(Action<Exception, IInjector> action);
        IMyAppBuilder OnTerminalAction(Action<string, Sandbox.ModAPI.Ingame.UpdateType, IInjector> action);
        IMyApp Build();
    }
}