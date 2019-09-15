using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadmeLinkVerifier.LinkRules;

namespace ReadmeLinkVerifier.UnitTests.LinkRules
{
    [TestClass]
    public class InternetLinkRuleTests
    {
        [TestMethod]
        [DataRow("#link")]
        [DataRow("https:SomeText")]
        [DataRow("\\SomeFile\\")]
        public void IsRuleApplicable_RuleNotApplicable(string link)
        {
            var linkDto = new LinkDto(link, "Hey", 1);
            var internetLinkRule = new InternetLinkRule();
            Assert.IsFalse(internetLinkRule.IsRuleApplicable(linkDto), "Link should not have been relevant");
        }

        [TestMethod]
        [DataRow("https://link")]
        [DataRow("http://link")]
        [DataRow("https://link/sdf/sdf")]
        public void IsRuleApplicable_RuleApplicable(string link)
        {
            var linkDto = new LinkDto(link, "Hey", 1);
            var internetLinkRule = new InternetLinkRule();
            Assert.IsTrue(internetLinkRule.IsRuleApplicable(linkDto), "Link should have been relevant");
        }

        [TestMethod]
        [DataRow("https://www.google.com/")]
        [DataRow("https://github.com/ZviRosenfeld/MinMaxSearch")]
        [DataRow("https://github.com/ZviRosenfeld/MinMaxSearch/blob/master/README.md")]
        public void IsLinkValid_LinkValid(string link)
        {
            var linkDto = new LinkDto(link, "Hey", 1);
            var internetLinkRule = new InternetLinkRule();
            Assert.AreEqual(LinkStatus.Good, internetLinkRule.IsLinkValid(linkDto), "Link should have been valid");
        }

        [TestMethod]
        [DataRow("https://github.com/ZviRosenfeld/MinMaxSearch23")]
        [DataRow("https://github.com/ZviRosenfeld /MinMaxSearch")]
        [DataRow("https://github.com/ZviRosenfeld/MinMaxSearch/blob/master/README.md3")]
        public void IsLinkValid_LinkNotValid(string link)
        {
            var linkDto = new LinkDto(link, "Hey", 1);
            var internetLinkRule = new InternetLinkRule();
            Assert.AreEqual(LinkStatus.Bad, internetLinkRule.IsLinkValid(linkDto), "Link should not have been valid");
        }
    }
}
