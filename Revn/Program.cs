using System;
using RevnCompiler;

namespace Revn
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexer = new Lexer(
                "using System\n" + 
                "namespace Revn:\n" +
                    "class Program:\n" +
                        "public static fun Main(args : String[]):\n" +
                            "Console.WriteLine(\"Hello World!\")" +
                        "end\n" +
                    "end\n" +
                "end");
            var tokens = lexer.GenerateTokens();
            foreach(var token in tokens)
            {
                Console.WriteLine($"{token.TokenType} : {token.Value}");
            }

        }
    }
}
