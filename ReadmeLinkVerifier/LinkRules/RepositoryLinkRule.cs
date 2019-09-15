using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReadmeLinkVerifier.Exceptions;
using ReadmeLinkVerifier.Interfaces;

namespace ReadmeLinkVerifier.LinkRules
{
    /// <summary>
    /// A rule for link into the repository
    /// </summary>
    public class RepositoryLinkRule : ILinkRule
    {
        private readonly IRepository repository;
        private readonly IReadmeFile readmeFile;
        private readonly string[] irrelevantLinkStartings = 
        {
            "#",
            "https://",
            "http://"
        };

        public RepositoryLinkRule(IRepository repository, IReadmeFile readmeFile)
        {
            this.repository = repository;
            this.readmeFile = readmeFile;
        }

        public LinkStatus IsLinkValid(LinkDto link)
        {
            try
            {
                var actualLink = NormalizePath(link.Link);
                return repository.FileOrDirectoryExists(actualLink) ? LinkStatus.Good : LinkStatus.Bad;
            }
            catch (BadPathFormatException)
            {
                return LinkStatus.Bad;
            }
        }

        private string NormalizePath(string path)
        {
            if (path.Contains("//"))
                throw new BadPathFormatException();

            if (path.StartsWith("/"))
                path = path.Substring(1);

            if (readmeFile.GetRelativePath() != string.Empty)
                path = readmeFile.GetRelativePath().Replace("\\", "/") + "/" + path;

            var files = path.Split('/');
            var fileList = new LinkedList<string>();
            foreach (var file in files)
            {
                if (file == ".") continue;
                else if (file == "..")
                {
                    if (fileList.Count == 0)
                        throw new BadPathFormatException();
                    fileList.RemoveLast();
                }
                else fileList.AddLast(file);
            }

            var normalizedPath = new StringBuilder();
            foreach (var file in fileList)
                normalizedPath.Append("\\" + file);
            return normalizedPath.ToString().Substring(1);
        }

        public bool IsRuleApplicable(LinkDto link) =>
            irrelevantLinkStartings.All(notReleventLinkStarting => !link.Link.StartsWith(notReleventLinkStarting));
    }
}
