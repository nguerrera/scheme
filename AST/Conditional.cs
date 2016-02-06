// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Diagnostics;

namespace Scheme
{
    public sealed class Conditional : Expression
    {
        private readonly Expression test;
        private readonly Expression consequent;
        private readonly Expression alternate;

        public Conditional(Expression test, Expression consequent, Expression alternate)
        {
            this.test = test;
            this.consequent = consequent;
            this.alternate = alternate;
        }

        public Expression Test
        {
            [DebuggerStepThrough]
            get { return this.test; }
        }

        public Expression Consequent
        {
            [DebuggerStepThrough]
            get { return this.consequent; }
        }

        public Expression Alternate
        {
            [DebuggerStepThrough]
            get { return this.alternate; }
        }

        public override NodeType NodeType
        {
            [DebuggerStepThrough]
            get { return NodeType.Conditional; }
        }
    }
}
