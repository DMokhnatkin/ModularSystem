using CommandLine;

namespace ModularSystem.Сonfigurator.InputOptions
{
    [Verb("install", HelpText = "Install new module")]
    public class InstallOptions
    {
        [Option('p', HelpText = "File of package")]
        public string PackagePath { get; set; }

        /*
        [Option("u", HelpText = "Install (if not exists) and add to user dependecies")]
        public string UserId { get; set; }*/
    }
}
