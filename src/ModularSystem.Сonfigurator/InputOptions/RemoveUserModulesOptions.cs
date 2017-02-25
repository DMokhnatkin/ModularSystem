using System.Collections.Generic;
using CommandLine;

namespace ModularSystem.Сonfigurator.InputOptions
{
    [Verb("remForUser", HelpText = "Remove module for specifed user")]
    public class RemoveUserModulesOptions
    {
        [Value(0, Required = true)]
        public string UserId { get; set; }

        [Value(1, Required = true)]
        public IEnumerable<string> ModuleIdentities { get; set; }
    }
}
