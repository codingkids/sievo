namespace Sievo.Assignments.ConsoleApp.Contracts.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public interface IInputData
    {
        string Project { get; set; }
        string Description { get; set; }
        string StartDate { get; set; }
        System.DateTime StartDateTime { get; set; }
        string Category { get; set; }
        string Responsible { get; set; }
        string SavingsAmount { get; set; }
        string Currency { get; set; }
        string Complexity { get; set; }
        IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }
}