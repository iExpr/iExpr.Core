using iExpr.Evaluators;
using iExpr.Exceptions;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exprs.Core
{
    public class CoreOperations
    {
        /// <summary>
        /// Build a list
        /// </summary>
        public static PreFunctionValue List { get; } = new PreFunctionValue(
            "list",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                List<IValue> ls = new List<IValue>();

                foreach (var v in args)
                {
                    switch (v)
                    {
                        case CollectionValue c:
                            ls.AddRange(c);
                            break;
                        case IValue c:
                            ls.Add(c);
                            break;
                        default:
                            throw new NotValueException();
                    }
                }
                return new ListValue(ls);
            });

        /// <summary>
        /// Build a set
        /// </summary>
        public static PreFunctionValue Set { get; } = new PreFunctionValue(
            "set",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                List<IValue> ls = new List<IValue>();

                foreach (var v in args)
                {
                    switch (v)
                    {
                        case CollectionValue c:
                            ls.AddRange(c);
                            break;
                        case IValue c:
                            ls.Add(c);
                            break;
                        default :
                            throw new NotValueException();
                    }
                }
                return new SetValue(ls);
            });

        /// <summary>
        /// Build a tuple
        /// </summary>
        public static PreFunctionValue Tuple { get; } = new PreFunctionValue(
            "tuple",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                List<IValue> ls = new List<IValue>();

                foreach (var v in args)
                {
                    switch (v)
                    {
                        case CollectionValue c:
                            ls.AddRange(c);
                            break;
                        case IValue c:
                            ls.Add(c);
                            break;
                        default:
                            throw new NotValueException();
                    }
                }
                return new TupleValue(ls);
            });

        /// <summary>
        /// Get the totle length of some objects
        /// </summary>
        public static PreFunctionValue Length { get; } = new PreFunctionValue(
            "len",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                int cnt = 0;
                foreach (var v in args)
                {
                    switch (v)
                    {
                        case CollectionValue c:
                            cnt += c.Count;
                            break;
                        default:
                            cnt++;
                            break;
                    }
                }
                return new ConcreteValue(cnt);
            });

        /// <summary>
        /// Lambda expression
        /// </summary>
        public static Operator Lambda { get; } = new Operator(
            "=>",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumber(2, args);
                var c = cal.GetChild();
                List<string> vs = new List<string>();
                if (args[0] is TupleValue)
                {
                    var _arg = (IEnumerable<IValue> )args[0];
                    foreach (VariableToken v in _arg)
                    {
                        vs.Add(v.ID);
                    }
                }
                else if (args[0] is VariableToken)
                {
                    vs.Add((args[0] as VariableToken).ID);
                }
                //throw new UncertainArgumentException();
                return new RuntimeFunctionValue(args[1], vs.ToArray());
            }, null, (double)Priority.Lowest, Association.Left, 2, OperationHelper.GetSelfCalculateAll()
            );

        public static Operator In { get; } = new Operator(
            "in",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumber(2, args);
                var item = cal.GetValue<IValue>(args[0]);var c = cal.GetValue< IContainsValue>( args[1]);
                return new ConcreteValue(c.Contains(item));
            }, (IExpr[] args)=>$"{args[0]} in {args[1]}", (double)Priority.LOWEST, Association.Left, 2
            );

        /// <summary>
        /// Check if the variable is in the environment
        /// </summary>
        public static PreFunctionValue HasVariable { get; } = new PreFunctionValue(
            "hasvar",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumber(1, args);
                return new ConcreteValue(cal.HasVariable((args[0] as VariableToken).ID));
            },1,true);

        public static PreFunctionValue Class { get; } = new PreFunctionValue(
                "class",
                (FunctionArgument _args, EvalContext cal) =>
                {
                    return new ClassValue();
                },
                -1
                );
    }
}
