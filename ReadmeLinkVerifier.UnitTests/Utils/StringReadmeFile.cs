using System.Text;
using ReadmeLinkVerifier.Interfaces;

namespace ReadmeLinkVerifier.UnitTests.Utils
{
    class StringReadmeFile : IReadmeFile
    {
        private readonly string text;
        private readonly string relativePath;

        public StringReadmeFile(string relativePath, params string[] lines)
        {
            this.relativePath = relativePath;
            var stringBuilder = new StringBuilder();
            foreach (var line in lines)
                stringBuilder.AppendLine(line);
            text = stringBuilder.ToString();
        }

        public string GetAllText() => text;
        public string GetRelativePath() => relativePath;
    }
}
