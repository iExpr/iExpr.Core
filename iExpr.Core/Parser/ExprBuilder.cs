using iExpr.Exceptions;
using iExpr.Helpers;
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
            /*string nextCons()
            {
                int s = cur;
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
            }*/
            var vtc = Symbols.VariableChecker;
            var btc = Symbols.BasicTokenChecker;
            var otc = Symbols.OperatorChecker;
            (string,SymbolType) nextItem()
            {
                otc.Clear();btc.Clear();vtc.Clear();
                int ocur = cur;
                int cnt = 3;
                StringBuilder sb = new StringBuilder();
                var c = expr[cur];
                while (cnt > 1 && cur<expr.Length)
                {
                    
                    if (otc.Check() != false) cnt -= otc.Append(c) ? 0 : 1;
                    if (btc.Check() != false) cnt -= btc.Append(c) ? 0 : 1;
                    if (vtc.Check() != false) cnt -= vtc.Append(c) ? 0 : 1;
                    sb.Append(c);
                    cur++;
                }
                if (cnt == 0) throw new ParseException(sb.ToString(), "Can't recoginize this token");
                TokenChecker curtoken = null;
                SymbolType ret = SymbolType.None;
                if (otc.Check() != false) { curtoken = otc; ret = SymbolType.Operation; }
                else if (btc.Check() != false) { curtoken = btc; ret = SymbolType.BasicValue; }
                else if(vtc.Check()!=false) { curtoken = vtc; ret = SymbolType.Variable; }
                while (cur < expr.Length)
                {
                    c = expr[cur];
                    if (!curtoken.Append(c)) break;
                    sb.Append(c);
                    cur++;
                }
                return (sb.ToString(), ret);
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

            while (cur < expr.Length)
            {
                var type = Symbols.GetSpecialSymbolType(expr[cur]);
                int _l = cur;
                switch (type)
                {
                    case SymbolType.Space:
                        {
                            /*if (sym.Count > 0)//忽视空格（a b会被理解为变量ab）
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
                            else */
                            sym.Add(new Symbol(expr[cur].ToString(), SymbolType.Space, _l, cur));
                        }
                        break;
                    case SymbolType.Comma:
                    case SymbolType.Access:
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
                    case SymbolType.None:
                        {
                            (var s, var st) = nextItem();
                            switch (st)
                            {
                                case SymbolType.Operation:
                                    sym.Add(new Symbol(s, SymbolType.Operation, _l, cur));
                                    break;
                                case SymbolType.BasicValue:
                                    sym.Add(new Symbol(s, SymbolType.BasicValue, _l, cur));
                                    break;
                                case SymbolType.Variable:
                                    if (Symbols.Constants.ContainsKey(s))
                                    {
                                        sym.Add(new Symbol(s, SymbolType.ConstantValue, _l, cur));
                                    }
                                    else if (Symbols.Modifiers.ContainsKey(s))
                                    {
                                        sym.Add(new Symbol(s, SymbolType.Modifier, _l, cur));
                                    }
                                    sym.Add(new Symbol(s, SymbolType.Variable, _l, cur));
                                    break;
                                default:
                                    throw new UndefinedExecuteException();
                            }
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

                var syms = GetSymbols(expr).Where(x=>x.Type!= SymbolType.Space).ToArray();//剔除空白，防止影响相邻的判断
                Stack<(IExpr val, int id)> val = new Stack<(IExpr, int)>();
                Stack<(IOperation val, int id)> opt = new Stack<(IOperation, int)>();
                Stack<(int,int)> leftbrs = new Stack<(int,int)>();
                Stack<int> edges = new Stack<int>();


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

                void _pop(int cur)
                {
                    var last = opt.Pop().val;
                    var p = edges.Peek();
                    IExpr l=null, r=null;
                    if (last.ArgumentCount != 1 && last.ArgumentCount != 1) throw new UndefinedExecuteException();

                    if (last.ArgumentCount == 1)
                    {
                        if (val.Peek().id > p) l = val.Pop().val;
                        if (l == null) throw new UndefinedExecuteException();
                        val.Push((new ExprNodeSingleOperation(last, l), cur));
                    }
                    else if (last.ArgumentCount == 2)
                    {
                        if (val.Peek().id > p) r = val.Pop().val;
                        if (val.Peek().id > p) l = val.Pop().val;
                        if (l == null) throw new UndefinedExecuteException();
                        if (r == null) throw new UndefinedExecuteException();
                        val.Push((new ExprNodeBinaryOperation(last, l, r), cur));
                    }
                    else throw new UndefinedExecuteException();
                }

                void _var(int cur,VariableToken v)
                {
                    var p = edges.Peek();
                    List<ModifierToken> ms = new List<ModifierToken>();
                    while(val.Count>0)
                    {
                        (var e, var id) = val.Peek();
                        if (!(id > p)) break;
                        if (!(e is ModifierToken)) break;
                        ms.Add(e as ModifierToken);
                        val.Pop();
                    }
                    v.Attached = ms.ToArray();
                    val.Push((v, cur));
                }

                void package(int cur)
                {
                    var p = edges.Peek();
                    if (opt.Count > 0)//由于最左边加了括号，那么如果没有运算符了，就意味着前面的都是组合好的val
                    {
                        while (opt.Count > 0 && opt.Peek().id > p)
                        {
                            _pop(cur - 1);//TODO: Not the cur but cur-1,The id are same!
                        }
                    }
                    if (val.Count == 0 || val.Peek().id < p)
                    {//空括号
                        val.Push((BuiltinValues.Null, cur - 1));
                    }
                }

                for (int cur = 0; cur < syms.Length; cur++)
                {
                    Symbol s = syms[cur];
                    switch (s.Type)
                    {
                        case SymbolType.Comma://最后一段整合成一个整体，相当于加了个括号，如果最后一段为空则会加入一个Null
                            {
                                package(cur);
                                edges.Push(cur);
                            }
                            break;
                        case SymbolType.LeftParentheses:
                        case SymbolType.LeftBracket:
                        case SymbolType.LeftBrace:
                            leftbrs.Push((toint(s.Type), cur)); edges.Push(cur); break;
                        case SymbolType.RightParentheses:
                        case SymbolType.RightBracket:
                        case SymbolType.RightBrace:
                            {
                                if (leftbrs.Count == 0) throw new ParseException("An unexpected right bracket.");
                                var p = leftbrs.Peek();
                                if (p.Item1 + toint(s.Type) != 10) throw new ParseException("An unexpected right bracket.");

                                package(cur);//最后一段打包

                                List<IExpr> l = new List<IExpr>();
                                while (val.Count > 0 && val.Peek().id > p.Item2)
                                {
                                    l.Add(val.Pop().val);
                                }
                                l.Reverse();
                                bool flg = false;
                                switch (s.Type)
                                {
                                    case SymbolType.RightParentheses://小括号
                                        {
                                            if (val.Count > 0)//fid(x,x,x)
                                            {
                                                (var ex, var id) = val.Peek();
                                                if (id == p.Item2 - 1)//函数情况下展开
                                                {
                                                    val.Pop();
                                                    //ExprFunction ef = new ExprFunction(ex);
                                                    ExprNode en = new ExprNodeCall(ex, l.ToArray());
                                                    val.Push((en, cur));
                                                    flg = true;
                                                }
                                            }
                                            if (flg == false)
                                            {
                                                if (l.Count > 1)//括号里有多项（如果只有一项零项就不生成Tuple）,TODO:注意这里
                                                {
                                                    val.Push((new TupleValue(l), cur));
                                                }
                                                else
                                                {
                                                    if (l.Count == 1) val.Push((l[0], cur));
                                                }
                                            }
                                        }
                                        break;
                                    case SymbolType.RightBracket:
                                        {
                                            if (val.Count > 0)//fid[x,x,x]
                                            {
                                                (var ex, var id) = val.Peek();
                                                if (id == p.Item2 - 1)//函数情况下展开
                                                {
                                                    val.Pop();
                                                    //ExprFunction ef = new ExprFunction(ex);
                                                    ExprNode en = new ExprNodeIndex(ex, l.ToArray());
                                                    val.Push((en, cur));
                                                    flg = true;
                                                }
                                            }
                                            if (flg == false) val.Push((new ListValue(l), cur));
                                        }
                                        break;
                                    case SymbolType.RightBrace:
                                        {
                                            if (val.Count > 0)//fid[x,x,x]
                                            {
                                                (var ex, var id) = val.Peek();
                                                if (id == p.Item2 - 1)//函数情况下展开
                                                {
                                                    val.Pop();
                                                    //ExprFunction ef = new ExprFunction(ex);
                                                    ExprNode en = new ExprNodeContent(ex, l.ToArray());
                                                    val.Push((en, cur));
                                                    flg = true;
                                                }
                                            }
                                            if (flg == false) val.Push((new SetValue(l), cur));
                                        }
                                        break;
                                }
                                leftbrs.Pop();
                            }
                            break;
                        case SymbolType.Access:
                            if(val.Count == 0 || val.Peek().id<edges.Peek()) throw new ParseException("No pre variable for access expr.");
                            if (cur + 1 >= syms.Length) throw new ParseException("No suffix variable for access expr.");
                            { 
                                cur++;var l = val.Pop().val;
                                var t = syms[cur];
                                if (t.Type != SymbolType.Variable) throw new ParseException("The access expr is only used by variable.");
                                val.Push((new ExprNodeAccess(l, new VariableToken(t)), cur));
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
                                    _pop(cur - 1);
                                    if (opt.Count == 0) break;
                                    pp = opt.Peek();
                                }
                                opt.Push((op, cur));
                            }
                            break;
                        case SymbolType.Variable:
                            _var(cur, new VariableToken(s));
                            break;
                        case SymbolType.ConstantValue://常量不支持修饰符
                            val.Push((Symbols.Constants[s], cur));
                            break;
                        case SymbolType.BasicValue:
                            val.Push((Symbols.GetBasicValue(s), cur));
                            break;
                        case SymbolType.Modifier:
                            val.Push((Symbols.Modifiers[s], cur));
                            break;
                        case SymbolType.UnionValue:
                            val.Push((parseUnionValue(s), cur));
                            break;
                    }
                }
                if (val.Count > 1) throw new ParseException(expr,"Not a complete expr");
                if (val.Count == 0) return BuiltinValues.Null;
                return val.Pop().val;
            }
            catch (Exception ex)
            {
                throw new ParseException("Parsing failed.",ex);
            }
        }
    }
}
