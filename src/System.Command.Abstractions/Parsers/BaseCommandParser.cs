using System;

namespace System.Command.Parsers
{
    /// <summary>
    /// The base class of a command parser.
    /// </summary>
    public abstract class BaseCommandParser
    {
        protected BaseCommandParser(IServiceProvider serviceProvider = null)
        {
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// A service provider for dependency injection.
        /// </summary>
        /// <value></value>
        protected IServiceProvider ServiceProvider { get; }
    }
}