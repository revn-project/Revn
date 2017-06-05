using System;
using System.Collections.Generic;
using RevnCompiler.ASTs;
using RevnCompiler.ParserHelpers;

namespace RevnCompiler
{
    public class Parser
    {
        private readonly TokenReader _tokenReader;

        public Token LastToken { get; private set; }

        private string _currentNamespace = string.Empty;
        private readonly List<string> _usings = new List<string>();

        // helpers
        private readonly IModifierGenerator _modifierGenerator;
        private readonly IFunctionASTGenerator _functionAstGenerator;

        public Parser(IEnumerable<Token> tokens)
        {
            _tokenReader = new TokenReader(tokens);

            _modifierGenerator = new ModifierGenerator(this);
            _functionAstGenerator = new FunctionASTGenerator(this, _modifierGenerator);
        }

        public IEnumerable<ASTBase> Parse()
        {
            var asts = new List<ASTBase>();

            while(_tokenReader.HasNext)
            {
                this.ProceedToken();

                switch(this.LastToken.TokenType)
                {
                    case TokenType.Using:
                        this.ProceedToken(); // using をスキップ
                        _usings.Add(this.LastToken.Value);
                        continue;
                    case TokenType.Namespace:
                        this.ProceedToken(); // namespace をスキップ
                        _currentNamespace = this.LastToken.Value;
                        this.ProceedToken(); // : を消費すように一個ずらす
                        continue;
                }

                GenericModifier modifier = _modifierGenerator.GenerateModifier();
                ClassPrototypeAST prototype = this.GenerateClassPrototype(modifier);
                ClassAST classAst = this.GenerateClassAST(prototype);

                asts.Add(classAst);

                if (this.LastToken.TokenType == TokenType.BlockEnd) break;
            }

            return asts;
        }

        internal Token ProceedToken()
        {
            this.LastToken = this._tokenReader.GetNext();
            return this.LastToken;
        }

        #region Class Prototype

        private ClassPrototypeAST GenerateClassPrototype(GenericModifier modifier)
        {
            var prototype = new ClassPrototypeAST
            {
                Modifier = modifier,
                Namespace = _currentNamespace
            };


            this.ProceedToken(); // class を消費

            prototype.ClassName = this.LastToken.Value;
            this.ProceedToken(); // クラス名を消費

            // TODO inheritence

            this.ProceedToken(); // :を消費

            return prototype;
        }

        #endregion

        private ClassAST GenerateClassAST(ClassPrototypeAST prototype)
        {
            var classAst = new ClassAST {prototype = prototype};

            // 関数系をパース
            var functions = new List<FunctionAST>();
            while (this.LastToken.TokenType != TokenType.BlockEnd)
            {
                functions.Add(_functionAstGenerator.GenerateFunctionAST());
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
