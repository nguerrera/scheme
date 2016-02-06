// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Scheme
{
    public sealed class ProcedureCall : Expression
    {
        private readonly Expression procedure;
        private readonly ReadOnlyCollection<Expression> arguments;

        internal ProcedureCall(Expression procedure, IList<Expression> arguments)
        {
            this.procedure = procedure;
            this.arguments = new ReadOnlyCollection<Expression>(arguments);
        }

        public Expression Procedure
        {
            [DebuggerStepThrough]
            get { return this.procedure; }
        }

        public ReadOnlyCollection<Expression> Arguments
        {
            [DebuggerStepThrough]
            get { return this.arguments; }
        }

        public override NodeType NodeType
        {
            [DebuggerStepThrough]
            get { return NodeType.ProcedureCall; }
        }
    }
}
