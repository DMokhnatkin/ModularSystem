using CommandLine;

namespace ModularSystem.Сonfigurator.InputOptions
{
    [Verb("remove", HelpText = "Remove module")]
    public class RemoveOptions
    {
        [Option('f', "file", Required = true, HelpText = "File of package")]
        public string FilePath { get; set; }
    }
}
