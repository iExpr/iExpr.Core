using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Evaluators
{
    public class FunctionArgument
    {
        public IValue[] Arguments { get; private set; }
        

        public FunctionArgument(IValue[] args)
        {
            Arguments = args;
        }
    }
}
