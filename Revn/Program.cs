using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using RevnCompiler;

namespace Revn
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var location = Path.GetDirectoryName( Assembly.GetEntryAssembly().Location );
            var sourceCodePath = Path.Combine( location, "HelloWorld.rv" );
            var program = File.ReadAllText(sourceCodePath);


            var lexer = new Lexer(program);
            var tokens = lexer.GenerateTokens();
            foreach(var token in tokens)
            {
                Console.WriteLine($"{token.TokenType} : {token.Value}");
            }

            Parser parser = new Parser(tokens);
            var result = parser.Parse();
            var ilPath = Path.Combine( location, "HelloWorld.il" );
            FileStream file = File.Create(ilPath);
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


            var commandProcess = new Process
                {
                    StartInfo =
                    {
                        FileName = Environment.GetEnvironmentVariable( "ComSpec" ),
                        CreateNoWindow = true,
                        Arguments = $"/c ilasm \"{ilPath}\""
                    }
                };
            commandProcess.Start();
            commandProcess.WaitForExit();

            Console.WriteLine("Execution Result");
            Console.WriteLine("=======================================");

            var executionPath = Path.Combine( location, "HelloWorld.exe" );
            var ilProcess = new Process
            {
                StartInfo =
                {
                    FileName = executionPath,
                    Arguments = "/c"
                }
            };
            ilProcess.Start();
            ilProcess.WaitForExit();

            Console.ReadLine();
        }
    }
}
