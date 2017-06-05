namespace RevnCompiler
{
    public enum TokenType
    {
        Identifier,
        Integer,
        FloatingPoint,
        Predefined,
        Operand,

        Using,
        Namespace,
        Class,

        LParen,
        RParen,
        LBracket,
        RBracket,
        BlockStartOrColon,
        BlockEnd,
        Period,
        Comma,
        Equals,

        Val,
        Var,

        Accessibility,
        Static,

        Fun,
        Comment,

        StringLiteral,
        Number,
    }

    public class Token
    {
        public TokenType TokenType { get; }
        public string Value { get; }
        public int LineNumber { get; }

        internal Token(TokenType tokenType, string val, int lineNumber)
        {
            TokenType = tokenType;
            Value = val;
            LineNumber = lineNumber;
        }
    }
}
