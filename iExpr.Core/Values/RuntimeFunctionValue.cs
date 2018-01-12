using System;
using System.Collections.Generic;
using System.Text;
using iExpr.Evaluators;
using iExpr.Exceptions;

namespace iExpr.Values
{
    public class RuntimeFunctionValue : FunctionValue
    {
        public override int ArgumentCount { get => VariableNames==null?0: VariableNames.Length; protected set => throw new UndefinedExecuteException(); }

        private static IExpr EvaluateExprValue(RuntimeFunctionValue func, FunctionArgument args, EvalContext calculator)
        {
            if (func.ArgumentCount == 0)
            {
                return calculator.Evaluate(func.Expr);
            }
            var vs = func.VariableNames;
            //List<double> vals = new List<double>();
            for (int i = 0; i < args.Arguments.Length && i < vs.Length; i++)
            {
                calculator.Variables.Set(vs[i], args.Arguments[i]);
            }
            return calculator.Evaluate(func.Expr);
        }

        public override Func<FunctionArgument, EvalContext, IExpr> EvaluateFunc
        {
            get;
            protected set;
        }

        public IExpr Expr { get; set; }

        public string[] VariableNames { get; set; }

        public RuntimeFunctionValue(IExpr expr,params string[] variableNames)
        {
            Expr = expr;
            VariableNames = variableNames;
            EvaluateFunc = new Func<FunctionArgument, EvalContext, IExpr>((a, c) => EvaluateExprValue(this, a, c));
        }

        public override bool Equals(IExpr other)
        {
            if (!(other is RuntimeFunctionValue)) return false;
            return other.ToString() == this.ToString();
        }

        public override string ToString()//TODO: 注意这里实现不同，不是()=>()实现
        {
            return Expr?.ToString();
        }
    }
}
