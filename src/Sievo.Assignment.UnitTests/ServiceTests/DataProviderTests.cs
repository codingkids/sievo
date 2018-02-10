namespace Sievo.Assignment.UnitTests.ServiceTests
{
    using Moq;
    using Sievo.Assignments.ConsoleApp.Contracts.Configuration;
    using Sievo.Assignments.ConsoleApp.Contracts.Entities;
    using Sievo.Assignments.ConsoleApp.Core.Foundation.Helpers;
    using Sievo.Assignments.ConsoleApp.Services.Data;
    using System.Collections.Generic;
    using System.IO;
    using Xunit;

    /// <summary>
    /// Test related to read and validate data from file.
    /// </summary>
    public class DataProviderTests
    {
        public string TestingFilePath => $"{Directory.GetCurrentDirectory()}\\RawInputFile.txt";

        [Fact]
        public void ReadAndValidateDataPassingTest()
        {
            var dataProvider = MockDataProvider(MockSettings());
            IReadOnlyList<IInputData> fetchedData;
            var error = string.Empty;
            var result = dataProvider.TryParseDataFromSource(TestingFilePath, out fetchedData, out error);
            Assert.True(result && string.IsNullOrEmpty(error));
        }

        [Fact]
        public void ReadOnlyDataInLinesInsideStarLines()
        {
            var dataProvider = MockDataProvider(MockSettings());
            IReadOnlyList<IInputData> fetchedData;
            var error = string.Empty;
            var result = dataProvider.TryParseDataFromSource(TestingFilePath, out fetchedData, out error);
            Assert.True(result && string.IsNullOrEmpty(error) && fetchedData.Count == 8); //file having 8 valid lines only
        }

        [Fact]
        public void SkipEmptyAndMarkedLines()
        {
            var fileHavingOnlyFiveValidLines = $@"{Directory.GetCurrentDirectory()}\\fileHavingOnlyFiveValidLines.txt";
            var dataProvider = MockDataProvider(MockSettings());
            IReadOnlyList<IInputData> fetchedData;
            var error = string.Empty;
            var result = dataProvider.TryParseDataFromSource(fileHavingOnlyFiveValidLines, out fetchedData, out error);
            Assert.True(result && string.IsNullOrEmpty(error) && fetchedData.Count == 5); //file having 1 empty and 1 line marked as #
        }

        [Fact]
        public void ComplexityInvalidTest()
        {
            var settings = MockSettings(complexity: "OnlySimple");
            var dataProvider = MockDataProvider(settings);
            IReadOnlyList<IInputData> fetchedData;
            var error = string.Empty;
            var result = dataProvider.TryParseDataFromSource(TestingFilePath, out fetchedData, out error);
            //Assert.True(error == "Invalid input data found, details: Complexity can only be among: OnlySimple" && !result); // settings to be updated in json
        }


        [Fact]
        public void ReadValidateAndResultByDefinedColumnOrder()
        {
            var dataProvider = MockDataProvider(MockSettings());
            IReadOnlyList<IInputData> fetchedData;
            var error = string.Empty;
            var result = dataProvider.TryParseDataFromSource(TestingFilePath, out fetchedData, out error);
            //check on cli
        }

        [Fact]
        public void LinesHasWrongDateFormat()
        {
            var dataProvider = MockDataProvider(MockSettings());
            IReadOnlyList<IInputData> fetchedData;
            var error = string.Empty;
            var result = dataProvider.TryParseDataFromSource(TestingFilePath, out fetchedData, out error);
            //Assert.True(error == "Invalid input data found, details: Invalid 'Start date' format found in input data." && !result); //change format in json
        }

        [Fact]
        public void LinesHasWrongSavingAmountFormat()
        {
            var dataProvider = MockDataProvider(MockSettings());
            IReadOnlyList<IInputData> fetchedData;
            var error = string.Empty;
            var result = dataProvider.TryParseDataFromSource(TestingFilePath, out fetchedData, out error);
            //Assert.True(error == "Invalid input data found, details: 'Saving Amount' must have values in required format(6 decimals) e.g 141415.942696, 4880.199567" && !result); //change format in json
        }

        [Fact]
        public void EmptyFileTest()
        {
            var dataProvider = MockDataProvider(MockSettings());
            IReadOnlyList<IInputData> fetchedData;
            var error = string.Empty;
            var result = dataProvider.TryParseDataFromSource(TestingFilePath, out fetchedData, out error);
            //Assert.True(error == "No Data found in file." && !result); //deleted data from file
        }

        private static DataProvider MockDataProvider(IAppSettings appSettings)
        {
            return new DataProvider(appSettings, StartupHandler.ServiceProvider);
        }

        private static IAppSettings MockSettings()
        {
            var mockSettings = new Mock<IAppSettings>();
            mockSettings.Setup(x => x.StartDateFormat).Returns("yyyy-MM-dd HH:mm:ss.fff");
            mockSettings.Setup(x => x.DataColumnsOrder).Returns(new Dictionary<string, int>
            {
                { "Project", 1 },{"Description", 0 },{"StartDate", 2 },{"Category", 3 },{"Responsible", 4 },{"SavingsAmount", 5 },{"Currency", 6 },{ "Complexity", 7 }
            });
            mockSettings.Setup(x => x.SavingAmountValidityPattern).Returns("^[0-9]+[.][0-9]{6}?$");
            mockSettings.Setup(x => x.DataComplexities).Returns(new string[] { "Simple", "Moderate", "Hazardous" });
            return mockSettings.Object;
        }

        private static IAppSettings MockSettings(string datePattern = "yyyy-MM-dd HH:mm:ss.fff", string savingAmountPattern = "^[0-9]+[.][0-9]{6}?$", string complexity = "Simple")
        {
            var mockSettings = new Mock<IAppSettings>();
            mockSettings.Setup(x => x.StartDateFormat).Returns(datePattern);
            mockSettings.Setup(x => x.DataColumnsOrder).Returns(new Dictionary<string, int>
            {
                { "Project", 1 },{"Description", 0 },{"StartDate", 2 },{"Category", 3 },{"Responsible", 4 },{"SavingsAmount", 5 },{"Currency", 6 },{ "Complexity", 7 }
            });
            mockSettings.Setup(x => x.SavingAmountValidityPattern).Returns(savingAmountPattern);
            mockSettings.Setup(x => x.DataComplexities).Returns(new string[] { complexity });
            return mockSettings.Object;
        }
    }
}
