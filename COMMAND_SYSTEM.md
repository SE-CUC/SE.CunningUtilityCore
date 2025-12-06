# Command System

The `SE.CunningUtilityCore` Command System provides a robust, zero-allocation framework for handling terminal commands in Space Engineers. It integrates seamlessly with the Dependency Injection system and supports modular command registration.

## enabling Commands
To enable the command system, call `AddCommands()` on your `MyAppBuilder` in the `Program` class (or where you configure the app):

```csharp
MyAppBuilder.Create(this)
    .AddCommands() // Enables command system
    .Build();
```

## Registering Custom Commands
You can register commands individually or as modules.

### 1. Define a Command
Implement the `ICommand` interface:
```csharp
public class HelloCommand : ICommand
{
    public string Name => "hello";
    public string HelpText => "Says hello to the target.";

    public void Execute(string arguments, Action<string> reply)
    {
        // 'arguments' contains everything after the command name.
        // e.g. if user typed: hello world "foo bar"
        // arguments = "world \"foo bar\""
        
        reply($"Hello! You said: {arguments}");
    }
}
```

### 2. Registering
Hook into the builder to register your command:

```csharp
MyAppBuilder.Create(this)
    .AddCommands()
    .AfterBuild(injector => 
    {
        var commandService = injector.GetService<ICommandService>();
        commandService.Register(new HelloCommand());
    })
    .Build();
```

## Argument Parsing
The system uses a smart `ArgumentReader` that handles quotes correctly without generating garbage arrays.

**Input:**
`rename "Small Grid 1" "Explorer"`

**Usage in Command:**
```csharp
public void Execute(string arguments, Action<string> reply)
{
    var reader = new ArgumentReader(arguments);
    var target = reader.Next(); // "Small Grid 1"
    var newName = reader.Next(); // "Explorer"
    
    // ... logic ...
}
```

## Built-in Commands
- `help`: Lists available commands (WIP)
- `version`: Displays the current version of the core.
