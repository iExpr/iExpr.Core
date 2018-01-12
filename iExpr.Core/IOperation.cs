using iExpr.Evaluators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr
{
    /// <summary>
    /// 运算结合性
    /// </summary>
    public enum Association
    {
        /// <summary>
        /// 左结合
        /// </summary>
        Left,
        /// <summary>
        /// 右结合
        /// </summary>
        Right
    }

    /// <summary>
    /// 预定义的优先级
    /// </summary>
    public enum Priority
    {
        HIGHEST=0,
        Highest=10,
        highest=20,
        HIGH = 30,
        High = 40,
        high = 50,
        MIDIUM = 60,
        Midium = 70,
        midium = 80,
        LOW = 90,
        Low = 100,
        low = 110,
        LOWEST=120,
        Lowest=130,
        lowest=140
    }

    /// <summary>
    /// 运算
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// 关键字
        /// </summary>
        string Keyword { get;  }

        /// <summary>
        /// 参数个数
        /// </summary>
        int ArgumentCount { get; }

        /// <summary>
        /// 结合性
        /// </summary>
        Association Association { get;  }

        /// <summary>
        /// 优先级
        /// </summary>
        double Priority { get; }

        /// <summary>
        /// 返回此运算的参数是否自主运算（而不是交由父级运算）
        /// </summary>
        uint[] SelfCalculate { get; }

        /// <summary>
        /// 计算实现
        /// </summary>
        Func<IExpr[], EvalContext, IExpr> EvaluateFunc { get; }

        bool CanPreparameter { get; }

        /// <summary>
        /// 转换表达式字符串实现
        /// </summary>
        Func<IExpr[], string> ToStringFunc { get; }

        /// <summary>
        /// 运算
        /// </summary>
        /// <param name="cal">运算环境</param>
        /// <param name="exps">参数</param>
        /// <returns></returns>
        IExpr Calculate(EvalContext cal, IExpr[] exps);

        /// <summary>
        /// 转换成表达式字符串
        /// </summary>
        /// <param name="exps">参数</param>
        /// <returns></returns>
        string ToString(IExpr[] exps);
    }
}