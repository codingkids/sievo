namespace Sievo.Assignments.ConsoleApp.Services.ArgumentParser
{
    using System.Linq;
    using System.Collections.Generic;
    using CommandLine;
    using Contracts.ArgumentParser;
    using System;

    /// <summary>
    /// Parser for Arguments.
    /// </summary>
    public class ArgumentParser : IArgumentParser
    {
        public T GetArguments<T>(string[] args)
        {
            var result = ParseArguments<T>(args);
            if (result.Tag == ParserResultType.Parsed)
            {
                return ((Parsed<T>)result).Value;
            }
            else
            {
                var errors = ((NotParsed<T>)result).Errors.Select(i => i.Tag.ToString());
                return (T)Convert.ChangeType(new Arguments { Errors = errors.ToArray(), IsValid = false }, typeof(T));
            }
        }

        private ParserResult<TResult> ParseArguments<TResult>(string[] args)
        {
            var argsResult = Parser.Default.ParseArguments<TResult>(args)
                .WithParsed(options => RunOptionsAndReturnExitCode(options))
                .WithNotParsed((errors) => HandleParseError(errors));
            return argsResult;
        }

        private void RunOptionsAndReturnExitCode<T>(T options)
        {
            var args = options as Arguments;
            if (args != null)
                args.IsValid = true;
        }

        private Arguments HandleParseError(IEnumerable<Error> errs)
        {
            return new Arguments { Errors = errs.Select(i => i.Tag.ToString()).ToArray() };
        }
    }
}