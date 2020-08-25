using Dotnet.Migrator.Migrations;

namespace Dotnet.Migrator.Commands
{
    public interface IMigrationCommand : IMigrationCommand<Migration> { }

    public interface IMigrationCommand<TMigration> where TMigration : Migration
    {
        TMigration Migration { get; }

        void Up();

        void Down();
    }
}