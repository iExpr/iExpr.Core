using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Values
{
    public class NativeExprValue : IValue
    {
        public IExpr Expr { get; set; }

        public bool IsCertain => false;

        public bool Equals(IExpr other)
        {
            if (other is NativeExprValue) return (other as NativeExprValue).Expr.Equals(Expr);
            return false;
        }

        public NativeExprValue(IExpr val)
        {
            Expr = val;
        }

        public override string ToString()
        {
            return $"<native expression {Expr?.ToString()}>";
        }
    }
}
