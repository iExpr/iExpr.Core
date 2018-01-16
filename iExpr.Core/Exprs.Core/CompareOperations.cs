using iExpr.Evaluators;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iExpr.Exprs.Core
{
    public class CompareOperations
    {
        /// <summary>
        /// 相等
        /// </summary>
        public static Operator Equal { get; } = new Operator(
            "==",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                OperationHelper.AssertCertainValueThrowIf(args);
                var ov = cal.GetValue<object>(args);
                return new ConcreteValue(ov[0].Equals(ov[1]));
            },
            (IExpr[] args) => string.Join("==", args.Select((IExpr exp) => Operator.BlockToString(exp))),
            (double)Priority.LOW,
            Association.Left,
            2);

        /// <summary>
        /// 不等
        /// </summary>
        public static Operator Unequal { get; } = new Operator(
            "!=",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                OperationHelper.AssertCertainValueThrowIf(args);
                var ov = cal.GetValue<object>(args);
                return new ConcreteValue(!(ov[0].Equals(ov[1])));
            },
            (IExpr[] args) => string.Join("!=", args.Select((IExpr exp) => Operator.BlockToString(exp))),
            (double)Priority.LOW,
            Association.Left,
            2);

        /// <summary>
        /// 大于
        /// </summary>
        public static Operator Bigger { get; } = new Operator(
            ">",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                OperationHelper.AssertCertainValueThrowIf(args);
                var ov = cal.GetValue<IComparable>(args);
                return new ConcreteValue(ov[0].CompareTo(ov[1]) > 0);
            },
            (IExpr[] args) => string.Join(">", args.Select((IExpr exp) => Operator.BlockToString(exp))),
            (double)Priority.LOW,
            Association.Left,
            2);

        /// <summary>
        /// 小于
        /// </summary>
        public static Operator Smaller { get; } = new Operator(
            "<",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                OperationHelper.AssertCertainValueThrowIf(args);
                var ov = cal.GetValue<IComparable>(args);
                return new ConcreteValue(ov[0].CompareTo(ov[1]) < 0);
            },
            (IExpr[] args) => string.Join("<", args.Select((IExpr exp) => Operator.BlockToString(exp))),
            (double)Priority.LOW,
            Association.Left,
            2);

        /// <summary>
        /// 大于等于
        /// </summary>
        public static Operator NotSmaller { get; } = new Operator(
            ">=",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                OperationHelper.AssertCertainValueThrowIf(args);
                var ov = cal.GetValue<IComparable>(args);
                return new ConcreteValue(ov[0].CompareTo(ov[1]) >= 0);
            },
            (IExpr[] args) => string.Join(">=", args.Select((IExpr exp) => Operator.BlockToString(exp))),
            (double)Priority.LOW,
            Association.Left,
            2);

        /// <summary>
        /// 小于等于
        /// </summary>
        public static Operator NotBigger { get; } = new Operator(
            "<=",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                OperationHelper.AssertCertainValueThrowIf(args);
                var ov = cal.GetValue<IComparable>(args);
                return new ConcreteValue(ov[0].CompareTo(ov[1]) <= 0);
            },
            (IExpr[] args) => string.Join("<=", args.Select((IExpr exp) => Operator.BlockToString(exp))),
            (double)Priority.LOW,
            Association.Left,
            2);
    }
}
