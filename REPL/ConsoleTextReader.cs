using System;
using System.IO;

namespace Scheme
{
    //
    // The TextReader used for the read-eval-print loop.
    //
    // We do not use Console.In directly for the following reasons:
    //     1. Console.In.Peek() returns -1 (EOF) after the first newline,
    //        which is nonsense and completely breaks the scanner.
    //
    //     2. We need a mechanism to discard trailing characters from
    //        a line of input with bad syntax.
    //
    //     3. We need a mechanism to determine if there is any buffered
    //        text left to be read by the scanner so that we will not 
    //        print superfluous '>' prompts while printing the remaining
    //        results.
    //
    internal sealed class ConsoleTextReader : TextReader
    {
        private string bufferedText;
        private int index;

        public override int Read()
        {
            return Read(true);
        }

        public override int Peek()
        {
            return Read(false);
        }

        private int Read(bool consumeCharacter)
        {
            int result;
            if (!this.BufferedTextAvailable)
            {
                bufferedText = Console.ReadLine() + System.Environment.NewLine;
                index = 0;
            }

            result = bufferedText[index];
            if (consumeCharacter)
                index++;
         
            return result;
        }

        public bool BufferedTextAvailable
        {
            get
            {
                if (bufferedText == null)
                {
                    return false;
                }
                return (index < bufferedText.Length);
            }
        }

        public void DiscardBufferedText()
        {
            bufferedText = null;
        }

        public void DiscardTrailingAtmosphere()
        {
            int n = bufferedText.Length;
            while (index < n)
            {
                char c = bufferedText[index];
                if (Char.IsWhiteSpace(c))
                {
                    index++;
                }
                else if (c == ';')
                {
                    index = n;
                }
                else
                {
                    break;
                }
            }
        }
    }
}