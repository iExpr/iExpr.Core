using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr
{
    /// <summary>
    /// 表达式树叶结点（值）
    /// </summary>
    public abstract class ExprToken : IExpr
    {
        public abstract bool Equals(IExpr other);

        public abstract string ToExprString();

        public override string ToString()
        {
            return ToExprString();
        }
    }
}
