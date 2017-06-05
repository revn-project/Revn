using RevnCompiler.ASTs;
namespace RevnCompiler.ParserHelpers
{
    internal interface IFunctionASTGenerator
    {
        FunctionAST GenerateFunctionAST();
    }
}
