using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr.Evaluators
{
    public interface IExprEvaluator
    {
        IExpr EvaluateNodeBinaryOperation(ExprNodeBinaryOperation expr, EvalContext environment);
        IExpr EvaluateNodeAccess(ExprNodeAccess expr, EvalContext environment);
        IExpr EvaluateNodeCombine(ExprNodeCombine expr, EvalContext environment);
        IExpr EvaluateVariable(VariableToken expr, EvalContext environment);
        IExpr Evaluate(IExpr expr, EvalContext context);
    }

    public class ExprEvaluator : IExprEvaluator
    {
        static ExprEvaluator exprEvaluator;
        public static ExprEvaluator Evaluator
        {
            get
            {
                if (exprEvaluator == null) exprEvaluator = new ExprEvaluator();
                return exprEvaluator;
            }
        }

        public virtual IExpr EvaluateBinaryOperation(ExprNodeBinaryOperation expr, EvalContext environment)
        {
            List<IExpr> args = new List<IExpr>();
            switch (expr.Operation)
            {
                case Operator op:
                    if (op.SelfCalculate != null && op.SelfCalculate.Length == 0)
                        return op.Calculate(environment, expr.Children);
                    for (uint i = 0; i < expr.Children.Length; i++)
                    {
                        if (op.SelfCalculate?.Contains(i) == true) args.Add(expr.Children[i]);
                        else args.Add(Evaluate(expr.Children[i], environment));
                    }
                    return op.Calculate(environment, args.ToArray());
                case Function op:
                    if (op.SelfCalculate != null && op.SelfCalculate.Length == 0)
                        return op.Calculate(environment, expr.Children);
                    for (uint i = 0; i < expr.Children.Length; i++)
                    {
                        if (op.SelfCalculate?.Contains(i) == true) args.Add(expr.Children[i]);
                        else args.Add(Evaluate(expr.Children[i], environment));
                    }
                    return op.Calculate(environment, args.ToArray());
                default:
                    return null;
            }
        }


        public IExpr EvaluateExprValue(ExprValue func, IExpr[] args, EvalContext calculator)
        {
            var vs = func.VariableNames;
            if (!(vs?.Length > 0))
            {
                return calculator.Evaluate(func.Expr);
            }
            else
            {
                //List<double> vals = new List<double>();
                for (int i = 0; i < args.Length && i < vs.Length; i++)
                {
                    calculator.Variables.Set(vs[i],
                        calculator.Evaluate(args[i]));//值是一个表达式
                }
                return calculator.Evaluate(func.Expr);
            }
        }

        public virtual IExpr EvaluateConcrete(ConcreteToken expr, EvalContext environment)
        {
            if (expr.Value == null) return BuiltinValues.Null;// if (environment.NullValue != null) return environment.NullValue; else return expr;
            switch (expr.Value)
            {
                case CollectionValue ls:
                    {
                        List<IExpr> vs = new List<IExpr>();
                        //var que = from x in ls.Contents select Calculate(x);
                        try
                        {
                            foreach (var v in ls)
                            {
                                vs.Add(Evaluate(v, environment));
                            }
                            var vc = ls.CreateNew();
                            vc.Reset(vs);
                            return ConcreteValueHelper.BuildValue(vc);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        //return ConcreteValueHelper.BuildValue(new ListValue(ls.Contents.Select(x=>Calculate(x))));
                    }
                case ExprValue ep:
                    {
                        return expr;//不计算，一般不会执行到这里，只有变量求值时才会
                        var c = environment.GetChild();
                        foreach (var v in ep.VariableNames) c.IgnoreVariables.Add(v);
                        return c.Evaluate(ep.Expr);
                    }
                    //break;
            }
            
            
            return expr;
        }



        public virtual IExpr EvaluateVariable(VariableToken expr, EvalContext environment)
        {
            var v = environment.GetVariableValue(expr.ID);
            if (v == null) return expr;
            else
            {
                return Evaluate(v, environment);
            }
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public virtual IExpr Evaluate(IExpr expr,EvalContext context)
        {
            try
            {
                context.AssertNotCancel();
                switch (expr)
                {
                    case ExprNode exp:
                        return EvaluateNode(exp, context);
                    case ConcreteToken exp:
                        return EvaluateConcrete(exp, context);
                    case VariableToken va:
                        return EvaluateVariable(va, context);
                    //TODO: 未提供ChildExpr计算服务
                    default:
                        return null;
                }
            }
            /*catch (OperationCanceledException)
            {
                return BuiltinValues.Null;
            }*/
            catch (OperationCanceledException)
            {
                return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
    }
    
}
