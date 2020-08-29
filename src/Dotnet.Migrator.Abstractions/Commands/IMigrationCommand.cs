namespace Dotnet.Migrator.Commands
{
    public interface IMigrationCommand : ICommand
    {
        void Up();

        void Down();
    }
}