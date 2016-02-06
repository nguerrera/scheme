using System;
using System.Globalization;
using System.Collections.Generic;

namespace Scheme
{
    internal sealed class Token : IToken
    {
        public static readonly Token Dot                  = new Token();
        public static readonly Token PoundOpenParenthesis = new Token();
        public static readonly Token OpenParenthesis      = new Token();
        public static readonly Token CloseParenthesis     = new Token();

        private Token() {}
    }

    internal sealed class Abbreviation : IToken
    {
        private static Dictionary<string, string> table = new Dictionary<string, string>(StringComparer.Ordinal);
        private string expansion;

        public static readonly Abbreviation Quote          = new Abbreviation("'",   "quote");
        public static readonly Abbreviation Backtick       = new Abbreviation("`",   "quasiquote");
        public static readonly Abbreviation Comma          = new Abbreviation(",",   "unquote");
        public static readonly Abbreviation CommaAt        = new Abbreviation(",@",  "unquote-splicing");
        public static readonly Abbreviation PoundQuote     = new Abbreviation("#'",  "syntax");
        public static readonly Abbreviation PoundBacktick  = new Abbreviation("#`",  "quasisyntax");
        public static readonly Abbreviation PoundComma     = new Abbreviation("#,",  "unsyntax");
        public static readonly Abbreviation PoundCommaAt   = new Abbreviation("#,@", "unsyntax-splicing");
        
        private Abbreviation(string abbreviation, string expansion)
        {
            this.expansion = expansion;
            Abbreviation.table.Add(expansion, abbreviation);
        }

        public static string Abbreviate(string expansion)
        {
            string result;
            if (!Abbreviation.table.TryGetValue(expansion, out result))
                return null;

            return result;
        }

        public string Expansion 
        {
            get { return this.expansion; }
        }
    }
}
