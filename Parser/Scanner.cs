// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Scheme
{
    internal sealed class Scanner
    {
        private int line;
        private int column;
        private StringBuilder resultBuffer;
        private StringBuilder matchBuffer;
        private TextReader input;
        private Stack<char> stack;

        public Scanner(TextReader input)
        {
            this.line = 1;
            this.column = 1;
            this.input = input;
            this.stack = new Stack<char>();
            this.resultBuffer = new StringBuilder();
            this.matchBuffer = new StringBuilder();
        }

        public IToken NextToken()
        {
            SkipAtmosphere();
            if (Eof) return null;

            char c = Peek();
            if (IsInitial(c))
                return NextSymbol();
            
            if (IsDigit(c)) 
                return NextNumber();

            Read();
            switch (c)
            {
                case '#':  return NextPound();
                case '"':  return NextString();
                case '.':  return NextDot();
                case ',':  return NextComma();
                case '+':  return NextPlus();
                case '-':  return NextMinus();
                case '`':  return Abbreviation.Backtick;
                case ')':  return Token.CloseParenthesis;
                case '(':  return Token.OpenParenthesis;
                case '\'': return Abbreviation.Quote;
            }

            throw new InvalidSyntaxException();
        }

        private void SkipAtmosphere()
        {
            bool inComment = false;
            while (!Eof)
            {
                char c = Peek();
                inComment |= (c == ';');
                inComment &= (c != '\r' && c != '\n');
                if (!inComment && !Char.IsWhiteSpace(c))
                    break;

                Read();
            }
        }

        private Symbol NextSymbol()
        {
            char c;
            this.resultBuffer.Length = 0;
            do 
            {
                this.resultBuffer.Append(Read());
                c = Peek();
            } while (!Eof && IsSubsequent(c));

            CheckForDelimiter();
            return Symbol.For(this.resultBuffer.ToString());
        }

        private SchemeNumber NextNumber()
        {
            char c;
            this.resultBuffer.Length = 0;

            do
            {
                this.resultBuffer.Append(Read());
                c = Peek();
            } while (!Eof && (IsDigit(c) || c == '.'));

            CheckForDelimiter();
            string s = this.resultBuffer.ToString();
            SchemeNumber result;
            if (!SchemeNumber.TryParse(s, out result))
            {
                throw InvalidSyntaxException.Format(Strings.UnmetExpectation, typeof(SchemeNumber).Name, s);
            }

            return result;
        }

        private IToken NextPound()
        {
            char c = Read();
            switch (c)
            {
                case '(':  return Token.PoundOpenParenthesis;
                case '`':  return Abbreviation.PoundBacktick;
                case '\'': return Abbreviation.PoundQuote;
                case 't':  return SchemeBoolean.True;
                case 'f':  return SchemeBoolean.False;
                
                case ',':
                    Read();
                    if (Peek() == '@')
                    {
                        Read();
                        return Abbreviation.PoundCommaAt;
                    }
                    return Abbreviation.PoundComma;

                case '\\':
                    if (ReadMatch("newline"))
                    {
                        c = '\n';
                    }
                    else if (ReadMatch("space"))
                    {
                        c = ' ';
                    }
                    else
                    {
                        c = ReadPreserveCase();
                    }
                    CheckForDelimiter();
                    return new SchemeChar(c);
            }

            throw InvalidSyntaxException.Format(Strings.InvalidToken, "#" + c);
        }

        private SchemeString NextString()
        {
            this.resultBuffer.Length = 0;

            char c;
            while (!Eof && PeekPreserveCase() != '"')
            {
                c = ReadPreserveCase();
                if (c == '\\' && !Eof)
                {
                    c = ReadPreserveCase();
                    if (c != '\\' && c != '"')
                    {
                        throw InvalidSyntaxException.Format(Strings.ExpectedBackslashOrDoubleQuote, c);
                    }
                }
                this.resultBuffer.Append(c);
            }

            if (Eof)
                throw new InvalidSyntaxException(Strings.UnexpectedEof);
  
            c = Read();
            Debug.Assert(c == '"');

            return new SchemeString(this.resultBuffer.ToString());
        }

        private IToken NextDot()
        {
            if (ReadMatch(".."))
                return Symbol.For("...");
  
            CheckForDelimiter();
            return Token.Dot;
        }

        private Abbreviation NextComma()
        {
            if (!Eof && Peek() == '@')
            {
                Read();
                return Abbreviation.CommaAt;
            }
            return Abbreviation.Comma;
        }

        private IToken NextPlus()
        {
            if (Eof || IsDelimiter(Peek()))
                return Symbol.For("+");

            return NextNumber();
        }

        private IToken NextMinus()
        {
            if (Eof || IsDelimiter(Peek()))
                return Symbol.For("-");

            return new SchemeNumber(-NextNumber().Value);
        }

        private void CheckForDelimiter()
        {
            if (Eof) return;
            char c = Peek();
            if (!IsDelimiter(c))
                throw InvalidSyntaxException.Format(Strings.ExpectedDelimiter, c);
        }

        private static bool IsDelimiter(char c)
        {
            return Char.IsWhiteSpace(c)
                || c == '(' || c == ')' || c == ';' || c == '"';
        }

        private static bool IsInitial(char c)
        {
            return IsLetter(c) || IsSpecialInitial(c);
        }
        
        private static bool IsLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        private static bool IsSpecialInitial(char c)
        {
            return (
                c == '!' || c == '$' || c == '%' || c == '&'
             || c == '*' || c == '/' || c == '*' || c == '/'
             || c == ':' || c == '<' || c == '=' || c == '>'
             || c == '?' || c == '^' || c == '_' || c == '~'
            );
        }

        private static bool IsSubsequent(char c)
        {
            return IsInitial(c) || IsDigit(c) || IsSpecialSubsequent(c);
        }

        private static bool IsSpecialSubsequent(char c)
        {
            return c == '+' || c == '-' || c == '.' || c == '@';
        }

        private static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool Eof
        {
            get { return (this.stack.Count == 0 && this.input.Peek() == -1); }
        }

        private char Read()
        {
            char c = ReadPreserveCase();
            return Char.ToLowerInvariant(c);
        }

        private char ReadPreserveCase()
        {
            if (this.stack.Count > 0)
                return this.stack.Pop();

            int i = this.input.Read();
            if (i == -1)
            {
                Debug.Assert(false);
                throw new InvalidOperationException();
            }

            char c = (char)i;
            if (c == '\n' || (c == '\r' && Peek() != '\n'))
            {
                this.column = 1;
                this.line++;
            }
            else
            {
                this.column++;
            }

            return c;
        }

        // If the upcoming characters match the given argument ignoring case,
        // return true and advance past the match. Otherwise, return false.
        private bool ReadMatch(string match)
        {
            this.matchBuffer.Length = 0;
            foreach (char m in match)
            {
                if (Eof)
                {
                    UnreadMatchBuffer();
                    return false;
                }

                char c = ReadPreserveCase();
                this.matchBuffer.Append(c);
                if (Char.ToLowerInvariant(c) != Char.ToLowerInvariant(m))
                {
                    UnreadMatchBuffer();
                    return false;
                }
            }

            return true;
        }

        private void Unread(char c)
        {
            this.stack.Push(c);
        }

        private void UnreadMatchBuffer()
        {
            for (int n = this.matchBuffer.Length - 1; n >= 0; n--)
                Unread(this.matchBuffer[n]);
        }

        private char Peek()
        {
            char c = PeekPreserveCase();
            return Char.ToLowerInvariant(c);
        }

        private char PeekPreserveCase()
        {
            if (this.stack.Count > 0)
                return this.stack.Peek();
   
            int i = this.input.Peek();
            if (i == -1)
            {
                Debug.Assert(false);
                throw new InvalidOperationException();
            }

            return (char)i;
        }
    }
}
