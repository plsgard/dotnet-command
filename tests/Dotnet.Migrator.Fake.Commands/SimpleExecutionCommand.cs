using Dotnet.Migrator.Commands;

namespace Dotnet.Migrator.Fake.Commands
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
