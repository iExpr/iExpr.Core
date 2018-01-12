using iExpr.Exceptions;
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
        /*IExpr EvaluateNodeBinaryOperation(ExprNodeBinaryOperation expr, EvalContext environment);
        IExpr EvaluateNodeAccess(ExprNodeAccess expr, EvalContext environment);
        IExpr EvaluateNodeCall(ExprNodeCall expr, EvalContext environment);
        IExpr EvaluateNodeIndex(ExprNodeIndex expr, EvalContext environment);
        IExpr EvaluateNodeContent(ExprNodeContent expr, EvalContext environment);
        IExpr EvaluateVariable(VariableToken expr, EvalContext environment);
        IExpr EvaluateColletionValue(CollectionValue expr, EvalContext environment);*/
        IExpr Evaluate(IExpr expr, EvalContext context, bool evalConstant = false);
        
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

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public virtual IExpr Evaluate(IExpr expr,EvalContext context,bool evalConstant=false)
        {
            try
            {
                context.AssertNotCancel();
                switch (expr)
                {
                    case ExprNodeBinaryOperation o:
                        return EvaluateNodeBinaryOperation(o, context);
                    case ExprNodeAccess o:
                        return EvaluateNodeAccess(o, context);
                    case ExprNodeCall o:
                        return EvaluateNodeCall(o, context);
                    case ExprNodeIndex o:
                        return EvaluateNodeIndex(o, context);
                    case ExprNodeContent o:
                        return EvaluateNodeContent(o, context);
                    case ConstantToken o:
                        if (evalConstant)
                            return o.Value;
                        else return o;
                    case VariableToken o:
                        return EvaluateVariable(o, context);
                    case CollectionValue o:
                        return EvaluateColletionValue(o, context);
                    case FunctionValue o:
                        return o;
                    case ConcreteValue o:
                        return o;
                    default:
                        throw new EvaluateException("Can't evaluate this kind of expr.");
                }
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (ExprException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exceptions.EvaluateException("Failed to evaluate.", ex);
            }
        }

        public static IExpr EvaluateColletionValue(CollectionValue expr, EvalContext environment)
        {
            List<IValue> vs = new List<IValue>();
            //var que = from x in ls.Contents select Calculate(x);
            try
            {
                foreach (var v in expr)
                {
                    var val = environment.Evaluate(v);
                    if (val is IValue)
                    {
                        vs.Add((IValue)val);
                    }
                    else//值是一个表达式
                    {
                        throw new NotValueException();
                    }
                }
                var vc = expr.CreateNew();
                vc.Reset(vs);
                return vc;
            }
            catch (ExprException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exceptions.EvaluateException("Failed to evaluate.", ex);
            }
        }

        public static IExpr EvaluateNodeAccess(ExprNodeAccess expr, EvalContext environment)
        {
            throw new NotImplementedException();
        }

        public static IExpr EvaluateNodeBinaryOperation(ExprNodeBinaryOperation expr, EvalContext environment)
        {
            List<IExpr> args = new List<IExpr>();
            var op = expr.Operation;
            if (op.SelfCalculate != null && op.SelfCalculate.Length == 0)
                return op.Calculate(environment, expr.Children);
            for (uint i = 0; i < expr.Children.Length; i++)
            {
                if (op.SelfCalculate?.Contains(i) == true) args.Add(expr.Children[i]);
                else args.Add(environment.Evaluate(expr.Children[i]));
            }
            return op.Calculate(environment, args.ToArray());
        }

        public static IExpr EvaluateNodeCall(ExprNodeCall expr, EvalContext environment)
        {
            var head = environment.Evaluate(expr.HeadExpr);
            //if (head is ConstantToken) head = (head as ConstantToken).Value;//TODO: Attention this
            if (!(head is FunctionValue)) throw new Exceptions.EvaluateException("The invoking expr must have a function.");
            var cs = environment.GetChild();//开辟一个子环境
            var func = head as FunctionValue;
            if (func.ArgumentCount == 0) return func.EvaluateFunc(null, cs);
            List<IValue> args = new List<IValue>();
            foreach(var v in expr.Children)
            {
                var val = cs.Evaluate(v);
                if (val is IValue)
                {
                    args.Add((IValue)val);
                }
                else//值是一个表达式
                {
                    throw new NotValueException();
                }
            }
            return func.EvaluateFunc(new FunctionArgument(args.ToArray()), cs);
        }

        public static IExpr EvaluateNodeContent(ExprNodeContent expr, EvalContext environment)
        {
            throw new NotImplementedException();
        }

        public static IExpr EvaluateNodeIndex(ExprNodeIndex expr, EvalContext environment)
        {
            if(expr.Children?.Length!=1)
                throw new EvaluateException("The index content is invalid.");
            var pind = environment.Evaluate(expr.Children[0]) as IValue;
            int ind = ConcreteValueHelper.GetValue<int>(pind);//TODO: Check for null
            var head = environment.Evaluate(expr.HeadExpr);
            switch (head)
            {
                case ListValue l:
                    return l[ind];
                case TupleValue l:
                    return l[ind];
                default:
                    throw new Exceptions.EvaluateException("The index expr must have a colletion that can be indexed.");
            }
        }

        public static IExpr EvaluateVariable(VariableToken expr, EvalContext environment)
        {
            var v = environment.GetVariableValue(expr.ID);
            if (v == null) return expr;
            else
            {
                return environment.Evaluate(v);//TODO: Attention here.
            }
        }
    }
    
}
