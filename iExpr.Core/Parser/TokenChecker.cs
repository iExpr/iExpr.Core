using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Parser
{
    public abstract class TokenChecker
    {
        public abstract void Clear();

        public abstract string GetValue();

        public abstract bool Test(char c);
        

        /// <summary>
        /// 加入一个字符，检查是否满足条件
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public abstract bool Append(char c);

        /// <summary>
        /// 检查当前串是否满足条件
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public abstract bool? Check();
    }

    public class VariableTokenChecker : TokenChecker
    {
        StringBuilder str = new StringBuilder();
        bool? flg = null;

        public override void Clear()
        {
            str.Clear();flg = null;
        }

        public override string GetValue()
        {
            return str.ToString();
        }

        public override bool Append(char c)
        {
            flg &= Test(c);
            str.Append(c);
            return flg.Value;
        }

        public override bool? Check()
        {
            return flg;
        }

        public override bool Test(char c)
        {
            bool isv() => c == '_' || char.IsLetterOrDigit(c);
            if (flg == null)
            {
                return isv() && char.IsDigit(c) == false;
            }
            else return isv();
        }
    }

    public class OperatorTokenChecker : TokenChecker
    {
        StringBuilder str = new StringBuilder();
        bool? flg = null;
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
            str.Clear(); flg = null; current = OperationTrie;
        }

        public override string GetValue()
        {
            return str.ToString();
        }

        public override bool Append(char c)
        {
            flg &= Test(c);
            str.Append(c);
            return flg.Value;
        }

        public override bool? Check()
        {
            return flg;
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
