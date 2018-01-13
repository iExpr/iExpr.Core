using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exceptions
{
    /// <summary>
    /// 变量赋值时使用非值类型时触发
    /// </summary>
    public class NotValueException: EvaluateException
    {
    }
}
