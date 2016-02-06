// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace Scheme
{
    public sealed class Reader
    {
        private Scanner scanner;
        private IToken unreadToken;

        public Reader(TextReader input)
        {
            this.scanner = new Scanner(input);
        }

        public Datum NextDatum()
        {
            IToken token = this.scanner.NextToken();
            if (token == null)
                return null;

            Unread(token);
            return ParseDatum();
        }

        // <datum> --> <boolean> 
        //           | <number>
        //           | <character> 
        //           | <string> 
        //           | <symbol>
        //           | <abbreviation>
        //           |  ( <list>
        //           | #( <vector>
        private Datum ParseDatum()
        {
            IToken token = NextToken();

            if (token == Token.PoundOpenParenthesis)
                return ParseVector();

            if (token == Token.OpenParenthesis)
                return ParseList();

            Datum datum = token as Datum;
            if (datum != null)
                return datum;

            Abbreviation abbreviation = token as Abbreviation;
            if (abbreviation != null)
            {
                Symbol car = Symbol.For(abbreviation.Expansion);
                Pair cdr = new Pair(ParseDatum(), SchemeNull.Instance);
                return new Pair(car, cdr);
            }

            throw new InvalidSyntaxException();
        }

        // <list> -> )                      
        //         | <datum> <sublist> )
        private Datum ParseList()
        {
            IToken token = NextToken();
            if (token == Token.CloseParenthesis)
                return SchemeNull.Instance;
            
            Unread(token);
            Datum car = ParseDatum();
            Datum cdr = ParseSublist();
            return new Pair(car, cdr);
        }

        // <sublist> -> . <datum> )
        //            | <list>
        private Datum ParseSublist()
        {
            IToken token = NextToken();
            if (token == Token.Dot)
            {
                Datum datum = ParseDatum();
                token = NextToken();
                if (token != Token.CloseParenthesis)
                    throw InvalidSyntaxException.Format(Strings.ExpectedCloseParenthesis, token);
            
                return datum;
            }

            Unread(token);
            return ParseList();
        }

        // <vector> -> <datum>* )
        private Vector ParseVector()
        {
            List<SchemeObject> list = new List<SchemeObject>();
            IToken token;
            
            while ((token = NextToken()) != Token.CloseParenthesis)
            {
                Unread(token);
                Datum datum = ParseDatum();
                list.Add(datum);
            }

            return new Vector(list);
        }

        private void Unread(IToken token)
        {
            if (this.unreadToken != null)
            {
                Debug.Assert(false);
                throw new InvalidOperationException();
            }
            this.unreadToken = token;
        }

        private IToken NextToken()
        {
            IToken token;
            if (this.unreadToken != null)
            {
                token = this.unreadToken;
                this.unreadToken = null;
                return token;
            }

            token = this.scanner.NextToken();
            if (token == null)
                throw new InvalidSyntaxException(Strings.UnexpectedEof);
         
            return token;
        }
    }
}
