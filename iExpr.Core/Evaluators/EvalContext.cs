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

    public class EvalContext : IDisposable
    {
        /// <summary>
        /// 运算提供者
        /// </summary>
        public virtual IExprEvaluator Evaluator { get; set; }

        public virtual VariableFindMode VariableFindMode { get; protected set; }

        public virtual VariableValueProvider BasicVariables { get; set; }

        /// <summary>
        /// 获取一个新的子环境
        /// </summary>
        /// <returns></returns>
        public virtual EvalContext GetChild(VariableFindMode mode = VariableFindMode.UpAll)
        {
            return new EvalContext() { Evaluator=Evaluator,CancelToken=CancelToken, Parent = this,BasicVariables=BasicVariables};
        }

        /// <summary>
        /// 变量列表
        /// </summary>
        public virtual VariableValueProvider Variables { get; set; } = new VariableValueProvider();

        /// <summary>
        /// 父级运算环境
        /// </summary>
        public virtual EvalContext Parent { get; set; }

        public virtual CancellationTokenSource CancelToken { get; protected set; }

        public virtual void Renew(CancellationTokenSource cancel=null)
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

        public virtual IExpr Evaluate(IExpr expr,bool evalConstant=false)
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

        public virtual void AssertNotCancel()
        {
            CancelToken.Token.ThrowIfCancellationRequested();
        }

        public virtual void Cancel()
        {
            CancelToken.Cancel();
        }

        public virtual bool HasVariable(string id)
        {
            if (this.Variables?.ContainsKey(id)==true) return true;
            if (this.BasicVariables?.ContainsKey(id) == true) return true;
            else if (VariableFindMode == VariableFindMode.UpAll || VariableFindMode == VariableFindMode.UpGetOnly) return this.Parent?.HasVariable(id) ?? false;
            return false;
        }

        public virtual T GetVariableValue<T>(string id)
        {
            if (this.Variables?.ContainsKey(id) == true) return this.Variables.Get<T>(id);
            if (this.BasicVariables?.ContainsKey(id) == true) return this.BasicVariables.Get<T>(id);
            else if (VariableFindMode == VariableFindMode.UpAll || VariableFindMode == VariableFindMode.UpGetOnly)
            {
                if (this.Parent == null) return default;
                else return this.Parent.GetVariableValue<T>(id);
            }
            else
            {
                return default;
            }
        }

        public virtual IExpr GetVariableValue(string id)
        {
            if (this.Variables?.ContainsKey(id) == true) return this.Variables.Get(id);
            if (this.BasicVariables?.ContainsKey(id) == true) return this.BasicVariables.Get(id);
            else if (VariableFindMode == VariableFindMode.UpAll || VariableFindMode == VariableFindMode.UpGetOnly)
            {
                if (this.Parent == null) return default;
                else return this.Parent.GetVariableValue(id);
            }
            else
            {
                return default;
            }
        }

        public virtual bool TrySetVariableValue(string id,IExpr val)
        {
            if (this.Variables?.ContainsKey(id) == true)
            {
                this.Variables.Set(id, val);
                return true;
            }
            if (this.BasicVariables?.ContainsKey(id) == true)
            {
                this.BasicVariables.Set(id, val);
                return true;
            }
            if (VariableFindMode == VariableFindMode.UpAll)
            {
                return this.Parent?.TrySetVariableValue(id, val) == true;
            }
            return false;
        }

        public virtual bool SetVariableValue(string id,IExpr val)
        {
            if (this.Variables?.ContainsKey(id) == true)
            {
                this.Variables.Set(id, val);
                return true;
            }
            if (VariableFindMode == VariableFindMode.UpAll)
            {
                //Only null
                if (this.Parent?.TrySetVariableValue(id, val) != true) this.Variables.Set(id, val);
                return true;
            }
            else
            {
                this.Variables.Set(id, val);
                return true;
            }
            //return false;
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

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                this.Evaluator = null;
                this.Variables = null;
                this.Parent = null;
                this.BasicVariables = null;
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~EvalContext() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
