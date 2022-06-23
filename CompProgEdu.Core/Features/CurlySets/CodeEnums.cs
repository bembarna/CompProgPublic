using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompProgEdu.Core.Features.CurlySets
{
    public class CodeEnums
    {
        public static List<string> AccessModifiers = new List<string>
        {
            "public",
            "private",
            "protected",
            "internal",
        };

        public static string Class = "class";

        public static List<string> PrimitiveType = new List<string>
        {
            "byte",
            "sbyte",
            "short",
            "ushort",
            "int",
            "uint",
            "long",
            "ulong",
            "float",
            "double",
            "decimal",
            "char",
            "bool",
            "string",
            "var"
        };

        public static string Void = "void";

        public static string Abstract = "abstract";

        public static string Static = "static";

        public static string Readonly = "readonly";

        public static string Async = "async";

        public static string Referance = "ref";

        public static List<string> Statements = new List<string>
        {
            "if",
            "else",
            "do",
            "while",
            "for",
            "foreach",
            "try",
            "catch",
            "finally"
        };

        public static List<string> Types =
            AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(y => y.GetTypes())
            .Select(x => x.Name)
            .ToList();
    }
}
