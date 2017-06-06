using RevnCompiler.ASTs;

namespace RevnCompiler.ParserHelpers
{
    internal interface IModifierGenerator
    {
        GenericModifier GenerateModifier();
    }
}
