using System.Collections.Generic;
using System.Threading.Tasks;
using Dotnet.Migrator.Commands;

namespace Dotnet.Migrator.Parsers
{
    public interface ICommandParser : ICommandParser<ICommand> { }

    public interface ICommandParser<TCommand> where TCommand : ICommand
    {
        Task<IList<TCommand>> GetAllAsync();
        IList<TCommand> GetAll();

        Task<TCommand> GetByNameAsync(string commandName);
        TCommand GetByName(string commandName);
    }
}