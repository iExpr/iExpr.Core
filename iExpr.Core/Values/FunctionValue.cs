using iExpr.Evaluators;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Values
{
    public abstract class FunctionValue : ICallableValue,IContentValue
    {
        public virtual bool IsCertain => true;

        public virtual bool IsSelfCalculate { get; protected set; } = false;

        public abstract Func<FunctionArgument, EvalContext, IExpr> EvaluateFunc { get; protected set; }

        public abstract int ArgumentCount { get; protected set; }

        public virtual EvalContextStartupInfo ContextInfo { get; protected set; }

        public abstract bool Equals(IExpr other);

        public virtual IExpr Call(FunctionArgument args, EvalContext cal)
        {
            return EvaluateFunc(args, cal);
        }

        public virtual IExpr Content(FunctionArgument args, EvalContext cal)
        {
            return EvaluateFunc(args, cal);
        }

        public virtual bool Equals(IValue other)
        {
            return false;
        }
    }
}
