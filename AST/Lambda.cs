// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Scheme
{
    public sealed class Lambda : Expression
    {
        private readonly Formals formals;
        private readonly Expression body;

        public Lambda(Formals formals, Expression body)
        {
            this.formals = formals;
            this.body = body;
        }

        public Formals Formals
        {
            [DebuggerStepThrough]
            get { return this.formals; }
        }

        public Expression Body
        {
            [DebuggerStepThrough]
            get { return this.body; }
        }

        public override NodeType NodeType
        {
            [DebuggerStepThrough]
            get { return NodeType.LambdaExpression; }
        }

        public ReadOnlyCollection<Variable> Parameters
        {
            [DebuggerStepThrough]
            get { return this.formals.Parameters; }
        }

        public int ParameterCount
        {
            [DebuggerStepThrough]
            get { return this.formals.ParameterCount; }
        }

        public Variable RestParameter
        {
            [DebuggerStepThrough]
            get { return this.formals.RestParameter; }
        }

      
    }
}

