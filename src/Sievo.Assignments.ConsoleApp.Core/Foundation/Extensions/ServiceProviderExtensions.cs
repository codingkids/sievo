namespace Sievo.Assignments.ConsoleApp.Core.Foundation.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Custom Extensions for <see cref="ServiceProvider"/>
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Adds logging with specified <see cref="LogLevel"/>
        /// </summary>
        /// <param name="serviceProvider"></param>
        internal static void ConfigureConsoleLogging(this ServiceProvider serviceProvider, LogLevel logLevel)
        {
            serviceProvider.GetService<ILoggerFactory>().AddConsole(logLevel);
        }

        /// <summary>
        /// Get Configured logger after intiating <see cref="Helpers.StartupHandler.AddConsolelogging"/>.
        /// </summary>
        /// <typeparam name="T">Type on which logging to be implemented.</typeparam>
        /// <param name="serviceProvider"><see cref="ServiceProvider"/></param>
        /// <param name="logLevel"><see cref="LogLevel"/></param>
        /// <returns><see cref="ILogger"/> for T.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ILogger GetLogger<T>(this ServiceProvider serviceProvider)
        {
            
            return serviceProvider.GetService<ILoggerFactory>().CreateLogger<T>();
        }
    }
}