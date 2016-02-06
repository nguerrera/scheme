using System;
// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace Scheme
{
    public sealed class Closure : Procedure
    {
        private readonly SchemeEnvironment environment;
        private readonly Lambda lambda;

        internal Closure(SchemeEnvironment environment, Lambda expression)
        {
            this.environment = environment;
            this.lambda = expression;
        }

        internal SchemeEnvironment Environment
        {
            [DebuggerStepThrough]
            get { return this.environment; }
        }

        internal Expression Body
        {
            [DebuggerStepThrough]
            get { return this.lambda.Body; }
        }

        internal ReadOnlyCollection<Variable> Parameters
        {
            [DebuggerStepThrough]
            get { return this.lambda.Parameters; }
        }

        internal Variable RestParameter
        {
            [DebuggerStepThrough]
            get { return this.lambda.RestParameter; }
        }

        public override void Write(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            writer.Write("#<procedure>");
        }
    }
}