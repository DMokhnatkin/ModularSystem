using System;
using System.Linq;

namespace ModularSystem.Common.Dependencies
{
    /// <summary>
    /// Complex error result for list of modules check.
    /// </summary>
    public class ComplexError : ICheckResult
    {
        public ComplexError(ICheckResult[] errors)
        {
            Errors = errors;
        }

        public ICheckResult[] Errors { get; }

        /// <inheritdoc />
        public bool IsSuccess => false;

        /// <inheritdoc />
        public string GetMessage()
        {
            return
                $"Some errors occured during resolving. [{string.Concat(Errors.Select(x => $"{x.GetMessage()}{Environment.NewLine};"))}]";
        }
    }
}
