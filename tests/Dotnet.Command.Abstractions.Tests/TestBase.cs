using System;
using Microsoft.Extensions.Configuration;

namespace Dotnet.Command.Abstractions.Tests
{
    public class TestBase
    {
        protected IConfigurationRoot Configuration { get; }

        public TestBase()
        {
            Configuration = BuildConfiguration(Environment.CurrentDirectory);
        }

        protected virtual IConfigurationRoot BuildConfiguration(string testDirectory)
        {
            return new ConfigurationBuilder()
                .SetBasePath(testDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }
    }
}