using iExpr.Evaluators;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iExpr.Exprs.Core
{
    public class ArithmeticOperations
    {
        /// <summary>
        /// 加法
        /// </summary>
        public static Operator Plus { get; } = new Operator(
            "+",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                OperationHelper.AssertCertainValueThrowIf(args);
                var ov = cal.GetValue<IAdditive>(args);
                var res = ov[0].Add(ov[1]);
                return res is IExpr ? (IExpr)res : new ConcreteValue(res);
            },
            null,
            (double)Priority.Midium,
            Association.Left,
            2);

        /// <summary>
        /// 减法
        /// </summary>
        public static Operator Minus { get; } = new Operator(
            "-",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertCertainValueThrowIf(args);
                if (args.Length == 2)
                {
                    var ov = cal.GetValue<ISubtractive>(args);
                    var res = ov[0].Subtract(ov[1]);
                    return res is IExpr ? (IExpr)res : new ConcreteValue(res);
                }
                else if (args.Length == 1)
                {
                    var ov = cal.GetValue<ISubtractive>(args);
                    var res = ov[0].Negtive();
                    return res is IExpr ? (IExpr)res : new ConcreteValue(res);
                }
                else throw new Exceptions.EvaluateException("The number of arguments is wrong");
            },
            (IExpr[] args) =>
            {
                return string.Join("-", args.Select((IExpr exp) => Operator.BlockToString(exp)));
            },
        (double)Priority.Midium,
            Association.Left,
            2);

        /// <summary>
        /// 乘法
        /// </summary>
        public static Operator Multiply { get; } = new Operator(
            "*",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                OperationHelper.AssertCertainValueThrowIf(args);
                var ov = cal.GetValue<IMultiplicable>(args);
                var res = ov[0].Multiply(ov[1]);
                return res is IExpr ? (IExpr)res : new ConcreteValue(res);
            },
            null,
            (double)Priority.MIDIUM,
            Association.Left,
            2);

        /// <summary>
        /// 除法
        /// </summary>
        public static Operator Divide { get; } = new Operator(
            "/",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                OperationHelper.AssertCertainValueThrowIf(args);
                var ov = cal.GetValue<IDivisible>(args);
                var res = ov[0].Divide(ov[1]);
                return res is IExpr ? (IExpr)res : new ConcreteValue(res);
            },
            null,
            (double)Priority.MIDIUM,
            Association.Left,
            2);

        /// <summary>
        /// 模运算
        /// </summary>
        public static Operator Mod { get; } = new Operator(
           "%",
           (IExpr[] args, EvalContext cal) =>
           {
               OperationHelper.AssertArgsNumberThrowIf(2, args);
               OperationHelper.AssertCertainValueThrowIf(args);
               var ov = cal.GetValue<IMouldable>(args);
               var res = ov[0].Mod(ov[1]);
               return res is IExpr ? (IExpr)res : new ConcreteValue(res);
           },
           null,
           (double)Priority.MIDIUM,
           Association.Left,
           2);

        /// <summary>
        /// 乘方运算
        /// </summary>
        public static Operator Pow { get; } = new Operator(
            "**",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                OperationHelper.AssertCertainValueThrowIf(args);
                var ov = cal.GetValue<IPowerable>(args);
                var res = ov[0].Pow(ov[1]);
                return res is IExpr ? (IExpr)res : new ConcreteValue(res);
            },
            null,
            (double)Priority.high,
            Association.Right,
            2);
    }
}
