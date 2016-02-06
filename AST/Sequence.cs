// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Scheme
{
    public sealed class Sequence : Expression
    {
        private readonly bool isDefinition;
        private readonly ReadOnlyCollection<Expression> expressions;

        public Sequence(IList<Expression> expressions)
        {
            this.expressions = new ReadOnlyCollection<Expression>(expressions);
            this.isDefinition = HasOnlyDefinitions(this.expressions);
        }

        private static bool HasOnlyDefinitions(IEnumerable<Expression> collection)
        {
            foreach (Expression expression in collection)
                if (!expression.IsDefinition)
                    return false;

            return true;
        }

        public ReadOnlyCollection<Expression> Expressions
        {
            get { return this.expressions; }
        }

        public override NodeType NodeType
        {
            get { return NodeType.Sequence; }
        }

        public override bool IsDefinition
        {
            get { return this.isDefinition; }
        }
    }
}
