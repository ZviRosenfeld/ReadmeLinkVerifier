﻿using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadmeLinkVerifier.Interfaces;
using ReadmeLinkVerifier.Services;
using ReadmeLinkVerifier.UnitTests.Utils;

namespace ReadmeLinkVerifier.UnitTests
{
    [TestClass]
    public class VerifyLinksServiceTests
    {
        private readonly LinkDto goodLink = new LinkDto("link1", "good link", 1);
        private readonly LinkDto badLink = new LinkDto("link2", "bad link", 1);
        private readonly LinkDto unknownLink = new LinkDto("link3", "unknown link", 1);
        private readonly List<LinkDto> links;
        private readonly ILinkRule bassicRule = A.Fake<ILinkRule>();

        public VerifyLinksServiceTests()
        {
            links = new List<LinkDto> {goodLink, badLink, unknownLink};
            bassicRule.SetLinkTo(true, LinkStatus.Bad, badLink);
            bassicRule.SetLinkTo(true, LinkStatus.Good, goodLink);
            bassicRule.SetLinkTo(false, LinkStatus.Unknown, unknownLink);
        }
        
        [TestMethod]
        public void RuleIgnoresLink_LinkReturnedUnknown()
        {
            var linkVerifierService = new RuleRunner(new List<ILinkRule> {bassicRule});
            var result = linkVerifierService.VerifyLinks(new List<LinkDto> { unknownLink });  
            AssertLinkIsUnknown(result, unknownLink);
        }

        [TestMethod]
        public void RuleApprovesLink_LinkReturnedGood()
        {
            var linkVerifierService = new RuleRunner(new List<ILinkRule> { bassicRule });
            var result = linkVerifierService.VerifyLinks(new List<LinkDto> { goodLink });
            AssertLinkIsGood(result, goodLink);
        }

        [TestMethod]
        public void RuleDisapprovesLink_LinkReturnedBad()
        {
            var linkVerifierService = new RuleRunner(new List<ILinkRule> { bassicRule });
            var result = linkVerifierService.VerifyLinks(new List<LinkDto> { badLink });
            AssertLinkIsBad(result, badLink);
        }

        [TestMethod]
        public void TwoRulesTest()
        {
            var firstRule = A.Fake<ILinkRule>();
            var secondRule = A.Fake<ILinkRule>();
            firstRule.SetLinkTo(true, LinkStatus.Bad, badLink);
            secondRule.SetLinkTo(true, LinkStatus.Good, goodLink);

            var linkVerifierService = new RuleRunner(new List<ILinkRule> { firstRule, secondRule });
            var result = linkVerifierService.VerifyLinks(links);
            AssertLinkIsBad(result, badLink);
            AssertLinkIsGood(result, goodLink);
            AssertLinkIsUnknown(result, unknownLink);
        }

        [TestMethod]
        public void RuleWithGoodAndBadAndUnknownLink()
        {
            var linkVerifierService = new RuleRunner(new List<ILinkRule> { bassicRule });
            var result = linkVerifierService.VerifyLinks(links);
            AssertLinkIsBad(result, badLink);
            AssertLinkIsGood(result, goodLink);
            AssertLinkIsUnknown(result, unknownLink);
        }

        /// <summary>
        /// Check that if one rule sets a link to unknow, the next rules will ignore it
        /// </summary>
        [TestMethod]
        public void RuleSetsLinkToUnknownTest_NextRulesIgnoreLinks()
        {
            var firstRule = A.Fake<ILinkRule>();
            var secondRule = A.Fake<ILinkRule>();
            firstRule.SetLinkTo(true, LinkStatus.Unknown, unknownLink);
            secondRule.SetLinkTo(true, LinkStatus.Good, unknownLink);

            var linkVerifierService = new RuleRunner(new List<ILinkRule> { firstRule, secondRule });
            var result = linkVerifierService.VerifyLinks(links);
            AssertLinkIsUnknown(result, unknownLink);
        }

        private void AssertLinkIsUnknown(Result result, LinkDto link)
        {
            Assert.IsTrue(result.UnknownLinks.Contains(link), "Link isn't unknown");
            Assert.IsFalse(result.GoodLinks.Contains(link), "Link is good");
            Assert.IsFalse(result.BadLinks.Contains(link), "Link is bad");
        }

        private void AssertLinkIsGood(Result result, LinkDto link)
        {
            Assert.IsTrue(result.GoodLinks.Contains(link), "Link isn't good");
            Assert.IsFalse(result.UnknownLinks.Contains(link), "Link is unknown");
            Assert.IsFalse(result.BadLinks.Contains(link), "Link is bad");
        }

        private void AssertLinkIsBad(Result result, LinkDto link)
        {
            Assert.IsTrue(result.BadLinks.Contains(link), "Link isn't bad");
            Assert.IsFalse(result.GoodLinks.Contains(link), "Link is good");
            Assert.IsFalse(result.UnknownLinks.Contains(link), "Link is unknown");
        }
    }
}
