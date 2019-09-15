using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadmeLinkVerifier.Interfaces;
using ReadmeLinkVerifier.LinkRules;
using ReadmeLinkVerifier.UnitTests.Utils;

namespace ReadmeLinkVerifier.UnitTests.LinkRules
{
    [TestClass]
    public class ReadmeFileLinkRulesTests
    {
        [TestMethod]
        [DataRow("\\SomeFile\\")]
        [DataRow("https://link")]
        public void IsRuleApplicable_RuleNotApplicable(string link)
        {
            var linkDto = new LinkDto(link, "Hey", 1);
            var rule = new ReadmeFileLinkRules(A.Fake<IReadmeFile>());
            Assert.IsFalse(rule.IsRuleApplicable(linkDto), "Link should not have been relevant");
        }

        [TestMethod]
        [DataRow("#link")]
        [DataRow("###link")]
        public void IsRuleApplicable_RuleApplicable(string link)
        {
            var linkDto = new LinkDto(link, "Hey", 1);
            var rule = new ReadmeFileLinkRules(A.Fake<IReadmeFile>());
            Assert.IsTrue(rule.IsRuleApplicable(linkDto), "Link should have been relevant");
        }

        [TestMethod]
        [DataRow("#link","# link")]
        [DataRow("#link", "#   link")]
        [DataRow("#link", "# \tlink")]
        [DataRow("#link", "### link")]
        [DataRow("#link", "# Link")]
        [DataRow("#link-link", "# link Link")]
        [DataRow("#link--link", "# link  Link")]
        [DataRow("#link-link", "# link-Link")]
        [DataRow("#link", "# link ")]
        [DataRow("#link", "# link\t")]
        [DataRow("#link", "  # link")]
        public void IsLinkValid_LinkValid(string link, string header)
        {
            var linkDto = new LinkDto(link, "Hey", 1);
            var readmeFile = new StringReadmeFile("SomeText", header, "SomeText");
            var rule = new ReadmeFileLinkRules(readmeFile);
            Assert.AreEqual(LinkStatus.Good, rule.IsLinkValid(linkDto), "Link should have been valid");
        }

        [TestMethod]
        [DataRow("#Hi", "# Hi")]
        [DataRow("#Hi", "# SomeOtherTitle")]
        [DataRow("d #link", "# link")]
        [DataRow("d#link", "# link")]
        [DataRow("#link", "# link2")]
        [DataRow("#link", "# link 2")]
        [DataRow("#link", "#link")]
        [DataRow("#link", "\t# link")]
        [DataRow("#link", "#\tlink")]
        [DataRow("#link", "#\t link")]
        public void IsLinkValid_LinkNotValid(string link, string header)
        {
            var linkDto = new LinkDto(link, "Hey", 1);
            var readmeFile = new StringReadmeFile("SomeText", header, "SomeText");
            var rule = new ReadmeFileLinkRules(readmeFile);
            Assert.AreEqual(LinkStatus.Bad, rule.IsLinkValid(linkDto), "Link should not have been valid");
        }
    }
}
