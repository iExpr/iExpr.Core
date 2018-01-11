using System;
using System.Collections.Generic;
using System.Text;
using iExpr.Evaluators;

namespace iExpr.Values
{
    public class PreFunctionValue : FunctionValue
    {
        public string Keyword { get; protected set; }

        public override int ArgumentCount { get; protected set; }

        public override Func<FunctionArgument, EvalContext, IExpr> EvaluateFunc { get; protected set; }

        public override bool Equals(IExpr other)
        {
            var t = other as PreFunctionValue;
            return t != null && t.ToString() == this.ToString();
        }

        public PreFunctionValue(string keyWord, Func<FunctionArgument, EvalContext, IExpr> calculate, int argsCount = -1)
        {
            Keyword = keyWord;
            EvaluateFunc = calculate;
            ArgumentCount = argsCount;
        }

        public override string ToString()
        {
            return Keyword;
        }
    }
}
