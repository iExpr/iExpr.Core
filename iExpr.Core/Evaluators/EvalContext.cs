using iExpr.Exceptions;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
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

        public CancellationTokenSource CancelToken { get; private set; }

        public void Renew(CancellationTokenSource cancel=null)
        { 
            CancelToken = cancel ?? new CancellationTokenSource();
        }

        private EvalContext() {  }

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
            catch (Exception ex)
            {
                throw new Exceptions.EvaluateException("Failed to evaluate.", ex);
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

        public void SetVariableValue(string id,IValue val)
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
    }
}
