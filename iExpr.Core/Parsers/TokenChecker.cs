using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Parsers
{
    public abstract class TokenChecker
    {
        protected StringBuilder Str = new StringBuilder();

        public string LastToken { get; protected set; }

        protected bool? Flag = null;

        public virtual void Clear()
        {
            Str.Clear();
            Flag = null;
            LastToken = null;
        }

        public virtual string GetValue()
        {
            return Str.ToString();
        }

        public abstract bool? Test(char c);

        /// <summary>
        /// 加入一个字符，检查是否满足条件
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual bool? Append(char c)
        {
            var b = Test(c);
            if (Flag == true)
                if (b != true)
                    LastToken = this.GetValue();
            if (Flag == null) Flag = b;
            else Flag &= b;
            Str.Append(c);
            return Flag;
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
        public override bool? Test(char c)
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

        public override bool? Test(char c)
        {
            if (current == null) return false;
            else if (current.ContainsKey(c) == false)
            {
                return false;
            }
            else
            {
                current = current[c];
                if (current.Flag) return true;
                else return null;
            }
        }
    }

    public class RealNumberTokenChecker : TokenChecker
    {
        int pointcnt = 0;
        //bool isneg = false;

        public override void Clear()
        {
            base.Clear(); pointcnt = 0;
            //isneg = false;
        }

        public override bool? Append(char c)
        {
            var res = base.Append(c);
            if (c == '.') { pointcnt++; return null; }
            //if (c == '-') isneg = true;
            return res;
        }

        public override bool? Test(char c)
        {
            if (Flag == null)
                return char.IsDigit(c);// || c=='-';
            if (c == '.') if (pointcnt == 0) return null; else return false;
            return char.IsDigit(c);
        }
    }

    public class StringTokenChecker : TokenChecker
    {
        bool? isended = null;
        //bool isneg = false;

        public override void Clear()
        {
            base.Clear(); isended = null;
            //isneg = false;
        }

        public override bool? Append(char c)
        {
            var res = base.Append(c);
            if (c == '"')
            {
                if (isended == null)
                {
                    isended = false;
                }
                else if (isended == false)
                {
                    isended = true;
                }
            }
            else
            {
                if (Flag != true)//str starts(not null, because it has been appended)
                    isended = true;
            }
            return res;
        }

        public override bool? Test(char c)
        {
            if (Flag == null)
                return c == '"';// || c=='-';
            if (isended == true) return false;
            return true;
        }
    }
}
