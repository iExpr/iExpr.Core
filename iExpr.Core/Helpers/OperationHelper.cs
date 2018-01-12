using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Data;
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
            foreach (var v in val)
            {
                if (v is IValue)
                {
                    if (!((IValue)v).IsConstant) return false;
                }
                else if (!(v is ConstantToken)) return false;
            }
            return true;
        }

        public static bool AssertArgsNumber(int n,params IExpr[] args)
        {
            return args.Length == n;
        }

        public static void AssertArgsNumberThrowIf(int n, params IExpr[] args)
        {
            if(!AssertArgsNumber(n,args)) throw new EvaluateException("The number of arguments is wrong");
        }

        /// <summary>
        /// 判断参数是否均为具体值，且具体值的值为指定类型
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool AssertConstantValue<T>(params IExpr[] val)
        {
            foreach (var v in val)
            {
                switch (v)
                {
                    case ConcreteValue c:
                        if (c.IsConstant == false) return false;
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
        /// 获取具体值的值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T[] GetValue<T>(params IExpr[] val)
        {
            return val.Select((IExpr e) => {
                if (e is ConcreteValue) return ConcreteValueHelper.GetValue<T>(e);
                else if (e is ConstantToken) return ConcreteValueHelper.GetValue<T>((e as ConstantToken).Value);
                throw new Exceptions.UndefinedExecuteException();
                }).ToArray();
        }

        public static T GetValue<T>(IExpr e)
        {
            if (e is ConcreteValue) return ConcreteValueHelper.GetValue<T>(e);
            else if (e is ConstantToken) return ConcreteValueHelper.GetValue<T>((e as ConstantToken).Value);
            throw new Exceptions.UndefinedExecuteException();
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
