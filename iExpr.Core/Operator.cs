using iExpr.Evaluators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr
{
    /// <summary>
    /// 运算符
    /// </summary>
    public class Operator : IOperation
    {
        /// <summary>
        /// 转换成表达式字符串，自动判断是否加入括号
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string BlockToString(IExpr v)
        {
            if (v is ExprToken) return v.ToString();
            else return $"({v.ToString()})";
        }

        public string Keyword { get; private set; }
        public int ArgumentCount { get; private set; }
        public Association Association { get; private set; }
        public double Priority { get; private set; }
        public uint[] SelfCalculate { get; private set; }

        public Func<IExpr[], EvalContext, IExpr> EvaluateFunc { get; private set; }
        public Func<IExpr[], string> ToStringFunc { get; private set; }

        public Operator(string keyWord, Func<IExpr[], EvalContext, IExpr> calculate, Func<IExpr[], string> toexprString = null, double priority = 0, Association association = Association.Left, int quantityNumber = -1, uint[] selfCalculate = null)
        {
            Keyword = keyWord;
            EvaluateFunc = calculate;
            if (toexprString != null)
            {
                ToStringFunc = toexprString;
            }
            else
            {
                ToStringFunc = (IExpr[] args) => string.Join(Keyword, args.Select((IExpr exp) => Operator.BlockToString(exp)));
            }
            Priority = priority;
            ArgumentCount = quantityNumber;
            Association = association;
            SelfCalculate = selfCalculate;

        }

        public IExpr Calculate(EvalContext cal, IExpr[] exps)
        {
            /*if (QuantityNumber != -1)
            {
                if (exps.Length != QuantityNumber) throw new Exception("The number of quantitys is wrong. It should be " + QuantityNumber.ToString());
            }*/
            return EvaluateFunc.Invoke(exps,cal);
        }

        public string ToString(IExpr[] exps)
        {
            /*if (QuantityNumber != -1)
            {
                if (exps.Length != QuantityNumber) throw new Exception("The number of quantitys is wrong. It should be " + QuantityNumber.ToString());
            }*/
            return ToStringFunc.Invoke(exps);
        }
    }
}
