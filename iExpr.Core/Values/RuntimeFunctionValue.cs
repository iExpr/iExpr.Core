using System;
using System.Collections.Generic;
using System.Text;
using iExpr.Evaluators;
using iExpr.Exceptions;

namespace iExpr.Values
{
    public class RuntimeFunctionValue : FunctionValue
    {
        public override int ArgumentCount { get => VariableNames.Length; protected set => throw new UndefinedExecuteException(); }

        private static IExpr EvaluateExprValue(RuntimeFunctionValue func, FunctionArgument args, EvalContext calculator)
        {
            var vs = func.VariableNames;
            if (!(vs?.Length > 0))
            {
                return calculator.Evaluate(func.Expr);
            }
            else
            {
                //List<double> vals = new List<double>();
                for (int i = 0; i < args.Arguments.Length && i < vs.Length; i++)
                {
                    var val = calculator.Evaluate(args.Arguments[i]);
                    if (val is IValue)
                    {
                        calculator.Variables.Set(vs[i], (IValue)val);
                    }
                    else//值是一个表达式
                    {
                        throw new NotValueException();
                    }
                }
                return calculator.Evaluate(func.Expr);
            }
        }

        public override Func<FunctionArgument, EvalContext, IExpr> EvaluateFunc
        {
            get;
            protected set;
        }

        public IExpr Expr { get; set; }

        public string[] VariableNames { get; set; }

        public RuntimeFunctionValue()
        {
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
