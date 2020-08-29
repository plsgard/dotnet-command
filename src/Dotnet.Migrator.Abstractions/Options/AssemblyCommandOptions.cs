using System.Reflection;
namespace Dotnet.Migrator.Options
{
    /// <summary>
    /// Represents the options used to call a command through an assembly.
    /// </summary>
    public class AssemblyCommandOptions : CommandOptions
    {
        public AssemblyCommandOptions(string name, Assembly assembly) : base(name)
        {
            Assembly = assembly ?? throw new System.ArgumentNullException(nameof(assembly));
        }

        public AssemblyCommandOptions(string name, string assembly) : this(name, Assembly.LoadFrom(assembly))
        {
        }

        /// <summary>
        /// The assembly containing the command to call.
        /// </summary>
        /// <value></value>
        public Assembly Assembly { get; private set; }
    }
}