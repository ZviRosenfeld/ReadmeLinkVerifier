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

        public string GetRelativePath() => inneReadmeFile.GetRelativePath();

        private string text;
        public string GetAllText()
        {
            if (text == null)
            {
                text = inneReadmeFile.GetAllText();
                text = RemoveCodeSamples(text);
            }

            return text;
        }

        private string RemoveCodeSamples(string text)
        {
            var stringBuilder = new StringBuilder();

            var collectedForOpen = 0; // The ` we've collected while opening the codeSample
            var collectedForClose = 0; // The ` we've collected while closing the codeSample 
            var tentativeIgnoreSince = 0;
            var mode = Mode.Normal;
            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (c == '`' && (i == 0 || text[i - 1] != '\\'))
                {
                    switch (mode)
                    {
                        case Mode.Normal:
                            collectedForOpen = 1;
                            mode = Mode.Opening;
                            break;
                        case Mode.Opening:
                            collectedForOpen++;
                            break;
                        case Mode.IgnoreText:
                        case Mode.TentativeIgnoreText:
                            collectedForClose = 1;
                            mode = Mode.Closing;
                            break;
                        case Mode.Closing:
                            collectedForClose++;
                            break;
                    }
                }
                else
                {
                    switch (mode)
                    {
                        case Mode.Normal:
                            stringBuilder.Append(c);
                            break;
                        case Mode.Opening:
                            mode = collectedForOpen >= 3 ? Mode.IgnoreText : Mode.TentativeIgnoreText;
                            tentativeIgnoreSince = i - collectedForOpen;
                            break;
                        case Mode.Closing:
                            if (collectedForOpen == collectedForClose)
                            {
                                mode = Mode.Normal;
                                stringBuilder.Append(c);
                            }
                            else if (collectedForOpen < collectedForClose)
                            {
                                if (collectedForOpen < 3 && collectedForClose < 3)
                                    mode = Mode.TentativeIgnoreText;
                                else if (collectedForOpen >= 3 && collectedForClose >= 3)
                                {
                                    mode = Mode.Normal;
                                    stringBuilder.Append(c);
                                }
                                else
                                {
                                    AddAllText(stringBuilder, tentativeIgnoreSince, i, text);
                                    mode = Mode.IgnoreText;
                                }
                            }
                            else // if (collectedForOpen > collectedForClose)
                            {
                                if (collectedForOpen < 3)
                                    mode = Mode.TentativeIgnoreText;
                                else
                                    mode = Mode.IgnoreText;
                            }
                            break;
                    }
                }
            }
            if (mode == Mode.TentativeIgnoreText)
                AddAllText(stringBuilder, tentativeIgnoreSince, text.Length, text);

            return stringBuilder.ToString();
        }

        private void AddAllText(StringBuilder stringBuilder, int from, int to, string allText)
        {
            for (int i = from; i < to; i++)
                stringBuilder.Append(text[i]);
        }
    }

    enum Mode
    {
        Normal,
        IgnoreText,
        TentativeIgnoreText, // When we're not yet sure if text needs to be ignored (e.g. when a ` was opened, but we're not sure if it will close
        Opening,
        Closing
    }
}
