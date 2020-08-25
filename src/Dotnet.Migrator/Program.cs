using System.Linq;
using System.Reflection;
using System.CommandLine;
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
            var rootCommand = new RootCommand("Apply or revert C# migration command.");
            var commandArgument = new Argument<string>("command", "The name of the command to execute.");

            var argumentAssemblyOptions = new Option<Assembly>(new[] { "--assembly", "-a" }, (ArgumentResult argResult) =>
            {
                var path = argResult.Tokens?.FirstOrDefault()?.Value;
                if (string.IsNullOrWhiteSpace(path))
                    return null;

                path = path.Trim();
                return Assembly.LoadFrom(path);
            }, true, "The assembly who contains the command to execute.")
            { IsRequired = true };

            var applyCommand = new Command("apply", "Apply a C# migration command.");
            applyCommand.AddArgument(commandArgument);
            applyCommand.AddOption(argumentAssemblyOptions);

            var revertCommand = new Command("revert", "Revert a C# migration command.");
            revertCommand.AddArgument(commandArgument);
            revertCommand.AddOption(argumentAssemblyOptions);

            rootCommand.AddCommand(applyCommand);
            rootCommand.AddCommand(revertCommand);

            applyCommand.Handler = CommandHandler.Create<MigrationCommandOptions, IHost>(Apply);
            revertCommand.Handler = CommandHandler.Create<MigrationCommandOptions, IHost>(Revert);

            return new CommandLineBuilder(rootCommand);
        }

        private static (IMigrationCommand, ILogger) Run(MigrationCommandOptions options, IHost host)
        {
            var serviceProvider = host.Services;
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(typeof(Program));

            logger.LogInformation("Run with {0} and {1}", options.Command, options.Assembly);

            var assemblyCommandParser = new AssemblyCommandParser(options.Assembly);
            var command = assemblyCommandParser.GetByName(options.Command);
            if (command == null)
                logger.LogInformation("Unable to find a command named '{0}' in assembly '{1}'", options.Command, options.Assembly);

            return (command, logger);
        }

        private static void Apply(MigrationCommandOptions options, IHost host)
        {
            var commandAndLogger = Run(options, host);

            if (commandAndLogger.Item1 == null)
                return;

            var logger = commandAndLogger.Item2;
            var command = commandAndLogger.Item1;

            logger.LogInformation("Applying command '{0}'...", options.Command);

            command.Up();

            logger.LogInformation("Command '{0}' successfully applied.", options.Command);
        }

        private static void Revert(MigrationCommandOptions options, IHost host)
        {
            var commandAndLogger = Run(options, host);

            if (commandAndLogger.Item1 == null)
                return;

            var logger = commandAndLogger.Item2;
            var command = commandAndLogger.Item1;

            logger.LogInformation("Reverting command '{0}'...", options.Command);

            command.Down();

            logger.LogInformation("Command '{0}' successfully revert.", options.Command);
        }
    }
}
