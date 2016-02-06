// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Scheme
{
    internal static class Interpreter
    {
        public static SchemeObject Evaluate(Expression expression, SchemeEnvironment environment)
        {
            Stack<ContinuationFrame> stack = new Stack<ContinuationFrame>();
            SchemeObject value = null;

        evaluate:
            switch (expression.NodeType)
            {
                case NodeType.Assignment:
                    {
                        Assignment assignment = (Assignment)expression;
                        stack.Push(new AssignmentContinuation(assignment.Destination.Symbol, assignment.IsDefinition, environment));
                        expression = assignment.Source;
                        goto evaluate;
                    }

                case NodeType.Conditional:
                    {
                        Conditional conditional = (Conditional)expression;
                        stack.Push(new ConditionalContinuation(conditional.Consequent, conditional.Alternate, environment));
                        expression = conditional.Test;
                        goto evaluate;
                    }

                case NodeType.LambdaExpression:
                    {
                        value = new Closure(environment, (Lambda)expression);
                        goto apply;
                    }

                case NodeType.Literal:
                    {
                        value = ((Literal)expression).Value;
                        goto apply;
                    }

                case NodeType.ProcedureCall:
                    {
                        ProcedureCall procedureCall = (ProcedureCall)expression;
                        stack.Push(new ProcedureContinuation(procedureCall.Arguments, environment));
                        expression = procedureCall.Procedure;
                        goto evaluate;
                    }

                case NodeType.Sequence:
                    {
                        Sequence sequence = (Sequence)expression;
                        stack.Push(new SequenceContinuation(sequence, environment));
                        goto apply;
                    }

                case NodeType.Variable:
                    {
                        Variable variable = (Variable)expression;
                        value = environment.GetValue(variable.Symbol);
                        goto apply;
                    }

                default:
                    {
                        System.Diagnostics.Debug.Assert(false);
                        throw new InvalidOperationException();
                    }
            }

        apply:
            if (stack.Count == 0)
                return value;

            ContinuationFrame frame = stack.Pop();
            environment = frame.Environment;
            switch (frame.Kind)
            {
                case ContinuationKind.Assignment:
                    {
                        AssignmentContinuation assignmentFrame = (AssignmentContinuation)frame;
                        environment.SetValue(assignmentFrame.Destination, value, assignmentFrame.IsDefinition);
                        value = SchemeVoid.Instance;
                        goto apply;
                    }

                case ContinuationKind.Procedure:
                    {
                        ProcedureContinuation procedureFrame = (ProcedureContinuation)frame;
                        ReadOnlyCollection<Expression> arguments = procedureFrame.Arguments;

                        Procedure procedure = value as Procedure;
                        if (procedure == null)
                            throw InvalidSyntaxException.Format("Error: {0} is not a procedure", value.PrettyPrint());

                        stack.Push(new ArgumentsContinuation(procedure, arguments, environment));
                        if (arguments.Count == 0)
                            goto apply;

                        expression = arguments[0];
                        goto evaluate;
                    }

                case ContinuationKind.Sequence:
                    {
                        SequenceContinuation sequenceFrame = (SequenceContinuation)frame;
                        Sequence sequence = sequenceFrame.Sequence;

                        int index = sequenceFrame.Index++;
                        if (index < sequence.Expressions.Count - 1)
                            stack.Push(sequenceFrame);

                        expression = sequence.Expressions[index];
                        goto evaluate;
                    }

                case ContinuationKind.Conditional:
                    {
                        ConditionalContinuation testFrame = (ConditionalContinuation)frame;
                        if (value != SchemeBoolean.False)
                        {
                            expression = testFrame.Consequent;
                            goto evaluate;
                        }

                        if (testFrame.Alternate != null)
                        {
                            expression = testFrame.Alternate;
                            goto evaluate;
                        }

                        value = SchemeVoid.Instance;
                        goto apply;
                    }

                case ContinuationKind.Arguments:
                    {
                        ArgumentsContinuation argumentsFrame = (ArgumentsContinuation)frame;

                        int index = argumentsFrame.Index++;
                        if (index < argumentsFrame.Arguments.Count)
                        {
                            argumentsFrame.EvaluatedArguments[index] = value;
                        }

                        if (argumentsFrame.Index < argumentsFrame.Arguments.Count)
                        {
                            stack.Push(argumentsFrame);
                            expression = argumentsFrame.Arguments[argumentsFrame.Index];
                            goto evaluate;
                        }

                        Primitive primitive = argumentsFrame.Procedure as Primitive;
                        if (primitive != null)
                        {
                            value = primitive.Callback(argumentsFrame.EvaluatedArguments);
                            goto apply;
                        }

                        Closure closure = (Closure)argumentsFrame.Procedure;
                        expression = closure.Body;
                        environment = BuildCallEnvironment(argumentsFrame.EvaluatedArguments, closure);
                        goto evaluate;
                    }

                default:
                    {
                        Debug.Assert(false);
                        throw new InvalidOperationException();
                    }
            }
            
        }

        private static SchemeEnvironment BuildCallEnvironment(SchemeObject[] arguments, Closure closure)
        {
            ReadOnlyCollection<Variable> parameters = closure.Parameters;
            Variable restParameter = closure.RestParameter;

            if (arguments.Length != parameters.Count && restParameter == null)
                throw InvalidSyntaxException.Format(Strings.IncorrectArgumentCount, parameters.Count, arguments.Length);

            if (arguments.Length < parameters.Count)
                throw InvalidSyntaxException.Format(Strings.TooFewArguments, parameters.Count, arguments.Length);

            SchemeEnvironment result = new SchemeEnvironment(closure.Environment);
            int index = 0;
            for (; index < parameters.Count; index++)
                result.Add(parameters[index], arguments[index]);

            if (restParameter != null)
            {
                Datum restArgument = BuildRestArgument(arguments, index);
                result.Add(closure.RestParameter, restArgument);
            }

            return result;
        }

        private static Datum BuildRestArgument(SchemeObject[] arguments, int index)
        {
            if (index == arguments.Length)
                return SchemeNull.Instance;

            Pair current = new Pair(SchemeNull.Instance, SchemeNull.Instance);
            Pair list = current;
            for (; index < arguments.Length - 1; index++)
            {
                current.Car = arguments[index];
                current.Cdr = new Pair(SchemeNull.Instance, SchemeNull.Instance);
                current = (Pair)current.Cdr;
            }

            current.Car = arguments[index];
            return list;
        }
    }
}
