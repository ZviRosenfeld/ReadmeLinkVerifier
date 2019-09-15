using ReadmeLinkVerifier.Interfaces;

namespace ReadmeLinkVerifier.LinkRules
{
    class IgnoreLinksWithBracketsRule : ILinkRule
    {
        public LinkStatus IsLinkValid(LinkDto link) => LinkStatus.Unknown;

        public bool IsRuleApplicable(LinkDto link) => link.Link.Contains("(");
    }
}
