using Microsoft.Extensions.Configuration;

namespace Dotnet.Command.Commands
{
    /// <inheritdoc />
    public abstract class MigrationCommand : Command, IMigrationCommand
    {
        protected MigrationCommand(string commandName, IConfiguration configuration = null) : base(commandName, configuration)
        {
        }

        /// <inheritdoc />
        public abstract void Down();

        /// <inheritdoc />
        public abstract void Up();
    }
}