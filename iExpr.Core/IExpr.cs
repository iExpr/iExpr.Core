using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr
{
    
    /// <summary>
    /// 表达式
    /// </summary>
    public interface IExpr : IEquatable<IExpr>
    {
        /// <summary>
        /// 转换成表达式字符串
        /// </summary>
        /// <returns></returns>
        string ToExprString();
    }
}
