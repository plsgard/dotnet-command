using Dotnet.Migrator.Commands;
using Dotnet.Migrator.Migrations;

namespace Dotnet.Migrator.Fake.Commands
{
    public class BaseMigrationCommand : IMigrationCommand
    {
        Migration IMigrationCommand<Migration>.Migration { get; }

        void IMigrationCommand<Migration>.Down()
        {
            throw new System.NotImplementedException();
        }

        void IMigrationCommand<Migration>.Up()
        {
            throw new System.NotImplementedException();
        }
    }
}
