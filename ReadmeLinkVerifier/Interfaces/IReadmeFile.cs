namespace ReadmeLinkVerifier.Interfaces
{
    /// <summary>
    /// An abstraction over the readme file
    /// </summary>
    public interface IReadmeFile
    {
        string GetAllText();

        string GetRelativePath();
    }
}
