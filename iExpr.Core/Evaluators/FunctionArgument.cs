using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Evaluators
{
    public class FunctionArgument
    {
        public IExpr[] Arguments { get; private set; }

        public IExpr[] Indexes { get; private set; }

        public IExpr[] Contents{ get; private set; }

        public FunctionArgument(IExpr[] args,IExpr[] indexes=null,IExpr[] contents=null)
        {
            Arguments = args;
            Indexes = indexes;
            Contents = contents;
        }
    }
}
