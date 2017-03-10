using CommandLine;

namespace PackPlugin
{
    public class Options
    {
        [Value(0, Required = true, HelpText = "File which contains configuration for pack")]
        public string ConfigFile { get; set; }

        [Option('o', HelpText = "Write result in this dir")]
        public string OutputDir { get; set; } = "Output";
    }
}
