using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IngameScript
{
    public class SystemCommands : ICommandModule
    {




        
        public IEnumerable<ICommand> GetCommands()
        {
            yield return new HelpCommand();
            yield return new VersionCommand();
        }

        private class VersionCommand : ICommand
        {
            public string Name => "version";
            public string HelpText => "Displays the current version.";

            public void Execute(string arguments, Action<string> reply)
            {
                reply("SE.CunningUtilityCore v1.0.0");
            }
        }

        private class HelpCommand : ICommand
        {
            public string Name => "help";
            public string HelpText => "Displays this help message.";

            public void Execute(string arguments, Action<string> reply)
            {
                reply("Available commands: help, version. (Full list requires ICommandService update)");
            }
        }
    }
}
