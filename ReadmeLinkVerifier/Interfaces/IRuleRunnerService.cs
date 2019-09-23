using System.Collections.Generic;

namespace ReadmeLinkVerifier.Interfaces
{
    public interface IRuleRunnerService
    {
        Result VerifyLinks(IEnumerable<LinkDto> links);
    }
}