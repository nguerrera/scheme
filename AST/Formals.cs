// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Scheme
{
    public class Formals
    {
        private readonly ReadOnlyCollection<Variable> parameters;
        private readonly Variable restParameter;

        public Formals(IList<Variable> parameters, Variable restParameter)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");
            this.parameters = new ReadOnlyCollection<Variable>(parameters);
            this.restParameter = restParameter;
        }

        public ReadOnlyCollection<Variable> Parameters
        {
            [DebuggerStepThrough]
            get { return this.parameters; }
        }

        public Variable RestParameter
        {
            [DebuggerStepThrough]
            get { return this.restParameter; }
        }

        public int ParameterCount
        {
            [DebuggerStepThrough]
            get { return this.parameters.Count; }
        }
    }
}
