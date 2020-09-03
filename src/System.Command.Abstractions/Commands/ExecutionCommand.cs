namespace System.Command.Commands
{
    /// <inheritdoc />
    public abstract class ExecutionCommand : Command, IExecutionCommand
    {
        protected ExecutionCommand(string commandName) : base(commandName)
        {
        }

        /// <inheritdoc />
        public abstract void Execute();
    }
}