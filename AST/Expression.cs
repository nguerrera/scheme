// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

namespace Scheme
{
    public abstract class Expression
    {
        protected Expression()
        {
        }

        public virtual bool IsDefinition
        {
            get { return false; }
        }

        public abstract NodeType NodeType { get; }
    }
}
