using iExpr.Calculators;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iExpr.Operations
{
    public static class CoreOperations
    {
        
        public static Operator Dot { get; } = new Operator(
            ".",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumber(2, args);
                switch (args[0])
                {
                    case ConcreteToken ct1:
                        switch (args[1])
                        {
                            case ConcreteToken ct2:
                                
                                /*if (OperationHelper.AssertConstantValue(ct1, ct2))
                                {
                                    long pn = OperationHelper.GetConcreteValue<long>(ct1)[0];
                                    double mn = OperationHelper.GetConcreteValue<double>(ct2)[0];
                                    while (mn >= 1) mn /= 10;
                                    return ConcreteValueHelper.BuildValue(pn + mn);
                                }*/
                                    //TODO: 暂不实现对list的访问
                                    /*else if(ct1.Value is CollectionValue && ct2.Value is int)
                                    {
                                        int ind = (int)ct2.Value;
                                        switch (ct1.Value)
                                        {
                                            case ListValue l:
                                                return l[ind];
                                            case TupleValue t:
                                                return t[ind];
                                            default:
                                                throw new Exception("No index function");
                                        }
                                    }*/
                                break;
                            case ExprNode en:
                                if (en.Operation.CanPreparameter == false) throw new Exception("Can't call the function");
                                return cal.Evaluate(new ExprNode(en.Operation, ct1, en.Children));

                            /*case VariableToken vt:
                                if (vt.Type == VariableType.Function)
                                {
                                    return cal.Evaluate(new VariableToken(vt.ID, ct1, vt.Attached) { Type = VariableType.Function });
                                }
                                break;*/
                        }
                        break;

                }
                return args[0];
            }, null, (double)Priority.HIGH, Association.Left, 2, OperationHelper.GetSelfCalculate(1)
            );

        public static Function List { get; } = new Function(
            "list",
            (IExpr[] args, EvalContext cal) =>
            {
                List<IExpr> ls = new List<IExpr>();

                foreach (var v in args)
                {
                    switch (v)
                    {
                        case ConcreteToken c:
                            if (c.Value is CollectionValue)
                            {
                                ls.AddRange((c.Value as CollectionValue));
                            }
                            else ls.Add(c);
                            break;
                        default:
                            ls.Add(v);
                            break;
                    }
                }
                return ConcreteValueHelper.BuildValue(new ListValue(ls));
            },
            -1, null
            );

        public static Function Set { get; } = new Function(
            "set",
            (IExpr[] args, EvalContext cal) =>
            {
                List<IExpr> ls = new List<IExpr>();

                foreach (var v in args)
                {
                    switch (v)
                    {
                        case ConcreteToken c:
                            if (c.Value is CollectionValue)
                            {
                                ls.AddRange((c.Value as CollectionValue));
                            }
                            else ls.Add(c);
                            break;
                        default:
                            ls.Add(v);
                            break;
                    }
                }
                return ConcreteValueHelper.BuildValue(new SetValue(ls));
            },
            -1, null
            );

        public static Function Tuple { get; } = new Function(
            "tuple",
            (IExpr[] args, EvalContext cal) =>
            {
                List<IExpr> ls = new List<IExpr>();

                foreach (var v in args)
                {
                    switch (v)
                    {
                        case ConcreteToken c:
                            if (c.Value is CollectionValue)
                            {
                                ls.AddRange((c.Value as CollectionValue));
                            }
                            else ls.Add(c);
                            break;
                        default:
                            ls.Add(v);
                            break;
                    }
                }
                return ConcreteValueHelper.BuildValue(new TupleValue(ls));
            },
            -1, null
            );

        /// <summary>
        /// 元素个数
        /// </summary>
        public static Function Length { get; } = new Function(
            "len",
            (IExpr[] args, EvalContext cal) =>
            {
                    /*if (!OperationHelper.AssertConstantValue(args))
                        return new ExprNode(Length, args);*/

                    //var vs = OperationHelper.GetConcreteValue<double>(args);
                    int cnt = 0;
                foreach (var v in args)
                {
                    switch (v)
                    {
                        case ConcreteToken c:
                            if (c.Value is CollectionValue)
                            {
                                cnt += ((c.Value as CollectionValue)).Count;
                            }
                            else cnt++;
                            break;
                        default:
                            cnt++;
                            break;
                    }
                }
                return ConcreteValueHelper.BuildValue(cnt);
            },
            -1, null, null, true
            );

        public static Operator Lambda { get; } = new Operator(
            "=>",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumber(2, args);
                var c = cal.GetChild();
                if (args[0] is ConcreteToken)
                {
                    var _arg = args[0] as ConcreteToken;
                    foreach (VariableToken v in (_arg.Value as CollectionValue))
                    {
                        c.IgnoreVariables.Add(v.ID);
                    }
                }
                else if (args[0] is VariableToken)
                {
                    c.IgnoreVariables.Add((args[0] as VariableToken).ID);
                }
                ExprValue ev = new ExprValue
                {
                        /*if (args[2] is ExprNode && (args[2] as ExprNode).Operation == Execute)
                        {
                            ev.Expr = args[2];
                        }
                        else
                        {
                            ev.Expr = c.Evaluate(args[2]);
                        }*/
                    Expr = args[1],
                    VariableNames = c.IgnoreVariables.ToArray()
                };
                return ConcreteValueHelper.BuildValue(ev);
            }, null, (double)Priority.Lowest, Association.Left, 2, OperationHelper.GetSelfCalculateAll()
            );

        public static Function Value { get; } = new Function(
            "val",
            (IExpr[] args, EvalContext cal) =>
            {
                if (OperationHelper.AssertArgsNumber(2, args))
                {
                    args[0] = cal.Evaluate(args[0]);
                    if (args[0] is ConcreteToken)
                    {
                        try
                        {
                            var c = (args[0] as ConcreteToken).Value;
                            int ind = OperationHelper.GetConcreteValue<int>(args[1])[0];
                            switch (c)
                            {
                                case ListValue l: return l[ind];
                                case TupleValue l: return l[ind];
                                case SetValue s: throw new Exception("The set can't index.");
                                default: throw new Exception("We only support list.");
                            }
                        }
                        catch
                        {
                            return new ExprNode(Value, args);
                        }
                    }
                    else throw new Exception("The expr can't be calculated.");
                }
                else if (OperationHelper.AssertArgsNumber(3, args))
                {
                    if (args[0] is VariableToken)
                    {
                        try
                        {
                            var c = ((cal.GetVariableValue((args[0] as VariableToken).ID)) as ConcreteToken).Value;
                            int ind = OperationHelper.GetConcreteValue<int>(args[1])[0];
                            switch (c)
                            {
                                case ListValue l: l[ind] = args[2]; break;
                                case TupleValue l: l[ind] = args[2]; break;
                                case SetValue s: throw new Exception("The set can't index.");
                                default: throw new Exception("We only support list.");
                            }
                            return ConcreteToken.Null;
                        }
                        catch
                        {
                            return new ExprNode(Value, args);
                        }
                    }
                    else throw new Exception("We only support variable");
                }
                else
                {
                    throw new Exception("The args is not correct.");
                }
            },
            3, null, OperationHelper.GetSelfCalculate(0)
            );
    }
}
