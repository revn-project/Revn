using System;
using System.IO;

namespace RevnCompiler
{
    internal class LineReader
    {
        private string Input { get; }

        internal LineReader(string input)
        {
            Input = input;

            var reader = new StringReader(input);
        }


    }
}
