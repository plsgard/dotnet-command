using Dotnet.Migrator.Commands;

namespace Dotnet.Migrator.Fake.Commands
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
