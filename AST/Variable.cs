// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

namespace Scheme
{
    public sealed class Variable : Expression
    {
        private readonly Symbol symbol;

        public Variable(Symbol symbol)
        {
            this.symbol = symbol;
        }

        public Symbol Symbol
        {
            get { return this.symbol; }
        }

        public override NodeType NodeType
        {
            get { return NodeType.Variable; }
        }
    }
}
