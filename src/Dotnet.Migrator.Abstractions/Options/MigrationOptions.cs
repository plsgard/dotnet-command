using System.Reflection;
namespace Dotnet.Migrator.Options
{
    public class MigrationCommandOptions
    {
        protected MigrationCommandOptions()
        {
        }

        public MigrationCommandOptions(string command, Assembly assembly)
        {
            Command = command;
            Assembly = assembly;
        }

        public MigrationCommandOptions(string command, string assembly) : this(command, Assembly.LoadFrom(assembly))
        {
        }

        public string Command { get; private set; }

        public Assembly Assembly { get; private set; }
    }
}