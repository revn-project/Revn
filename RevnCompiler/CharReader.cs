using System;
namespace RevnCompiler
{
    internal class CharReader
    {
        string Input { get; }
        char currentChar = ' ';
        int currentIndex = 0;

		internal char CurrentChar { get { return currentChar; } }
        internal int CurrentIndex{ get { return currentIndex; }}
        internal bool HasNext { get { return currentIndex < Input.Length; }}

        private int localIndex = 0;
        internal int LocalIndex { get; }

        internal CharReader(string input)
        {
            this.Input = input;
        }

        /// <summary>
        /// Returns the next character. This skips all whitespaces.
        /// </summary>
        /// <returns></returns>
        internal char GetNext()
        {
            if(currentIndex < Input.Length)
            {
                localIndex++;
                return currentChar = Input[currentIndex++];   
            }
            else
            {
                return '\0';
            }
        }

        internal void ResetLocalIndex()
        {
            localIndex = 0;
        }
    }
}
