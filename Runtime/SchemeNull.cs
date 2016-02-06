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
    public sealed class SchemeNull : Datum, IEnumerable<SchemeObject>
    {
        private static readonly IEnumerable<SchemeObject> EmptyList = new SchemeObject[0];

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly SchemeNull Instance = new SchemeNull();

        private SchemeNull()
        {
        }

        public override bool IsList()
        {
            return true;
        }

        public override void Write(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            writer.Write("()");
        }

        public IEnumerator<SchemeObject> GetEnumerator()
        {
            return EmptyList.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}