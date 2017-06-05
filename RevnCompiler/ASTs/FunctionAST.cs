using System.Collections.Generic;

namespace RevnCompiler.ASTs
{
    internal class FunctionPrototype
    {
        private string functionName;
        internal string FunctionName
        {
            get { return functionName; }
            set 
            {
                functionName = value;
                if (value != "Main") return;
                entryPoint = ".entrypoint\n";
            }
        }
        internal string FullClassName;
        internal GenericModifier Modifier;
		internal string ReturnType = "void";
		internal List<Argument> args;
        internal string entryPoint;
    }

	internal class FunctionAST : ASTBase
	{
        internal FunctionPrototype Prototype;
        internal List<VariableExpressionAST> Variables = new List<VariableExpressionAST>();
		internal List<ExpressionAST> Expressions;

		public override string GenerateIL()
		{
			string argsCode = string.Empty;
			foreach (var arg in Prototype.args)
			{
				argsCode += $"{arg.Type} {arg.Name},\n";
			}
			argsCode = argsCode.Substring(0, argsCode.Length - 2) + "\n"; // , を削除

			string body = string.Empty;
			foreach (var expression in Expressions)
			{
				body += expression.GenerateIL();
			}

            string variableString = string.Empty;
            foreach(var variable in Variables)
            {
                variableString += $"[{variable.Index}] {variable.ReturnType} {variable.Name},\n";
            }
		    variableString = variableString.Substring(0, variableString.Length - 2) + "\n"; // , を削除

            return
                $".method {Prototype.Modifier.Accessibility.ToString().ToLower()} hidebysig {Prototype.Modifier.Static} void\n" +
					$"{Prototype.FunctionName}(\n" +
						argsCode +
					") cil managed\n" +
				"{\n" +
                    ".locals init(\n" +
                        variableString +
                    ")" +
                    Prototype.entryPoint +
					body +
                    "ret\n" + // TODO fix!!
				"}\n";
		}
	}
}
