using System.Threading;
using System.Threading.Tasks;

namespace System.Command.Commands
{
    /// <summary>
    /// Represents a command to simply execute.
    /// </summary>
    public interface IExecutionCommand : ICommand
    {
        /// <summary>
        /// The operation called when the command is executed.
        /// </summary>
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}