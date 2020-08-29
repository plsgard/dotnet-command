using System.Reflection;
namespace Dotnet.Migrator.Options
{
    public class CommandOptions
    {
        protected CommandOptions()
        {
        }

        public CommandOptions(string command, Assembly assembly)
        {
            Command = command;
            Assembly = assembly;
        }

        public CommandOptions(string command, string assembly) : this(command, Assembly.LoadFrom(assembly))
        {
        }

        public string Command { get; private set; }

        public Assembly Assembly { get; private set; }
    }
}