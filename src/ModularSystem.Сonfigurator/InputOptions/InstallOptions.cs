using CommandLine;

namespace ModularSystem.Сonfigurator.InputOptions
{
    [Verb("install", HelpText = "Install new module")]
    public class InstallOptions
    {
        [Option('f', "file", Required = true, HelpText = "File of package")]
        public string FilePath { get; set; }
    }
}
