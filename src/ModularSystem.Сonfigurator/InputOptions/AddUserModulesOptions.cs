using System.Collections.Generic;
using CommandLine;

namespace ModularSystem.Сonfigurator.InputOptions
{
    [Verb("addForUser", HelpText = "Add module to specifed user")]
    public class AddUserModulesOptions
    {
        [Value(0, Required = true)]
        public string UserId { get; set; }

        [Value(1, Required = true)]
        public IEnumerable<string> ModuleIdentities { get; set; }
    }
}
