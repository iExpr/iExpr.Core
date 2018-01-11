using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr
{
    public class ExprNodeBinaryOperation : ExprNode
    {
        /// <summary>
        /// 根运算
        /// </summary>
        public IOperation Operation { get; set; }
        
        public ExprNodeBinaryOperation(IOperation opt, params IExpr[] children) : base(children)
        {
            Operation = opt;
        }

        public ExprNodeBinaryOperation(IOperation opt, IExpr child0, params IExpr[] children):base(child0,children)
        {
            Operation = opt;
        }

        public override string ToString()
        {
            return Operation.ToString(Children);
        }
    }
}
