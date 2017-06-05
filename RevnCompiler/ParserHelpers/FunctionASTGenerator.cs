using System;
using System.Linq;
using System.Collections.Generic;
using RevnCompiler.ASTs;

namespace RevnCompiler.ParserHelpers
{
    internal class FunctionASTGenerator : IFunctionASTGenerator
    {
        private readonly IModifierGenerator modifierGenerator;
        private readonly IExpressionASTGenerator expressionGenerator;

        // インターフェースにする
        private readonly Parser parser;
        private readonly List<VariableExpressionAST> localVariables = new List<VariableExpressionAST>();

        internal FunctionASTGenerator(Parser parser, 
                                      IModifierGenerator modifierGenerator )
        {
            this.modifierGenerator = modifierGenerator;
            this.parser = parser;
            this.expressionGenerator = new ExpressionASTGenerator(parser, this);
        }

        public FunctionAST GenerateFunctionAST()
        {
            localVariables.Clear();
			var modifier = modifierGenerator.GenerateModifier();
			var prototype = GenerateFunctionPrototype(modifier);

			var functionAst = new FunctionAST();
			functionAst.Prototype = prototype;

			// 中身をパース
			var expressions = new List<ExpressionAST>();
			while (parser.LastToken.TokenType != TokenType.BlockEnd)
			{
                expressions.Add(expressionGenerator.GenerateExpressionAST());
			}
            functionAst.Expressions = expressions;

            parser.ProceedToken(); // end を消費

            functionAst.Variables = localVariables;

			return functionAst;
        }

		private FunctionPrototype GenerateFunctionPrototype(GenericModifier modifier)
		{
            Assert.AssertTypeMatch( parser.LastToken, TokenType.Fun );
			var prototype = new FunctionPrototype();
			prototype.Modifier = modifier;
			parser.ProceedToken(); // fun を消費

            Assert.AssertTypeMatch( parser.LastToken, TokenType.Identifier );
			prototype.FunctionName = parser.LastToken.Value;
			parser.ProceedToken(); // 関数名を消費

			prototype.args = GenerateArgs();

			// TODO 戻り値

			parser.ProceedToken(); // : を消費

			return prototype;
		}

		private List<Argument> GenerateArgs()
		{
            Assert.AssertTypeMatch( parser.LastToken, TokenType.LParen );

			var args = new List<Argument>();

			Argument arg = new Argument();
			// 一回目のループで ( は消費される
			while (parser.ProceedToken().TokenType != TokenType.RParen)
			{
				// TODO 引数を区切るコンマの場合 arg を初期化
				// if (LastToken.TokenType == TokenType.Comma)
				arg.Name = parser.LastToken.Value;
				parser.ProceedToken(); // 引数名を消費

				if (parser.LastToken.TokenType != TokenType.BlockStartOrColon)
				{
                    RevnException.ThrowParserException("Excpected ':'", parser.LastToken);
				}
				parser.ProceedToken(); // : を消費

				arg.Type = parser.LastToken.Value;
				parser.ProceedToken(); // 型名を消費

                // TODO string だけ一応今は特別対応
                if(arg.Type.ToLower() == "string")
                {
                    arg.Type = "string"; 
                }

				// 配列だけ一応分けておく（多分プロパティになる気がする）
				if (parser.LastToken.TokenType == TokenType.LBracket)
				{
					arg.Type += parser.LastToken.Value;
					parser.ProceedToken();
					arg.Type += parser.LastToken.Value;
				}

				args.Add(arg);
			}
			parser.ProceedToken(); // ) を消費

			return args;
		}

        internal void AddVariable(VariableExpressionAST variable)
        {
            variable.Index = localVariables.Count;
            localVariables.Add(variable);
        }

        internal bool HasLocalVariable(string variableName)
        {
            return localVariables.Any(v => v.Name == variableName);
        }

        internal VariableExpressionAST GetVariable(string name)
        {
            return localVariables.First(v => v.Name == name);
        }
    }
}
