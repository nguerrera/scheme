// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.ObjectModel;

namespace Scheme
{
    internal enum ContinuationKind
    {
        Conditional,
        Assignment,
        Procedure,
        Arguments,
        Sequence
    }

    internal abstract class ContinuationFrame
    {
        internal readonly SchemeEnvironment Environment;
        internal readonly ContinuationKind Kind;

        protected ContinuationFrame(SchemeEnvironment environment, ContinuationKind kind)
        {
            this.Environment = environment;
            this.Kind = kind;
        }
    }

    internal class ConditionalContinuation : ContinuationFrame
    {
        internal readonly Expression Consequent;
        internal readonly Expression Alternate;

        internal ConditionalContinuation(Expression consequent, Expression alternate, SchemeEnvironment environment)
            : base(environment, ContinuationKind.Conditional)
        {
            this.Consequent = consequent;
            this.Alternate = alternate;
        }
    }

    internal class AssignmentContinuation : ContinuationFrame
    {
        internal readonly Symbol Destination;
        internal readonly bool IsDefinition;

        internal AssignmentContinuation(Symbol destination, bool isDefinition, SchemeEnvironment environment)
            : base(environment, ContinuationKind.Assignment)
        {
            this.Destination = destination;
            this.IsDefinition = isDefinition;
        }
    }

    internal class ProcedureContinuation : ContinuationFrame
    {
        internal readonly ReadOnlyCollection<Expression> Arguments;
        internal ProcedureContinuation(ReadOnlyCollection<Expression> arguments, SchemeEnvironment environment)
            : base(environment, ContinuationKind.Procedure)
        {
            this.Arguments = arguments;
        }
    }

    internal class ArgumentsContinuation : ContinuationFrame
    {
        internal readonly Procedure Procedure;
        internal readonly ReadOnlyCollection<Expression> Arguments;
        internal readonly SchemeObject[] EvaluatedArguments;
        internal int Index;
        
        internal ArgumentsContinuation(Procedure procedure, ReadOnlyCollection<Expression> arguments, SchemeEnvironment environment)
            : base(environment, ContinuationKind.Arguments)
        {
            this.Procedure = procedure;
            this.Arguments = arguments;
            this.EvaluatedArguments = new SchemeObject[arguments.Count];
        }
    }

    internal class SequenceContinuation : ContinuationFrame
    {
        internal readonly Sequence Sequence;
        internal int Index;

        internal SequenceContinuation(Sequence sequence, SchemeEnvironment environment)
            : base(environment, ContinuationKind.Sequence)
        {
            this.Sequence = sequence;
        }
    }
}