namespace System.Command.Commands
{
    /// <inheritdoc />
    public abstract class MigrationCommand : Command, IMigrationCommand
    {
        protected MigrationCommand(string commandName) : base(commandName)
        {
        }

        /// <inheritdoc />
        public abstract void Down();

        /// <inheritdoc />
        public abstract void Up();
    }
}