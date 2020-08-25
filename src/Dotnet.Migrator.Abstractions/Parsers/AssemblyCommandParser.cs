using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dotnet.Migrator.Commands;

namespace Dotnet.Migrator.Parsers
{
    public class AssemblyCommandParser : ICommandParser
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

        private IList<IMigrationCommand> Commands { get; set; }

        public IList<IMigrationCommand> GetAll()
        {
            if (!(Commands?.Any() ?? false))
            {
                var commandTypes = Assembly.GetExportedTypes().Where(t => typeof(IMigrationCommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                var commands = new List<IMigrationCommand>();
                foreach (var commandType in commandTypes)
                    commands.Add((IMigrationCommand)Activator.CreateInstance(commandType));

                Commands = commands;
            }

            return Commands;
        }

        public Task<IList<IMigrationCommand>> GetAllAsync()
        {
            return Task.FromResult(GetAll());
        }

        public IMigrationCommand GetByName(string commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName))
                throw new ArgumentNullException(nameof(commandName));

            return GetAll().FirstOrDefault(c => c.Migration?.Name?.Equals(commandName, StringComparison.InvariantCultureIgnoreCase) ?? false);
        }

        public async Task<IMigrationCommand> GetByNameAsync(string commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName))
                throw new ArgumentNullException(nameof(commandName));

            return (await GetAllAsync()).FirstOrDefault(c => c.Migration?.Name?.Equals(commandName, StringComparison.InvariantCultureIgnoreCase) ?? false);
        }
    }
}