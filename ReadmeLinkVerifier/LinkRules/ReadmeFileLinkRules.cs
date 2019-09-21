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
        private readonly string readmeText;

        public ReadmeFileLinkRules(string readmeText)
        {
            this.readmeText = readmeText.ToLower();
        }

        public LinkStatus IsLinkValid(LinkDto link)
        {
            if (link.Link.Any(char.IsUpper))
                return LinkStatus.Bad;

            var expectedHeader = link.Link.Substring(1).Trim().ToLower();
            expectedHeader = expectedHeader.Replace("-", "[ -]");
            var linkRegexPattern = $"^( )*##?#?( )\\s*{expectedHeader}\\s*$";
            return Regex.IsMatch(readmeText, linkRegexPattern, RegexOptions.Multiline)
                ? LinkStatus.Good
                : LinkStatus.Bad;
        }

        public bool IsRuleApplicable(LinkDto link) => link.Link.StartsWith("#");
    }
}
