using Microsoft.Extensions.Configuration;

namespace Dotnet.Migrator.Commands
{
    /// <inheritdoc />
    public abstract class ExecutionCommand : Command, IExecutionCommand
    {
        protected ExecutionCommand(string commandName, IConfiguration configuration = null) : base(commandName, configuration)
        {
        }

        /// <inheritdoc />
        public abstract void Execute();
    }
}