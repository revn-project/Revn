using System;
using System.Collections.Generic;
using RevnCompiler.ASTs;

namespace RevnCompiler.ParserHelpers
{
    public class ExpressionASTGenerator : IExpressionASTGenerator
    {
        private readonly Dictionary<string, int> BinopPrecedence = new Dictionary<string, int>()
        {
            {"<", 10},
            {"+", 20},
            {"-", 20},
            {"*", 40},
        };
        private readonly Parser parser;
        private int _localVariableIndex = 0;
        private int localVariableIndex 
        {
            get { return _localVariableIndex++; }
        }

        private readonly Dictionary<string, string> localVariableMap = new Dictionary<string, string>();

        public ExpressionASTGenerator(Parser parser)
        {
            this.parser = parser;
        }

        public ExpressionAST GenerateExpressionAST()
        {
            var LHS = ParsePrimary();
            return ParseBinOpRHS(0, LHS);
        }

        private ExpressionAST ParsePrimary()
        {
            switch(parser.LastToken.TokenType)
            {
                case TokenType.Identifier:
                    return ParseIdentifier(null);
                case TokenType.StringLiteral:
                    var returnStringLiteral = new StringLiteralAST(parser.LastToken.Value);
                    parser.ProceedToken(); // 文字列を消費
                    return returnStringLiteral;
                default:
                    throw new NotImplementedException();
            }
        }

        private ExpressionAST ParseIdentifier(string inferedType)
        {
            string identifier = parser.LastToken.Value;
            parser.ProceedToken();

            while(parser.LastToken.TokenType == TokenType.Period)
            {
                parser.ProceedToken(); // . を消費
                if(parser.LastToken.TokenType != TokenType.Identifier)
                {
                    // TODO New Exception
                    throw new NotImplementedException();
                }
                identifier += "." + parser.LastToken.Value;
                parser.ProceedToken(); // 変数を消費
            }

            if(parser.LastToken.TokenType != TokenType.LParen)
            {
                if(!localVariableMap.ContainsKey(parser.LastToken.Value))
                {
                    // TODO Parser exception
                    throw new Exception("Variable is not assigned.");
                }

                string type = localVariableMap[parser.LastToken.Value];
                return new VariableExpressionAST(type, parser.LastToken.Value, localVariableIndex);
            }

            if (inferedType == null)
            {
                inferedType = "void";
            }

            parser.ProceedToken(); // ( を消費

            var args = new List<ExpressionAST>();
            if(parser.LastToken.TokenType != TokenType.RParen)
            {
				while (true)
				{
                    var arg = GenerateExpressionAST();
                    args.Add(arg);

                    if (parser.LastToken.TokenType == TokenType.RParen)
                        break;

                    if (parser.LastToken.TokenType != TokenType.Comma)
                    {
                        // TODO excpetion
                        throw new Exception("Expected comma or )");
                    }
                    parser.ProceedToken(); // , を消費
				}
            }

            parser.ProceedToken(); // ) を消費

            return new CallExpressionAST(identifier, args, inferedType);
        }

        private ExpressionAST ParseBinOpRHS(int expressionPrecedence, ExpressionAST LHS)
        {
            while(true)
            {
                int tokenPrecedence = GetPrecedence(parser.LastToken);
                if (tokenPrecedence < expressionPrecedence) return LHS;

                string _operator = parser.LastToken.Value;
                parser.ProceedToken(); // 演算子を消費

                var RHS = ParsePrimary();
                RHS.IsReturnValueUsed = true;

                int nextPrecedence = GetPrecedence(parser.LastToken);
                if(tokenPrecedence < nextPrecedence)
                {
                    RHS = ParseBinOpRHS(tokenPrecedence + 1, RHS);
                }

                LHS = new BinaryExpressionAST(LHS, RHS, _operator);
            }
        }

        private int GetPrecedence(Token token)
        {
            if (!BinopPrecedence.ContainsKey(token.Value)) return -1;
            return BinopPrecedence[token.Value];
        }
    }
}
