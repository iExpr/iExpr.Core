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

        //IExpr PreEvaluate(IExpr expr, EvalContext context, bool evalConstant = false);

    }

    public struct EvalContextStartupInfo
    {
        public EvalContextStartupInfo(bool isNew,bool isProtected=false, VariableFindMode findMode = VariableFindMode.UpAll) : this()
        {
            IsNew = isNew;
            FindMode = findMode;
            IsProtected = isProtected;
        }

        public bool IsNew { get; set; }

        public bool IsProtected { get; set; }

        public VariableFindMode FindMode { get; set; }
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
                if (expr == null) return null;//TODO:Attention this
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
                        if (o.Value is IExpr) return context.Evaluate((IExpr)o.Value);
                        return o;
                    case NativeExprValue o:
                        return context.Evaluate(o.Expr);
                    default:
                        ExceptionHelper.RaiseInvalidExpressionFailed(this, expr, "Can't evaluate the expr");
                        return null;
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
            catch (IgnoredException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                throw new Exceptions.EvaluateException(this, "Evaluate failed.",ex);
            }
        }

        /// <summary>
        /// 计算，并创建新的集合
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static IExpr EvaluateColletionValue(CollectionValue expr, EvalContext environment)
        {
            if (expr.IsCertain == true) return expr;
            List<IValue> vs = new List<IValue>();
            //var que = from x in ls.Contents select Calculate(x);
            try
            {
                foreach (var v in (IEnumerable<IValue> )expr)
                {
                    if (v.IsCertain) vs.Add(v);
                    else
                    {
                        var val = environment.Evaluate(v);
                        if (val is IValue)
                        {
                            vs.Add((IValue)val);
                        }
                        else//值是一个表达式
                        {
                            ExceptionHelper.RaiseNotValue(expr, v);
                        }
                    }
                }
                var res = expr.CreateNew();
                res.Reset(vs);
                return res;
            }
            catch (ExprException ex)
            {
                throw ex;
            }
        }

        public static IExpr EvaluateNodeAccess(ExprNodeAccess expr, EvalContext environment)
        {
            try
            {
                var func = environment.GetValue<IAccessibleValue>(environment.Evaluate(expr.HeadExpr));
                return func.Access(expr.Variable.ID);
            }
            catch (ExprException ex)
            {
                throw ex;
            }
            catch(IgnoredException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                ExceptionHelper.RaiseAccessFailed(expr,expr.Variable.ID,null,ex);
                return default;
            }
        }

        public static IExpr EvaluateNodeBinaryOperation(ExprNodeBinaryOperation expr, EvalContext environment)
        {
            List<IExpr> args = new List<IExpr>();
            var op = expr.Operation;
            EvalContext en = null;
            if (op.ContextInfo.IsNew)
            {
                en = environment.GetChild(op.ContextInfo.FindMode);
            }
            else
            {
                en = environment;
            }
            if (op.SelfCalculate != null && op.SelfCalculate.Length == 0)
                return op.Calculate(en, expr.Children);
            for (uint i = 0; i < expr.Children.Length; i++)
            {
                if (op.SelfCalculate?.Contains(i) == true) args.Add(expr.Children[i]);
                else args.Add(en.Evaluate(expr.Children[i]));
            }
            var res=op.Calculate(en, args.ToArray());//在同一环境
            if (op.ContextInfo.IsNew == true && op.ContextInfo.IsProtected == false) en.Dispose();
            return res;
        }

        public static IExpr EvaluateNodeCall(ExprNodeCall expr, EvalContext environment)
        {
            try
            {

                var func = environment.GetValue<ICallableValue>(environment.Evaluate(expr.HeadExpr));
                EvalContext en = null;
                if (func.ContextInfo.IsNew)
                {
                    en = environment.GetChild(func.ContextInfo.FindMode);
                }
                else
                {
                    en = environment;
                }
                if (func.ArgumentCount == 0) return func.Call(null, en);
                IExpr res = null;
                if (func.IsSelfCalculate == false)
                {
                    List<IExpr> args = new List<IExpr>();
                    foreach (var v in expr.Children)
                    {
                        var val = en.Evaluate(v);
                        args.Add(val);
                        continue;
                        /*if (val is IValue)
                        {
                            args.Add((IValue)val);
                        }
                        else//值是一个表达式
                        {
                            throw new NotValueException();
                        }*/
                    }
                    res= func.Call(new FunctionArgument(args.ToArray()), en);
                }
                else
                {
                    res= func.Call(new FunctionArgument(expr.Children), en);
                }
                if (func.ContextInfo.IsNew==true && func.ContextInfo.IsProtected == false) en.Dispose();
                return res;
            }
            catch (ExprException ex)
            {
                throw ex;
            }
            catch (IgnoredException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ExceptionHelper.RaiseCallFailed(expr, null, null, ex);
                return default;
            }
        }

        public static IExpr EvaluateNodeContent(ExprNodeContent expr, EvalContext environment)
        {
            try
            {
                
                var func = environment.GetValue<IContentValue>(environment.Evaluate(expr.HeadExpr));
                EvalContext en = null;
                if (func.ContextInfo.IsNew)
                {
                    en = environment.GetChild(func.ContextInfo.FindMode);
                }
                else
                {
                    en = environment;
                }
                var res= func.Content(new FunctionArgument() { Contents = expr.Children }, en);
                if (func.ContextInfo.IsNew==true && func.ContextInfo.IsProtected == false) en.Dispose();
                return res;
            }
            catch (ExprException ex)
            {
                throw ex;
            }
            catch (IgnoredException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ExceptionHelper.RaiseContentFailed(expr, null, null, ex);
                return default;
            }
        }

        public static IExpr EvaluateNodeIndex(ExprNodeIndex expr, EvalContext environment)
        {
            try
            {
                var func = environment.GetValue<IIndexableValue>(environment.Evaluate(expr.HeadExpr));
                EvalContext en = null;
                if (func.ContextInfo.IsNew)
                {
                    en = environment.GetChild(func.ContextInfo.FindMode);
                }
                else
                {
                    en = environment;
                }
                var res= func.Index(new FunctionArgument() { Indexs = expr.Children }, en);
                if (func.ContextInfo.IsNew==true && func.ContextInfo.IsProtected == false) en.Dispose();
                return res;
            }
            catch (ExprException ex)
            {
                throw ex;
            }
            catch (IgnoredException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ExceptionHelper.RaiseIndexFailed(expr, null, null, ex);
                return default;
            }
        }

        public static IExpr EvaluateVariable(VariableToken expr, EvalContext environment)
        {
            var v = environment.GetVariableValue(expr.ID);
            if (v == null) return expr;
            else
            {
                if(v is IValue)
                {
                    var t = (IValue)v;
                    if (t.IsCertain) return t;
                }
                return environment.Evaluate(v);//TODO: Attention here.
            }
        }
    }
    
}
