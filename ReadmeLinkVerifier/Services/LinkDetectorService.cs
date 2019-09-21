using System.Collections.Generic;
using System.Text.RegularExpressions;
using ReadmeLinkVerifier.Interfaces;

namespace ReadmeLinkVerifier.Services
{
    public class LinkDetectorService : ILinkDetector
    {
        private const string textRegex = @"\[(?<text>.+?)\]";
        private const string balancedParenthesesRegex = @"(((?<open>\()[^\(\)]*)+((?<-open>\))[^\)\(]*)+)*"; // A regex that will accept strings with parentheses parentheses
        private const string linkRegex = @"\(\s*(?<link>[^\(\)]*?" + balancedParenthesesRegex + @")\s*\)";
        private const string LinkRegexPattern = textRegex + linkRegex;

        public ICollection<LinkDto> DetectLinks(string[] text)
        {
            var stringMatches = new Dictionary<int, LinkDto>();
            var lineNumber = 0;
            foreach (var line in text)
                FindMatchesInLine(line, ++lineNumber, stringMatches);
            
            return stringMatches.Values;
        }

        private static void FindMatchesInLine(string line, int lineNumber, Dictionary<int, LinkDto> stringMatches)
        {
            var matches = Regex.Matches(line, LinkRegexPattern);
            foreach (Match match in matches)
            {
                var link = match.Groups["link"].ToString();
                var linkText = match.Groups["text"].ToString();
                var linkDto = new LinkDto(link, linkText, lineNumber);
                if (stringMatches.ContainsKey(linkDto.GetHashCode()))
                    stringMatches[linkDto.GetHashCode()].Lines.Add(lineNumber);
                else
                    stringMatches.Add(linkDto.GetHashCode(), linkDto);
            }
        }
    }
}
