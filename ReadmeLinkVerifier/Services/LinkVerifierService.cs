using System;
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
                new ReadmeFileLinkRules(readmeFile.GetAllText())
            };
            if (Utils.IsInternetConnected())
                rules.Add(new InternetLinkRule());
            
            ruleRunner = new RuleRunner(rules);
        }

        public Result VerifyLinks()
        {
            var readmeLines = readmeFile.GetAllText().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var allLinks = linkDetector.DetectLinks(readmeLines);
            return ruleRunner.VerifyLinks(allLinks);
        }
    }
}
