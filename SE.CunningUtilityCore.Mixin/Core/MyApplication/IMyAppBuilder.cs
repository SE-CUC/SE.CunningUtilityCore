using System;

namespace IngameScript
{
    public interface IMyAppBuilder
    {
        IMyAppServices Services { get; }
        IMyAppBuilder BeforeBuild(Action<IMyAppServices> action);
        IMyAppBuilder AfterBuild(Action<IMyAppServices> action);
        IMyAppBuilder OnFirstMainTriggerStart(Action<string, Sandbox.ModAPI.Ingame.UpdateType, IMyAppServices> action);
        IMyAppBuilder OnFirstMainTriggerEnd(Action<string, Sandbox.ModAPI.Ingame.UpdateType, IMyAppServices> action);
        IMyAppBuilder OnSave(Action<IMyAppServices> action);
        IMyAppBuilder OnError(Action<Exception, IMyAppServices> action);
        IMyAppBuilder OnTerminalAction(Action<string, Sandbox.ModAPI.Ingame.UpdateType, IMyAppServices> action);
        IMyApp Build();
    }
}