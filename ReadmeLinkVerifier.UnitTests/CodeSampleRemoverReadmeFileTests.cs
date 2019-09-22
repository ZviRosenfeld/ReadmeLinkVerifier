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
        [DataRow("` `` `` " + CodeSampleText + "`")]
        //[DataRow("` NotSample ``" + CodeSampleText + "``")]
        [DataRow("` ``` `" + CodeSampleText)]
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
        [DataRow("` `` `" + CodeSampleText)]
        public void NotCodeSamples_DontIgnore(string codeSample)
        {
            var readmeFile = new CodeSampleRemoverReadmeFile(new StringReadmeFile("", codeSample));
            var text = readmeFile.GetAllText();
            Assert.IsTrue(text.Contains(CodeSampleText), "Text should have contained the false code samples. Text = " + text);
        }

        [TestMethod]
        public void TwoCodeSamples_IgnoreBoth()
        {
            var NotCodeSampleText = "Not_Code_Sample";
            var readmeFile = new CodeSampleRemoverReadmeFile(new StringReadmeFile("", $"``` {CodeSampleText} ``` {NotCodeSampleText} ``` {CodeSampleText} ```"));
            var text = readmeFile.GetAllText();
            Assert.IsFalse(text.Contains(CodeSampleText), "Text shouldn't have contained the code samples. Text = " + text);
            Assert.IsTrue(text.Contains(NotCodeSampleText), "Text should have contained the false code samples. Text = " + text);
        }

        [TestMethod]
        public void CodeSampleContainsNewLines_NewLinesNotLost()
        {
            var readmeFile = new CodeSampleRemoverReadmeFile(new StringReadmeFile("", $" Before``` {Environment.NewLine} Sample {Environment.NewLine} ``` After"));
            var text = readmeFile.GetAllText();
            Assert.AreEqual(4, text.Split(new[] { Environment.NewLine }, StringSplitOptions.None), "NewLines lost");
        }
    }
}
