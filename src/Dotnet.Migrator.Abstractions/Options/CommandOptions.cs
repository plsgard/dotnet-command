using System.Reflection;
namespace Dotnet.Migrator.Options
{
    public class CommandOptions
    {
        public CommandOptions(string name, Assembly assembly)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            Assembly = assembly ?? throw new System.ArgumentNullException(nameof(assembly));
        }

        public CommandOptions(string name, string assembly) : this(name, Assembly.LoadFrom(assembly))
        {
        }

        public string Name { get; private set; }

        public Assembly Assembly { get; private set; }
    }
}