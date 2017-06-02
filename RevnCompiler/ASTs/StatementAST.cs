using System;
namespace RevnCompiler.ASTs
{
    internal class StatementAST : ASTBase
    {
        /// <summary>
        /// Statement
        ///     ::= identifier '=' expression
        /// </summary>
        internal StatementAST()
        {
            
        }

        public override string GenerateIL()
        {
            throw new NotImplementedException();
        }
    }
}
