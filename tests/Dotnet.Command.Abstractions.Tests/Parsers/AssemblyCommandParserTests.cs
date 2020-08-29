using System.IO;
using Dotnet.Command.Commands;
using Dotnet.Command.Parsers;
using Shouldly;
using Xunit;
using System.Linq;

namespace Dotnet.Command.Abstractions.Tests.Parsers
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
            command.Name.ShouldBe("simple:command");
            command.ShouldBeAssignableTo<ICommand>();
            command.ShouldBeAssignableTo<Commands.Command>();
            command.ShouldBeAssignableTo<IExecutionCommand>();
            command.ShouldBeAssignableTo<ExecutionCommand>();
        }

        [Fact]
        public void GetAll_Should_All_Commands_Inheriting_Command()
        {
            var commands = _assemblyCommandParser.GetAll();
            commands.ShouldNotBeNull();
            commands.ShouldNotBeEmpty();
            commands.Count.ShouldBe(2);
            foreach (var command in commands)
                command.ShouldBeAssignableTo<ICommand>();
        }

        [Fact]
        public void GetByName_Should_Have_Configuration()
        {
            var command = _assemblyCommandParser.GetByName("simple:command");
            command.ShouldNotBeNull();
            var currentCommand = command.ShouldBeAssignableTo<Commands.Command>();
            currentCommand.Configuration.ShouldNotBeNull();
            var configurationSection = currentCommand.Configuration.GetSection("Toto:Tata:Path");
            configurationSection.ShouldNotBeNull();
            configurationSection.Value.ShouldBe("/path/");
        }
    }
}