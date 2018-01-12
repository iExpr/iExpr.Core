using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Parser
{
    public abstract class TokenChecker
    {
        protected StringBuilder Str = new StringBuilder();

        protected bool? Flag = null;

        public virtual void Clear()
        {
            Str.Clear();
            Flag = null;
        }

        public virtual string GetValue()
        {
            return Str.ToString();
        }

        public abstract bool Test(char c);

        /// <summary>
        /// 加入一个字符，检查是否满足条件
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual bool Append(char c)
        {
            if (Flag == null) Flag = Test(c);
            else Flag &= Test(c);
            Str.Append(c);
            return Flag.Value;
        }

        /// <summary>
        /// 检查当前串是否满足条件
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual bool? Check()
        {
            return Flag;
        }
    }

    public class VariableTokenChecker : TokenChecker
    {
        public override bool Test(char c)
        {
            bool isv() => c == '_' || char.IsLetterOrDigit(c);
            if (Flag == null)
            {
                return isv() && char.IsDigit(c) == false;
            }
            else return isv();
        }
    }

    public class OperatorTokenChecker : TokenChecker
    {
        Trie current = null;

        /// <summary>
        /// 运算关键字组成的字典树
        /// </summary>
        protected Trie OperationTrie { get; set; }

        public OperatorTokenChecker(IEnumerable<IOperation> opts)
        {
            OperationTrie = new Trie();
            foreach (var v in opts)
            {
                Trie.Insert(v.Keyword, OperationTrie);
            }
            current = OperationTrie;
        }

        public override void Clear()
        {
            base.Clear(); current = OperationTrie;
        }

        public override bool Test(char c)
        {
            if (current == null) return false;
            else if (current.ContainsKey(c) == false)
            {
                return false;
            }
            else
            {
                current = current[c];
                return true;//attention to flg==null
            }
        }
    }
}
