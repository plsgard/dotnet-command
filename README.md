This repository contains the code for the `Plsgd.System.Command.Abstractions` libraries and the `dotnet-command` global tool.

## Packages

| Package                             | Version | Description                                                                                       |
| ----------------------------------- | ------- | ------------------------------------------------------------------------------------------------- |
| `Plsgd.System.Command.Abstractions` |         | user-defined C# command, command parser, migration command, ...                                   |
| `dotnet-command`                    |         | A command-line tool to call user-defined C# command based on `Plsgd.System.Command.Abstractions`. |

## Documentation

The need behind this package was to be able to execute some operations like update external datas of a provider, migrate some project datas who are not stored in database, etc... without depending on a project and in C# in order to be easily testable by the project containing the commands.

### `Plsgd.System.Command.Abstractions`

This package contains all abstractions who can be used to create your command to call. You will have to import it in the project containing your command. The root namespace is `System.Command`.

### `dotnet-command`

This is the tool to use in order to call the commands that you've created.

#### Install the `dotnet-command` tool

```
dotnet tool install -g dotnet-command
```

#### Get help

```
dotnet cmd --help
```

### How does it work?

1. You create a .NET Core project, with some C# custom command by using `Plsgd.System.Command.Abstractions` and the corresponding base classes.
2. You use the `dotnet-command` tool with CLI to call the command that you want to execute, providing the correct path to the project containing your command
3. The `dotnet-command` tool will parse your arguments, try to find the provided command in the provided project, and execute it

### Command

A command is a class who contains some operations to execute. Currently, there are 2 kinds of commands : "execution command" and "migration command".
An "execution command" has only one operation. A "migration command" is a command who can be applied or reverted.

The `Plsgd.System.Command.Abstractions` package provides 2 base abstract classes: `ExecutionCommand` and `MigrationCommand`.
All commands inherit from `Command` abstract class, who itself implements `ICommand` interface. And each abstract command class implements respectively of `IExecutionCommand` and `IMigrationCommand`.

#### ExecutionCommand

Contains one operation `Execute()` and can only be executed.

#### MigrationCommand

Contains two operations: `Up()` and `Down()`. It can be applied (with `Up`), or reverted (with `Down`).

### Parser

A parser is a way to retrieve the command(s) to call. Currently, there is only one parser : `AssemblyCommandParser`.

#### AssemblyCommandParser

This parser searches for command inside a provided assembly.

### Options

The options represent all the parameters used to find and call the commands. Currently, there is only one options class, corresponding to the parser above : `AssemblyCommandOptions`.

#### AssemblyCommandOptions

The options class simply contains the name of the command to execute, and the assembly where to find it (the command).

## Getting started

### 1. Create a custom command

First of all, you have to import the `Plsgd.System.Command.Abstractions` package in the project where you want to create your custom commands:

```
dotnet add package Plsgd.System.Command.Abstractions

# don't forget to add -v <version> if you want to use specific version, especially for the preview versions who are not downloaded by default
```

Create a new class derived from `ExecutionCommand`, if you only want to execute this command, or from `MigrationCommand` if you want create a migration who can be applied or reverted:

```csharp
using System.Command.Commands

public class MyExecutionCommand : ExecutionCommand
{
    public MyExecutionCommand() : base("my-exec-command-name") // the unique name of your command
    { }

    public override void Execute()
    {
        // you custom code to execute
    }
}
```

If you want to create a migration command, inherit from `MigrationCommand`:

```csharp
using System.Command.Commands

public class MyMigrationCommand : MigrationCommand
{
    public MyMigrationCommand() : base("my-migration-command-name") // the unique name of your command
    { }

    public override void Up()
    {
        // you custom code to apply
    }

    public override void Down()
    {
        // you custom code to revert
    }
}
```

> Don't forget to build this project in order to get the assembly generated

### 2. Execute your command or Apply a migration

#### Execute an `ExecutionCommand` > `dotnet cmd exec`

```
dotnet cmd exec <name_of_the_command> --assembly <path_to_dll_containing_your_command>

# e.g.
dotnet cmd exec my-exec-command-name --assembly ../src/MyProject.Commands/bin/Debug/netcoreapp31/MyProject.Commands.dll
```

#### Apply a `MigrationCommand` > `dotnet cmd migration apply`

```
dotnet cmd migration apply <name_of_the_command> --assembly <path_to_dll_containing_your_command>

# e.g.
dotnet cmd migration apply my-migration-command-name --assembly ../src/MyProject.Commands/bin/Debug/netcoreapp31/MyProject.Commands.dll
```

#### Revert a `MigrationCommand` > `dotnet cmd migration revert`

```
dotnet cmd migration revert <name_of_the_command> --assembly <path_to_dll_containing_your_command>

# e.g.
dotnet cmd migration apply my-migration-command-name --assembly ../src/MyProject.Commands/bin/Debug/netcoreapp31/MyProject.Commands.dll
```

### Configuration

You may need to execute some commands using some parameters through a `IConfiguration` instance like a `appsettings.json` configuration.

Each command who inherits from `Command` will have a `Configuration` property of type `IConfiguration`. By default, this property is populated trying to load an `appsettings.json` file **placed next to the provided assembly**.

So if you want to use a configuration file, follow these steps:

- create an `appsettings.json` file in the project containing your commands
- edit your `cproj` file to add an operation to copy the `appsettings.json` file during the build (in order to get it next to your assembly):

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <!-- create a ConfigFiles property including all files following the pattern -->
  <ItemGroup>
    <ConfigFiles Include="appsettings.json"/>
  </ItemGroup>
  <!-- add a target who, after build, will copy all ConfigFiles in the output directory -->
  <Target Name="CopyConfigFiles" AfterTargets="AfterBuild">
    <Copy SourceFiles="@(ConfigFiles)" DestinationFolder="$(OutDir)" SkipUnchangedFiles="true"/>
  </Target>
</Project>
```

- use the `Configuration` property in your command to retrieve what you want:

```csharp
using System.Command.Commands

public class MyExecutionCommand : ExecutionCommand
{
    public MyExecutionCommand() : base("my-exec-command-name") // the unique name of your command
    { }

    public override void Execute()
    {
        var apiSecret = Configuration.GetSection("Api:Secret").Value;
        // use your config value...
    }
}
```

> Note that your classes who inherits from `ExecutionCommand` or `MigrationCommand` may have a constructor with a `IConfiguration configuration` parameter, in order to mock the configuration in your tests.

### Logging

By default, the `dotnet-command` tool will inject its own `ILogger` when he calls the command. So you can add a `ILogger` in the constructor of your command, and use it where you want:

```csharp
using System.Command.Commands

public class MyExecutionCommand : ExecutionCommand
{
    private readonly ILogger<MyExecutionCommand> _logger;

    public MyExecutionCommand(ILogger<MyExecutionCommand> logger) : base("my-exec-command-name") // the unique name of your command
    {
        _logger = logger;
    }

    public override void Execute()
    {
        _logger.LogInformation("Executing my operation...");
        // ...
    }
}
```

## License

Both projects are licensed under the terms of the [MIT license](LICENSE.md).
