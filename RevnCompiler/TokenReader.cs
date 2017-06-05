using System.Collections.Generic;
using System.Linq;

namespace RevnCompiler
{
    internal class TokenReader
    {
        private List<Token> Tokens { get; }
        private int _currentIndex;

        // todo: use count property
        internal bool HasNext => _currentIndex < this.Tokens.Count();

        internal TokenReader(IEnumerable<Token> tokens)
        {
            this.Tokens = tokens.ToList();
        }

        internal Token GetNext()
        {
            return _currentIndex >= this.Tokens.Count() ? null : this.Tokens[_currentIndex++];
        }

        /// <summary>
        /// 先のトークンをまとめて取得できます。
        /// GetNext には影響しません。
        /// </summary>
        /// <returns>The peek.</returns>
        /// <param name="count">何個先を見るか。省略すると次が見れます。</param>
        internal Token Peek(int count = 1)
        {
            return this.Tokens[_currentIndex + count];
        }

        /// <summary>
        /// 先のトークンをまとめて取得できます。
        /// GetNext には影響しません。
        /// </summary>
        /// <returns>The forward.</returns>
        /// <param name="count">Count.</param>
        internal List<Token> PeekForward(int count)
        {
            var tokens = new List<Token>();

            for (var i = 1; i <= count; i++)
            {
                tokens.Add(this.Tokens[_currentIndex + i]);
            }

            return tokens;
        }

        internal List<Token> GetUntil(TokenType tokenType)
        {
            var tokens = new List<Token>();

            Token token;
            while((token = this.Tokens[_currentIndex++]).TokenType != tokenType)
            {
                tokens.Add(token);
            }

            return tokens;
        }

    }
}
