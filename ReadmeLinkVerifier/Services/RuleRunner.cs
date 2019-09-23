using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadmeLinkVerifier.Interfaces;

namespace ReadmeLinkVerifier.Services
{
    public class RuleRunner : IRuleRunnerService
    {
        private readonly IEnumerable<ILinkRule> validationRules;

        public RuleRunner(IEnumerable<ILinkRule> validationRules)
        {
            this.validationRules = validationRules;
        }
        
        public Result VerifyLinks(IEnumerable<LinkDto> links)
        {
            var unknownLinks = new ConcurrentBag<LinkDto>();
            var goodLinks = new ConcurrentBag<LinkDto>();
            var badLinks = new ConcurrentBag<LinkDto>();
            Parallel.ForEach(links, link =>
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
            });
            
            return new Result(goodLinks, badLinks, unknownLinks);
        }
    }
}
