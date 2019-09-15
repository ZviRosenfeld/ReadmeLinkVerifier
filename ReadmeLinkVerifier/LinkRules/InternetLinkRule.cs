using System;
using System.Linq;
using System.Net;
using ReadmeLinkVerifier.Interfaces;

namespace ReadmeLinkVerifier.LinkRules
{
    public class InternetLinkRule : ILinkRule
    {
        private readonly string[] relevantLinkStartings =
        {
            "https://",
            "http://"
        };

        public LinkStatus IsLinkValid(LinkDto link)
        {
            try
            {
                var fragment = new Uri(link.Link).Fragment;
                var url = link.Link.Substring(0, link.Link.Length - fragment.Length);
                using (var client = new WebClient())
                using (client.OpenRead(url))
                    return string.IsNullOrEmpty(fragment) ? LinkStatus.Good: LinkStatus.Unknown;
            }
            catch
            {
                return LinkStatus.Bad;
            }
        }

        public bool IsRuleApplicable(LinkDto link) => 
            relevantLinkStartings.Any(releventLinkStarting => link.Link.StartsWith(releventLinkStarting));       
    }
}
