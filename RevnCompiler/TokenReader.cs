using System;
using System.Collections.Generic;
using System.Linq;

namespace RevnCompiler
{
    internal class TokenReader
    {
        private List<Token> Tokens { get; }
        private int currentIndex = 0;

        internal bool HasNext { get { return currentIndex < Tokens.Count(); }}

        internal TokenReader(IEnumerable<Token> tokens)
        {
            Tokens = tokens.ToList();
        }

        internal Token GetNext()
        {
            if (currentIndex >= Tokens.Count()) return null;
            return Tokens[currentIndex++];
        }

        /// <summary>
        /// 先のトークンをまとめて取得できます。
        /// GetNext には影響しません。
        /// </summary>
        /// <returns>The peek.</returns>
        /// <param name="count">何個先を見るか。省略すると次が見れます。</param>
        internal Token Peek(int count = 1)
        {
            return Tokens[currentIndex + count];
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

			for (int i = 1; i <= count; i++)
			{
				tokens.Add(Tokens[currentIndex + i]);
			}

			return tokens;
        }

        internal List<Token> GetUntil(TokenType tokenType)
        {
            var tokens = new List<Token>();

            Token token;
            while((token = Tokens[currentIndex++]).TokenType != tokenType)
            {
                tokens.Add(token);
            }

            return tokens;
        }

    }
}
