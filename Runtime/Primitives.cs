// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;

namespace Scheme
{
    public static class Primitives
    {
        public static SchemeObject Add(params SchemeObject[] numbers)
        {
            double sum = 0;
            foreach (SchemeNumber number in numbers)
            {
                sum += number.Value;
            }
            return new SchemeNumber(sum);
        }

        public static SchemeObject Display(params SchemeObject[] objects)
        {
            if (objects.Length != 1) throw new InvalidSyntaxException();
            objects[0].Display(Console.Out);
            return SchemeVoid.Instance;
        }

        public static SchemeObject Exit(params SchemeObject[] objects)
        {
            System.Environment.Exit(0);
            return SchemeVoid.Instance;
        }

        public static SchemeNumber Subtract(params SchemeObject[] numbers)
        {
            double difference;
            SchemeNumber first = (SchemeNumber)numbers[0];

            if (numbers.Length == 1)
            {
                difference = 0 - first.Value;
            }
            else
            {
                difference = first.Value;
                for (int i = 1; i < numbers.Length; i++)
                {
                    SchemeNumber number = (SchemeNumber)numbers[i];
                    difference -= number.Value;
                }
            }
            return new SchemeNumber(difference);
        }

        public static SchemeNumber Multiply(params SchemeObject[] numbers)
        {
            double product = 1;
            foreach (SchemeNumber number in numbers)
            {
                product *= number.Value;
            }
            return new SchemeNumber(product);
        }

        public static SchemeNumber Divide(params SchemeObject[] numbers)
        {
            double quotient;
            SchemeNumber first = (SchemeNumber)numbers[0];

            if (numbers.Length == 1)
            {
                quotient = 1 / first.Value;
            }
            else
            {
                quotient = first.Value;
                for (int i = 1; i < numbers.Length; i++)
                {
                    SchemeNumber number = (SchemeNumber)numbers[i];
                    quotient /= number.Value;
                }
            }
            return new SchemeNumber(quotient);
        }

        public static SchemeObject NumericEqual(params SchemeObject[] numbers)
        {
            if (numbers.Length != 2)
            {
                throw new InvalidSyntaxException();
            }
            SchemeNumber first = (SchemeNumber)numbers[0];
            SchemeNumber second = (SchemeNumber)numbers[1];
            return first.Value == second.Value ? SchemeBoolean.True : SchemeBoolean.False;
        }

        public static SchemeObject Cons(params SchemeObject[] objects)
        {
            if (objects.Length != 2)
            {
                throw new InvalidSyntaxException();
            }
            return new Pair(objects[0], objects[1]);
        }

        public static SchemeObject Car(params SchemeObject[] objects)
        {
            if (objects.Length != 1)
            {
                throw new InvalidSyntaxException();
            }
            return ((Pair)objects[0]).Car;
        }

        public static SchemeObject Cdr(params SchemeObject[] objects)
        {
            if (objects.Length != 1)
            {
                throw new InvalidSyntaxException();
            }
            return ((Pair)objects[0]).Cdr;
        }
    }
}