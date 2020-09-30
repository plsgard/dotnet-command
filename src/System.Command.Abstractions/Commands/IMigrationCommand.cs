using System.Threading;
using System.Threading.Tasks;

namespace System.Command.Commands
{
    /// <summary>
    /// Represents a migration command.
    /// A migration can be applied or can be reverted.
    /// </summary>
    public interface IMigrationCommand : ICommand
    {
        /// <summary>
        /// The operation called when a migration command is applied.
        /// </summary>
        Task UpAsync(CancellationToken cancellationToken);

        /// <summary>
        /// The operation called when a migration command is reverted.
        /// </summary>
        Task DownAsync(CancellationToken cancellationToken);
    }
}