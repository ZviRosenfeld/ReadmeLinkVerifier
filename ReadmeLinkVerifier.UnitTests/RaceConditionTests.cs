﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadmeLinkVerifier.Interfaces;
using ReadmeLinkVerifier.UnitTests.Utils;

namespace ReadmeLinkVerifier.UnitTests
{
    /// <summary>
    /// These tests check that there are no race conditions.
    /// </summary>
    [TestClass]
    public class RaceConditionTests
    {
        private const int LinkLoop = 1000;

        [TestMethod]
        public void AnylizeThousandsOfLinks_AllLinksInResult()
        {
            var rule1 = A.Fake<ILinkRule>();
            var rule2 = A.Fake<ILinkRule>();
            var linkDetector = A.Fake<ILinkDetector>();
            var allLinks = CreateAllLinks(rule1, rule2);
            A.CallTo(() => linkDetector.DetectLinks(A<string[]>._)).Returns(allLinks);
            var facade = new Facade(linkDetector, new []{rule1, rule2}, A.Fake<IReadmeFile>());

            var results = facade.VerifyLinks();

            AssertLinkWereClassifiedCorrectly(results);
            Assert.AreEqual(LinkLoop * 2, results.GoodLinks.Count(), "Didn't get enough good links");
            Assert.AreEqual(LinkLoop, results.BadLinks.Count(), "Didn't get enough bad links");
            Assert.AreEqual(LinkLoop * 3, results.UnknownLinks.Count(), "Didn't get enough unknown links");
        }

        private static List<LinkDto> CreateAllLinks(ILinkRule rule1, ILinkRule rule2)
        {
            var allLinks = new List<LinkDto>();
            for (int i = 0; i < LinkLoop; i++)
            {
                allLinks.Add(TestUtils.CreateLink(rule1, true, LinkStatus.Bad));
                allLinks.Add(TestUtils.CreateLink(rule1, true, LinkStatus.Good));
                allLinks.Add(TestUtils.CreateLink(rule2, false, LinkStatus.Unknown));
                rule1.SetLinkTo(false, LinkStatus.Unknown, allLinks.Last());
                allLinks.Add(TestUtils.CreateLink(rule2, true, LinkStatus.Unknown));
                rule1.SetLinkTo(false, LinkStatus.Unknown, allLinks.Last());
                allLinks.Add(TestUtils.CreateLink(rule1, true, LinkStatus.Unknown));
                allLinks.Add(TestUtils.CreateLink(rule2, true, LinkStatus.Good));
                rule1.SetLinkTo(false, LinkStatus.Unknown, allLinks.Last());
            }
            return allLinks;
        }

        private static void AssertLinkWereClassifiedCorrectly(Result result)
        {
            foreach (var link in result.BadLinks)
                Assert.AreEqual(LinkStatus.Bad.ToString(), link.Text, "Not-bad link marked as bad link. " + link);
            foreach (var link in result.GoodLinks)
                Assert.AreEqual(LinkStatus.Good.ToString(), link.Text, "Not-good link marked as good link. " + link);
            foreach (var link in result.UnknownLinks)
                Assert.AreEqual(LinkStatus.Unknown.ToString(), link.Text, "Not-unknown link marked as unknown link. " + link);
        }
    }
}
