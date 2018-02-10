namespace Sievo.Assignments.ConsoleApp.Services.ArgumentParser
{
    using CommandLine;
    using Contracts.ArgumentParser;

    /// <summary>
    /// Arguments to be used by application.
    /// </summary>
    public class Arguments : IArguments
    {
        [Option('f', "file", Required = true, HelpText = "Input file to be processed.")]
        public string File { get; set; }
        [Option('s', "sortbystartdate", HelpText = "Sort results by column 'Start date' in ascending order.", Default = false)]
        public bool SortByStartDate { get; set; }
        [Option('p', "project", HelpText = "<project id> for filter results by column 'Project'.", Default = "")]
        public string Project { get; set; }
        public bool IsValid { get; set; }
        public string[] Errors { get; set; }
    }
}