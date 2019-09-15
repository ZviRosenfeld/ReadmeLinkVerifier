namespace ReadmeLinkVerifier.Interfaces
{
    public interface ILinkRule
    {
        LinkStatus IsLinkValid(LinkDto link);

        bool IsRuleApplicable(LinkDto link);
    }
}
