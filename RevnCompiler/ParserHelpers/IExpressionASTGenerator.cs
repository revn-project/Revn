using System;
namespace RevnCompiler.ParserHelpers
{
    public interface IExpressionASTGenerator
    {
        ExpressionAST GenerateExpressionAST();
    }
}
