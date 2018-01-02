using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr
{
    /// <summary>
    /// 值类型
    /// </summary>
    public interface IValue
    {
        /// <summary>
        /// 转换为值的字符串
        /// </summary>
        /// <returns></returns>
        string ToValueString();

        /// <summary>
        /// 判断是否为常量值
        /// </summary>
        bool IsConstant { get; }

    }
}
