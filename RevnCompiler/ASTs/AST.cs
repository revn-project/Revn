﻿using System;
using System.Collections.Generic;

namespace RevnCompiler
{
    interface ILGenerator
    {
        string GenerateIL();
    }

    public abstract class ASTBase : ILGenerator
    {
        public abstract string GenerateIL();
    }

    public abstract class ExpressionAST : ASTBase
    {
        internal bool IsReturnValueUsed = false;
        internal string ReturnType;
    }

    internal class Argument
    {
        internal string Type;
        internal string Name;
    }

    internal class StringLiteralAST : ExpressionAST
    {
        private string stringLiteral;

        internal StringLiteralAST(string str)
        {
            stringLiteral = str;
            ReturnType = "string";
        }

        public override string GenerateIL()
        {
            return $"ldstr \"{stringLiteral}\"\n";
        }
    }

}
