namespace Sievo.Assignments.ConsoleApp.Core.Foundation.Configuration
{
    using System.Collections.Generic;
    using Contracts.Configuration;

    /// <summary>
    /// Settings to be configured.
    /// </summary>
    public class AppSettings : IAppSettings
    {
        public string[] DataComplexities { get; set; }
        public Dictionary<string, int> DataColumnsOrder { get; set; }
        public string StartDateFormat { get; set; }
        public string SavingAmountValidityPattern { get; set; }
    }
}