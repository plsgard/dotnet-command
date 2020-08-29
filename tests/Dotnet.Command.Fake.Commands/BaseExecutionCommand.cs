using Dotnet.Command.Commands;

namespace Dotnet.Command.Fake.Commands
{
    public class BaseExecutionCommand : IExecutionCommand
    {
        public string Name { get; }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
