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
    }
}
