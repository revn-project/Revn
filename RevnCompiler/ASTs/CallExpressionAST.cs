using System;
using System.Collections.Generic;
using System.Linq;

namespace RevnCompiler.ASTs
{

    internal class CallExpressionAST : ExpressionAST
    {
        string name;
        List<ExpressionAST> args;
        IFunctionDefinitionFinder functionDefinitionFinder
            = new DummyDefinitionFinder();

        internal CallExpressionAST(string name,
                                   List<ExpressionAST> args, 
                                   string returnType)
        {
            this.name = name;
            this.args = args;
            this.ReturnType = returnType;
        }

        public override string GenerateIL()
        {
            string argLoad = string.Empty;
            string argLine = string.Empty;
            foreach(var arg in args)
            {
                argLoad += arg.GenerateIL();
                argLine += arg.ReturnType + ",";
            }
            if(args.Count > 0)
            {
				// remove final ,
				argLine = argLine.Substring(0, argLine.Length - 1);
            }

            var prototype = functionDefinitionFinder.Find(name);

            string IL = argLoad + string.Format("call {0} {1}::{2}({3})\n",
                                               prototype.ReturnType,
                                               prototype.FullClassName,
                                               prototype.FunctionName,
                                               argLine);
            if (!IsReturnValueUsed && prototype.ReturnType != "void")
            {
                IL += "pop\n";
            }

            return IL;
        }

        private class DummyDefinitionFinder : IFunctionDefinitionFinder
        {
            public FunctionPrototype Find(string functionCallIdentifier)
            {
                var prototype = new FunctionPrototype();

                // 関数名取得
                var funcCallParts = functionCallIdentifier.Split('.');
				var funcName = funcCallParts.Last();
                prototype.FunctionName = funcName;

                // クラス名取得
				var fullClassName = string.Empty;
				if (funcCallParts.Count() > 2)
				{
					// TODO namespace check
					throw new NotImplementedException();
				}
				else
				{
					// TODO propperly look for class
					switch (funcCallParts[0])
					{
						case "Console":
							fullClassName += "[mscorlib]System." + funcCallParts[0];
							break;
						default: throw new NotImplementedException();
					}
				}
                prototype.FullClassName = fullClassName;

				// TODO properly find returnType
				if (funcName == "ReadLine")
				{
                    prototype.ReturnType = "string";
				}

                return prototype;
            }
        }
    }

    internal interface IFunctionDefinitionFinder
    {
        FunctionPrototype Find(string functionCallIdentifier);
    }
}
