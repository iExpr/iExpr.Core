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
                OperationHelper.AssertArgsNumberThrowIf(Plus, 2 ,args);
                OperationHelper.AssertCertainValueThrowIf(Plus,args);
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
                OperationHelper.AssertCertainValueThrowIf(Minus,args);
                if (args.Length == 2)
                {
                    var ov = cal.GetValue<ISubtractive>(args[0]);
                    var or = OperationHelper.GetValue(args[1]);
                    var res = ov.Subtract(or);
                    return res is IExpr ? (IExpr)res : new ConcreteValue(res);
                }
                else if (args.Length == 1)
                {
                    var ov = cal.GetValue<ISubtractive>(args[0]);
                    var res = ov.Negtive();
                    return res is IExpr ? (IExpr)res : new ConcreteValue(res);
                }
                else ExceptionHelper.RaiseWrongArgsNumber(Minus, 2, args.Length);
                return default;
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
                OperationHelper.AssertArgsNumberThrowIf(Multiply, 2, args);
                OperationHelper.AssertCertainValueThrowIf(Multiply,args);
                var ov = cal.GetValue<IMultiplicable>(args[0]);
                var or = OperationHelper.GetValue(args[1]);
                var res = ov.Multiply(or);
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
                OperationHelper.AssertArgsNumberThrowIf(Divide,2, args);
                OperationHelper.AssertCertainValueThrowIf(Divide, args);
                var ov = cal.GetValue<IDivisible>(args[0]);
                var or = OperationHelper.GetValue(args[1]);
                var res = ov.Divide(or);
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
               OperationHelper.AssertArgsNumberThrowIf(Mod,2, args);
               OperationHelper.AssertCertainValueThrowIf(Mod, args);
               var ov = cal.GetValue<IMouldable>(args[0]);
               var or = OperationHelper.GetValue(args[1]);
               var res = ov.Mod(or);
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
                OperationHelper.AssertArgsNumberThrowIf(Pow,2, args);
                OperationHelper.AssertCertainValueThrowIf(Pow, args);
                var ov = cal.GetValue<IPowerable>(args[0]);
                var or = OperationHelper.GetValue(args[1]);
                var res = ov.Pow(or);
                return res is IExpr ? (IExpr)res : new ConcreteValue(res);
            },
            null,
            (double)Priority.high,
            Association.Right,
            2);
    }
}
