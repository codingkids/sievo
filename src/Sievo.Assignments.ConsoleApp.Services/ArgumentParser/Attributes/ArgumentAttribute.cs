namespace Sievo.Assignments.ConsoleApp.Entities.Attributes
{
    using System;

    /// <summary>
    /// For evaluating behaviour of argument
    /// </summary>
    /// <exception cref="InvalidCastException"></exception>
    [AttributeUsage(AttributeTargets.Property)]
    public class ArgumentAttribute : Attribute
    {
        public bool IsRequired;
        public char ShortName;
        public string LongName;
        public string HelpText;
        public object DefaultValue;

        public ArgumentAttribute(string HelpText, string LongName = "", char ShortName = '\0', bool IsRequired = false, object defualtValue = null)
        {
            this.IsRequired = IsRequired;
            this.ShortName = ShortName;
            this.LongName = LongName;
            this.HelpText = HelpText;
            this.DefaultValue = defualtValue;
        }
    }
}