using CommandLine;

namespace ModularSystem.Сonfigurator.InputOptions
{
    [Verb("list", HelpText = "Get all modules")]
    public class ListOptions
    {
        [Option('u', HelpText = "Get list of modules for user")]
        public string UserId { get; set; }
    }
}
