using CommandLine;

namespace ModularSystem.Сonfigurator.InputOptions
{
    [Verb("install", HelpText = "Install new module")]
    public class InstallOptions
    {
        [Value(0, HelpText = "File of package")]
        public string FilePath { get; set; }
    }
}
