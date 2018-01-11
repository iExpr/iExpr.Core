using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr
{
    /// <summary>
    /// 值类型
    /// </summary>
    public interface IValue : IExpr
    {
        /// <summary>
        /// 判断是否为常量值
        /// </summary>
        bool IsConstant { get; }



    }
}
