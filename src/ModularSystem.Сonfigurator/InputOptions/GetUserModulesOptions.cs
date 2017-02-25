using CommandLine;

namespace ModularSystem.Сonfigurator.InputOptions
{
    [Verb("getForUser", HelpText = "Get modules added to specifed user")]
    public class GetUserModulesOptions
    {
        [Value(0, Required = true)]
        public string UserId { get; set; }
    }
}
