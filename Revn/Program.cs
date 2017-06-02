using System;
using System.Reflection;
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
                            "val x = 10" +
                            "Console.WriteLine(\"Hello World!\")" +
                            "Console.WriteLine(x)"+
                            "Console.ReadLine()" +
                        "end\n" +
                    "end\n" +
                "end");
            var tokens = lexer.GenerateTokens();
            foreach(var token in tokens)
            {
                Console.WriteLine($"{token.TokenType} : {token.Value}");
            }

            Parser parser = new Parser(tokens);
            var result = parser.Parse();
            foreach(var res in result)
            {
                Console.WriteLine(res.GenerateIL());
            }
        }
    }
}
