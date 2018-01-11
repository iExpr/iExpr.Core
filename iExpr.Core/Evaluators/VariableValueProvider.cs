using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr.Evaluators
{
    /// <summary>
    /// 变量提供者
    /// </summary>
    public class VariableValueProvider : Dictionary<string, IValue>
    {
        /// <summary>
        /// 添加变量（不允许递归定义）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Set(string key, IValue val)
        {
            /*var s = ExprHelper.GetVariables(val as IExpr);
            if (s.Contains(new VariableToken(key)))
            {
                throw new Exception("Cannot recusive define.");
            }*/
            if (this.ContainsKey(key)) this[key] = val;
            else base.Add(key, val);
        }

        public IExpr Get(string key)
        {
            return this[key];
        }

        public T Get<T>(string key)
        {
            var e = Get(key);
            if (e is ConcreteValue)
            {
                return (T)(e as ConcreteValue).Value;
            }
            else throw new Exception();
        }
    }
}
