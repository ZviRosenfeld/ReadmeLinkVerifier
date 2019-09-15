using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadmeLinkVerifier.Interfaces;
using ReadmeLinkVerifier.LinkRules;
using ReadmeLinkVerifier.UnitTests.Utils;

namespace ReadmeLinkVerifier.UnitTests.LinkRules
{
    [TestClass]
    public class RepositoryLinkRuleTests
    {
        [TestMethod]
        [DataRow("#link")]
        [DataRow("https://link")]
        [DataRow("http://link")]
        public void IsRuleApplicable_RuleNotApplicable(string link)
        {
            var linkDto = new LinkDto(link, "Hey", 1);
            var repositoryLinkRule = new RepositoryLinkRule(A.Fake<IRepository>(), A.Fake<IReadmeFile>());
            Assert.IsFalse(repositoryLinkRule.IsRuleApplicable(linkDto), "Link should not have been relevant");
        }

        [TestMethod]
        [DataRow("link")]
        [DataRow("/link")]
        [DataRow("/link/link/link")]
        [DataRow("link45.cs")]
        [DataRow("link$#")]
        [DataRow("http:")]
        [DataRow("https")]
        public void IsRuleApplicable_RuleApplicable(string link)
        {
            var linkDto = new LinkDto(link, "Hey", 1);
            var repositoryLinkRule = new RepositoryLinkRule(A.Fake<IRepository>(), A.Fake<IReadmeFile>());
            Assert.IsTrue(repositoryLinkRule.IsRuleApplicable(linkDto), "Link should have been relevant");
        }

        [TestMethod]
        [DataRow("link1/link2","link1\\link2")]
        [DataRow("/link1/link2", "link1\\link2")]
        [DataRow("link/../link", "link")]
        [DataRow("link44.cs", "link44.cs")]
        [DataRow("link1/././link2", "link1\\link2")]
        public void IsLinkValid_LinkValid(string link, string actualFile)
        {
            var linkDto = new LinkDto(link, "Hey", 1);
            var repository = new FakeRepository(actualFile);
            var repositoryLinkRule = new RepositoryLinkRule(repository, A.Fake<IReadmeFile>());
            Assert.AreEqual(LinkStatus.Good, repositoryLinkRule.IsLinkValid(linkDto), "Link should have been valid");
        }

        [TestMethod]
        [DataRow("link/link", "link")]
        [DataRow("link/../../link", "link")]
        [DataRow("link//link", "link\\link")]
        [DataRow("link", "Link")]
        [DataRow("link1 /link2", "link1/link2")]
        public void IsLinkValid_LinkNotValid(string link, string actualFile)
        {
            var linkDto = new LinkDto(link, "Hey", 1);
            var repository = new FakeRepository(actualFile);
            var repositoryLinkRule = new RepositoryLinkRule(repository, A.Fake<IReadmeFile>());
            Assert.AreEqual(LinkStatus.Bad, repositoryLinkRule.IsLinkValid(linkDto), "Link should not have been valid");
        }
    }
}
