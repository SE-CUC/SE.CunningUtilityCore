using System;

namespace IngameScript
{
    public interface ICommandService
    {
        void Register(ICommand command);
        void RegisterModule(ICommandModule module);
        void Handle(string argument, Action<string> reply);
    }
}
