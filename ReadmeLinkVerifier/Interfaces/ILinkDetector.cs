using System.Collections.Generic;

namespace ReadmeLinkVerifier.Interfaces
{
    public interface ILinkDetector
    {
        ICollection<LinkDto> DetectLinks(string[] text);
    }
}