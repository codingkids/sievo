namespace Sievo.Assignments.ConsoleApp.Services.Data
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Sievo.Assignments.ConsoleApp.Contracts.Configuration;
    using Sievo.Assignments.ConsoleApp.Contracts.Data;
    using Sievo.Assignments.ConsoleApp.Contracts.Entities;
    using System.Text;

    public class DataProvider : IDataProvider
    {
        private IAppSettings _appSettings;
        private IServiceProvider _serviceProvider;

        public DataProvider(IAppSettings appSettings, IServiceProvider serviceProvider)
        {
            _appSettings = appSettings;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Data extractor with validations.
        /// </summary>
        /// <param name="filelocation">File Location</param>
        /// <param name="inputData"><see cref="IReadOnlyCollection{IInputData}"/>Result object to be intialized if successful read.</param>
        /// <param name="errorDetails"><see cref="string[]"/> to which error details to be added, if any.</param>
        /// <returns></returns>
        public bool TryParseDataFromSource(string filelocation, out IReadOnlyList<IInputData> inputData, out string errorDetails)
        {
            var error = string.Empty;
            var resultData = new List<IInputData>();
            var isValid = false;
            try
            {
                if (Directory.Exists(Path.GetDirectoryName(filelocation)))
                {
                    var lines = new List<string>();
                    using (var file = new StreamReader($@"{filelocation}", Encoding.UTF8))
                    {
                        string line;
                        while ((line = file.ReadLine()) != null)
                        {
                            lines.Add(line);
                        }
                    }
                    var startEndLines = Enumerable.Range(0, lines.Count).Where(l => lines[l].Contains("****")).ToList(); //actual data line indexes
                    if (startEndLines.Count != 0)
                    {
                        var dataLines = Enumerable.Range(startEndLines.First() + 1, startEndLines.Last() - 1 - startEndLines.First())
                            .Select(index => lines[index]).Where(l => !string.IsNullOrEmpty(l) && !l.StartsWith('#')).ToList();

                        dataLines.RemoveAt(0); //Removing header from actual data
                        var data = new List<IInputData>(); var isDataValid = false; var validationErrors = new List<ValidationResult>();

                        dataLines.Select(i => i.Split('\t', StringSplitOptions.RemoveEmptyEntries)).ToList().All((i) =>
                        {
                            if (i.Length != 8)
                            {
                                validationErrors.Add(new ValidationResult("Data must have values for all columns."));
                                return isDataValid = false;
                            }
                            var lineData = (IInputData)_serviceProvider.GetService(typeof(IInputData));
                            lineData.Project = i[GetDataIndexByPrpertyName("Project")];
                            lineData.Description = i[GetDataIndexByPrpertyName("Description")];
                            lineData.StartDate = i[GetDataIndexByPrpertyName("StartDate")];
                            lineData.Category = i[GetDataIndexByPrpertyName("Category")];
                            lineData.Responsible = i[GetDataIndexByPrpertyName("Responsible")];
                            lineData.SavingsAmount = i[GetDataIndexByPrpertyName("SavingsAmount")];
                            lineData.Currency = i[GetDataIndexByPrpertyName("Currency")];
                            lineData.Complexity = i[GetDataIndexByPrpertyName("Complexity")];
                            validationErrors = lineData.Validate(new ValidationContext(lineData)).ToList();
                            if (!validationErrors.Any())
                            {
                                if (lineData.SavingsAmount == "NULL") { lineData.SavingsAmount = string.Empty; }
                                if (lineData.Currency == "NULL") { lineData.Currency = string.Empty; }
                                data.Add(lineData);
                                return isDataValid = true;
                            };
                            return isDataValid = false; //terminate loop with setting data validation to false.
                        });

                        if (isDataValid)
                        {
                            isValid = true;
                        }
                        else
                        {
                            error = $"Invalid input data found, details: {validationErrors.Select(e => e.ErrorMessage).Aggregate((a, b) => $"{a},{b}")}";
                        }
                        resultData = data;
                    }
                    else { error = "No Data found in file."; }
                    
                }
                else { error = "Directory not found!"; }

            }
            catch (Exception e)
            {
                error = e.Message;
            }
            errorDetails = error;
            inputData = resultData;
            return isValid;
        }

        /// <summary>
        /// For evaluating the order of colums from configuration.
        /// </summary>
        /// <param name="propertyName">Column Name</param>
        /// <returns>Configured Index</returns>
        private int GetDataIndexByPrpertyName(string propertyName)
        {
            return _appSettings.DataColumnsOrder.FirstOrDefault(i => i.Key == propertyName).Value;
        }
    }
}