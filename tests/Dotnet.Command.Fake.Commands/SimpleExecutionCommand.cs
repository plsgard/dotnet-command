using Dotnet.Command.Commands;

namespace Dotnet.Command.Fake.Commands
{
    public class SimpleExecutionCommand : ExecutionCommand
    {
        public SimpleExecutionCommand() : base("simple:command")
        {
        }

        public override void Execute()
        {
        }
    }
}
