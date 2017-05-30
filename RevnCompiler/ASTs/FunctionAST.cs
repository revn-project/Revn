using System.Collections.Generic;

namespace RevnCompiler.ASTs
{
	internal class FunctionAST : ASTBase
	{
		internal string FunctionName;
		internal Accessibility Accessibility;
		internal string Static = string.Empty;
		internal string returnValue = "void";
		internal List<Argument> args;
		internal List<ExpressionAST> Expressions;

		public override string GenerateIL()
		{
			string argsCode = string.Empty;
			foreach (var arg in args)
			{
				argsCode += $"{arg.Type} {arg.Name},\n";
			}
			argsCode = argsCode.Substring(0, argsCode.Length - 2) + "\n"; // , を削除

			string body = string.Empty;
			foreach (var expression in Expressions)
			{
				body += expression.GenerateIL();
			}

			return
				$".method {Accessibility.ToString().ToLower()} hidebysig {Static} void\n" +
					$"{FunctionName}(\n" +
						argsCode +
					") cil managed\n" +
				"{\n" +
					body +
				"}\n";
		}
	}
}
