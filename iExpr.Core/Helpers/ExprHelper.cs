using iExpr.Parser;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iExpr.Helpers
{
    public class ExprHelper
    {
        public static VariableToken[] GetVariables(IExpr exp,ParseEnvironment syms=null)
        {
            switch (exp)
            {
                case ExprNode ex:
                    {
                        HashSet<VariableToken> ss = new HashSet<VariableToken>();
                        for (uint i = 0; i < ex.Children.Length; i++)
                        {
                            if (ex.Operation.SelfCalculate?.Contains(i) != true)
                                ss.UnionWith(GetVariables(ex.Children[i]));
                        }
                        return ss.ToArray();
                    }
                case VariableToken ex:
                    if(syms!=null && syms.Constants.ContainsKey(ex.ID)) return new VariableToken[] { };
                    return new VariableToken[] { ex };
                case ConcreteToken ex:
                    switch (ex.Value)
                    {
                        case CollectionValue lv:
                            HashSet<VariableToken> ss = new HashSet<VariableToken>();
                            foreach (var v in lv)
                            {
                                ss.UnionWith(GetVariables(v));
                            }
                            return ss.ToArray();
                        default:
                            break;
                    }
                    return new VariableToken[] { };
                default:
                    return new VariableToken[] { };
            }
        }
    }
}
