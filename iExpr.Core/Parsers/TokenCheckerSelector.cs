using iExpr.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Parsers
{
    public class TokenCheckerSelector : TokenChecker
    {
        public List<TokenChecker> Checkers { get; set; } = new List<TokenChecker>();

        public override void Clear()
        {
            foreach (var v in Checkers)
            {
                v.Clear();
            }
            Flag = null;
            LastToken = null;
        }

        public override bool? Append(char c)
        {
            var r = base.Append(c);
            foreach (var x in Checkers) x.Append(c);
            return r;
        }

        public override string GetValue()
        {
            return Str.ToString();
        }

        public override bool? Test(char c)
        {
            int r = -1;
            foreach (var v in Checkers)
            {
                switch (v.Test(c))
                {
                    case null:
                        r = Math.Max(0, r);
                        break;
                    case false:
                        r = Math.Max(-1, r);
                        break;
                    case true:
                        r = Math.Max(1, r);
                        break;
                }
            }
            switch (r)
            {
                case -1: return false;
                case 0: return null;
                case 1: return true;
                default: throw new UndefinedExecuteException();
            }
        }

        /// <summary>
        /// 检查当前串是否满足条件
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public override bool? Check()
        {
            return Flag;
        }
    }
}
