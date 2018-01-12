using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr
{
    public class ExprNodeSingleOperation : ExprNodeBinaryOperation
    {
        public ExprNodeSingleOperation(IOperation opt, IExpr children) : base(opt,children)
        {
            Operation = opt;
        }
    }
}
