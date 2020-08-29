namespace Dotnet.Command.Options
{
    /// <summary>
    /// Represents all the options can be provided to call a command.
    /// </summary>
    public abstract class CommandOptions
    {
        protected CommandOptions()
        {
        }

        protected CommandOptions(string name)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// The unique name of the command to call.
        /// </summary>
        /// <value></value>
        public string Name { get; private set; }
    }
}