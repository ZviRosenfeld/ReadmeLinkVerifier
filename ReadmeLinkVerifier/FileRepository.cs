using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReadmeLinkVerifier.Interfaces;

namespace ReadmeLinkVerifier
{
    public class FileRepository : IRepository
    {
        private readonly string repositoryPath;

        public FileRepository(string repositoryPath)
        {
            this.repositoryPath = repositoryPath;
        }

        public string GetRepositoryPath() => repositoryPath;

        public bool FileOrDirectoryExists(string relativePath)
        {
            var direcotryPath = (string) repositoryPath.Clone();
            var path = new LinkedList<string>(relativePath.Split('\\'));
            while (path.Count > 1)
            {
                var last = path.First.Value;
                path.RemoveFirst();
                var direcotry = new DirectoryInfo(direcotryPath);
                if (!direcotry.Exists)
                    return false;
                if (!direcotry.GetDirectories(last)
                    .Any(d => string.Equals(d.Name, last, StringComparison.CurrentCulture)))
                    return false;
                direcotryPath += Path.DirectorySeparatorChar + last;
            }

            var fileName = path.First.Value;
            var finalDirecotry = new DirectoryInfo(direcotryPath);
            return finalDirecotry.Exists && 
                (finalDirecotry.GetFiles(fileName).Any(f => string.Equals(f.Name, fileName)) ||
                 finalDirecotry.GetDirectories(fileName).Any(f => string.Equals(f.Name, fileName)));
        }
    }
}
