using System;
using System.Collections.Generic;
using System.Text;

namespace RevnCompiler
{
    internal class RevnException
    {
        internal static void ThrowParserException( string message, Token token )
        {
            throw new Exception($"[{token?.LineNumber+1}:{token?.Value}] {message}");
        }
    }
}
