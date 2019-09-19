using System;
using System.Collections.Generic;
using CommandLine;
using ReadmeLinkVerifier;

namespace ReadmeLinkVerifierConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithNotParsed(e => Environment.Exit(-1))
                .WithParsed(RunVerifyLinks);

            Console.Read();
        }

        private static void RunVerifyLinks(Options options)
        {
            try
            {
                var verifyLinksService = ServiceBuilder.GetVerifyLinksService(options.RepositoryPath, options.ReadmePath);
                var result = verifyLinksService.VerifyLinks();
                PrintResults(result.GoodLinks, nameof(result.GoodLinks));
                PrintResults(result.UnknownLinks, nameof(result.UnknownLinks));
                PrintResults(result.BadLinks, nameof(result.BadLinks));
            }
            catch (Exception e)
            {
                Console.WriteLine("Oops: " + e.Message);
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
