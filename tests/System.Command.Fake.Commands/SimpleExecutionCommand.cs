using System.Command.Commands;

namespace System.Command.Fake.Commands
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
