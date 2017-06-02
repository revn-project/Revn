using System;
using System.IO;
using System.Reflection;
using RevnCompiler;

namespace Revn
{
    class Program
    {
        static void Main(string[] args)
        {
            var program =
                File.ReadAllText( @"C:\Users\ichi-dohi\Documents\Visual Studio 2015\Projects\Revn\Revn\HelloWorld.rv" );
            var lexer = new Lexer(program);
            var tokens = lexer.GenerateTokens();
            foreach(var token in tokens)
            {
                Console.WriteLine($"{token.TokenType} : {token.Value}");
            }

            Parser parser = new Parser(tokens);
            var result = parser.Parse();
            FileStream file = File.Create(@"C:\Users\ichi-dohi\Desktop\hoge.il");
            using ( var writer = new StreamWriter( file ) )
            {
                writer.WriteLine(".assembly extern mscorlib { }\n.assembly test { }");
                foreach ( var astBase in result )
                {
                    var IL = astBase.GenerateIL();
                    Console.WriteLine(IL);
                    writer.WriteLine(IL);
                }
            }
        }
    }
}
