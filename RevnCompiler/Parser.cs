using System;
using System.Collections.Generic;
using RevnCompiler.ASTs;
using RevnCompiler.ParserHelpers;

namespace RevnCompiler
{
    public class Parser
    {
        TokenReader tokenReader { get; }

        Token lastToken;
        public Token LastToken { get => lastToken; }

        string currentNamespace = string.Empty;
        List<string> usings = new List<string>();

        // helpers
        readonly IModifierGenerator modifierGenerator;
        readonly IFunctionASTGenerator functionAstGenerator;

        public Parser(IEnumerable<Token> tokens)
        {
            tokenReader = new TokenReader(tokens);

            modifierGenerator = new ModifierGenerator(this);
            functionAstGenerator = new FunctionASTGenerator(this, modifierGenerator);
        }

        public IEnumerable<ASTBase> Parse()
        {
            var asts = new List<ASTBase>();

            while(tokenReader.HasNext)
            {
                ProceedToken();

                switch(lastToken.TokenType)
                {
                    case TokenType.Using:
                        ProceedToken(); // using をスキップ
                        usings.Add(lastToken.Value);
                        continue;
                    case TokenType.Namespace:
                        ProceedToken(); // namespace をスキップ
                        currentNamespace = lastToken.Value;
                        ProceedToken(); // : を消費すように一個ずらす
                        continue;
                }

                GenericModifier modifier = modifierGenerator.GenerateModifier();
                ClassPrototypeAST prototype = GenerateClassPrototype(modifier);
                ClassAST classAst = GenerateClassAST(prototype);

                asts.Add(classAst);

                if (LastToken.TokenType == TokenType.BlockEnd) break;
            }

            return asts;
        }

        internal Token ProceedToken()
        {
            lastToken = tokenReader.GetNext();
            return lastToken;
        }

        #region Class Prototype

        private ClassPrototypeAST GenerateClassPrototype(GenericModifier modifier)
        {
            var prototype = new ClassPrototypeAST();

            prototype.Modifier = modifier;
            prototype.Namespace = currentNamespace;

            ProceedToken(); // class を消費

            prototype.ClassName = lastToken.Value;
            ProceedToken(); // クラス名を消費

            // TODO inheritence

            ProceedToken(); // :を消費

            return prototype;
        }

        #endregion

        internal ClassAST GenerateClassAST(ClassPrototypeAST prototype)
        {
            var classAst = new ClassAST();
            classAst.prototype = prototype;

            // 関数系をパース
            var functions = new List<FunctionAST>();
            while (lastToken.TokenType != TokenType.BlockEnd)
            {
                functions.Add(functionAstGenerator.GenerateFunctionAST());
            }
            classAst.Functions = functions;

            return classAst;
        }

        private ExpressionAST GenerateExpressionAST()
        {
            throw new NotImplementedException();
        }


    }

}
