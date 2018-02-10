namespace Sievo.Assignments.ConsoleApp.Entities
{
    using Sievo.Assignments.ConsoleApp.Entities.Attributes;

    /// <summary>
    /// Arguments to be used by application.
    /// </summary>
    public class Arguments
    {
        [Argument("Input file to be processed.", "read", 'r', true)]
        public string File { get; set; }
        [Argument("Sort results by column 'Start date' in ascending order.", DefaultValue = false)]
        public bool SortByStartDate { get; set; }
        [Argument("<project id> for filter results by column 'Project'.", DefaultValue = "")]
        public string Project { get; set; }
    }
}