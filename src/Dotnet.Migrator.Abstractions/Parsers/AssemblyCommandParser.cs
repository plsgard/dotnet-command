using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dotnet.Migrator.Commands;

namespace Dotnet.Migrator.Parsers
{
    public class AssemblyCommandParser : AssemblyCommandParser<ICommand>, ICommandParser
    {
        public AssemblyCommandParser(string assemblyPath) : base(assemblyPath)
        {
        }

        public AssemblyCommandParser(Assembly assembly) : base(assembly)
        {
        }
    }

    public class AssemblyCommandParser<TCommand> : ICommandParser<TCommand> where TCommand : ICommand
    {
        public AssemblyCommandParser(string assemblyPath)
        {
            Assembly = Assembly.LoadFrom(assemblyPath);
        }

        public AssemblyCommandParser(Assembly assembly)
        {
            Assembly = assembly;
        }

        public Assembly Assembly { get; }

        private IList<TCommand> Commands { get; set; }

        public IList<TCommand> GetAll()
        {
            if (!(Commands?.Any() ?? false))
            {
                var commandTypes = Assembly.GetExportedTypes().Where(t => typeof(TCommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                var commands = new List<TCommand>();
                foreach (var commandType in commandTypes)
                    commands.Add((TCommand)Activator.CreateInstance(commandType));

                Commands = commands;
            }

            return Commands;
        }

        public Task<IList<TCommand>> GetAllAsync()
        {
            return Task.FromResult(GetAll());
        }

        public TCommand GetByName(string commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName))
                throw new ArgumentNullException(nameof(commandName));

            return GetAll().FirstOrDefault(c => c.Name?.Equals(commandName, StringComparison.InvariantCultureIgnoreCase) ?? false);
        }

        public async Task<TCommand> GetByNameAsync(string commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName))
                throw new ArgumentNullException(nameof(commandName));

            return (await GetAllAsync()).FirstOrDefault(c => c.Name?.Equals(commandName, StringComparison.InvariantCultureIgnoreCase) ?? false);
        }
    }
}