namespace Sievo.Assignments.ConsoleApp.Core.Foundation.Extensions
{
    using Sievo.Assignments.ConsoleApp.Contracts.Configuration;
    using Sievo.Assignments.ConsoleApp.Contracts.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;

    public static class InputDataExtensions
    {
        public static string ToOutputData(this IReadOnlyList<IInputData> inputData, IAppSettings appSettings)
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.AppendLine(GetHeader(appSettings));
            inputData.All((dataLine) =>
            {
                resultBuilder.AppendLine(GetOutputLine(dataLine, appSettings));
                return true;
            });
            return resultBuilder.ToString();
        }

        private static string GetHeader(IAppSettings appSettings)
        {
            return appSettings.DataColumnsOrder.OrderBy(i => i.Value).Select(i => $"{i.Key}").Aggregate((a, b) => $"{a}\t{b}").ToString();
        }

        private static string GetOutputLine(IInputData data, IAppSettings appSettings)
        {
            var dataByOrder = new Dictionary<int, string>();
            data.GetType().GetProperties().ToList().ForEach((propertyInfo) =>
            {
                var orderIfFound = appSettings.DataColumnsOrder.Where(i => i.Key == propertyInfo.Name).FirstOrDefault();
                if (!orderIfFound.Equals(default(KeyValuePair<string, int>)))
                {
                    var value = propertyInfo.GetValue(data).ToString();
                    dataByOrder.Add(orderIfFound.Value, propertyInfo.GetValue(data).ToString());
                }
            });
            return dataByOrder.OrderBy(i => i.Key).Select(i => i.Value).Aggregate((a, b) => $"{a}\t{b}").ToString();
        }
    }
}