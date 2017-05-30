using System.Collections.Generic;

namespace RevnCompiler.ASTs
{
	public class ClassAST : ASTBase
	{
		internal string Namespace;
		internal string ClassName;
		private string inheritedClassName = string.Empty;
		internal string InheritedClassName
		{
			get => string.IsNullOrWhiteSpace(inheritedClassName)
						 ? "[mscorlib]System.Object"
						 : inheritedClassName;
			set { inheritedClassName = value; }
		}
		internal Accessibility Accessibility;
		internal List<FunctionAST> Functions = new List<FunctionAST>();

		private string defaultCtor =>
		".method public hidebysig specialname rtspecialname instance void\n" +
			".ctor() cil managed\n" +
				"{\n" +
					".maxstack 8\n" +
					"ldarg.0\n" +
					"call\n" +
					"nop\n" +
					"ret\n" +
				"}\n";

		public override string GenerateIL()
		{
			bool hasCtor = false;

			string internalCode = string.Empty;
			foreach (var function in Functions)
			{
				internalCode += function.GenerateIL();
				if (function.FunctionName == ClassName)
				{
					hasCtor = true;
				}
			}

			return
				$".class {Accessibility.ToString().ToLower()} auto ansi beforefieldinit\n" +
					$"{Namespace}.{ClassName}\n" +
						$"extends {InheritedClassName}\n" +
				"{\n" +
					internalCode + "\n" +
					(hasCtor ? "" : defaultCtor) + "\n" +
				"}\n";
		}
	}
}
