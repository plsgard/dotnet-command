using System.Collections.Generic;
using System.Threading.Tasks;
using Dotnet.Migrator.Commands;

namespace Dotnet.Migrator.Parsers
{
    public interface ICommandParser
    {
        Task<IList<IMigrationCommand>> GetAllAsync();
        IList<IMigrationCommand> GetAll();

        Task<IMigrationCommand> GetByNameAsync(string commandName);
        IMigrationCommand GetByName(string commandName);
    }
}