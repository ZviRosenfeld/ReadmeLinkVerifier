using System.Collections.Generic;

namespace ReadmeLinkVerifier
{
    public class Result
    {
        public Result(IEnumerable<LinkDto> goodLinks, IEnumerable<LinkDto> badLinks, IEnumerable<LinkDto> unknownLinks)
        {
            GoodLinks = goodLinks;
            BadLinks = badLinks;
            UnknownLinks = unknownLinks;
        }

        public IEnumerable<LinkDto> GoodLinks { get; }
        public IEnumerable<LinkDto> BadLinks { get; }
        public IEnumerable<LinkDto> UnknownLinks { get; }
    }
}
