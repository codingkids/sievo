namespace Sievo.Assignments.ConsoleApp.Contracts.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// Application level settings.
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// Complexities allowed
        /// </summary>
        string[] DataComplexities { get; set; }
        /// <summary>
        /// Colums orders according to data inputs.
        /// </summary>
        Dictionary<string, int> DataColumnsOrder { get; set; }
        /// <summary>
        /// Date format to validate StartDate.
        /// </summary>
        string StartDateFormat { get; set; }
        /// <summary>
        /// Saving amount regex validator.
        /// </summary>
        string SavingAmountValidityPattern { get; set; }
    }
}