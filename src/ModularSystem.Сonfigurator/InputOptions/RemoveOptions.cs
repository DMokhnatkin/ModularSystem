using CommandLine;

namespace ModularSystem.Сonfigurator.InputOptions
{
    [Verb("remove", HelpText = "Remove module")]
    public class RemoveOptions
    {
        [Value(0, HelpText = "Name of module")]
        public string Name { get; set; }

        [Value(1, HelpText = "Version of module")]
        public string Version { get; set; }

        [Value(2, HelpText = "Type of module")]
        public string Type { get; set; }
    }
}
