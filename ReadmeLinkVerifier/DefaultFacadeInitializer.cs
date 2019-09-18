using System.Collections.Generic;
using System.IO;
using ReadmeLinkVerifier.Interfaces;
using ReadmeLinkVerifier.LinkRules;

namespace ReadmeLinkVerifier
{
    public static class DefaultFacadeInitializer
    {
        private const string README_DEFAILT_PATH = "README.md";

        public static Facade GetFacade(string repositoryPath, string readmeRelativePath = null)
        {
            var repository = new FileRepository(repositoryPath);
            readmeRelativePath = readmeRelativePath ?? README_DEFAILT_PATH;
            var readmeFilePath = Path.Combine(repository.GetRepositoryPath(), readmeRelativePath);
            var readmeFile = new ReadmeFile(readmeFilePath, readmeRelativePath);
            var rules = new List<ILinkRule>
            {
                new RepositoryLinkRule(repository, readmeFile),
                new ReadmeFileLinkRules(readmeFile)
            };
            if (Utils.IsInternetConnected())
                rules.Add(new InternetLinkRule());

            return new Facade(new LinkDetector(), rules, readmeFile);
        }
    }
}
