namespace Sievo.Assignment.UnitTests.FoundationTests
{
    using Xunit;
    using Moq;
    using Sievo.Assignments.ConsoleApp.Core.Foundation.Entities;
    using Sievo.Assignments.ConsoleApp.Contracts.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Tests related to foundation logics.
    /// </summary>
    public class FoundationTests
    {
        

        [Fact]
        public void InputDataPassingTest()
        {
            var settings = MockSettings();

            var inputData = MockInputData();

            Assert.True(!inputData.Validate(new ValidationContext(inputData)).Any());

        }

        [Fact]
        public void InputDataWrongDateFormatTest()
        {
            var inputData = MockInputData();

            inputData.StartDate = "2012-06-01 00.00.00.000";

            Assert.True(inputData.Validate(new ValidationContext(inputData)).Any());
        }

        [Fact]
        public void InputDataWrongComplexityTest()
        {
            var inputData = MockInputData();

            inputData.StartDate = "None";

            Assert.True(inputData.Validate(new ValidationContext(inputData)).Any());
        }

        [Fact]
        public void InputDataWrongSavingAmountTest()
        {
            var inputData = MockInputData();

            inputData.StartDate = "4880.19956";

            Assert.True(inputData.Validate(new ValidationContext(inputData)).Any());
        }

        private static InputData MockInputData()
        {
            return new InputData(MockSettings()) { Project = "1", Description = "Description", StartDate = "2013-04-01 00:00:00.000", Category = "Category", Responsible = "Responsible", SavingsAmount = "11689.322459", Currency = "EU", Complexity = "Simple" };
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
    }
}
