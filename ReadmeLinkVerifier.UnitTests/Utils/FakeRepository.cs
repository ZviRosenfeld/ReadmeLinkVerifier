using System.Collections.Generic;
using ReadmeLinkVerifier.Interfaces;

namespace ReadmeLinkVerifier.UnitTests.Utils
{
    class FakeRepository : IRepository
    {
        private readonly HashSet<string> files = new HashSet<string>();

        public FakeRepository(params string[] files)
        {
            foreach (var file in files)
                AddFile(file);
        }

        public void AddFile(string relativePath)
        {
            if (!files.Contains(relativePath))
                files.Add(relativePath);
        }

        public string GetRepositoryPath()
        {
            throw new System.NotImplementedException();
        }

        public bool FileOrDirectoryExists(string relativePath) => files.Contains(relativePath);
    }
}
