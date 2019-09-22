using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadmeLinkVerifier.ReadmeFiles;
using ReadmeLinkVerifier.UnitTests.Utils;

namespace ReadmeLinkVerifier.UnitTests
{
    [TestClass]
    public class CodeSampleRemoverReadmeFileTests
    {
        private const string CodeSampleText = "CodeSample";

        [TestMethod]
        [DataRow("`"+ CodeSampleText + "`")]
        [DataRow("``" + CodeSampleText + "``")]
        [DataRow("```" + CodeSampleText + "```")]
        [DataRow("``` " + CodeSampleText + " ```")]
        [DataRow("```\n" + CodeSampleText + "\n```")]
        [DataRow("```\n" + CodeSampleText + "```")]
        [DataRow("````" + CodeSampleText + "````")]
        [DataRow("```" + CodeSampleText + "````")]
        [DataRow("```" + CodeSampleText + "`````")]
        [DataRow("```" + CodeSampleText)]
        [DataRow("``` SampleStart ``" + CodeSampleText)]
        [DataRow("```` SampleStart ```" + CodeSampleText)]
        [DataRow("` NotSample ``" + CodeSampleText + "``")]
        public void IgnoreCodeSamples(string codeSample)
        {
            var readmeFile = new CodeSampleRemoverReadmeFile(new StringReadmeFile("", codeSample));
            var text = readmeFile.GetAllText();
            Assert.IsFalse(text.Contains(CodeSampleText), "Text shouldn't have contained the code samples. Text = " + text);
        }

        [TestMethod]
        [DataRow("`" + CodeSampleText + "``")]
        [DataRow("``" + CodeSampleText + "```")]
        [DataRow("`" + CodeSampleText)]
        [DataRow("``" + CodeSampleText)]
        [DataRow("\\`" + CodeSampleText + "\\`")]
        [DataRow("``` Sample ````" + CodeSampleText + "````")]
        public void NotCodeSamples_DontIgnore(string codeSample)
        {
            var readmeFile = new CodeSampleRemoverReadmeFile(new StringReadmeFile("", codeSample));
            var text = readmeFile.GetAllText();
            Assert.IsTrue(text.Contains(CodeSampleText), "Text should have contained the code samples. Text = " + text);
        }
    }
}
