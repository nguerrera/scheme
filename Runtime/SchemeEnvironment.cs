// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;

namespace Scheme
{
    internal sealed class SchemeEnvironment
    {
        private static SchemeEnvironment @default = InitializeDefaultEnvironment();

        private static SchemeEnvironment InitializeDefaultEnvironment()
        {
            SchemeEnvironment env = new SchemeEnvironment(null);
            env.Add(Symbol.For("+"), new Primitive(Primitives.Add));
            env.Add(Symbol.For("-"), new Primitive(Primitives.Subtract));
            env.Add(Symbol.For("/"), new Primitive(Primitives.Divide));
            env.Add(Symbol.For("*"), new Primitive(Primitives.Multiply));
            env.Add(Symbol.For("="), new Primitive(Primitives.NumericEqual));
            env.Add(Symbol.For("cons"), new Primitive(Primitives.Cons));
            env.Add(Symbol.For("car"), new Primitive(Primitives.Car));
            env.Add(Symbol.For("cdr"), new Primitive(Primitives.Cdr));
            env.Add(Symbol.For("display"), new Primitive(Primitives.Display));
            env.Add(Symbol.For("exit"), new Primitive(Primitives.Exit));
            return env;
        }

        private Dictionary<Symbol, SchemeObject> dictionary;
        private SchemeEnvironment parent;

        public static SchemeEnvironment Default
        {
            get { return SchemeEnvironment.@default; }
        }
       
        public SchemeEnvironment(SchemeEnvironment parent)
        {
            this.parent = parent;
            this.dictionary = new Dictionary<Symbol, SchemeObject>();
        }

        public SchemeObject GetValue(Symbol key)
        {
            for (SchemeEnvironment env = this; env != null; env = env.parent)
            {
                SchemeObject value;
                if (env.dictionary.TryGetValue(key, out value))
                    return value;
            }
            throw new InvalidSyntaxException();
        }

        public void SetValue(Symbol key, SchemeObject value, bool isDefinition)
        {
            SchemeEnvironment env = this;
            if (!isDefinition)
            {
                for (; env != null; env = env.parent)
                    if (env.dictionary.ContainsKey(key))
                        break;

                if (env == null)
                    throw new InvalidSyntaxException();
            }
            env.dictionary[key] = value;
        }

        public void Add(Variable key, SchemeObject value)
        {
            Add(key.Symbol, value);
        }

        public void Add(Symbol key, SchemeObject value)
        {
            this.dictionary[key] = value;
        }
    }
}
