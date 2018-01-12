using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Helpers
{
    /// <summary>
    /// 提供对具体值的相关支持
    /// </summary>
    public class ConcreteValueHelper
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static T GetValue<T>(IExpr exp)
        {
            try
            {
                var v = (exp as ConcreteValue);
                if (v.Value is T) return (T)v.Value;
                return (T)Convert.ChangeType(v.Value, typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception($"{exp} is not a constant", ex);
            }
        }



        public static double DoubleEps { get; set; } = 1e-12;

        /*/// <summary>
        /// 合成值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ConcreteToken BuildValue(object val)
        {
            if (val is double)
            {
                //if (Math.Abs((double)val) < DoubleEps) val = 0;
            }
            return new ConcreteToken(val);
        }*/
    }
}
