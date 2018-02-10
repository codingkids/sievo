namespace Sievo.Assignments.ConsoleApp.Contracts.ArgumentParser
{
    /// <summary>
    /// Contract for parser to grab the arguments, validations etc.
    /// </summary>
    public interface IArgumentParser
    {
        T GetArguments<T>(string[] args);
    }
}