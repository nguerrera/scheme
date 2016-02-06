// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.IO;
using System.Collections.Generic;

namespace Scheme
{
    public sealed class Symbol : Datum
    {
        private readonly static Dictionary<string, Symbol> table = new Dictionary<string, Symbol>();
        private readonly string value;

        public static Symbol For(string value)
        {
            Symbol result;
            if (Symbol.table.TryGetValue(value, out result))
                return result;

            result = new Symbol(value);
            Symbol.table.Add(value, result);
            return result;
        }

        private Symbol(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return this.value; }
        }

        public override void Write(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            writer.Write(this.value);
        }
    }
}
