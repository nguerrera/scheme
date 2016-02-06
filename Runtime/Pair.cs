// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Scheme
{
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public sealed class Pair : Datum, IEnumerable<SchemeObject>
    {
        private SchemeObject car;
        private SchemeObject cdr;

        public Pair(SchemeObject car, SchemeObject cdr)
        {
            this.car = car;
            this.cdr = cdr;
        }

        public SchemeObject Car
        {
            get { return this.car; }
            set { this.car = value; }
        }

        public SchemeObject Cdr
        {
            get { return this.cdr; }
            set { this.cdr = value; }
        }

        public override void Write(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            string prettyCar = null;
            Symbol carSymbol = this.car as Symbol;
            Pair cdrPair = this.cdr as Pair;

            if (carSymbol != null && cdrPair != null)
            {
                prettyCar = Abbreviation.Abbreviate(carSymbol.Value);
            }
            if (prettyCar != null)
            {
                writer.Write(prettyCar);
                cdrPair.PrettyPrintRest(writer, false);
            }
            else
            {
                writer.Write("(");
                this.PrettyPrintRest(writer, true);
            }
        }

        private void PrettyPrintRest(TextWriter writer, bool printClosingParen)
        {
            this.car.Write(writer);
            if (this.cdr == SchemeNull.Instance)
            {
                if (printClosingParen)
                {
                    writer.Write(")");
                }
            }
            else
            {
                Pair cdrPair = cdr as Pair;
                if (cdrPair == null)
                {
                    writer.Write(" . ");
                    cdr.Write(writer);
                    writer.Write(")");
                }
                else
                {
                    writer.Write(" ");
                    cdrPair.PrettyPrintRest(writer, printClosingParen);
                }
            }
        }

        public override bool IsList()
        {
            PairEnumerator enumerator = this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                continue;
            }
            return enumerator.FinalCdr == SchemeNull.Instance;
        }

        public PairEnumerator GetEnumerator()
        {
            return new PairEnumerator(this);
        }

        IEnumerator<SchemeObject> IEnumerable<SchemeObject>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}