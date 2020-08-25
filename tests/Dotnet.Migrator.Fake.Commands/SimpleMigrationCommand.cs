using Dotnet.Migrator.Commands;

namespace Dotnet.Migrator.Fake.Commands
{
    public class SimpleMigrationCommand : MigrationCommand
    {
        public SimpleMigrationCommand() : base("simple:command")
        {
        }

        public override void Down()
        {
        }

        public override void Up()
        {
        }
    }
}
