using System.Collections.Generic;

namespace ReadmeLinkVerifier
{
    public class Result
    {
        public Result(ICollection<LinkDto> goodLinks, ICollection<LinkDto> badLinks, ICollection<LinkDto> unknownLinks)
        {
            GoodLinks = goodLinks;
            BadLinks = badLinks;
            UnknownLinks = unknownLinks;
        }

        public ICollection<LinkDto> GoodLinks { get; }
        public ICollection<LinkDto> BadLinks { get; }
        public ICollection<LinkDto> UnknownLinks { get; }
    }
}
