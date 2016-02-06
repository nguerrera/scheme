// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.IO;
using System.Diagnostics.CodeAnalysis;

namespace Scheme
{
    public sealed class SchemeBoolean : Datum
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly SchemeBoolean True = new SchemeBoolean(true);

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly SchemeBoolean False = new SchemeBoolean(false);

        private readonly bool value;

        private SchemeBoolean(bool value)
        {
            this.value = value;
        }

        public bool Value
        {
            get { return this.value; }
        }

        public override void Write(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            writer.Write(value ? "#t" : "#f");
        }
    }
}
