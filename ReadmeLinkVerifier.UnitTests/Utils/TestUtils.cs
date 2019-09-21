using System.Collections.Generic;
using FakeItEasy;
using ReadmeLinkVerifier.Interfaces;

namespace ReadmeLinkVerifier.UnitTests.Utils
{
    static class TestUtils
    {
        public static void SetLinkTo(this ILinkRule rule, bool ruleApply, LinkStatus linkStatus, LinkDto link)
        {
            A.CallTo(() => rule.IsRuleApplicable(A<LinkDto>.That.Matches(l => l.Equals(link)))).Returns(ruleApply);
            A.CallTo(() => rule.IsLinkValid(A<LinkDto>.That.Matches(l => l.Equals(link)))).Returns(linkStatus);
        }

        public static LinkDto CreateLink(ILinkRule rule, bool ruleApply, LinkStatus linkStatus)
        {
            var linkDto = new LinkDto("link", linkStatus.ToString(), 1);
            rule.SetLinkTo(ruleApply, linkStatus, linkDto);
            return linkDto;
        }

        public static ICollection<LinkDto> DetectLinks(this ILinkDetector linkDetector, string text) =>
            linkDetector.DetectLinks(new []{text});
    }
}
