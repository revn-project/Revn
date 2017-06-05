using System;
namespace RevnCompiler.ASTs
{
    internal class BinaryExpressionAST : ExpressionAST
    {
        private ExpressionAST LHS;
        private ExpressionAST RHS;
        private string _operator;

        internal BinaryExpressionAST(ExpressionAST LHS,
                                     ExpressionAST RHS, 
                                     string _operator)
        {
            this.LHS = LHS;
            this.RHS = RHS;
            this._operator = _operator;
            ReturnType = LHS.ReturnType;
        }

        public override string GenerateIL()
        {
            string operatorIL = string.Empty;
            switch(_operator)
            {
                case "+": operatorIL = "add"; break;
                case "-": operatorIL = "sub"; break;
                case "*": operatorIL = "mul"; break;
                case "/": operatorIL = "div"; break;
                default: throw new NotImplementedException();
            }
            operatorIL += "\n";
            return LHS.GenerateIL() + RHS.GenerateIL() + operatorIL;
        }
    }
}
