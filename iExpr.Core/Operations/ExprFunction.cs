using iExpr.Calculators;
using iExpr.Values;
using System;

namespace iExpr.Operations
{
    public class ExprFunction : Function
    {
        IExpr content;

        IExpr Evaluate(IExpr[] args, EvalContext cal)
        {
            var cont = cal.Evaluate(content);
            ExprValue ep;
            switch (cont)
            {
                case VariableToken v:
                    ep = (cal.GetVariableValue(v.ID) as ConcreteToken).Value as ExprValue;
                    break;
                case ConcreteToken c:
                    ep = c.Value as ExprValue;
                    break;
                default:
                    throw new Exception();
            }
            return cal.EvaluateExprValue(ep, args);
        }

        public ExprFunction(IExpr content) : base("", null, -1, null, null, true)
        {
            this.content = content;
            base.EvaluateFunc = Evaluate;
            base.Keyword = $"({content.ToExprString()})";
        }
    }
}
