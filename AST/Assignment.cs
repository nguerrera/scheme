// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Diagnostics;

namespace Scheme
{
    public sealed class Assignment : Expression
    {
        private readonly Variable destination;
        private readonly Expression source;
        private readonly bool isDefinition;

        public Assignment(Variable destination, Expression source, bool isDefinition)
        {
            this.destination = destination;
            this.source = source;
            this.isDefinition = isDefinition;
        }

        public Variable Destination
        {
            [DebuggerStepThrough] 
            get { return this.destination; }
        }

        public Expression Source
        {
            [DebuggerStepThrough]
            get { return this.source; }
        }

        public override bool IsDefinition
        {
            [DebuggerStepThrough]
            get { return this.isDefinition; }
        }

        public override NodeType NodeType
        {
            [DebuggerStepThrough]
            get { return NodeType.Assignment; }
        }
    }
}
