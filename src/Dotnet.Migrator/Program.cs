using System.Linq;
using System.Reflection;
using CommandLine = System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine.Hosting;
using Dotnet.Migrator.Options;
using Dotnet.Migrator.Parsers;
using Dotnet.Migrator.Commands;

namespace Dotnet.Migrator
{
    class Program
    {
        static async Task Main(string[] args) => await BuildCommandLine()
            .UseHost(_ => Host.CreateDefaultBuilder())
            .UseDefaults()
            .Build()
            .InvokeAsync(args);

        private static CommandLineBuilder BuildCommandLine()
        {
            var rootCommand = new CommandLine.RootCommand("Execute user defined C# command.");
            var commandArgument = new CommandLine.Argument<string>("name", "The name of the C# command to execute.") { Arity = CommandLine.ArgumentArity.ExactlyOne };

            var argumentAssemblyOptions = new CommandLine.Option<Assembly>(new[] { "--assembly", "-a" }, (ArgumentResult argResult) =>
            {
                var path = argResult.Tokens?.FirstOrDefault()?.Value;
                if (string.IsNullOrWhiteSpace(path))
                    return null;

                path = path.Trim();
                return Assembly.LoadFrom(path);
            }, true, "The assembly who contains the command to execute.")
            { IsRequired = true };

            rootCommand.AddArgument(commandArgument);
            rootCommand.AddOption(argumentAssemblyOptions);

            rootCommand.Handler = CommandHandler.Create<CommandOptions, IHost>(Execute);

            var migrationCommand = new CommandLine.Command("migration", "Apply or revert C# migration command.");

            var applyCommand = new CommandLine.Command("apply", "Apply a C# migration command.");
            applyCommand.AddArgument(commandArgument);
            applyCommand.AddOption(argumentAssemblyOptions);

            var revertCommand = new CommandLine.Command("revert", "Revert a C# migration command.");
            revertCommand.AddArgument(commandArgument);
            revertCommand.AddOption(argumentAssemblyOptions);

            migrationCommand.AddCommand(applyCommand);
            migrationCommand.AddCommand(revertCommand);

            rootCommand.AddCommand(migrationCommand);

            applyCommand.Handler = CommandHandler.Create<CommandOptions, IHost>(Apply);
            revertCommand.Handler = CommandHandler.Create<CommandOptions, IHost>(Revert);

            return new CommandLineBuilder(rootCommand);
        }

        private static (TCommand, ILogger) GetCommand<TCommand>(CommandOptions options, IHost host) where TCommand : ICommand
        {
            var serviceProvider = host.Services;
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(typeof(Program));

            logger.LogInformation("Run with {0} and {1}", options.Name, options.Assembly);

            var assemblyCommandParser = new AssemblyCommandParser<TCommand>(options.Assembly);
            var command = assemblyCommandParser.GetByName(options.Name);
            if (command == null)
                logger.LogInformation("Unable to find a command named '{0}' in assembly '{1}'", options.Name, options.Assembly);

            return (command, logger);
        }

        private static void Execute(CommandOptions options, IHost host)
        {
            var commandAndLogger = GetCommand<IExecutionCommand>(options, host);

            if (commandAndLogger.Item1 == null)
                return;

            var logger = commandAndLogger.Item2;
            var command = commandAndLogger.Item1;

            logger.LogInformation("Executing command '{0}'...", options.Name);

            command.Execute();

            logger.LogInformation("Command '{0}' successfully executed.", options.Name);
        }

        private static void Apply(CommandOptions options, IHost host)
        {
            var commandAndLogger = GetCommand<IMigrationCommand>(options, host);

            if (commandAndLogger.Item1 == null)
                return;

            var logger = commandAndLogger.Item2;
            var command = commandAndLogger.Item1;

            logger.LogInformation("Applying command '{0}'...", options.Name);

            command.Up();

            logger.LogInformation("Command '{0}' successfully applied.", options.Name);
        }

        private static void Revert(CommandOptions options, IHost host)
        {
            var commandAndLogger = GetCommand<IMigrationCommand>(options, host);

            if (commandAndLogger.Item1 == null)
                return;

            var logger = commandAndLogger.Item2;
            var command = commandAndLogger.Item1;

            logger.LogInformation("Reverting command '{0}'...", options.Name);

            command.Down();

            logger.LogInformation("Command '{0}' successfully revert.", options.Name);
        }
    }
}
