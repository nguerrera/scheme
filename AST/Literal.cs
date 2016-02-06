// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

namespace Scheme
{
    public sealed class Literal : Expression
    {
        private SchemeObject value;

        public Literal(SchemeObject value)
        {
            this.value = value;
        }

        public SchemeObject Value
        {
            get { return this.value; }
        }

        public override NodeType NodeType
        {
            get { return NodeType.Literal; }
        }
    }
}
