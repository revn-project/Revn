using System;
namespace RevnCompiler
{
    public class Assert
    {
        public static void AssertTypeMatch(Token token, TokenType tokenType)
        {
            if(token.TokenType != tokenType)
            {
                // TODO propper exception
                throw new Exception();
            }
        }
    }
}
