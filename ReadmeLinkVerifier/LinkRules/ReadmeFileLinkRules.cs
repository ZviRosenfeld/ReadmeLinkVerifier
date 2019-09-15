using System.Linq;
using System.Text.RegularExpressions;
using ReadmeLinkVerifier.Interfaces;

namespace ReadmeLinkVerifier.LinkRules
{
    /// <summary>
    /// Used for links into the readme file
    /// </summary>
    public class ReadmeFileLinkRules : ILinkRule
    {
        private readonly IReadmeFile readmeFile;

        public ReadmeFileLinkRules(IReadmeFile readmeFile)
        {
            this.readmeFile = readmeFile;
        }

        public LinkStatus IsLinkValid(LinkDto link)
        {
            if (link.Link.Any(char.IsUpper))
                return LinkStatus.Bad;

            var expectedHeader = link.Link.Substring(1).Trim().ToLower();
            expectedHeader = expectedHeader.Replace("-", "[ -]");
            var linkRegexPattern = $"^( )*##?#?( )\\s*{expectedHeader}\\s*$";
            return readmeFile.GetAllLines().Any(line => Regex.IsMatch(line.ToLower(), linkRegexPattern))
                ? LinkStatus.Good
                : LinkStatus.Bad;
        }

        public bool IsRuleApplicable(LinkDto link) => link.Link.StartsWith("#");
    }
}
