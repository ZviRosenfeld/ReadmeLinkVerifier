using System;
using System.Runtime.CompilerServices;
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
            var ignoreSince = 0;
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
                            if (StartsAtBeginingOfLine(i - 1, text))
                            {
                                collectedForClose = 1;
                                mode = Mode.Closing;
                            }
                            break;
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
                        case Mode.TentativeIgnoreText:
                            if (collectedForOpen >= 3 && c == '\n')
                            {
                                AddAllText(stringBuilder, ignoreSince, i - collectedForClose, text);
                                stringBuilder.Append(c);
                                mode = Mode.Normal;
                            }
                            break;
                        case Mode.Opening:
                            mode = collectedForOpen >= 3 && StartsAtBeginingOfLine(i - collectedForOpen - 1, text)
                                ? Mode.IgnoreText
                                : Mode.TentativeIgnoreText;
                            ignoreSince = i - collectedForOpen;
                            break;
                        case Mode.Closing:
                            if (collectedForOpen == collectedForClose)
                            {
                                mode = Mode.Normal;
                                AddMissedNewLines(stringBuilder, ignoreSince, i, text);
                                stringBuilder.Append(c);
                            }
                            else if (collectedForOpen < collectedForClose)
                            {
                                if (collectedForOpen < 3 && collectedForClose < 3)
                                    mode = Mode.TentativeIgnoreText;
                                else if (collectedForOpen >= 3 && collectedForClose >= 3)
                                {
                                    mode = Mode.Normal;
                                    AddMissedNewLines(stringBuilder, ignoreSince, i, text);
                                    stringBuilder.Append(c);
                                }
                                else
                                {
                                    AddAllText(stringBuilder, ignoreSince, i - collectedForClose, text);
                                    ignoreSince = i - collectedForClose;
                                    collectedForOpen = collectedForClose;
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
                AddAllText(stringBuilder, ignoreSince, text.Length, text);
            if (mode == Mode.IgnoreText)
                AddMissedNewLines(stringBuilder, ignoreSince, text.Length, text);

            return stringBuilder.ToString();
        }

        private bool StartsAtBeginingOfLine(int from, string fullText)
        {
            for (int i = from; i >= 0; i--)
            {
                if (fullText[i] == '\n')
                    return true;
                if (!char.IsWhiteSpace(fullText[i]))
                    return false;
            }

            return true;
        }

        private void AddAllText(StringBuilder stringBuilder, int from, int to, string allText)
        {
            for (int i = from; i < to; i++)
                stringBuilder.Append(allText[i]);
        }

        private void AddMissedNewLines(StringBuilder stringBuilder, int from, int to, string allText)
        {
            for (int i = from; i < to; i++)
                if (allText[i] == '\n')
                    stringBuilder.Append(Environment.NewLine);
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
