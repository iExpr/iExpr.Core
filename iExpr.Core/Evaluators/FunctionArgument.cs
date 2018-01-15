using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Evaluators
{
    public class FunctionArgument
    {
        public IExpr[] Arguments { get; private set; }
        
        public IExpr[] Contents { get; set; }

        public IExpr[] Indexs { get; set; }

        public FunctionArgument(params IExpr[] args)
        {
            Arguments = args;
        }
    }
}
