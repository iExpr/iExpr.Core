using iExpr.Evaluators;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr
{
    /// <summary>
    /// 值类型
    /// </summary>
    public interface IValue : IExpr,IEquatable<IValue>
    {
        /// <summary>
        /// 判断是否为常量值
        /// </summary>
        bool IsConstant { get; }
    }

    public interface ICallableValue : IValue
    {
        bool IsSelfCalculate { get; }

        int ArgumentCount { get; }

        IExpr Call(FunctionArgument args, EvalContext cal);
    }

    public interface IIndexableValue : IValue
    {
        IExpr Index(FunctionArgument args, EvalContext cal);
    }

    public interface IContentValue : IValue
    {
        IExpr Content(FunctionArgument args, EvalContext cal);
    }

    public interface IAccessibleValue : IValue
    {
        IExpr Access(string id);
    }
}
