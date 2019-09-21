using System;
using System.IO;
using ReadmeLinkVerifier.Interfaces;

namespace ReadmeLinkVerifier.ReadmeFiles
{
    public class ReadmeFile : IReadmeFile
    {
        private readonly string path;
        private readonly string relativePath;

        public ReadmeFile(string path, string relativePath)
        {
            if (!File.Exists(path))
                throw new ApplicationException($"File {path} doesn't exist ");

            this.path = path;
            this.relativePath =
                Path.GetDirectoryName(relativePath.StartsWith("\\") ? relativePath.Substring(1) : relativePath);
        }
        
        public string GetAllText() => File.ReadAllText(path);

        public string GetRelativePath() => relativePath;
    }
}
