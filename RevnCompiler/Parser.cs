using System;
using System.Collections.Generic;
using RevnCompiler.ASTs;

namespace RevnCompiler
{
    public class Parser
    {
        TokenReader Reader { get; }

        Token lastToken;

        string currentNamespace = string.Empty;
        List<string> usings = new List<string>();

        public Parser(IEnumerable<Token> tokens)
        {
            Reader = new TokenReader(tokens);
        }

        public IEnumerable<ASTBase> Parse()
        {
            var asts = new List<ASTBase>();

            while(Reader.HasNext)
            {
                lastToken = Reader.GetNext();
                switch(lastToken.TokenType)
                {
					case TokenType.Using:
                        lastToken = Reader.GetNext(); // using を消費
                        usings.Add(lastToken.Value);
						break;
					case TokenType.Namespace:
                        lastToken = Reader.GetNext(); // namespace を消費
						currentNamespace = lastToken.Value;
						break;
                    case TokenType.Class:
                        lastToken = Reader.GetNext(); // class を消費
                        asts.Add(GenerateClassAST());
                        break;
                    case TokenType.Accessibility:
                        Accessibility accessibility;
                        switch(lastToken.Value)
                        {
                            case "private"  : accessibility = Accessibility.Private;    break;
                            case "public"   : accessibility = Accessibility.Public;     break;
                            case "protected": accessibility = Accessibility.Protected;  break;
                            case "internal" : accessibility = Accessibility.Internal;   break;
                            default: throw new NotImplementedException();
                        }

                        var nextToken = Reader.Peek();
                        switch(nextToken.TokenType)
                        {
                            case TokenType.Class:
                                asts.Add(GenerateClassAST(accessibility));
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
					default:
						throw new NotImplementedException();
                }
            }


            return asts;
        }

        private ClassAST GenerateClassAST(Accessibility accessibility = Accessibility.Public)
        {
            var classAst = new ClassAST();
            classAst.Accessibility = accessibility;
            classAst.ClassName = lastToken.Value;

            lastToken = Reader.GetNext(); // クラス名を消費
            // TODO: inheritence

            lastToken = Reader.GetNext(); // : を消費

            // 関数系をパース
            var functions = new List<FunctionAST>();
            while(lastToken.TokenType != TokenType.BlockEnd)
            {
                functions.Add(GenerateFunctionAST());
            }
            classAst.Functions = functions;

            return classAst;
        }

        private FunctionAST GenerateFunctionAST()
        {
            var functionAst = new FunctionAST();



            return functionAst;
        }

    }

}
