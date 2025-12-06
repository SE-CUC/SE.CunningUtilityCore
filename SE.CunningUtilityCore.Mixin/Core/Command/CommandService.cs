using System;
using System.Collections.Generic;

namespace IngameScript
{
    public class CommandService : ICommandService
    {
        private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>(StringComparer.OrdinalIgnoreCase);
        private readonly ILogger _logger;

        public CommandService(ILogger logger)
        {
            _logger = logger;
        }

        public void Register(ICommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (string.IsNullOrEmpty(command.Name)) throw new ArgumentException("Command name cannot be empty.", nameof(command));

            if (_commands.ContainsKey(command.Name))
            {
                _logger?.Error($"Command '{command.Name}' is already registered. Overwriting.");
            }
            _commands[command.Name] = command;
            _logger?.Info($"Registered command: {command.Name}");
        }

        public void RegisterModule(ICommandModule module)
        {
            if (module == null) throw new ArgumentNullException(nameof(module));
            foreach (var command in module.GetCommands())
            {
                Register(command);
            }
        }

        public void Handle(string argument, Action<string> reply)
        {
            if (string.IsNullOrEmpty(argument)) return;

            var reader = new ArgumentReader(argument);
            var commandName = reader.Next();

            if (string.IsNullOrEmpty(commandName)) return;

            ICommand command;
            if (_commands.TryGetValue(commandName, out command))
            {
                try
                {
                    command.Execute(reader.Rest(), reply);
                }
                catch (Exception ex)
                {
                    _logger?.Error(ex, $"Error executing command '{commandName}'");
                    reply?.Invoke($"Error: {ex.Message}");
                }
            }
            else
            {
                reply?.Invoke($"Unknown command: {commandName}");
                _logger?.Info($"Unknown command requested: {commandName}");
            }
        }
    }
}
