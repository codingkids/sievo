namespace Sievo.Assignments.ConsoleApp.Contracts.ArgumentParser
{
    /// <summary>
    /// Arguments to be used for application.
    /// </summary>
    public interface IArguments
    {
        /// <summary>
        /// Input file location.
        /// </summary>
        string File { get; set; }
        /// <summary>
        /// Sort by Start date True/False.
        /// </summary>
        bool SortByStartDate { get; set; }
        /// <summary>
        /// Filter by project value.
        /// </summary>
        string Project { get; set; }
    }
}
