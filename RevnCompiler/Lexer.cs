using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RevnCompiler
{
    public class Lexer
    {
        string SourceCode { get; }
        CharReader Reader { get; }

        int lineNumber = 0;
        char lastChar = ' ';
        char LastChar
        {
            get { return lastChar; }
            set
            {
                lastChar = value;
                if (value == '\n')
                {
                    lineNumber++;
                    Reader.ResetLocalIndex();
                }
            }
        }

        public Lexer(string sourceCode)
        {
            SourceCode = sourceCode;
            Reader = new CharReader(SourceCode);
        }

        public IEnumerable<Token> GenerateTokens()
        {
            var tokens = new List<Token>();

            Token token;
            while((token = GetToken()) != null)
            {
                tokens.Add(token);
            }

            return tokens;
        }

        private Token GetToken()
        {
            // 空白を飛ばす
            while(Reader.HasNext && char.IsWhiteSpace(LastChar))
            { 
                LastChar = Reader.GetNext();
            }

            if (char.IsLetter(LastChar)) // identifier: [a-zA-Z][a-zA-Z0-9]*
			{
                string tokenString = LastChar.ToString();
                while(char.IsLetterOrDigit(LastChar = Reader.GetNext()))
                {
                    tokenString += LastChar.ToString();
                }

                switch(tokenString)
                {
                    case "using":
                        return new Token(TokenType.Using, tokenString, lineNumber);
                    case "namespace":
                        return new Token(TokenType.Namespace, tokenString, lineNumber);
                    case "class":
                        return new Token(TokenType.Class, tokenString, lineNumber);
                    case "end":
                        return new Token(TokenType.BlockEnd, tokenString, lineNumber);
                    case "private":
                    case "public":
                    case "internal":
                    case "protected":
                        return new Token(TokenType.Accessibility, tokenString, lineNumber);
                    case "static":
                        return new Token(TokenType.Static, tokenString, lineNumber);
                    case "fun":
                        return new Token(TokenType.Fun, tokenString, lineNumber);
                    default:
                        return new Token(TokenType.Identifier, tokenString, lineNumber);
                }
            }

            switch(LastChar)
            {
                case ':':
	                LastChar = Reader.GetNext(); // : を消費
	                return new Token(TokenType.BlockStart, ":", lineNumber);
                case '(':
                    LastChar = Reader.GetNext(); // ( を消費
                    return new Token(TokenType.LParen, "(", lineNumber);
                case ')':
                    LastChar = Reader.GetNext(); // ) を消費
                    return new Token(TokenType.RParen, ")", lineNumber);
                case '[':
					LastChar = Reader.GetNext(); // [ を消費
                    return new Token(TokenType.LBracket, "[", lineNumber);
				case ']':
					LastChar = Reader.GetNext(); // ] を消費
					return new Token(TokenType.RBracket, "]", lineNumber);
                case '.':
                    LastChar = Reader.GetNext(); // . を消費
                    return new Token(TokenType.Period, ".", lineNumber);
                case '"':
                    LastChar = Reader.GetNext(); // " を消費
                    return GenerateStringToken();
                case ',':
                    LastChar = Reader.GetNext(); // , を消費
                    return new Token(TokenType.Comma, ",", lineNumber);
            }

            // 終了
            if (!Reader.HasNext) return null;

            // 生成できないならエラー
            throw new Exception($"[{lineNumber}:{Reader.LocalIndex}] Compiler error. Last char: {LastChar}");
        }

        private Token GenerateStringToken()
        {
            string stringLiteral = LastChar.ToString();
            while((LastChar = Reader.GetNext()) != '"')
            {
                stringLiteral += LastChar;
            }
            LastChar = Reader.GetNext(); // " を消費

            return new Token(TokenType.StringLiteral, stringLiteral, lineNumber);
        }

    }

}
