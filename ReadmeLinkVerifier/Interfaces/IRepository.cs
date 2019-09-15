namespace ReadmeLinkVerifier.Interfaces
{
    /// <summary>
    /// An Abstraction over the repository
    /// </summary>
    public interface IRepository
    {
        string GetRepositoryPath();

        bool FileOrDirectoryExists(string relativePath);
    }
}
