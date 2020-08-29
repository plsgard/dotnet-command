namespace Dotnet.Migrator.Commands
{
    public abstract class ExecutionCommand : Command, IExecutionCommand
    {
        protected ExecutionCommand(string commandName) : base(commandName)
        {
        }

        public abstract void Execute();
    }
}