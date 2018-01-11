using iExpr.Helpers;
using iExpr.Parser;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr.Parser
{
    /// <summary>
    /// 符号类型
    /// </summary>
    public enum SymbolType
    {
        /// <summary>
        /// 未知
        /// </summary>
        None,
        /// <summary>
        /// 运算
        /// </summary>
        Operation,
        /// <summary>
        /// 变量
        /// </summary>
        Variable,
        /// <summary>
        /// 常量
        /// </summary>
        ConstantValue,
        /// <summary>
        /// 整体值
        /// </summary>
        UnionValue,
        //ChildExpr,
        /// <summary>
        /// 整体值指示符
        /// </summary>
        At,
        /// <summary>
        /// 整体值定界符
        /// </summary>
        UnionEdge,
        /// <summary>
        /// 逗号
        /// </summary>
        Comma,
        /// <summary>
        /// 左小括号
        /// </summary>
        LeftParentheses,
        /// <summary>
        /// 右小括号
        /// </summary>
        RightParentheses,
        /// <summary>
        /// 左中括号
        /// </summary>
        LeftBracket,
        /// <summary>
        /// 右中括号
        /// </summary>
        RightBracket,
        /// <summary>
        /// 左大括号
        /// </summary>
        LeftBrace,
        /// <summary>
        /// 右大括号
        /// </summary>
        RightBrace,
        /// <summary>
        /// 空白
        /// </summary>
        Space
    }

    public class OperationList: Dictionary<string, IOperation>
    {
        public void Add(params IOperation[] val)
        {
            foreach (var v in val)
            {
                base.Add(v.Keyword, v);
            }
        }
    }

    public class ModifierList
    {
        Dictionary<Guid, ModifierToken> gid = new Dictionary<Guid, ModifierToken>();
        Dictionary<string, Guid> sid = new Dictionary<string, Guid>();

        public void Add(params ModifierToken[] val)
        {
            foreach (var v in val)
            {
                gid.Add(v.ID, v);
                sid.Add(v.Content, v.ID);
            }
        }
    }

    public class ConstantList : Dictionary<string, ConstantToken>
    {
        //Dictionary<string, ConstantToken> sid = new Dictionary<string, ConstantToken>();

        public void Add(params ConstantToken[] val)
        {
            foreach (var v in val)
            {
                this.Add(v.ID, v);
            }
        }
    }

    /// <summary>
    /// 环境提供者
    /// </summary>
    public abstract class ParseEnvironment
    {
        /// <summary>
        /// 获取或设置所有运算
        /// </summary>
        protected OperationList Operations { get; set; }

        /// <summary>
        /// 获取所有修饰符
        /// </summary>
        public string[] Modifiers { get; set; }

        /// <summary>
        /// 获取或设置所有常量符
        /// </summary>
        public string[] Constants { get; protected set; }

        /// <summary>
        /// 运算关键字组成的字典树
        /// </summary>
        public Trie OperationTrie { get; protected set; }

        Dictionary<string, IOperation> symbols;

        /// <summary>
        /// 获取所有符号
        /// </summary>
        public IReadOnlyDictionary<string, IOperation> Symbols { get => symbols; }

        /// <summary>
        /// 自动构建字典树和符号列表
        /// </summary>
        protected void BuildOpt()
        {
            OperationTrie = new Trie(); symbols = new Dictionary<string, IOperation>();
            foreach (var v in Operations.Values)
            {
                symbols.Add(v.Keyword, v);
                Trie.Insert(v.Keyword, OperationTrie);
            }
        }

        /// <summary>
        /// 获取特殊符号类型
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public virtual SymbolType GetSpecialSymbolType(char expr)
        {
            switch (expr)
            {
                case ',':
                    return SymbolType.Comma;
                case '@':
                    return SymbolType.At;
                case '"':
                    return SymbolType.UnionEdge;
                case '(':
                    return SymbolType.LeftParentheses;
                case ')':
                    return SymbolType.RightParentheses;
                case '[':
                    return SymbolType.LeftBracket;
                case ']':
                    return SymbolType.RightBracket;
                case '{':
                    return SymbolType.LeftBrace;
                case '}':
                    return SymbolType.RightBrace;
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                    return SymbolType.Space;
                default:
                    return SymbolType.None;
            }
        }

        /// <summary>
        /// 判断一个符号是否是变量
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public virtual bool IsVariable(Symbol symbol)
        {
            if (VariableToken.IsVariable(symbol) == false) return false;
            if (Symbols.ContainsKey(symbol)) return false;
            /*if (Constants != null)
            {
                return Constants.ContainsKey(symbol) == false;
            }*/
            return true;
        }

        /// <summary>
        /// 获取识别后的值
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public abstract ExprToken GetValue(Symbol symbol);

        public virtual bool IsVariableChar(char c)
        {
            return VariableToken.IsVariableChar(c);
        }

        public virtual bool IsVariableBeginChar(char c)
        {
            return VariableToken.IsVariableBeginChar(c);
        }

        public virtual bool IsConstant(Symbol symbol)
        {
            foreach (var v in symbol.ToString()) if (IsConstantChar(v) == false) return false;
            return true;
        }

        public abstract bool IsConstantChar(char c);

        public virtual bool IsConstantBeginChar(char c)
        {
            return IsVariableBeginChar(c) == false && IsOperationBeginChar(c) == false;
        }

        public virtual bool IsOperationBeginChar(char c)
        {
            return OperationTrie.ContainsKey(c);
        }

        public virtual bool IsOperation(Symbol expr)
        {
            return Symbols.ContainsKey(expr.Value);
        }

        /*
        /// <summary>
        /// 判断指定字符是否为运算关键字的首字符
        /// </summary>
        /// <param name="begin"></param>
        /// <returns></returns>
        public virtual bool IsOperation(char begin)
        {
            return OperationTrie.ContainsKey(begin);
        }*/
        
        /// <summary>
        /// 获取识别后的整体值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public virtual IExpr GetUnionValue(string str)
        {
            if (Constants.ContainsKey(str))
            {
                var v= Constants[str];
                if (v is IExpr) return (IExpr)v;
                else return ConcreteValueHelper.BuildValue(v);
            }
            return ConcreteValueHelper.BuildValue(str);
        }

        /// <summary>
        /// 获取指定的运算
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public IOperation this[Symbol symbol]
        {
            get
            {
                if (!IsOperation(symbol))
                    throw new Exception("The symbol is not a operator.");
                return Symbols[symbol.Value];
            }
        }

    }
}
