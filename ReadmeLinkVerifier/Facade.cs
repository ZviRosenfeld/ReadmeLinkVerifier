using System.Collections.Generic;
using System.Linq;
using ReadmeLinkVerifier.Interfaces;

namespace ReadmeLinkVerifier
{
    public class Facade
    {
        private readonly ILinkDetector linkDetector;
        private readonly IEnumerable<ILinkRule> validationRules;
        private readonly IReadmeFile readmeFile;

        public Facade(ILinkDetector linkDetector, IEnumerable<ILinkRule> validationRules, IReadmeFile readmeFile)
        {
            this.linkDetector = linkDetector;
            this.validationRules = validationRules;
            this.readmeFile = readmeFile;
        }

        public Result VerifyLinks()
        {
            
            var allLinks = linkDetector.DetectLinks(readmeFile.GetAllLines());
            return ValidateLinks(allLinks.ToList());
        }

        private Result ValidateLinks(List<LinkDto> allLinks)
        {
            var unknownLinks = new List<LinkDto>();
            var goodLinks = new List<LinkDto>();
            var badLinks = new List<LinkDto>();
            foreach (var link in allLinks)
            {
                var resolvedLink = false;
                foreach (var validationRule in validationRules)
                {
                    if (resolvedLink) break;
                    if (!validationRule.IsRuleApplicable(link)) continue;

                    resolvedLink = true;
                    switch (validationRule.IsLinkValid(link))
                    {
                        case LinkStatus.Good:
                            goodLinks.Add(link);
                            break;
                        case LinkStatus.Bad:
                            badLinks.Add(link);
                            break;
                        default:
                            unknownLinks.Add(link);
                            break;
                    }
                }

                if (!resolvedLink)
                    unknownLinks.Add(link);
            }
            
            return new Result(goodLinks, badLinks, unknownLinks);
        }
    }
}
