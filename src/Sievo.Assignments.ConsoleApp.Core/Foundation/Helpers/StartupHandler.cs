namespace Sievo.Assignments.ConsoleApp.Core.Foundation.Helpers
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;

    using Foundation.Extensions;

    /// <summary>
    /// Type responible for loading start up.
    /// </summary>
    public static class StartupHandler
    {

        private static ServiceProvider _serviceProvider;

        /// <summary>
        /// Sets up boot level service provider
        /// </summary>
        public static ServiceProvider ServiceProvider => _serviceProvider ?? (_serviceProvider = new ServiceCollection().ConfigureDI());

        /// <summary>
        /// Add/Configure logging with configured <see cref="LogLevel"/> for application.
        /// </summary>
        public static void AddConsolelogging()
        {
            //TODO: can read log level from configuration.
            ServiceProvider.ConfigureConsoleLogging(LogLevel.Debug);
        }
    }
}