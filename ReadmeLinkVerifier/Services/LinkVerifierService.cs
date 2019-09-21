using System.Collections.Generic;
using System.IO;
using ReadmeLinkVerifier.Interfaces;
using ReadmeLinkVerifier.LinkRules;

namespace ReadmeLinkVerifier.Services
{
    public class LinkVerifierService
    {
        private const string README_DEFAILT_PATH = "README.md";

        private IReadmeFile readmeFile;
        private ILinkDetector linkDetector = new LinkDetectorService();
        private IRuleRunnerService ruleRunner;

        public LinkVerifierService(string repositoryPath, string readmeRelativePath = null)
        {
            var repository = new FileRepository(repositoryPath);
            readmeRelativePath = readmeRelativePath ?? README_DEFAILT_PATH;
            var readmeFilePath = Path.Combine(repository.GetRepositoryPath(), readmeRelativePath);
            readmeFile = new ReadmeFile(readmeFilePath, readmeRelativePath);
            var rules = new List<ILinkRule>
            {
                new RepositoryLinkRule(repository, readmeFile),
                new ReadmeFileLinkRules(readmeFile)
            };
            if (Utils.IsInternetConnected())
                rules.Add(new InternetLinkRule());
            
            ruleRunner = new RuleRunner(rules);
        }

        public Result VerifyLinks()
        {
            var allLinks = linkDetector.DetectLinks(readmeFile.GetAllLines());
            return ruleRunner.VerifyLinks(allLinks);
        }
    }
}
