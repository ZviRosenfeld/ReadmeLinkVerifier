using System.Text;
using ReadmeLinkVerifier.Interfaces;

namespace ReadmeLinkVerifier.ReadmeFiles
{
    public class CodeSampleRemoverReadmeFile : IReadmeFile
    {
        private readonly IReadmeFile inneReadmeFile;

        public CodeSampleRemoverReadmeFile(IReadmeFile inneReadmeFile)
        {
            this.inneReadmeFile = inneReadmeFile;
        }

        private string text;
        public string GetAllText()
        {
            if (text == null)
            {
                text = inneReadmeFile.GetAllText();
            }

            return text;
        }
        
        public string GetRelativePath() => inneReadmeFile.GetRelativePath();
    }
}
