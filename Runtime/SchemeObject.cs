// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Globalization;
using System.IO;

namespace Scheme
{
    public abstract class SchemeObject
    {
        protected SchemeObject()
        {
        }

        public abstract void Write(TextWriter writer);

        public virtual void Display(TextWriter writer)
        {
            Write(writer);
        }

        public void WriteLine(TextWriter writer)
        {
            Write(writer);
            writer.WriteLine();
        }

        public sealed override string ToString()
        {
            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                writer.Write('[');
                writer.Write(this.GetType().Name);
                writer.Write(" : ");
                this.Write(writer);
                writer.Write(']');
                return writer.ToString();
            }
        }

        public string PrettyPrint()
        {
            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                this.Write(writer);
                return writer.ToString();
            }
        }

        public virtual bool IsList()
        {
            return false;
        }
    }
}
