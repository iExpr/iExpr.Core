using iExpr.Exceptions;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iExpr.Helpers
{
    /// <summary>
    /// 提供对操作符构建的相关支持
    /// </summary>
    public static class OperationHelper
    {
        public static bool AssertArgsNumber(int n,params IExpr[] args)
        {
            if (args == null) return 0 == n;
            return args.Length == n;
        }

        public static void AssertArgsNumberThrowIf(object sender, int n, params IExpr[] args)
        {
            if (!AssertArgsNumber(n, args)) ExceptionHelper.RaiseWrongArgsNumber(sender, n, args?.Length ?? 0);
        }

        /// <summary>
        /// 判断参数是否均为具体值，且具体值的值为指定类型
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool AssertCertainValue<T>(params IExpr[] val)
        {
            foreach (var v in val)
            {
                switch (v)
                {
                    case ConcreteValue c:
                        if (c.IsCertain == false) return false;
                        if (!(c.Value is T)) return false;
                        break;
                    case ConstantToken c:
                        if (!(c.Value is T)) return false;
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断参数是否均为具体值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool AssertCertainValue(params IExpr[] val)
        {
            foreach (var v in val)
            {
                if (v is IValue)
                {
                    if (!((IValue)v).IsCertain) return false;
                }
                else if (!(v is ConstantToken)) return false;
            }
            return true;
        }

        public static void AssertCertainValueThrowIf(object sender,params IExpr[] val)
        {
            if (AssertCertainValue(val) == false) ExceptionHelper.RaiseUncertainArgument(sender);
        }
        public static void AssertCertainValueThrowIf<T>(object sender, params IExpr[] val)
        {
            if (AssertCertainValue<T>(val) == false) ExceptionHelper.RaiseUncertainArgument(sender);
        }

        /// <summary>
        /// 获取具体值的值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static object GetValue(params IExpr[] val)
        {
            return val.Select((IExpr e) => {
                return GetValue(e);
            }).ToArray();
        }

        /// <summary>
        /// 获取具体值的值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static object GetValue(IExpr e)
        {
            if (e is IHasValue) return ((e as IHasValue).Value);
            return e;
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
