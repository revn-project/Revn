using System;
namespace RevnCompiler.ASTs
{
	internal class IntegerLiteralAST : ExpressionAST
	{
		private string integer;

		internal IntegerLiteralAST(string str)
		{
			integer = str;
			ReturnType = "int32";
		}

		public override string GenerateIL()
		{
			int val = int.Parse(integer);
			if (val == -1) return "ldc.i4.m1\n";
			// 9以上と-2以下はショートカット無し
			if (val > 8 || val < 0)
			{
				return $"ldc.i4.s {integer}\n";
			}
			return $"ldc.i4.{integer}\n";
		}
	}

	internal class FloatLiteralAST : ExpressionAST
	{
		private string floatVal;

		internal FloatLiteralAST(string str)
		{
			floatVal = str;
			ReturnType = "float64";
		}

		public override string GenerateIL()
		{
            return $"ldc.r8 {floatVal}\n";
			
		}
	}
}
