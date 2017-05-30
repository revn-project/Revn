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
        public virtual string GenerateIL()
        {
            throw new NotImplementedException();
        }
    }

    public class ExpressionAST : ASTBase
	{
        public override string GenerateIL()
		{
			throw new NotImplementedException();
		}
	}

    internal enum Accessibility
    {
        Public,
        Private,
        Internal,
        Protected
    }

    internal class Argument
    {
        internal string Type;
        internal string Name;
    }

}
