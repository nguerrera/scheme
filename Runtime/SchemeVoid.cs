// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.IO;
using System.Diagnostics.CodeAnalysis;

namespace Scheme
{
    public sealed class SchemeVoid : SchemeObject
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly SchemeVoid Instance = new SchemeVoid();

        private SchemeVoid()
        {
        }

        public override void Write(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            writer.Write("#<void>");
        }
    }
}
