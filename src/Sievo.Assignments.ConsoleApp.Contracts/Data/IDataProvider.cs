namespace Sievo.Assignments.ConsoleApp.Contracts.Data
{
    using Sievo.Assignments.ConsoleApp.Contracts.Entities;
    using System.Collections.Generic;

    public interface IDataProvider
    {
        bool TryParseDataFromSource(string filelocation, out IReadOnlyList<IInputData> inputData, out string errorDetails);
    }
}