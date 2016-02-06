// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.IO;

namespace Scheme
{
    public sealed class SchemeChar : Datum, IToken
    {
        private readonly char value;

        public SchemeChar(char value)
        {
            this.value = value;
        }

        public char Value
        {
            get { return this.value; }
        }

        public override void Write(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            writer.Write(@"#\");
            if (this.value == ' ')
            {
                writer.Write("space");
            }
            else if (this.value == '\n')
            {
                writer.Write("newline");
            }
            else
            {
                writer.Write(this.value);
            }
        }

        public override void Display(TextWriter writer)
        {
            writer.Write(this.value);
        }
    }
}
