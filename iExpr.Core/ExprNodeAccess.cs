using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iExpr
{
    public class ExprNodeAccess : ExprNode
    {
        public IExpr HeadExpr { get; set; }

        public ExprNodeAccess(IExpr opt, params IExpr[] children) : base(children)
        {
            HeadExpr = opt;
        }

        public ExprNodeAccess(IExpr opt, IExpr child0, params IExpr[] children) : base(child0, children)
        {
            HeadExpr = opt;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(HeadExpr is ExprNode ? $"({HeadExpr.ToString()})" : HeadExpr.ToString());
            if (Children != null && Children.Length != 0)
            {
                sb.Append(Children?.Length > 1 ? $"({String.Join(", ", Children.Select(x => x.ToString()))})" : Children[0].ToString());
            }
            return sb.ToString();
        }
    }
}
