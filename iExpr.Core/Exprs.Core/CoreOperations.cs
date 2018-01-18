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
        public static List<IValue> BuildValueList(IExpr[] args)
        {
            List<IValue> ls = new List<IValue>();

            foreach (var v in args)
            {
                switch (v)
                {
                    case IEnumerableValue c:
                        foreach (var x in c)
                        {
                            ls.Add(x);
                        }
                        break;
                    case IHasValue c:
                        if (c.Value is IEnumerableValue)
                        {
                            foreach (var x in (c.Value as IEnumerableValue))
                            {
                                ls.Add(x);
                            }
                        }
                        else if (c is IValue) ls.Add((IValue)c);
                        else ExceptionHelper.RaiseNotValue(List, v);
                        break;
                    case IValue c:
                        ls.Add(c);
                        break;
                    default:
                        ExceptionHelper.RaiseNotValue(List, v);
                        break;
                }
            }
            return ls;
        }

        /// <summary>
        /// Build a list
        /// </summary>
        public static PreFunctionValue List { get; } = new PreFunctionValue(
            "list",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                
                return new ListValue(BuildValueList(args));
            });

        /// <summary>
        /// Build a set
        /// </summary>
        public static PreFunctionValue Set { get; } = new PreFunctionValue(
            "set",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                
                return new SetValue(BuildValueList(args));
            });

        /// <summary>
        /// Build a tuple
        /// </summary>
        public static PreFunctionValue Tuple { get; } = new PreFunctionValue(
            "tuple",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                
                return new TupleValue(BuildValueList(args));
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
                        case ICountableValue c:
                            cnt += c.Count;
                            break;
                        case IHasValue c:
                            if (c.Value is ICountableValue)
                            {
                                cnt += (c.Value as ICountableValue).Count;
                            }
                            else cnt++;
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
                    var res= new ClassValue(cal);
                    var children = _args.Contents;
                    foreach (var x in children)
                    {
                        cal.Evaluate(x);
                    }
                    return res;
                }
                );

        public static PreFunctionValue Iterator { get; } = new PreFunctionValue(
                "iter",
                (FunctionArgument _args, EvalContext cal) =>
                {
                    if (_args.Arguments != null && _args.Arguments.Length>0)
                    {
                        var args = _args.Arguments;
                        OperationHelper.AssertArgsNumberThrowIf(Iterator, 1, args);
                        var v = cal.GetValue<IEnumerable<IValue>>(args[0]);
                        return new PreEnumeratorValue(v);
                    }
                    else
                    {
                        var res = new EnumeratorValue( new ConcreteValue(null), new ConcreteValue(null),cal);
                        var children = _args.Contents;
                        
                        foreach (var x in children)
                        {
                            cal.Evaluate(x);
                        }
                        var next = cal.GetValue<FunctionValue>(res.Next);
                        //使Next无法被修改
                        res.Next = ClassValueBuilder.BuildFunction(
                           (x, y) => next.Call(x, cal), "next",0);
                        return res;
                    }
                },1
                );
    }
}
