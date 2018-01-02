using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr.Parser
{

    /// <summary>
    /// 代表表达式中的一个符号
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// 符号类型
        /// </summary>
        public SymbolType Type { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 在表达式中的起点
        /// </summary>
        public int StartPosition { get; set; }

        /// <summary>
        /// 在表达式中的终点
        /// </summary>
        public int EndPosition { get; set; }

        public static implicit operator string(Symbol s)
        {
            return s.Value;
        }

        public static implicit operator Symbol(string s)
        {
            return new Symbol(s);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="type">类型</param>
        /// <param name="left">起点</param>
        /// <param name="right">终点</param>
        public Symbol(string value = "",SymbolType type= SymbolType.None,int left=0,int right=0)
        {
            Value = value;
            Type = type;
            StartPosition = left;
            EndPosition = right;
        }
        
        public override string ToString()
        {
            return Value;
        }
    }
}
