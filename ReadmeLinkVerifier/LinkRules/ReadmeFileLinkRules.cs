using System.Collections.Generic;
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
        private const string headerRegexPattern = @"^( )*##?#?( )\s*(?<header>(\S+[^\S\r\n]*)+?)\s*$";
        private readonly List<string> headers = new List<string>();

        public ReadmeFileLinkRules(string readmeText)
        {
            var matches = Regex.Matches(readmeText, headerRegexPattern, RegexOptions.Multiline);
            foreach (Match match in matches)
            {
                var headerText = match.Groups["header"].ToString();
                headers.Add(headerText.ToLower().Trim());
            }
        }

        public LinkStatus IsLinkValid(LinkDto link)
        {
            if (link.Link.Any(char.IsUpper))
                return LinkStatus.Bad;

            var expectedHeader = link.Link.Substring(1).Trim();
            expectedHeader = expectedHeader.Replace("-", "[ -]");
            var linkRegexPattern = $"^{expectedHeader}$";
            return headers.Any(header => Regex.IsMatch(header, linkRegexPattern))
                ? LinkStatus.Good
                : LinkStatus.Bad;
        }

        public bool IsRuleApplicable(LinkDto link) => link.Link.StartsWith("#");
    }
}
