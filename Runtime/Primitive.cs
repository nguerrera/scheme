// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.IO;

namespace Scheme
{
    public delegate SchemeObject PrimitiveCallback(SchemeObject[] arguments);

    public sealed class Primitive : Procedure
    {
        private PrimitiveCallback callback;
        
        public Primitive(PrimitiveCallback callback)
        {
            this.callback = callback;
        }

        public PrimitiveCallback Callback
        {
            get { return this.callback; }
        }

        public override void Write(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            writer.Write("#<primitive>");
        }
    }
}