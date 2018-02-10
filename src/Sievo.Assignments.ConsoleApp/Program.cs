namespace Sievo.Assignments.ConsoleApp
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;

    using Core.Foundation.Helpers;
    using Core.Foundation.Extensions;
    using Contracts.ArgumentParser;
    using Services.ArgumentParser;
    using Sievo.Assignments.ConsoleApp.Contracts.Entities;
    using Sievo.Assignments.ConsoleApp.Contracts.Data;
    using Sievo.Assignments.ConsoleApp.Contracts.Configuration;

    class Program
    {
        #region Start up
        private static ServiceProvider _serviceProvider = StartupHandler.ServiceProvider;
        private static ILogger _logger;
        private static IArgumentParser _argumentsParser;
        private static IDataProvider _dataProvider;

        /// <summary>
        /// App Initialization.
        /// </summary>
        private static void ApplicationIntialization()
        {
            StartupHandler.AddConsolelogging();
            _logger = _serviceProvider.GetLogger<Program>();
            _argumentsParser = _serviceProvider.GetService<IArgumentParser>();
            _dataProvider = _serviceProvider.GetService<IDataProvider>();
        }
        #endregion

        static void Main(string[] args)
        {
            ApplicationIntialization();

            var arguments = _argumentsParser.GetArguments<Arguments>(args);
            var error = string.Empty;
            IReadOnlyList<IInputData> data;
            if (arguments.IsValid && _dataProvider.TryParseDataFromSource(arguments.File, out data, out error))
            {
                var filteredData = FilterDataByArguments(data, arguments);
                var result = filteredData.ToOutputData(_serviceProvider.GetService<IAppSettings>());
                Console.WriteLine(result);
            }
            else if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine(error);
            }
#if DEBUG
            Console.ReadKey();
#endif
        }

        /// <summary>
        /// Filter data by arguments behaviour.
        /// </summary>
        /// <param name="data"><see cref="List{IInputData}"/>Data tp filter</param>
        /// <param name="arguments">Provided <see cref="Arguments"/></param>
        /// <returns>Resultant <see cref="List{IInputData}"/></returns>
        private static IReadOnlyList<IInputData> FilterDataByArguments(IReadOnlyList<IInputData> data, Arguments arguments)
        {
            var modifiedData = data;
            if (arguments.SortByStartDate)
            {
                modifiedData = data.OrderBy(i => i.StartDateTime).ToList();
            }
            if (!string.IsNullOrEmpty(arguments.Project))
            {
                modifiedData = modifiedData.Where(i => i.Project == arguments.Project).ToList();
            }
            return modifiedData;
        }
    }
}