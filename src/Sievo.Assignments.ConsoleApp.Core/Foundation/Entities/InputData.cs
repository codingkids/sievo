namespace Sievo.Assignments.ConsoleApp.Core.Foundation.Entities
{
    using System;
    using System.Globalization;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sievo.Assignments.ConsoleApp.Contracts.Configuration;
    using Sievo.Assignments.ConsoleApp.Contracts.Entities;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class InputData : IInputData, IValidatableObject
    {
        private IAppSettings _appSettings;
        public InputData(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public string Project { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public DateTime StartDateTime { get; set; }
        public string Category { get; set; }
        public string Responsible { get; set; }
        public string SavingsAmount { get; set; }
        public string Currency { get; set; }
        public string Complexity { get; set; }

        /// <summary>
        /// Validator for a single line data in file
        /// </summary>
        /// <param name="validationContext"><see cref="ValidationContext"/></param>
        /// <returns><see cref="IEnumerable{ValidationResult}"/> for validations.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            try
            {
                StartDateTime =  DateTime.ParseExact(StartDate, _appSettings.StartDateFormat, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                validationResults.Add(new ValidationResult("Invalid 'Start date' format found in input data."));
            }
            if (!_appSettings.DataComplexities.Contains(this.Complexity))
            {
                validationResults.Add(new ValidationResult($"Complexity can only be among: {_appSettings.DataComplexities.Aggregate((a, b) => $"{a},{b}").ToString()}"));
            };

            if (SavingsAmount!= "NULL" && !Regex.Match(SavingsAmount, _appSettings.SavingAmountValidityPattern).Success)
            {
                validationResults.Add(new ValidationResult("'Saving Amount' must have values in required format(6 decimals) e.g 141415.942696, 4880.199567"));
            }

            return validationResults;
        }
    }
}