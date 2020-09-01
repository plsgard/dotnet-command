namespace System.Command.Commands
{
    /// <summary>
    /// Represents a command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// The name of the command.
        /// Should be unique and use to identify the command.
        /// </summary>
        /// <value>A unique string.</value>
        string Name { get; }
    }
}