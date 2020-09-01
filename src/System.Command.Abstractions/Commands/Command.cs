using System.Reflection;
using System.IO;
using System;
using Microsoft.Extensions.Configuration;

namespace System.Command.Commands
{
    /// <inheritdoc />
    public abstract class Command : ICommand
    {
        protected Command(string commandName, IConfiguration configuration = null)
        {
            Name = commandName ?? throw new ArgumentNullException(nameof(commandName));
            Configuration = configuration ?? BuildConfiguration();
        }

        /// <inheritdoc />
        public virtual string Name { get; }

        /// <summary>
        /// The configuration who can be used by the command implementation.
        /// </summary>
        /// <value></value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// If no configuration is provided, try to create a configuration based on an appsettings.json file stored in the command assembly.
        /// </summary>
        /// <returns></returns>
        protected virtual IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetAssembly(GetType()).Location))
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }
    }
}