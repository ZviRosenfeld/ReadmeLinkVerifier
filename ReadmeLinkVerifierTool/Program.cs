using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using ReadmeLinkVerifier;
using ReadmeLinkVerifier.Services;

namespace ReadmeLinkVerifierConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithNotParsed(e => Environment.Exit(-1))
                .WithParsed(RunVerifyLinks);
        }

        private static void RunVerifyLinks(Options options)
        {
            try
            {
                var verifyLinksService = new LinkVerifierService(options.RepositoryPath, options.ReadmePath);
                var result = verifyLinksService.VerifyLinks();
                if (!options.OnlyPrintBadLinks)
                {
                    PrintResults(result.GoodLinks, nameof(result.GoodLinks));
                    PrintResults(result.UnknownLinks, nameof(result.UnknownLinks));
                }
                PrintResults(result.BadLinks, nameof(result.BadLinks));

                if (result.BadLinks.Any())
                    Environment.Exit(-2);
            }
            catch (Exception e)
            {
                Console.WriteLine("Oops: " + e.Message);
                Environment.Exit(-1);
            }
        }

        private static void PrintResults(IEnumerable<LinkDto> links, string title)
        {
            Console.WriteLine(title);
            foreach (var link in links)
                Console.WriteLine(link);
            Console.WriteLine();
        }
    }
}
