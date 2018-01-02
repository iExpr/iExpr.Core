using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

namespace iExpr.Calculators
{
    public enum EvalState
    {
        None,
        Working,
        Finished,
        Failed
    }

    public class EvalContext
    {
        /// <summary>
        /// 空值
        /// </summary>
        public IExpr NullValue { get; set; }

        /// <summary>
        /// 运算提供者
        /// </summary>
        public ExprEvaluator Evaluator { get; set; }

        /// <summary>
        /// 获取一个新的子环境
        /// </summary>
        /// <returns></returns>
        public virtual EvalContext GetChild()
        {
            return new EvalContext() { Evaluator=Evaluator,CancelToken=CancelToken, NullValue = NullValue, Parent = this, Constants = Constants };
        }

        /// <summary>
        /// 变量列表
        /// </summary>
        public VariableValueProvider Variables { get; set; } = new VariableValueProvider();

        /// <summary>
        /// 常量集合
        /// </summary>
        public ConstantValueProvider Constants { get; set; }

        /// <summary>
        /// 忽视变量列表
        /// </summary>
        public HashSet<string> IgnoreVariables { get; set; } = new HashSet<string>();

        /// <summary>
        /// 父级运算环境
        /// </summary>
        public EvalContext Parent { get; set; }

        public CancellationTokenSource CancelToken { get; private set; }

        public EvalState State { get; private set; }

        public void Renew(CancellationTokenSource cancel=null)
        {
            State = EvalState.None;
            CancelToken = cancel ?? new CancellationTokenSource();
        }

        private EvalContext() {  }

        public static EvalContext Create(CancellationTokenSource cancel)
        {
            var res = new EvalContext
            {
                CancelToken = cancel,
                State = EvalState.None
            };
            return res;
        }

        public async Task<IExpr> EvaluateAsync(IExpr expr)
        {
            if (State != EvalState.None) throw new Exception("The context is not in None state.");
            return await Task.Run(() => 
            {
                try
                {
                    State = EvalState.Working;
                    var v= Evaluate(expr);
                    State = EvalState.Finished;
                    return v;
                }
                catch (OperationCanceledException)
                {
                    return ConcreteToken.Null;
                }
                catch (Exception ex)
                {
                    return ConcreteToken.Null;//TODO: Attention
                    //State = EvalState.Failed;
                    throw ex;
                }
            }
            ,CancelToken.Token);
        }

        public IExpr EvaluateExprValue(ExprValue func, params IExpr[] args)
        {
            try
            {
                return Evaluator.EvaluateExprValue(func, args, this);
            }
            catch (OperationCanceledException)
            {
                return ConcreteToken.Null;
            }
            catch
            {
                return ConcreteToken.Null;//TODO: Attention
            }
        }

        public IExpr Evaluate(IExpr expr)
        {
            try
            {
                return Evaluator.Evaluate(expr, this);
            }
            catch(OperationCanceledException)
            {
                return ConcreteToken.Null;
            }
            catch
            {
                return ConcreteToken.Null;//TODO: Attention
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
            while (p != null && p.IgnoreVariables.Contains(id) == false) p = p.Parent;
            if (p != null) return default(T);
            p = this;
            while (p != null && p.Variables?.ContainsKey(id) != true && p.Constants?.ContainsKey(id) != true) p = p.Parent;
            if (p == null) return default(T);
            else
            {
                var v = p.Variables?.ContainsKey(id) == true ? p.Variables.Get<T>(id) : p.Constants.Get<T>(id);
                return v;
            }
        }

        public IExpr GetVariableValue(string id)
        {
            var p = this;
            while (p != null && p.IgnoreVariables.Contains(id) == false) p = p.Parent;
            if (p != null) return null;
            p = this;
            while (p != null && p.Variables?.ContainsKey(id) != true && p.Constants?.ContainsKey(id) != true) p = p.Parent;
            if (p == null) return null;
            else
            {
                var v = p.Variables?.ContainsKey(id) == true ? p.Variables.Get(id) : p.Constants.Get(id);
                return v;
            }
        }


        public void SetVariableValue(string id,object val)
        {
            var p = this;
            while (p != null && p.Variables?.ContainsKey(id) != true && p.Constants?.ContainsKey(id) != true) p = p.Parent;
            if (p == null)
            {
                this.Variables.Set(id,val);
            }
            else
            {
                if(p.Variables?.ContainsKey(id) == true)
                {
                    p.Variables.Set(id, val);
                }
                else//常量列表中含有id，但仍添加到变量列表中（隐式替换常量（优先计算））
                {
                    p.Variables.Set(id, val);
                }
            }
        }
    }
}
