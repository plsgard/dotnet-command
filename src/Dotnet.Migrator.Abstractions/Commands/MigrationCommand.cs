namespace Dotnet.Migrator.Commands
{
    public abstract class MigrationCommand : Command, IMigrationCommand
    {
        protected MigrationCommand(string commandName) : base(commandName)
        {
        }

        public abstract void Down();
        public abstract void Up();
    }
}