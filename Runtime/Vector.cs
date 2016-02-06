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
    public sealed class Vector : Datum, IList<SchemeObject>
    {
        private readonly SchemeObject[] data;

        public Vector(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            this.data = new SchemeObject[count];
        }

        public Vector(ICollection<SchemeObject> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            this.data = new SchemeObject[collection.Count];
            collection.CopyTo(this.data, 0);
        }

        public SchemeObject this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                    throw new ArgumentOutOfRangeException("index");
 
                return this.data[index];
            }
            set
            {
                if (index < 0 || index >= this.Count)
                    throw new ArgumentOutOfRangeException("index");
     
                this.data[index] = value;
            }
        }

        public int Count
        {
            get { return this.data.Length; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Contains(SchemeObject item)
        {
            return (this.IndexOf(item) != -1);
        }

        public int IndexOf(SchemeObject item)
        {
            return Array.IndexOf(this.data, item);
        }

        public void CopyTo(SchemeObject[] array, int arrayIndex)
        {
            IList<SchemeObject> list = this.data;
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<SchemeObject> GetEnumerator()
        {
            IList<SchemeObject> list = this.data;
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override void Write(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            writer.Write("#(");
            for (int i = 0, n = this.data.Length; i < n; i++)
            {
                SchemeObject d = this.data[i];
                d.Write(writer);
                if (i < n - 1)
                {
                    writer.Write(" ");
                }
            }
            writer.Write(")");
        }

        void IList<SchemeObject>.Insert(int index, SchemeObject item)
        {
            throw new NotSupportedException();
        }

        void IList<SchemeObject>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void ICollection<SchemeObject>.Add(SchemeObject item)
        {
            throw new NotSupportedException();
        }

        void ICollection<SchemeObject>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<SchemeObject>.Remove(SchemeObject item)
        {
            throw new NotSupportedException();
        }
    }

}