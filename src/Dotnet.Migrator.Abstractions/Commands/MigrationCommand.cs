using Dotnet.Migrator.Migrations;

namespace Dotnet.Migrator.Commands
{
    public abstract class MigrationCommand<TMigration> : IMigrationCommand<TMigration> where TMigration : Migration
    {
        public abstract TMigration Migration { get; }

        public abstract void Down();
        public abstract void Up();
    }

    public abstract class MigrationCommand : MigrationCommand<Migration>, IMigrationCommand
    {
        public MigrationCommand(string migrationName)
        {
            Migration = new Migration(migrationName);
        }

        public override Migration Migration { get; }
    }
}