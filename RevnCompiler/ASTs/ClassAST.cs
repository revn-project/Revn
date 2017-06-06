using System.Collections.Generic;

namespace RevnCompiler.ASTs
{
    public class ClassPrototypeAST
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
        internal GenericModifier Modifier;
    }

    public class ClassAST : ASTBase
    {
        internal ClassPrototypeAST prototype;
        internal List<FunctionAST> Functions = new List<FunctionAST>();

        private string defaultCtorFormatTop =
        ".method public hidebysig specialname rtspecialname instance void\n" +
            ".ctor() cil managed\n" +
        "{\n" +
            ".maxstack 8\n" +
            "ldarg.0\n" +
            "call instance void ";
        private string defaultCtorFormatBottom =
                                "::.ctor()\n" +
            "ret\n" +
        "}\n";

        public override string GenerateIL()
        {
            bool hasCtor = false;

            string internalCode = string.Empty;
            foreach (var function in Functions)
            {
                internalCode += function.GenerateIL();
                if (function.Prototype.FunctionName == prototype.ClassName)
                {
                    hasCtor = true;
                }
            }

            return
                $".class {prototype.Modifier.Accessibility.ToString().ToLower()} auto ansi beforefieldinit\n" +
                    $"{prototype.Namespace}.{prototype.ClassName}\n" +
                        $"extends {prototype.InheritedClassName}\n" +
                "{\n" +
                    internalCode + "\n" +
                    (hasCtor ? "" : GetDefaultCtor()) + "\n" +
                "}\n";
        }

        private string GetDefaultCtor()
        {
            return defaultCtorFormatTop + prototype.InheritedClassName + defaultCtorFormatBottom;
        }
    }
}
