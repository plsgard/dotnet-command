using System.Threading.Tasks;
using System.Command.Commands;
using System.Threading;

namespace System.Command.Fake.Commands
{
    public class SimpleExecutionCommand : ExecutionCommand
    {
        public SimpleExecutionCommand() : base("simple:command")
        {
        }

        public override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
