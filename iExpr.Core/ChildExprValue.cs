using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr
{
    public class ChildExprValue : ExprValue
    {
        public ChildExprValue(string expr = null)
        {
            Expr = expr;
        }

        public string Expr { get; set; }

        public override string ToExprString()
        {
            return $"{{{this.Expr}}}";
        }
    }
}
