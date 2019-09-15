using System.Collections.Generic;
using FakeItEasy;
using ReadmeLinkVerifier.Interfaces;

namespace ReadmeLinkVerifier.UnitTests.Utils
{
    static class TestUtils
    {
        public static ILinkDetector CreateLinkDetector(this LinkDto link) =>
            CreateLinkDetector(new LinkDto[] {link});

        public static ILinkDetector CreateLinkDetector(this ICollection<LinkDto> links)
        {
            var detector = A.Fake<ILinkDetector>();
            A.CallTo(() => detector.DetectLinks(A<string[]>._)).Returns(links);
            return detector;
        }

        public static void SetLinkTo(this ILinkRule rule, bool ruleApply, LinkStatus linkStatus, LinkDto link)
        {
            A.CallTo(() => rule.IsRuleApplicable(A<LinkDto>.That.Matches(l => l.Equals(link)))).Returns(ruleApply);
            A.CallTo(() => rule.IsLinkValid(A<LinkDto>.That.Matches(l => l.Equals(link)))).Returns(linkStatus);
        }

        public static ICollection<LinkDto> DetectLinks(this ILinkDetector linkDetector, string text) =>
            linkDetector.DetectLinks(new []{text});
    }
}
