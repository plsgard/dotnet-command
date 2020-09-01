using System.Collections.Generic;
using System.Threading.Tasks;
using System.Command.Commands;

namespace System.Command.Parsers
{
    /// <inheritdoc />
    public interface ICommandParser : ICommandParser<ICommand> { }

    /// <summary>
    /// Represents a command parser.
    /// </summary>
    public interface ICommandParser<TCommand> where TCommand : ICommand
    {
        /// <summary>
        /// Returns all commands found by the parser.
        /// </summary>
        /// <returns>A list of commands of type TCommand /></returns>
        Task<IList<TCommand>> GetAllAsync();

        /// <inheritdoc cref="GetAllAsync" />
        IList<TCommand> GetAll();

        /// <summary>
        /// Returns the command with the provided name.
        /// </summary>
        /// <param name="commandName">The name of the command to find.</param>
        /// <returns>A command with the provided name, and of type TCommand /></returns>
        Task<TCommand> GetByNameAsync(string commandName);

        /// <inheritdoc cref="GetByNameAsync" />
        TCommand GetByName(string commandName);
    }
}