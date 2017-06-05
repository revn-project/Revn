namespace RevnCompiler
{
    public class Assert
    {
        public static void AssertTypeMatch(Token token, TokenType tokenType)
        {
            if(token.TokenType != tokenType)
            {
                RevnException.ThrowParserException( $"{tokenType} expected", token );
            }
        }
    }
}
