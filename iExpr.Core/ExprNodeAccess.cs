using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iExpr
{
    public class ExprNodeAccess : ExprNode
    {
        public IExpr HeadExpr { get; set; }

        public VariableToken Variable { get => base.Children[0] as VariableToken; }

        public ExprNodeAccess(IExpr opt, VariableToken id) : base(id)
        {
            HeadExpr = opt;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(HeadExpr is ExprNode ? $"({HeadExpr.ToString()})" : HeadExpr.ToString());
            sb.Append("." + Variable.ID);
            return sb.ToString();
        }
    }
}
