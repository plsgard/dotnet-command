namespace Dotnet.Migrator.Commands
{
    public interface IExecutionCommand : ICommand
    {
        void Execute();
    }
}