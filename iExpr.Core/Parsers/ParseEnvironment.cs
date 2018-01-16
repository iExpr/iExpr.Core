using iExpr.Helpers;
using iExpr.Parsers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr.Parsers
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
        /// 基本值
        /// </summary>
        BasicValue,
        /// <summary>
        /// 整体值
        /// </summary>
        UnionValue,
        /// <summary>
        /// 修饰符
        /// </summary>
        Modifier,
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
        /// 访问符
        /// </summary>
        Access,
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
    
    public class ModifierList : Dictionary<string, ModifierToken>
    {
        //Dictionary<string, ConstantToken> sid = new Dictionary<string, ConstantToken>();

        public void Add(params ModifierToken[] val)
        {
            foreach (var v in val)
            {
                this.Add(v.Content, v);
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

        public void AddFunction(PreFunctionValue func)
        {
            this.Add(new ConstantToken(func.Keyword, func));
        }

        public void AddClassValue(PreClassValue cla)
        {
            this.Add(new ConstantToken(cla.ClassName, cla));
        }
    }

    /// <summary>
    /// 环境提供者
    /// </summary>
    public abstract class ParseEnvironment
    {
        Dictionary<string, IOperation> symbols;

        /// <summary>
        /// 获取或设置所有运算
        /// </summary>
        protected OperationList Operations { get; set; }

        /// <summary>
        /// 获取所有修饰符
        /// </summary>
        public ModifierList Modifiers { get; protected set; }

        /// <summary>
        /// 获取或设置所有常量符（始终以标识符形式表示）
        /// </summary>
        public ConstantList Constants { get; protected set; }

        public TokenChecker OperatorChecker { get; protected set; }

        public TokenChecker VariableChecker { get; protected set; }

        /// <summary>
        /// 基础元素的检验器（如数字，预定义量）
        /// </summary>
        public TokenChecker BasicTokenChecker { get; protected set; }

        /// <summary>
        /// 获取所有符号
        /// </summary>
        public IReadOnlyDictionary<string, IOperation> Symbols { get => symbols; }

        public virtual ListValueBase GetListValue()
        {
            return new ListValue();
        }

        public virtual SetValueBase GetSetValue()
        {
            return new SetValue();
        }

        public virtual TupleValueBase GetTupleValue()
        {
            return new TupleValue();
        }

        /// <summary>
        /// 自动构建字典树和符号列表
        /// </summary>
        protected void BuildOpt()
        {
            symbols = new Dictionary<string, IOperation>();
            foreach (var v in Operations.Values)
            {
                symbols.Add(v.Keyword, v);
            }
            OperatorChecker = new OperatorTokenChecker(Operations.Values);
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
                case '.':
                    return SymbolType.Access;
                case '@':
                    return SymbolType.At;
                case '$':
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
        /// 获取识别后的值
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public abstract IValue GetBasicValue(Symbol symbol);
        
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
                else return new ConcreteValue(v);
            }
            return new ConcreteValue(str);
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
                return Symbols[symbol.Value];
            }
        }

    }
}
