using iExpr.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr
{
    /// <summary>
    /// 变量提供者
    /// </summary>
    public class VariableValueProvider : Dictionary<string, IExpr>
    {
        /// <summary>
        /// 添加变量（不允许递归定义）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Set(string key,object val)
        {
            IExpr e = val is IExpr ? (val as IExpr) : ConcreteValueHelper.BuildValue(val);
            if (val is IExpr)
            {
                var s = ExprHelper.GetVariables(val as IExpr);
                if (s.Contains(new VariableToken(key)))
                {
                    throw new Exception("Cannot recusive define.");
                }
            }
            if (this.ContainsKey(key)) this[key] = e;
            else base.Add(key, e);
        }

        public IExpr Get(string key)
        {
            return this[key];
        }

        public T Get<T>(string key)
        {
            var e = Get(key);
            if (e is ConcreteToken)
            {
                return (T)(e as ConcreteToken).Value;
            }
            else throw new Exception();
        }
    }

    /// <summary>
    /// 变量提供者
    /// </summary>
    public class ConstantValueProvider : Dictionary<string, IExpr>
    {
        /// <summary>
        /// 添加变量（不允许递归定义）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Set(string key, object val)
        {
            IExpr e = val is IExpr ? (val as IExpr) : ConcreteValueHelper.BuildValue(val);
            if (val is IExpr)
            {
                var s = ExprHelper.GetVariables(val as IExpr);
                if (s.Contains(new VariableToken(key)))
                {
                    throw new Exception("Cannot recusive define.");
                }
            }
            if (this.ContainsKey(key)) this[key] = e;
            else base.Add(key, e);
        }

        public IExpr Get(string key)
        {
            return this[key];
        }

        public T Get<T>(string key)
        {
            var e = Get(key);
            if (e is ConcreteToken)
            {
                return (T)(e as ConcreteToken).Value;
            }
            else throw new Exception();
        }
    }
}
