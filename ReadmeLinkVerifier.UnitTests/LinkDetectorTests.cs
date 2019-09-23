using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadmeLinkVerifier.Services;
using ReadmeLinkVerifier.UnitTests.Utils;

namespace ReadmeLinkVerifier.UnitTests
{
    [TestClass]
    public class LinkDetectorTests
    {
        [TestMethod]
        [DataRow("Hi", "some(link)")]
        [DataRow("Hi", "some(li(n)k)")]
        [DataRow("Hi", "#SomeLink")]
        [DataRow("Hi", "#SomeLink ")]
        [DataRow("Hi", " #SomeLink")]
        [DataRow("Hi2", "\\Number\\Vector")]
        [DataRow("Hi3", "NumberVector")]
        [DataRow("Hi Hi", "#SomeLink")]
        [DataRow("Hi\tHi", "#SomeLink")]
        [DataRow("Hi", "#Some44Link")]
        [DataRow("Hi", "\\SomeLink.cs")]
        [DataRow("Hi", "\\SomeLink$.cs")]
        [DataRow("Hi","#Some Link")]
        [DataRow("Hi","\\Some\tLink")]
        public void OneLink_FindLink(string linkText, string link)
        {
            var fullText = $"SomeTextBefore[{linkText}]({link})SomeTextAfter";
            var linkDetector = new LinkDetectorService();
            var links = linkDetector.DetectLinks(fullText);

            Assert.AreEqual(1, links.Count, "We should have found a single link. Found " + links.Count);
            Assert.AreEqual(linkText, links.First().Text, "Got wrong link text");
            Assert.AreEqual(link.Trim(), links.First().Link, "Got wrong link");
        }
        
        [TestMethod]
        public void TwoLinksOnSameLine_FindBoth()
        {
            string link1Text = "link1", link2Text = "link2";
            string link1 = "/link1", link2 = "#link2";
            var fullText = $"SomeTextBefore[{link1Text}]({link1})SomeTextAfter[{link2Text}]({link2})";
            var linkDetector = new LinkDetectorService();
            var links = linkDetector.DetectLinks(fullText);

            Assert.IsTrue(links.Contains(new LinkDto(link1, link1Text, 1)), "Didn't find link1");
            Assert.IsTrue(links.Contains(new LinkDto(link2, link2Text, 1)), "Didn't find link2");
        }

        [TestMethod]
        public void TwoLinksTheSame_FindBoth()
        {
            string link = "link", linkText = "linkText";
            var line1 = $"SomeTextBefore[{linkText}]({link})SomeTextAfter";
            var line2 = $"SomeTextBefore[{linkText}]({link})SomeTextAfter";
            var linkDetector = new LinkDetectorService();
            var links = linkDetector.DetectLinks(new []{line1, line2});

            Assert.AreEqual(1, links.Count, "We should have only found one link (with 2 lines)");
            Assert.IsTrue(links.Contains(new LinkDto(link, linkText, 1)), "Didn't find the link");
            AssertAreSameLines(new List<int> {1, 2}, links.First().Lines);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(22)]
        public void FindRightLineNumber(int lineNumber)
        {
            string link = "[Text](Link)";
            var linkDetector = new LinkDetectorService();
            var links = linkDetector.DetectLinks(AddLinkAtLine(lineNumber, link));

            Assert.AreEqual(lineNumber, links.First().Lines.First(), "Got wrong line number");
        }

        [TestMethod]
        [DataRow("[Hi] (#SomeLink)")]
        [DataRow("(Hi)[#SomeLink]")]
        [DataRow("\\[Hi](#SomeLink)")]
        [DataRow("[Hi\\](#SomeLink)")]
        [DataRow("[Hi](#SomeLink\\)")]
        public void BadLink_DontFindAnything(string linkText)
        {
            var linkDetector = new LinkDetectorService();
            var links = linkDetector.DetectLinks(linkText);
            Assert.AreEqual(0, links.Count, "We shouldn't have found anything; Link found: " + (links.Count > 0 ? links.First().ToString() : ""));
        }

        [TestMethod]
        [DataRow("[Hi](#SomeLink)")]
        [DataRow("[Hi](#SomeLink)tt")]
        [DataRow("tt[Hi](#SomeLink)")]
        [DataRow("[Hi](\\Some\\Link)")]
        public void GoodLink_FindLink(string linkText)
        {
            var linkDetector = new LinkDetectorService();
            var links = linkDetector.DetectLinks(linkText);
            Assert.AreEqual(1, links.Count, "We should have found the link.");
        }

        [TestMethod]
        [DataRow("Hey", "s)")]
        [DataRow("He(y)", "s)")]
        [DataRow("(H(e)y)", "s)")]
        public void LinkWithParentheses_FindLink(string link, string textAfterLink)
        {
            var fullText = $"SomeTextBefore[Hi]({link}){textAfterLink}";
            var linkDetector = new LinkDetectorService();
            var links = linkDetector.DetectLinks(fullText);
            
            Assert.AreEqual(link, links.First().Link, "Got wrong link");
        }

        private void AssertAreSameLines(List<int> lines1, List<int> lines2)
        {
            Assert.AreEqual(lines1.Count, lines2.Count);
            for (var i = 0; i < lines1.Count; i++)
                Assert.AreEqual(lines1[i], lines2[i]);
        }

        private string[] AddLinkAtLine(int line, string link)
        {
            var lines = new string[line + 1];
            for (int i = 0; i < line - 1; i++)
                lines[i] = "Not a link";
            lines[line -1] = link;
            lines[line] = "Not a link";
            return lines;
        }
    }
}
