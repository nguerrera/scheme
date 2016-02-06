// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Globalization;
using System.IO;

namespace Scheme
{
    public sealed class SchemeNumber : Datum
    {
        private readonly double value;

        public SchemeNumber(double value)
        {
            this.value = value;
        }

        public static bool TryParse(string input, out SchemeNumber result)
        {
            double value;
            if (Double.TryParse(input, out value))
            {
                result = new SchemeNumber(value);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override void Write(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            writer.Write(this.value.ToString(CultureInfo.InvariantCulture));
        }

        public double Value
        {
            get { return this.value; }
        }
    }
}
     
