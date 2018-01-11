using iExpr.Helpers;
using iExpr.Operations;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr.Parser
{
    /// <summary>
    /// 表达式解析器
    /// </summary>
    public class ExprBuilder
    {
        public ExprBuilder() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syms">符号列表</param>
        public ExprBuilder(ParseEnvironment syms)
        {
            Symbols = syms;
        }

        /// <summary>
        /// 获取或设置符号列表
        /// </summary>
        public ParseEnvironment Symbols { get; set; }

        /// <summary>
        /// 生成符号解析
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public Symbol[] GetSymbols(string expr)
        {
            if (String.IsNullOrEmpty(expr))
            {
                return new Symbol[] { };
            }
            int cur = 0;
            string nextCons()
            {
                int s = cur;
                StringBuilder sb = new StringBuilder();
                while (s < expr.Length)
                {
                    if (!Symbols.IsConstantChar(expr[s])) break;
                    //ans = ans * 10 + Convert.ToInt32(expr[s]) - Convert.ToInt32('0');
                    sb.Append(expr[s]);
                    s++;
                }
                cur = s - 1;
                var res=sb.ToString();
                if (Symbols.IsConstant(res) == false) return null;//throw new Exception("it's not a constant");
                return res;
            }
            string nextVal()
            {
                int s = cur;
                StringBuilder sb = new StringBuilder();
                while (s < expr.Length)
                {
                    if (!Symbols.IsVariableChar(expr[s])) break;
                    //ans = ans * 10 + Convert.ToInt32(expr[s]) - Convert.ToInt32('0');
                    sb.Append(expr[s]);
                    s++;
                }
                cur = s-1;

                var res = sb.ToString();
                if (Symbols.IsVariable(res) == false) return null;// throw new Exception("it's not a var");
                return res;
            }
            string nextOpt()
            {
                string ans = ""; int s = cur;
                Trie tc = Symbols.OperationTrie;
                while (s < expr.Length)
                {
                    if (Symbols.GetSpecialSymbolType(expr[s]) != SymbolType.None || !tc.ContainsKey(expr[s])) break;
                    tc = tc[expr[s]];
                    ans = ans + expr[s];
                    s++;
                }
                cur = s-1;
                if (Symbols.IsOperation(ans) == false) return null;// throw new Exception("it's not a opt");
                return ans;
            }
            (string,SymbolType) nextItem()
            {
                int ocur = cur;
                string s = null;
                if(Symbols.IsConstantBeginChar(expr[cur]))s=nextCons();
                if (s != null) return (s,SymbolType.ConstantValue);
                cur = ocur;
                if (Symbols.IsOperationBeginChar(expr[cur])) s = nextOpt();
                if (s != null) return (s, SymbolType.Operation);
                cur = ocur;
                if (Symbols.IsVariableBeginChar(expr[cur])) s = nextVal();
                if (s != null) return (s, SymbolType.Variable);

                
                throw new Exception("no item");
            }
            bool isopt()
            {
                return !Symbols.IsVariableChar(expr[cur]);
            }
            List<Symbol> sym = new List<Symbol>();

            string goUnionValue()
            {
                cur++;//跳过第一个@
                if (cur >= expr.Length) return "";
                if(Symbols.GetSpecialSymbolType(expr[cur])!= SymbolType.UnionEdge)//@abc 形式
                {
                    throw new Exception("except a \" but can't find it.");
                    //return nextVal();
                }
                //@"abc" 形式
                cur++;//跳过第一个"
                StringBuilder sb = new StringBuilder();
                while (cur < expr.Length)//如果没能匹配就跑到最后
                {
                    /*
                    switch (Symbols.GetSpecialSymbolType(expr[cur]))
                    {
                        case SymbolType.LeftBracket:
                            left++; break;
                        case SymbolType.RightBracket:
                            left--; break;
                        default:
                            break;
                    }
                    if (left == 0) { cur++; break; }//跳过最后一个]*/
                    if (Symbols.GetSpecialSymbolType(expr[cur]) == SymbolType.UnionEdge) break;
                    sb.Append(expr[cur]);
                    cur++;
                }
                //cur--;
                return sb.ToString();
            }

            /*string goChildExpr()
            {
                int left = 1; cur++;
                StringBuilder sb = new StringBuilder();
                while (cur < expr.Length)//如果没能匹配就跑到最后
                {
                    switch (Symbols.GetSpecialSymbolType(expr[cur]))
                    {
                        case SymbolType.LeftBrace:
                            left++; break;
                        case SymbolType.RightBrace:
                            left--; break;
                        default:
                            break;
                    }
                    if (left == 0) break;
                    sb.Append(expr[cur]);
                    cur++;
                }
                //if(cur<expr.Length) ;//指向最后一个}
                return sb.ToString();
            }*/

            while (cur < expr.Length)
            {
                var type = Symbols.GetSpecialSymbolType(expr[cur]);
                int _l = cur;
                switch (type)
                {
                    case SymbolType.Space://忽视空格（a b会被理解为变量ab）
                        {
                            if (sym.Count > 0)
                            {
                                var lst = sym[sym.Count - 1];
                                if (lst.Type == SymbolType.Space)
                                {
                                    //上一个如果是常量则这个一定是变量，因为常量会自动延伸至非常量
                                    lst.Type = SymbolType.Variable;
                                    lst.Value += expr[cur];
                                    lst.EndPosition = cur;
                                    break;
                                }
                            }
                            else sym.Add(new Symbol(expr[cur].ToString(), SymbolType.Space, _l, cur));
                        }
                        break;
                    case SymbolType.Comma:
                    case SymbolType.LeftParentheses:
                    case SymbolType.RightParentheses:
                    case SymbolType.LeftBracket:
                    case SymbolType.RightBracket:
                    case SymbolType.LeftBrace:
                    case SymbolType.RightBrace:
                        sym.Add(new Symbol(expr[cur].ToString(), type,_l,cur));
                        
                        break;
                    case SymbolType.At:
                        {
                            string s = goUnionValue();
                            sym.Add(new Symbol(s, SymbolType.UnionValue, _l, cur));
                        }
                        break;
                    /*
                        throw new Exception("An unexpected right bracket.");
                        throw new Exception("An unexpected left brace.");
                        throw new Exception("An unexpected right brace.");*/
                    case SymbolType.None:
                        {

                            (var s, var st) = nextItem();
                            if(st== SymbolType.Operation)
                            {
                                sym.Add(new Symbol(s, SymbolType.Operation, _l, cur));
                                break;
                            }
                            //bool isv = st == SymbolType.Variable;
                            /*if (sym.Count > 0)
                            {
                                var lst = sym[sym.Count - 1];
                                if (lst.Type == SymbolType.Variable || lst.Type == SymbolType.ConstantValue)
                                {
                                    //TODO: Go on!
                                    //上一个如果是常量则这个一定是变量，因为常量会自动延伸至非常量
                                    lst.Type = SymbolType.Variable;
                                    lst.Value += s;
                                    lst.EndPosition = cur;
                                    break;
                                }
                            }*/
                            sym.Add(new Symbol(s, st,_l,cur));
                        }
                
                        break;
                }
                cur++;
            }
            return sym.ToArray();
        }

        bool _isedged(string s, string l,string r)
        {
            return s.StartsWith(l) && s.EndsWith(r);
        }


        IExpr parseUnionValue(string str)
        {
            #region old list parse
            /*
            if (_isedged(str, "[","]"))
            {
                ListValue l = new ListValue();
                str = str.Substring(1, str.Length - 2);
                var ls = str.Split(',');
                if (ls.Length > 0)
                {
                    l.Contents = new List<IExpr>();
                    foreach (var v in ls)
                    {
                        l.Contents.Add(GetExpr(v));
                    }
                }
                return ConcreteValueHelper.BuildValue(l);
            }
            else if (_isedged(str, "{", "}"))
            {
                SetValue l = new SetValue();
                str = str.Substring(1, str.Length - 2);
                var ls = str.Split(',');
                if (ls.Length > 0)
                {
                    l.Contents = new HashSet<IExpr>();
                    foreach (var v in ls)
                    {
                        l.Contents.Add(GetExpr(v));
                    }
                }
                return ConcreteValueHelper.BuildValue(l);
            }
            else if (_isedged(str, "(", ")"))
            {
                str = str.Substring(1, str.Length - 2);
                var ls = str.Split(',');
                
                if (ls.Length > 0)
                {
                    List<IExpr> l = new List<IExpr>();
                    foreach (var v in ls)
                    {
                        l.Add(GetExpr(v));
                    }
                    return ConcreteValueHelper.BuildValue(new TupleValue(l));
                }
                else return ConcreteValueHelper.BuildValue(new TupleValue());
            }*/
#endregion
            return Symbols.GetUnionValue(str);
        }

        /// <summary>
        /// 解析表达式
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public IExpr GetExpr(string expr)
        {
            try
            {
                if (String.IsNullOrEmpty(expr))
                {
                    return BuiltinValues.Null;
                }
                expr = $"({expr})";

                var syms = GetSymbols(expr);
                Stack<(IExpr val, int id)> val = new Stack<(IExpr, int)>();
                Stack<(IOperation val, int id)> opt = new Stack<(IOperation, int)>();
                Stack<(int,int)> leftbrs = new Stack<(int,int)>();

                int toint(SymbolType type)
                {
                    switch (type)
                    {
                        case SymbolType.LeftParentheses:
                            return 1;
                        case SymbolType.RightParentheses:
                            return 9;
                        case SymbolType.LeftBracket:
                            return 2;
                        case SymbolType.RightBracket:
                            return 8;
                        case SymbolType.LeftBrace:
                            return 3;
                        case SymbolType.RightBrace:
                            return 7;
                        default:
                            return 0;
                    }
                }

                void _pop(int cur, bool isallbra = false)
                {
                    var last = opt.Pop().val;
                    var p = leftbrs.Peek().Item2;
                    List<IExpr> exp = new List<IExpr>();
                    if (isallbra == false && last.QuantityNumber > -1)
                    {
                        while (exp.Count < last.QuantityNumber && val.Count > 0 && val.Peek().id > p)
                        {
                            exp.Add(val.Pop().val);
                        }
                    }
                    else
                    {
                        while (val.Count > 0 && val.Peek().id >p)
                        {
                            exp.Add(val.Pop().val);
                        }
                    }
                    exp.Reverse();
                    if (last.QuantityNumber != -1 && exp.Count > last.QuantityNumber)
                        exp.RemoveRange(last.QuantityNumber, exp.Count - last.QuantityNumber);
                    var trn = new ExprNode(last, exp.ToArray());
                    val.Push((trn, cur));
                }

                for (int cur = 0; cur < syms.Length; cur++)
                {
                    Symbol s = syms[cur];
                    switch (s.Type)
                    {
                        case SymbolType.Comma://最后一段整合成一个整体，相当于加了个括号，如果最后一段为空则会加入一个Null
                            {
                                var p = leftbrs.Peek();
                                if (opt.Count > 0)//由于最左边加了括号，那么如果没有运算符了，就意味着前面的都是组合好的val
                                {
                                    while (opt.Count > 0 && opt.Peek().id > p.Item2)
                                    {
                                        _pop(cur-1);//TODO: Not the cur but cur-1,The id are same!
                                    }
                                }
                                if(val.Count==0 || val.Peek().id<p.Item2){//空括号
                                    val.Push((BuiltinValues.Null, cur-1));
                                }
                            }
                            break;
                        case SymbolType.LeftParentheses:
                        case SymbolType.LeftBracket:
                        case SymbolType.LeftBrace:
                            leftbrs.Push((toint(s.Type),cur)); break;
                        case SymbolType.RightParentheses:
                        case SymbolType.RightBracket:
                        case SymbolType.RightBrace:
                            {
                                if (leftbrs.Count == 0) throw new Exception("An unexpected right bracket.");
                                var p = leftbrs.Peek();
                                if(p.Item1+toint(s.Type)!=10) throw new Exception("An unexpected right bracket.");

                                if (opt.Count > 0)
                                {
                                    while (opt.Count > 0 && opt.Peek().id > p.Item2)//Modify！
                                    {
                                        _pop(cur);//The id are same!
                                    }
                                }
                                switch (s.Type)
                                {
                                    case SymbolType.RightParentheses://小括号
                                        {
                                            bool flg = false;
                                            if (opt.Count > 0)
                                            {
                                                (var v, var id) = opt.Peek();
                                                if (id == p.Item2 - 1 && v is Function)//函数情况下展开
                                                {
                                                    _pop(cur, true); flg = true;
                                                }
                                            }
                                            if (flg == false)
                                            {
                                                
                                                List<IExpr> l = new List<IExpr>();
                                                while (val.Count > 0 && val.Peek().id > p.Item2)
                                                {
                                                    l.Add(val.Pop().val);
                                                }
                                                l.Reverse();
                                                if (val.Count > 0)//fid(x,x,x)
                                                {
                                                    (var ex,var id) = val.Peek();
                                                    if (id == p.Item2 - 1)//函数情况下展开
                                                    {
                                                        val.Pop();
                                                        ExprFunction ef = new ExprFunction(ex);
                                                        ExprNode en = new ExprNode(ef, l.ToArray());
                                                        val.Push((en, cur));
                                                        flg = true;
                                                    }
                                                }
                                                if(flg==false)
                                                {
                                                    if (l.Count > 1)//括号里有多项（如果只有一项或零项就不生成Tuple）
                                                    {
                                                        val.Push((ConcreteValueHelper.BuildValue(new TupleValue(l)), cur));
                                                    }
                                                    else
                                                    {
                                                        if (l.Count == 1) val.Push((l[0], cur));
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case SymbolType.RightBracket:
                                        {
                                            List<IExpr> l = new List<IExpr>();
                                            while (val.Count > 0 && val.Peek().id > p.Item2)
                                            {
                                                l.Add(val.Pop().val);
                                            }
                                            l.Reverse();
                                            bool flg = false;
                                            /*if (val.Count > 0)//fid[x,x,x]
                                            {
                                                (var ex, var id) = val.Peek();
                                                if (id == p.Item2 - 1 && ex is VariableToken)//函数情况下展开
                                                {
                                                    var v = ((VariableToken)val.Peek().val);
                                                    v.Attached = l.ToArray();
                                                    v.Type = VariableType.Index;
                                                    flg = true;
                                                }
                                            }*/
                                            if(flg==false)val.Push((ConcreteValueHelper.BuildValue(new ListValue(l)), cur));
                                        }
                                        break;
                                    case SymbolType.RightBrace:
                                        {
                                            SetValue l = new SetValue
                                            {
                                                Contents = new HashSet<IExpr>()
                                            }; while (val.Count > 0 && val.Peek().id > p.Item2)
                                            {
                                                l.Contents.Add(val.Pop().val);
                                            }
                                            val.Push((ConcreteValueHelper.BuildValue(l), cur));
                                        }
                                        break;
                                }
                                leftbrs.Pop();
                            }
                            break;
                        case SymbolType.Operation:
                            if (opt.Count == 0) opt.Push((Symbols[s], cur));
                            else
                            {
                                var op = Symbols[s];
                                var pp = opt.Peek();
                                var p = leftbrs.Peek();
                                while (pp.id > p.Item2
                                    && (op.Priority > pp.val.Priority
                                    || op.Priority == pp.val.Priority && pp.val.Association == Association.Left))
                                {
                                    _pop(cur-1);
                                    if (opt.Count == 0) break;
                                    pp = opt.Peek();
                                }
                                opt.Push((op, cur));
                            }
                            break;
                        case SymbolType.Variable:
                        case SymbolType.ConstantValue:
                            val.Push((Symbols.GetValue(s), cur));
                            break;
                        case SymbolType.UnionValue:
                            val.Push((parseUnionValue(s), cur));
                            break;
                        /*case SymbolType.ChildExpr:
                            val.Push((new ChildExprValue(s.Value), cur));
                            break;*/
        }
                }
                if (val.Count > 1) throw new Exception("Not a complete expr");
                if (val.Count == 0) return BuiltinValues.Null;
                return val.Pop().val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
