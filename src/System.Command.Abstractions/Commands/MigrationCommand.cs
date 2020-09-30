using System.Threading;
using System.Threading.Tasks;

namespace System.Command.Commands
{
    /// <inheritdoc />
    public abstract class MigrationCommand : Command, IMigrationCommand
    {
        protected MigrationCommand(string commandName) : base(commandName)
        {
        }

        /// <inheritdoc />
        public abstract Task DownAsync(CancellationToken cancellationToken);

        /// <inheritdoc />
        public abstract Task UpAsync(CancellationToken cancellationToken);
    }
}