using iExpr.Evaluators;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Values
{
    public abstract class FunctionValue : IValue
    {
        public bool IsConstant => true;

        public abstract Func<FunctionArgument, EvalContext, IExpr> EvaluateFunc { get; protected set; }

        public abstract int ArgumentCount { get; protected set; }

        public abstract bool Equals(IExpr other);
    }
}
