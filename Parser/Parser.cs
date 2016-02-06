// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Scheme
{
    internal static class Parser
    {
        private enum Context  { TopLevel, StartBody, Expression }

        public static Expression ParseTopLevelExpression(SchemeObject input)
        {
            return ParseExpression(input, Context.TopLevel);
        }

        private static Expression ParseExpression(SchemeObject input)
        {
            return ParseExpression(input, Context.Expression);
        }

        private static Expression ParseExpression(SchemeObject input, Context context)
        {
            Symbol symbol = input as Symbol;
            if (symbol != null)
                return ParseVariable(symbol);

            Pair pair = input as Pair;
            if (pair == null)
                return ParseSelfEvaluating(input);

            Symbol car = pair.Car as Symbol;
            if (car != null)
            {
                switch (car.Value)
                {
                    case "lambda": return ParseLambda(pair);
                    case "set!":   return ParseAssignment(pair);
                    case "define": return ParseDefinition(pair, context);
                    case "if":     return ParseConditional(pair);
                    case "quote":  return ParseQuotation(pair);
                    case "begin":  return ParseSequence(pair, context);
                }
            }

            return ParseProcedureCall(pair);
        }

        //
        // <definition> -> (define <variable> <expression>)
        //               | (define (<variable> <variable>*)
        //               | (define (<variable> <variable>+ . <variable>)
        private static Assignment ParseDefinition(Pair pair, Context context)
        {
            if (context == Context.Expression)
                throw new InvalidSyntaxException("Illegal definition in expression context.");

            Variable destination;
            Expression source;

            PairEnumerator enumerator = GetEnumerator(pair, "define");
            MoveNext(ref enumerator);
            Symbol symbol = enumerator.Current as Symbol;
            if (symbol != null)
            {
                destination = ParseVariable(symbol);
                source = ParseExpression(MoveLast(ref enumerator));
            }
            else
            {
                Pair cadr = Expect<Pair>(enumerator.Current);
                Pair cddr = Expect<Pair>(enumerator.CurrentPair.Cdr);
                destination = ParseVariable(Expect<Symbol>(cadr.Car));
                Pair desugared = new Pair(Symbol.For("lambda"), new Pair(cadr.Cdr, cddr));
                source = ParseLambda(desugared);
            }

            return new Assignment(destination, source, true);
        }

        //
        // According to strict R5RS:
        //    <self-evaluating> --> <number> | <character> | <string> | <boolena>
        //
        // However, in this implemenation:
        //    <self-evaluating --> 'any object which is not also a pair or symbol'
        //
        // This is consistent with many popular Scheme implementations including
        // MzScheme, Chez Scheme, and MIT Scheme.
        //
        private static Expression ParseSelfEvaluating(SchemeObject input)
        {
            Debug.Assert(!(input is Symbol) && !(input is Pair));
            return new Literal(input);
        }

        // <begin> -> (begin <expression>+)
        private static Sequence ParseSequence(Pair pair, Context context)
        {
            PairEnumerator enumerator = GetEnumerator(pair, "begin");
            MoveNext(ref enumerator);

            List<Expression> expressions = new List<Expression>();
            do
            {
                Expression expression = ParseExpression(enumerator.Current, context);
                expressions.Add(expression);
                if (context == Context.StartBody && !expression.IsDefinition)
                    context = Context.Expression;

            } while (enumerator.MoveNext());

            return new Sequence(expressions);
        }

        // <variable> -> <symbol>
        private static Variable ParseVariable(Symbol symbol)
        {
            return new Variable(symbol);
        }

        // <conditonal> -> (if <expression> <expression>  <expression>?)
        private static Conditional ParseConditional(Pair pair)
        {
            PairEnumerator enumerator = GetEnumerator(pair, "if");

            SchemeObject obj = MoveNext(ref enumerator);
            Expression test = ParseExpression(obj);

            obj = MoveNext(ref enumerator);
            Expression consequent = ParseExpression(obj);

            Expression alternate = null;
            if (enumerator.CurrentPair.Cdr != SchemeNull.Instance)
            {
                obj = MoveLast(ref enumerator);
                alternate = ParseExpression(obj);
            }

            return new Conditional(test, consequent, alternate);
        }

        // <lambda expression> --> (lambda <formals> <expression>+)
        private static Lambda ParseLambda(Pair pair)
        {
            PairEnumerator enumerator = GetEnumerator(pair, "lambda");
            Datum datum = MoveNext<Datum>(ref enumerator);
            Formals formals = ParseFormals(datum);
            MoveNext(ref enumerator);

            SchemeObject obj = enumerator.Current;
            if (!enumerator.Finished)
                obj = new Pair(Symbol.For("begin"), enumerator.CurrentPair);
      
            Expression body = ParseExpression(obj, Context.StartBody);
            return new Lambda(formals, body);
        }


        // <formals> --> <variable>
        //             | (<variable>*) 
        //             | (<variable>+ . <variable>)
        private static Formals ParseFormals(Datum datum)
        {
            List<Variable> parameters = new List<Variable>();
            Variable restParameter = null;

            Symbol symbol = datum as Symbol;
            if (symbol != null)
            {
                restParameter = ParseVariable(symbol);
            }
            else if (datum != SchemeNull.Instance)
            {
                Pair pair = Expect<Pair>(datum);
                PairEnumerator enumerator = pair.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    symbol = Expect<Symbol>(enumerator.Current);
                    parameters.Add(ParseVariable(symbol));
                }

                SchemeObject cdr = enumerator.FinalCdr;
                if (cdr != SchemeNull.Instance)
                {
                    symbol = Expect<Symbol>(cdr);
                    restParameter = ParseVariable(symbol);
                }
            }
            return new Formals(parameters, restParameter);
        }

        // <quotation> -> (quote <datum>)
        private static Literal ParseQuotation(Pair pair)
        {
            PairEnumerator enumerator = GetEnumerator(pair, "quote");
            Datum datum = MoveNext<Datum>(ref enumerator);
            return new Literal(datum);
        }

        // <assignment> -> (set! <variable> <expression>)
        private static Assignment ParseAssignment(Pair pair)
        {
            PairEnumerator enumerator = GetEnumerator(pair, "set!");
            Symbol symbol = MoveNext<Symbol>(ref enumerator);
            Variable destination = ParseVariable(symbol);
            SchemeObject obj = MoveLast(ref enumerator);
            Expression source = ParseExpression(obj);
            return new Assignment(destination, source, false);
        }

        // <procedure call> --> (<expression> <expression>*)
        private static ProcedureCall ParseProcedureCall(Pair pair)
        {
            PairEnumerator enumerator = pair.GetEnumerator();
            SchemeObject obj = MoveNext(ref enumerator);
            Expression procedure = ParseExpression(obj);

            List<Expression> arguments = new List<Expression>();
            while (enumerator.MoveNext())
            {
                obj = enumerator.Current;
                Expression expression = ParseExpression(obj);
                arguments.Add(expression);
            }
            ExpectFinished(ref enumerator);
            return new ProcedureCall(procedure, arguments);
        }

        private static SchemeObject MoveNext(ref PairEnumerator enumerator)
        {
            if (!enumerator.MoveNext())
                throw new InvalidSyntaxException();
          
            return enumerator.Current;
        }

        private static SchemeObject MoveLast(ref PairEnumerator enumerator)
        {
            SchemeObject result = MoveNext(ref enumerator);
            enumerator.MoveNext();
            ExpectFinished(ref enumerator);
            return result;
        }

        private static void ExpectFinished(ref PairEnumerator enumerator)
        {
            if (!enumerator.Finished || enumerator.FinalCdr != SchemeNull.Instance)
            {
                throw new InvalidSyntaxException();
            }
        }

        private static T MoveNext<T>(ref PairEnumerator enumerator) where T : Datum
        {
            return Expect<T>(MoveNext(ref enumerator));
        }

        private static T Expect<T>(SchemeObject obj) where T : Datum
        {
            T result = obj as T;
            if (result == null)
                throw InvalidSyntaxException.Format(Strings.UnmetExpectation, obj.PrettyPrint(), typeof(T).Name);

            return result;
        }

        private static PairEnumerator GetEnumerator(Pair pair, string expectedCar)
        {
            PairEnumerator enumerator = pair.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                Debug.Assert(false);
                throw new InvalidOperationException();
            }

            Symbol symbol = (Symbol)enumerator.Current;
            if (symbol.Value != expectedCar)
            {
                Debug.Assert(false);
                throw new InvalidOperationException();
            }

            return enumerator;
        }
    }
}