using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReadmeLinkVerifier.IntegrationTests
{
    [TestClass]
    [TestCategory("IntegrationTest")]
    public class IntegrationTest
    {
        private const string BadLinkText = "Bad";
        private const string GoodLinkText = "Good";
        private const string UnknonwLinkText = "Unknow";

        [TestMethod]
        [DataRow("TestReadme1.rm")]
        [DataRow("ReadmeLinkVerifier\\TestReadme2.rm")]
        public void FullIntegrationTest(string readmePath)
        {
            var repositoryPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..");
            repositoryPath = Path.GetFullPath(repositoryPath);
            var verifyLinksService = ServiceBuilder.GetVerifyLinksService(repositoryPath, readmePath);
            var result = verifyLinksService.VerifyLinks();

            AssertLinkWereClassifiedCorrectly(result);
        }

        private static void AssertLinkWereClassifiedCorrectly(Result result)
        {
            foreach (var link in result.BadLinks)
                Assert.AreEqual(BadLinkText, link.Text, "Not-bad link marked as bad link. " + link);
            foreach (var link in result.GoodLinks)
                Assert.AreEqual(GoodLinkText, link.Text, "Not-good link marked as good link. " + link);
            foreach (var link in result.UnknownLinks)
                Assert.AreEqual(UnknonwLinkText, link.Text, "Not-unknown link marked as unknown link. " + link);
        }
    }
}
