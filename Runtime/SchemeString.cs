// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.IO;

namespace Scheme
{
    public sealed class SchemeString : Datum
    {
        private readonly string value;

        public SchemeString(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.value = value;
        }

        public string Value
        {
            get { return this.value; }
        }

        public override void Write(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            writer.Write('"');
            string s = this.value.Replace(@"\", @"\\");
            s = s.Replace("\"", "\\\"");
            writer.Write(s);
            writer.Write('"');
        }

        public override void Display(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            writer.Write(this.value);
        }
    }
}
