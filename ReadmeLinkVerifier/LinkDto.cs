using System.Collections.Generic;
using System.Linq;

namespace ReadmeLinkVerifier
{
    public class LinkDto
    {
        public LinkDto(string link, string text, int line)
        {
            Link = link;
            Text = text;
            Lines = new List<int> {line};
        }

        public string Link { get; }

        public string Text { get; }

        public List<int> Lines { get; }

        public override string ToString() => $"[{Text}]({Link}); Lines: {LinesToString()}";

        private string LinesToString()
        {
            var lines = Lines.Aggregate(string.Empty, (current, line) => current + ", " + line);
            return lines.Substring(2);
        }

        public override bool Equals(object obj) => 
            obj is LinkDto other && other.Text == Text && other.Link == Link;

        public override int GetHashCode() => Text.GetHashCode() + Link.GetHashCode();
    }
}
