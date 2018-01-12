using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Evaluators
{
    public class FunctionArgument
    {
        public IExpr[] Arguments { get; private set; }
        

        public FunctionArgument(IExpr[] args)
        {
            Arguments = args;
        }
    }
}
