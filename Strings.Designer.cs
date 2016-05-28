﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Scheme {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Scheme.Strings", typeof(Strings).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Expected backslash or dobule quote, found &quot;{0}&quot;..
        /// </summary>
        internal static string ExpectedBackslashOrDoubleQuote {
            get {
                return ResourceManager.GetString("ExpectedBackslashOrDoubleQuote", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Expected closing parenthesis, found &quot;{0}&quot;..
        /// </summary>
        internal static string ExpectedCloseParenthesis {
            get {
                return ResourceManager.GetString("ExpectedCloseParenthesis", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Expected whitespace, parenthesis, semicolon, or double-quote, found &quot;{0}&quot;..
        /// </summary>
        internal static string ExpectedDelimiter {
            get {
                return ResourceManager.GetString("ExpectedDelimiter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Procedure expects {0} argument(s), given {1}..
        /// </summary>
        internal static string IncorrectArgumentCount {
            get {
                return ResourceManager.GetString("IncorrectArgumentCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: The given syntax is invalid..
        /// </summary>
        internal static string InvalidSyntax {
            get {
                return ResourceManager.GetString("InvalidSyntax", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Invalid token: &quot;{0}&quot;..
        /// </summary>
        internal static string InvalidToken {
            get {
                return ResourceManager.GetString("InvalidToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Procedure expects at least {0} argument(s), given {1}..
        /// </summary>
        internal static string TooFewArguments {
            get {
                return ResourceManager.GetString("TooFewArguments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Unexpected end of file..
        /// </summary>
        internal static string UnexpectedEof {
            get {
                return ResourceManager.GetString("UnexpectedEof", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: {0} is not a valid {1}..
        /// </summary>
        internal static string UnmetExpectation {
            get {
                return ResourceManager.GetString("UnmetExpectation", resourceCulture);
            }
        }
    }
}
