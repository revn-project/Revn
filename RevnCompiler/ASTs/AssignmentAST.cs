namespace RevnCompiler.ASTs
{
    /// <summary>
    /// Assignment Expression:
    ///     ::= expression '=' expression
    /// </summary>
    internal class AssignmentAST : ExpressionAST
    {
        internal ExpressionAST LHS;
        internal ExpressionAST RHS;

        public override string GenerateIL()
        {
            return RHS.GenerateIL() + LHS.GenerateIL();
        }
    }
}
