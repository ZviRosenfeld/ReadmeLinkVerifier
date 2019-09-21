using CommandLine;

namespace ReadmeLinkVerifierConsoleApp
{
    class Options
    {
        [Value(0, HelpText = "The repository's full path.")]
        public string RepositoryPath { get; set; }

        [Option('r', Required = false, HelpText = "The relative path to the readme file. If this argument is omitted, we'll use the file named README.rm at the root of the repository.")]
        public string ReadmePath { get; set; }

        [Option('e', Required = false, HelpText = "Only print bad links")]
        public bool OnlyPrintBadLinks { get; set; }

        [Option('w', Required = false, HelpText = "Will wait for the user to input something before exiting")]
        public bool WaitBeforeExit { get; set; }
    }
}
