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

    public class EvalContext
    {
        /// <summary>
        /// 运算提供者
        /// </summary>
        public IExprEvaluator Evaluator { get; set; }

        /// <summary>
        /// 获取一个新的子环境
        /// </summary>
        /// <returns></returns>
        public virtual EvalContext GetChild()
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
            var p = this;
            while (p != null && p.Variables?.ContainsKey(id) != true) p = p.Parent;
            if (p == null) return false;
            else
            {
                return true;
            }
        }

        public T GetVariableValue<T>(string id)
        {
            var p = this;
            while (p != null && p.Variables?.ContainsKey(id) != true) p = p.Parent;
            if (p == null) return default(T);
            else
            {
                var v = p.Variables.Get<T>(id);
                return v;
            }
        }

        public IExpr GetVariableValue(string id)
        {
            var p = this;
            while (p != null && p.Variables?.ContainsKey(id) != true) p = p.Parent;
            if (p == null) return null;
            else
            {
                var v = p.Variables.Get(id);
                return v;
            }
        }

        public void SetVariableValue(string id,IExpr val)
        {
            var p = this;
            while (p != null && p.Variables?.ContainsKey(id) != true) p = p.Parent;
            if (p == null)
            {
                this.Variables.Set(id,val);
            }
            else
            {
                p.Variables.Set(id, val);
            }
        }

        protected virtual T GetValue<T>(ConcreteValue exp)
        {
            try
            {
                if (exp == null) return default(T);
                var v = exp;
                if (v.Value is T) return (T)v.Value;
                try
                {
                    return (T)Convert.ChangeType(v.Value, typeof(T));
                }
                catch
                {
                    if (v.Value == null) return default(T);
                    return (T)v.Value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{exp} is not a constant", ex);
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
            if (e is ConcreteValue) return GetValue<T>((ConcreteValue)e);
            else if (e is ConstantToken)
            {
                var t = e as ConstantToken;
                if (t.Value is ConcreteValue)
                    return GetValue<T>((ConcreteValue)t.Value);
                else return (T)t.Value;
            }
            return (T)e;
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
