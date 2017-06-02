using System;
namespace RevnCompiler.ParserHelpers
{
    internal interface IExpressionASTGenerator
    {
        ExpressionAST GenerateExpressionAST();
    }
}
