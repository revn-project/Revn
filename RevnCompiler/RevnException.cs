using System;
using System.Collections.Generic;
using System.Text;

namespace RevnCompiler
{
    internal class RevnException
    {
        internal static void ThrowParserException( string message, Token token )
        {
            throw new RevnParserException($"[{token?.LineNumber+1}:{token?.Value}] {message}");
        }
    }

    internal class RevnParserException : Exception
    {
        internal RevnParserException(string message) : base(message){}
    }
}
