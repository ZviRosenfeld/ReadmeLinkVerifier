using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReadmeLinkVerifier.UnitTests
{
    [TestClass]
    public class LinkDtoTests
    {
        [TestMethod]
        [DataRow("Hi","#someLink")]
        [DataRow("Hi34#$", "Some\\Link#$99")]
        public void SameLink_AreEqualeReturnsTrue(string link, string text)
        {
            var link1 = new LinkDto(link, text, 1);
            var link2 = new LinkDto(link, text, 1);

            Assert.AreEqual(link1, link2, "The links should have been equale");
            Assert.AreEqual(link1.GetHashCode(), link2.GetHashCode(), "The links should have the same hase code");
        }

        [TestMethod]
        [DataRow("Hi", "#someLink")]
        [DataRow("Hi34#$", "Some\\Link#$99")]
        public void SameLinkDiffrentLines_AreEqualeReturnsTrue(string link, string text)
        {
            var link1 = new LinkDto(link, text, 1);
            var link2 = new LinkDto(link, text, 2);

            Assert.AreEqual(link1, link2, "The links should have been equale");
            Assert.AreEqual(link1.GetHashCode(), link2.GetHashCode(), "The links should have the same hase code");
        }
    }
}
