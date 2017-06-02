using System;
namespace RevnCompiler.ASTs
{
    internal class VariableExpressionAST : ExpressionAST
    {
        internal string Name { get; }
        internal int Index { get; }

        internal VariableExpressionAST(string type, string name, int index)
        {
            this.ReturnType = type;
            this.Name = name;
            this.Index = index;
        }

        public override string GenerateIL()
        {
            return $"ldloc.{Index} //{Name} \n";
        }
    }
}
