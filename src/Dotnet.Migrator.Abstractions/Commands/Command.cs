namespace Dotnet.Migrator.Commands
{
    public abstract class Command : ICommand
    {
        protected Command(string commandName)
        {
            Name = commandName ?? throw new System.ArgumentNullException(nameof(commandName));
        }

        public virtual string Name { get; }
    }
}