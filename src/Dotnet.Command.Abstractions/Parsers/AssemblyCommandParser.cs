using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dotnet.Command.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Command.Parsers
{
    /// <inheritdoc />
    public class AssemblyCommandParser : AssemblyCommandParser<ICommand>, ICommandParser
    {
        public AssemblyCommandParser(string assemblyPath, IServiceProvider serviceProvider = null) : base(assemblyPath, serviceProvider)
        {
        }

        public AssemblyCommandParser(Assembly assembly, IServiceProvider serviceProvider = null) : base(assembly, serviceProvider)
        {
        }
    }

    /// <summary>
    /// Represents a command parser through an assembly.
    /// </summary>
    public class AssemblyCommandParser<TCommand> : BaseCommandParser, ICommandParser<TCommand> where TCommand : ICommand
    {
        public AssemblyCommandParser(string assemblyPath, IServiceProvider serviceProvider = null) : base(serviceProvider)
        {
            Assembly = Assembly.LoadFile(assemblyPath);
        }

        public AssemblyCommandParser(Assembly assembly, IServiceProvider serviceProvider = null) : base(serviceProvider)
        {
            Assembly = assembly;
        }

        public Assembly Assembly { get; }

        private IList<TCommand> Commands { get; set; }

        /// <inheritdoc />
        public IList<TCommand> GetAll()
        {
            if (!(Commands?.Any() ?? false))
            {
                var commandTypes = Assembly.GetExportedTypes().Where(t => typeof(TCommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                var commands = new List<TCommand>();

                foreach (var commandType in commandTypes)
                    commands.Add((TCommand)ActivatorUtilities.CreateInstance(ServiceProvider, commandType));

                Commands = commands;
            }

            return Commands;
        }

        /// <inheritdoc />
        public Task<IList<TCommand>> GetAllAsync()
        {
            return Task.FromResult(GetAll());
        }

        /// <inheritdoc />
        public TCommand GetByName(string commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName))
                throw new ArgumentNullException(nameof(commandName));

            return GetAll().FirstOrDefault(c => c.Name?.Equals(commandName, StringComparison.InvariantCultureIgnoreCase) ?? false);
        }

        /// <inheritdoc />
        public async Task<TCommand> GetByNameAsync(string commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName))
                throw new ArgumentNullException(nameof(commandName));

            return (await GetAllAsync()).FirstOrDefault(c => c.Name?.Equals(commandName, StringComparison.InvariantCultureIgnoreCase) ?? false);
        }
    }
}