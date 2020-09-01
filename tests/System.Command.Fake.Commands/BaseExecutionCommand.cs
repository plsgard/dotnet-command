using System.Command.Commands;

namespace System.Command.Fake.Commands
{
    public class BaseExecutionCommand : IExecutionCommand
    {
        public string Name { get; }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
