namespace Dotnet.Migrator.Migrations
{
    public class Migration
    {
        protected Migration()
        {
        }

        public Migration(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}