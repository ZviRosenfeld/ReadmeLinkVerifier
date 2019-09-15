using System;
using System.Collections.Generic;
using ReadmeLinkVerifier;

namespace ReadmeLinkVerifierConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var repositoryPath = args[0];
            var readmePath = args.Length > 1 ? args[1] : null;
            var facade = DefaultFacadeInitializer.GetFacade(repositoryPath, readmePath);
            var result = facade.VerifyLinks();
            PrintResults(result.UnknownLinks, nameof(result.UnknownLinks));
            PrintResults(result.GoodLinks, nameof(result.GoodLinks));
            PrintResults(result.BadLinks, nameof(result.BadLinks));

            Console.Read();
        }

        private static void PrintResults(ICollection<LinkDto> links, string title)
        {
            Console.WriteLine(title);
            foreach (var link in links)
                Console.WriteLine(link);
            Console.WriteLine();
        }
    }
}
