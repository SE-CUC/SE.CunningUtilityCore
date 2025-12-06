using System;

namespace IngameScript
{
    public interface ICommand
    {
        string Name { get; }
        string HelpText { get; }
        void Execute(string arguments, Action<string> reply);
    }
}
