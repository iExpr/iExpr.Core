using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iExpr
{
    /*public class ExprNodeCombine: ExprNode
    {
        public IExpr HeadExpr { get; set; }

        public IExpr[] IndexChildren { get; set; }

        public IExpr[] ContentChildren { get; set; }

        public ExprNodeCombine(IExpr opt, params IExpr[] children) : base(children)
        {
            HeadExpr = opt;
        }

        public ExprNodeCombine(IExpr opt, IExpr child0, params IExpr[] children) : base(child0, children)
        {
            HeadExpr = opt;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(HeadExpr is ExprNode ? $"({HeadExpr.ToString()})" : HeadExpr.ToString());
            if (Children != null) sb.Append($"({String.Join(", ", Children.Select(x => x.ToString()))})");
            if (IndexChildren != null) sb.Append($"[{String.Join(", ", IndexChildren.Select(x => x.ToString()))}]");
            if (ContentChildren != null) sb.Append($"{{{String.Join(", ", ContentChildren.Select(x => x.ToString()))}}}");
            return sb.ToString();
        }
    }*/

    public class ExprNodeCall : ExprNode
    {
        public IExpr HeadExpr { get; set; }

        public ExprNodeCall(IExpr opt, params IExpr[] children) : base(children)
        {
            HeadExpr = opt;
        }

        public ExprNodeCall(IExpr opt, IExpr child0, params IExpr[] children) : base(child0, children)
        {
            HeadExpr = opt;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(HeadExpr is ExprNode ? $"({HeadExpr.ToString()})" : HeadExpr.ToString());
            if (Children != null) sb.Append($"({String.Join(", ", Children.Select(x => x.ToString()))})");
            return sb.ToString();
        }
    }

    public class ExprNodeIndex : ExprNode
    {
        public IExpr HeadExpr { get; set; }

        public ExprNodeIndex(IExpr opt, params IExpr[] children) : base(children)
        {
            HeadExpr = opt;
        }

        public ExprNodeIndex(IExpr opt, IExpr child0, params IExpr[] children) : base(child0, children)
        {
            HeadExpr = opt;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(HeadExpr is ExprNode ? $"({HeadExpr.ToString()})" : HeadExpr.ToString());
            if (Children != null) sb.Append($"[{String.Join(", ", Children.Select(x => x.ToString()))}]");
            return sb.ToString();
        }
    }

    public class ExprNodeContent : ExprNode
    {
        public IExpr HeadExpr { get; set; }

        public ExprNodeContent(IExpr opt, params IExpr[] children) : base(children)
        {
            HeadExpr = opt;
        }

        public ExprNodeContent(IExpr opt, IExpr child0, params IExpr[] children) : base(child0, children)
        {
            HeadExpr = opt;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(HeadExpr is ExprNode ? $"({HeadExpr.ToString()})" : HeadExpr.ToString());
            if (Children != null) sb.Append($"{{{String.Join(", ", Children.Select(x => x.ToString()))}}}");
            return sb.ToString();
        }
    }
}
