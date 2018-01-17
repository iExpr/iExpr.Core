using iExpr.Exceptions;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

namespace iExpr.Evaluators
{
    /*public enum EvalState
    {
        None,
        Working,
        Finished,
        Failed
    }*/

    public enum VariableFindMode
    {
        UpAll,
        UpGetOnly,
        NoUp
    }

    public class EvalContext
    {
        /// <summary>
        /// 运算提供者
        /// </summary>
        public IExprEvaluator Evaluator { get; set; }

        public VariableFindMode VariableFindMode { get; protected set; }

        /// <summary>
        /// 获取一个新的子环境
        /// </summary>
        /// <returns></returns>
        public virtual EvalContext GetChild(VariableFindMode mode = VariableFindMode.UpAll)
        {
            return new EvalContext() { Evaluator=Evaluator,CancelToken=CancelToken, Parent = this};
        }

        /// <summary>
        /// 变量列表
        /// </summary>
        public VariableValueProvider Variables { get; set; } = new VariableValueProvider();

        /// <summary>
        /// 父级运算环境
        /// </summary>
        public EvalContext Parent { get; set; }

        public CancellationTokenSource CancelToken { get; protected set; }

        public void Renew(CancellationTokenSource cancel=null)
        { 
            CancelToken = cancel ?? new CancellationTokenSource();
        }

        protected EvalContext() {  }

        public static EvalContext Create(CancellationTokenSource cancel)
        {
            var res = new EvalContext
            {
                CancelToken = cancel
            };
            return res;
        }

        public IExpr Evaluate(IExpr expr,bool evalConstant=false)
        {
            try
            {
                return Evaluator.Evaluate(expr, this,evalConstant);
            }
            catch(OperationCanceledException)
            {
                return BuiltinValues.Null;
            }
            catch (ExprException ex)
            {
                throw ex;
            }
        }
        
        public void AssertNotCancel()
        {
            CancelToken.Token.ThrowIfCancellationRequested();
        }

        public void Cancel()
        {
            CancelToken.Cancel();
        }

        public bool HasVariable(string id)
        {
            switch (VariableFindMode)
            {
                case VariableFindMode.UpAll:
                case VariableFindMode.UpGetOnly:
                    {
                        var p = this;
                        while (p != null && p.Variables.ContainsKey(id) != true) p = p.Parent;
                        if (p == null) return false;
                        else
                        {
                            return true;
                        }
                    }
                case VariableFindMode.NoUp:
                    return this.Variables.ContainsKey(id);
                default:
                    throw new UndefinedExecuteException();
            }
            
        }

        public T GetVariableValue<T>(string id)
        {
            switch (VariableFindMode)
            {
                case VariableFindMode.UpAll:
                case VariableFindMode.UpGetOnly:
                    {
                        var p = this;
                        while (p != null && p.Variables.ContainsKey(id) != true) p = p.Parent;
                        if (p == null) return default(T);
                        else
                        {
                            var v = p.Variables.Get<T>(id);
                            return v;
                        }
                    }
                case VariableFindMode.NoUp:
                    return this.Variables.ContainsKey(id)?this.Variables.Get<T>(id):default(T);
                default:
                    throw new UndefinedExecuteException();
            }
        }

        public IExpr GetVariableValue(string id)
        {
            switch (VariableFindMode)
            {
                case VariableFindMode.UpAll:
                case VariableFindMode.UpGetOnly:
                    {
                        var p = this;
                        while (p != null && p.Variables.ContainsKey(id) != true) p = p.Parent;
                        if (p == null) return null;
                        else
                        {
                            var v = p.Variables.Get(id);
                            return v;
                        }
                    }
                case VariableFindMode.NoUp:
                    return this.Variables.ContainsKey(id) ? this.Variables.Get(id) : null;
                default:
                    throw new UndefinedExecuteException();
            }
        }

        public void SetVariableValue(string id,IExpr val)
        {
            switch (VariableFindMode)
            {
                case VariableFindMode.UpAll:
                    {
                        var p = this;
                        while (p != null && p.Variables?.ContainsKey(id) != true) p = p.Parent;
                        if (p == null)
                        {
                            this.Variables.Set(id, val);
                        }
                        else
                        {
                            p.Variables.Set(id, val);
                        }
                    }
                    break;
                case VariableFindMode.UpGetOnly:
                case VariableFindMode.NoUp:
                    this.Variables.Set(id, val);
                    break;
                default:
                    throw new UndefinedExecuteException();
            }
            
        }

        protected virtual T ConvertValue<T>(object val)
        {
            try
            {
                if (val == null) return default(T);
                if (val is T) return (T)val;
                try
                {
                    return (T)Convert.ChangeType(val, typeof(T));
                }
                catch
                {
                    return (T)val;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{val} is not a constant", ex);
            }
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public virtual T GetValue<T>(IExpr e)
        {
            if (e is T) return (T)e;
            if (e is IHasValue) return ConvertValue<T>((e as IHasValue).Value);
            return ConvertValue<T>(e);
        }

        /// <summary>
        /// 获取具体值的值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual T[] GetValue<T>(params IExpr[] val)
        {
            return val.Select((IExpr e) => {
                return GetValue<T>(e);
            }).ToArray();
        }
    }
}
