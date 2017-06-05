using RevnCompiler.ASTs;
using RevnCompiler.Utils;

namespace RevnCompiler.ParserHelpers
{
    public class ModifierGenerator : IModifierGenerator
    {
        private static readonly TokenType[] Modifiers = { TokenType.Accessibility, TokenType.Static };

        private readonly Parser parser;

        public ModifierGenerator(Parser parser)
        {
            this.parser = parser;
        }

        public GenericModifier GenerateModifier()
        {
            var modifier = new GenericModifier();
            while (parser.LastToken.TokenType.IsIn(Modifiers))
            {
                switch (parser.LastToken.TokenType)
                {
                    case TokenType.Accessibility:
                        modifier.Accessibility = parser.LastToken.Value.ConvertToAccessibility();
                        break;
                    case TokenType.Static:
                        modifier.Static = "static";
                        break;
                }
                parser.ProceedToken();
            }
            return modifier;
        }
    }
}
