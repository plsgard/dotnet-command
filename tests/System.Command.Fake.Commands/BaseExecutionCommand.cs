using System.Threading;
using System.Command.Commands;
using System.Threading.Tasks;

namespace System.Command.Fake.Commands
{
    public class BaseExecutionCommand : IExecutionCommand
    {
        public string Name { get; }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
