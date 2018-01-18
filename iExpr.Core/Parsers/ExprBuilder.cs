using iExpr.Exceptions;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr.Parsers
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
            Environment = syms;
        }

        /// <summary>
        /// 获取或设置符号列表
        /// </summary>
        public ParseEnvironment Environment { get; set; }

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
                if (Symbols.IsCertain(res) == false) return null;//throw new Exception("it's not a constant");
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
            var vtc = Environment.VariableChecker;
            var btc = Environment.BasicTokenChecker;
            var otc = Environment.OperatorChecker;
            (string,SymbolType) nextItem()
            {
                otc.Clear();btc.Clear();vtc.Clear();
                int ocur = cur;
                int cnt = 3;
                StringBuilder sb = new StringBuilder();
                char c;
                while (cnt > 1 && cur<expr.Length)
                {
                    c = expr[cur];
                    if (otc.Check() != false) cnt -= otc.Append(c)==false ? 1:0;
                    if (btc.Check() != false) cnt -= btc.Append(c) == false ? 1 : 0;
                    if (vtc.Check() != false) cnt -= vtc.Append(c) == false ? 1 : 0;
                    sb.Append(c);
                    cur++;
                }
                SymbolType ret = SymbolType.None;
                if (cnt == 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    string to = otc.LastToken, tb = btc.LastToken, tv = vtc.LastToken;
                    int lo = to?.Length ?? 0, lb = tb?.Length ?? 0, lv = tv?.Length ?? 0;
                    int mx = Math.Max(lo, Math.Max(lv, lb));
                    if(mx==0)ExceptionHelper.RaiseUnrecognizedToken(sb.ToString());
                    if (lo == mx) ret = SymbolType.Operation;
                    else if (lb == mx) ret = SymbolType.BasicValue;
                    else ret = SymbolType.Variable;
                    cur--;//Attention!
                }
                else
                {
                    TokenChecker curtoken = null;
                    if (otc.Check() != false) { curtoken = otc; ret = SymbolType.Operation; }
                    else if (btc.Check() != false) { curtoken = btc; ret = SymbolType.BasicValue; }
                    else if (vtc.Check() != false) { curtoken = vtc; ret = SymbolType.Variable; }
                    while (cur < expr.Length)
                    {
                        c = expr[cur];
                        if (curtoken.Append(c)==false) break;
                        sb.Append(c);
                        cur++;
                    }
                }
                cur--;
                return (sb.ToString(), ret);
            }
            List<Symbol> sym = new List<Symbol>();

            string goUnionValue()
            {
                cur++;//跳过第一个@
                if (cur >= expr.Length) return "";
                if(Environment.GetSpecialSymbolType(expr[cur])!= SymbolType.UnionEdge)//@abc 形式
                {
                    ExceptionHelper.RaiseUnrecognizedToken(expr[cur].ToString(), "except a \" but can't find it.");
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
                    if (Environment.GetSpecialSymbolType(expr[cur]) == SymbolType.UnionEdge) break;
                    sb.Append(expr[cur]);
                    cur++;
                }
                //cur--;
                return sb.ToString();
            }

            while (cur < expr.Length)
            {
                var type = Environment.GetSpecialSymbolType(expr[cur]);
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
                                    if (Environment.Constants?.ContainsKey(s)==true)
                                    {
                                        sym.Add(new Symbol(s, SymbolType.ConstantValue, _l, cur));
                                    }
                                    else if (Environment.Modifiers?.ContainsKey(s)==true)
                                    {
                                        sym.Add(new Symbol(s, SymbolType.Modifier, _l, cur));
                                    }
                                    else sym.Add(new Symbol(s, SymbolType.Variable, _l, cur));
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
            return Environment.GetUnionValue(str);
        }

        struct Segment
        {
            public int Left { get; set; }

            public int Right { get; set; }

            public Segment(int l,int r)
            {
                Left = l;Right = r;
            }

            public Segment(int l):this(l,l)
            {
            }

            public static Segment Combine(Segment a,Segment b)
            {
                return new Segment(Math.Min(a.Left, b.Left), Math.Max(a.Right, b.Right));
            }
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
                Stack<(IExpr val, Segment s)> val = new Stack<(IExpr, Segment)>();
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

                void _pop()
                {
                    var last = opt.Pop();
                    var p = edges.Peek();
                    if (last.val.ArgumentCount != 1 && last.val.ArgumentCount != 2) throw new UndefinedExecuteException();
                    //TODO: Support ovvri.. functions
                    if (last.val.ArgumentCount == 1)//like !x but not x!
                    {
                        if(last.val.Association == Association.Left) throw new UndefinedExecuteException();
                        var k = val.Peek().s.Left;
                        if (k > p && k==last.id+1)//在运算符右侧
                        {
                            var r = val.Pop();
                            val.Push((new ExprNodeSingleOperation(last.val, r.val), new Segment(last.id,r.s.Right)));
                        }
                        else throw new UndefinedExecuteException();
                        
                    }
                    else if (last.val.ArgumentCount == 2)
                    {
                        var kl = val.Peek().s.Left;
                        if (kl > p && kl==last.id+1)
                        {
                            var r = val.Pop();
                            if (val.Count == 0)
                            {
                                val.Push((new ExprNodeSingleOperation(last.val, r.val), new Segment(last.id, r.s.Right)));
                            }
                            else if (val.Peek().s.Left > p && val.Peek().s.Right == last.id - 1)
                                {
                                    var l = val.Pop();
                                    val.Push((new ExprNodeBinaryOperation(last.val, l.val, r.val), Segment.Combine(l.s, r.s)));
                                }
                            else
                            {
                                val.Push((new ExprNodeSingleOperation(last.val, r.val), new Segment(last.id, r.s.Right)));
                            }
                        }
                        else throw new UndefinedExecuteException();
                        
                    }
                    else throw new UndefinedExecuteException();
                }

                void _var(int cur,VariableToken v)
                {
                    var p = edges.Peek();
                    List<ModifierToken> ms = new List<ModifierToken>();
                    int ik = cur;
                    while(val.Count>0)
                    {
                        (var e, var id) = val.Peek();
                        if (!(e is ModifierToken)) break;
                        if (!(id.Right!=ik-1)) break;
                        ms.Add(e as ModifierToken);
                        ik--;
                        val.Pop();
                    }
                    v.Attached = ms.ToArray();
                    val.Push((v, new Segment(ik, cur)));
                }

                void package(int cur)
                {
                    var p = edges.Peek();
                    if (opt.Count > 0)//由于最左边加了括号，那么如果没有运算符了，就意味着前面的都是组合好的val
                    {
                        while (opt.Count > 0 && opt.Peek().id > p)
                        {
                            _pop();
                        }
                    }
                    if (val.Count == 0 || val.Peek().s.Left < p)
                    {//空括号
                        //throw new ParseException("No elements.");这样会导致无参函数调用失败
                        //val.Push((BuiltinValues.Null, cur - 1));
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
                                if (leftbrs.Count == 0) ExceptionHelper.RaiseExtraBracket(s, "An unexpected right bracket.");
                                var p = leftbrs.Peek();
                                if (p.Item1 + toint(s.Type) != 10) ExceptionHelper.RaiseExtraBracket(s,"An unexpected right bracket.");

                                package(cur);//最后一段打包

                                List<IExpr> l = new List<IExpr>();
                                while (val.Count > 0 && val.Peek().s.Left >= p.Item2)
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
                                                if (id.Right == p.Item2 - 1)//函数情况下展开
                                                {
                                                    val.Pop();
                                                    //ExprFunction ef = new ExprFunction(ex);
                                                    ExprNode en = new ExprNodeCall(ex, l.ToArray());
                                                    val.Push((en, new Segment(id.Left,cur)));
                                                    flg = true;
                                                }
                                            }
                                            if (flg == false)
                                            {
                                                if (l.Count > 1)//括号里有多项（如果只有一项零项就不生成Tuple）,TODO:注意这里
                                                {
                                                    var ls = Environment.GetTupleValue();
                                                    ls.Reset(l.Select(x => x is IValue ? (IValue)x : new NativeExprValue(x)));
                                                    val.Push((ls, new Segment(p.Item2,cur)));
                                                }
                                                else
                                                {
                                                    if (l.Count == 1) val.Push((l[0], new Segment(p.Item2, cur)));
                                                }
                                            }
                                        }
                                        break;
                                    case SymbolType.RightBracket:
                                        {
                                            if (val.Count > 0)//fid[x,x,x]
                                            {
                                                (var ex, var id) = val.Peek();
                                                if (id.Right == p.Item2 - 1)//函数情况下展开
                                                {
                                                    val.Pop();
                                                    //ExprFunction ef = new ExprFunction(ex);
                                                    ExprNode en = new ExprNodeIndex(ex, l.ToArray());
                                                    val.Push((en, new Segment(id.Left, cur)));
                                                    flg = true;
                                                }
                                            }
                                            if (flg == false)
                                            {
                                                var ls = Environment.GetListValue();
                                                ls.Reset(l.Select(x => x is IValue ? (IValue)x : new NativeExprValue(x)));
                                                val.Push((ls, new Segment(p.Item2, cur)));
                                            }
                                        }
                                        break;
                                    case SymbolType.RightBrace:
                                        {
                                            if (val.Count > 0)//fid[x,x,x]
                                            {
                                                (var ex, var id) = val.Peek();
                                                if (id.Right == p.Item2 - 1)//函数情况下展开
                                                {
                                                    val.Pop();
                                                    //ExprFunction ef = new ExprFunction(ex);
                                                    ExprNode en = new ExprNodeContent(ex, l.ToArray());
                                                    val.Push((en, new Segment(id.Left, cur)));
                                                    flg = true;
                                                }
                                            }
                                            if (flg == false)
                                            {
                                                var ls = Environment.GetSetValue();
                                                ls.Reset(l.Select(x => x is IValue ? (IValue)x : new NativeExprValue(x)));
                                                val.Push((ls, new Segment(p.Item2, cur)));
                                            }
                                        }
                                        break;
                                }
                                while (edges.Count > 0 && edges.Peek() >= p.Item2) edges.Pop();
                                leftbrs.Pop();
                            }
                            break;
                        case SymbolType.Access:
                            if(val.Count == 0 || val.Peek().s.Right!=cur-1) ExceptionHelper.RaiseRelatedExpressionNotFound(s,"No pre variable for access expr.");
                            if (cur + 1 >= syms.Length) ExceptionHelper.RaiseRelatedExpressionNotFound(s, "No suffix variable for access expr.");
                            { 
                                cur++;var l = val.Pop();
                                var t = syms[cur];
                                if (t.Type != SymbolType.Variable) ExceptionHelper.RaiseUnexpectedExpression(t,"The access expr is only used by variable.");
                                val.Push((new ExprNodeAccess(l.val, new VariableToken(t)), new Segment(l.s.Left, cur)));
                            }
                            break;
                        case SymbolType.Operation:
                            {
                                var op = Environment[s];
                                if (op.ArgumentCount == 0)
                                {
                                    val.Push((new ExprNodeSingleOperation(op, null),new Segment(cur)));
                                }
                                else
                                {
                                    if (opt.Count == 0)
                                    {
                                        if (op.ArgumentCount == 1 && op.Association == Association.Left)//x!
                                        {
                                            if (val.Count == 0 || val.Peek().s.Right!=cur-1) ExceptionHelper.RaiseRelatedExpressionNotFound(s,"No left expr.");
                                            var vl = val.Pop();
                                            val.Push((new ExprNodeSingleOperation(op,vl.val), new Segment(vl.s.Left,cur)));
                                        }
                                        else opt.Push((op, cur));
                                    }
                                    else
                                    {
                                        var pp = opt.Peek();
                                        var p = edges.Peek();
                                        while (pp.id > p
                                            && (op.Priority > pp.val.Priority
                                            || op.Priority == pp.val.Priority && pp.val.Association == Association.Left))
                                        {
                                            _pop();
                                            if (opt.Count == 0) break;
                                            pp = opt.Peek();
                                        }
                                        if (op.ArgumentCount == 1 && op.Association == Association.Left)
                                        {
                                            if (val.Count == 0 || val.Peek().s.Right != cur - 1) ExceptionHelper.RaiseRelatedExpressionNotFound(s, "No left expr.");
                                            var vl = val.Pop();
                                            val.Push((new ExprNodeSingleOperation(op, vl.val), new Segment(vl.s.Left, cur)));
                                        }
                                        else opt.Push((op, cur));
                                    }
                                }
                            }
                            break;
                        case SymbolType.Variable:
                            _var(cur, new VariableToken(s));
                            break;
                        case SymbolType.ConstantValue://常量不支持修饰符
                            val.Push((Environment.Constants[s], new Segment( cur)));
                            break;
                        case SymbolType.BasicValue:
                            val.Push((Environment.GetBasicValue(s), new Segment(cur)));
                            break;
                        case SymbolType.Modifier:
                            val.Push((Environment.Modifiers[s], new Segment(cur)));
                            break;
                        case SymbolType.UnionValue:
                            val.Push((parseUnionValue(s), new Segment(cur)));
                            break;
                    }
                }
                if (val.Count > 1) ExceptionHelper.RaiseIncompleteExpression(expr, "Not a complete expr");
                if (val.Count == 0) return BuiltinValues.Null;
                return val.Pop().val;
            }
            catch(ParseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ParseException(expr,"Parsing failed.",ex);
            }
        }
    }
}
