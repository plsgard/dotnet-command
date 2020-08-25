using System.IO;
using Dotnet.Migrator.Commands;
using Dotnet.Migrator.Parsers;
using Shouldly;
using Xunit;
using System.Linq;

namespace Dotnet.Migrator.Abstractions.Tests.Parsers
{
    public class AssemblyCommandParserTests : TestBase
    {
        private readonly AssemblyCommandParser _assemblyCommandParser;

        public AssemblyCommandParserTests()
        {
            var fakeProjectPath = Configuration.GetSection("Commands:Fake:ProjectPath").Value;
            var fakeDllPath = Directory.GetFiles(fakeProjectPath, "*.Fake.Commands.dll", SearchOption.AllDirectories).FirstOrDefault();
            _assemblyCommandParser = new AssemblyCommandParser(fakeDllPath);
        }

        [Fact]
        public void GetByName_Exists_Should_Return_Command()
        {
            var command = _assemblyCommandParser.GetByName("simple:command");
            command.ShouldNotBeNull();
            command.Migration.Name.ShouldBe("simple:command");
            command.ShouldBeAssignableTo<IMigrationCommand>();
        }

        [Fact]
        public void GetAll_Should_All_Commands_Inheriting_IMigrationCommand()
        {
            var commands = _assemblyCommandParser.GetAll();
            commands.ShouldNotBeNull();
            commands.ShouldNotBeEmpty();
            commands.Count.ShouldBe(2);
            foreach (var command in commands)
                command.ShouldBeAssignableTo<IMigrationCommand>();
        }
    }
}