// Copyright (c) Nick Guerrera. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Scheme
{
    [Serializable]
    public sealed class InvalidSyntaxException : Exception
    {
        public InvalidSyntaxException()
            : this(Strings.InvalidSyntax, null)
        {
        }

        public InvalidSyntaxException(string message)
            : this(message, null)
        {
        }

        public InvalidSyntaxException(string message, Exception inner)
            : base(message, inner)
        {
        }

        private InvalidSyntaxException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        internal static InvalidSyntaxException Format(string format, params object[] arguments)
        {
            string message = String.Format(CultureInfo.CurrentCulture, format, arguments);
            return new InvalidSyntaxException(message);
        }
    }
}
