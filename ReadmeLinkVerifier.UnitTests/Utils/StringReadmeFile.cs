using ReadmeLinkVerifier.Interfaces;

namespace ReadmeLinkVerifier.UnitTests.Utils
{
    class StringReadmeFile : IReadmeFile
    {
        private readonly string[] text;
        private readonly string relativePath;

        public StringReadmeFile(string relativePath, params string[] lines)
        {
            this.relativePath = relativePath;
            text = lines;
        }

        public string[] GetAllLines() => text;
        public string GetRelativePath() => relativePath;
    }
}
