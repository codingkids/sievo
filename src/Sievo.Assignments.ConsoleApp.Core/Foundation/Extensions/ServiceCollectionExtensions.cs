namespace Sievo.Assignments.ConsoleApp.Core.Foundation.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Sievo.Assignments.ConsoleApp.Contracts.ArgumentParser;
    using Sievo.Assignments.ConsoleApp.Contracts.Configuration;
    using Sievo.Assignments.ConsoleApp.Contracts.Data;
    using Sievo.Assignments.ConsoleApp.Contracts.Entities;
    using Sievo.Assignments.ConsoleApp.Core.Foundation.Configuration;
    using Sievo.Assignments.ConsoleApp.Core.Foundation.Entities;
    using Sievo.Assignments.ConsoleApp.Services.ArgumentParser;
    using Sievo.Assignments.ConsoleApp.Services.Data;
    using System.IO;

    /// <summary>
    /// Custom Extensions for <see cref="ServiceCollection"/>
    /// </summary>
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register/Configure Services before startup.
        /// </summary>
        /// <param name="serviceCollection"><see cref="ServiceCollection"/> container to be used.</param>
        internal static ServiceProvider ConfigureDI(this ServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddLogging()
                .InjectConfigurations()
                .InjectCustomRegistations()
                .BuildServiceProvider();
        }

        /// <summary>
        /// Register Custom services here.
        /// </summary>
        /// <param name="serviceCollection"><see cref="IServiceCollection"/> to which custom scoped services injected.</param>
        /// <returns></returns>
        private static IServiceCollection InjectCustomRegistations(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IInputData, InputData>();
            serviceCollection.AddTransient<IArgumentParser, ArgumentParser>();
            serviceCollection.AddTransient<IDataProvider, DataProvider>();
            return serviceCollection;
        }

        /// <summary>
        /// Load & Inject settings resides in json file
        /// </summary>
        /// <param name="serviceCollection"><see cref="IServiceCollection"/></param>
        /// <returns></returns>
        private static IServiceCollection InjectConfigurations(this IServiceCollection serviceCollection)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var section = builder.Build().GetSection("AppSettings");
            var settings = new AppSettings();
            section.Bind(settings);
            serviceCollection.AddSingleton<IAppSettings>(settings);
            return serviceCollection;
        }
    }
}