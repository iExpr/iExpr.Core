using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iExpr.Helpers
{
    /// <summary>
    /// 提供对操作符构建的相关支持
    /// </summary>
    public class OperationHelper
    {
        /// <summary>
        /// 判断参数是否均为具体值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool AssertConstantValue(params IExpr[] val)
        {
            foreach (var v in val) if (!(v is ConcreteToken) || ((ConcreteToken)v).IsConstant == false) return false;
            return true;
        }

        public static bool AssertArgsNumber(int n,params IExpr[] args)
        {
            return args.Length == n;
        }

        /// <summary>
        /// 判断参数是否均为具体值，且具体值的值为指定类型
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool AssertConstantValue<T>(params IExpr[] val)
        {
            foreach (var v in val) if (!(v is ConcreteToken) || ((ConcreteToken)v).IsConstant==false || !(((ConcreteToken)v).Value is T)) return false;
            return true;
        }

        /// <summary>
        /// 获取具体值的值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T[] GetConcreteValue<T>(params IExpr[] val)
        {
            return val.Select((IExpr e) => ConcreteValueHelper.GetValue<T>(e)).ToArray();
        }


        /// <summary>
        /// 生成适用的全部独立运算参数设置
        /// </summary>
        /// <returns></returns>
        public static uint[] GetSelfCalculateAll()
        {
            return GetSelfCalculate();
        }

        /// <summary>
        /// 根据参数生成适用的独立运算参数设置
        /// </summary>
        /// <returns></returns>
        public static uint[] GetSelfCalculate(params uint[] u)
        {
            return u;
        }
    }
}
