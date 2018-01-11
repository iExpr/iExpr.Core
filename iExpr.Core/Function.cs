using iExpr.Evaluators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr
{
    /// <summary>
    /// 函数
    /// </summary>
    public class Function : IOperation
    {
        public string Keyword { get; protected set; }
        public int QuantityNumber { get; protected set; } = -1;
        public Association Association { get;  } = Association.Left;
        public double Priority { get;} = 0;
        public uint[] SelfCalculate { get; protected set; }
        public bool CanPreparameter { get; protected set; }

        public Func<IExpr[], EvalContext, IExpr> EvaluateFunc { get; protected set; }

        Func<IExpr[], string> toExprString;

        public Func<IExpr[], string> ToStringFunc { get => toExprString; protected set { } }

        public IExpr Calculate(EvalContext cal,params IExpr[] exps)
        {
            /*if (QuantityNumber != -1)
            {
                if (exps.Length != QuantityNumber) throw new Exception("The number of quantitys is wrong. It should be " + QuantityNumber.ToString());
            }*/
            return EvaluateFunc.Invoke(exps,cal);
        }

        public string ToString(params IExpr[] exps)
        {
            /*if (QuantityNumber != -1)
            {
                if (exps.Length != QuantityNumber) throw new Exception("The number of quantitys is wrong. It should be " + QuantityNumber.ToString());
            }*/
            return ToStringFunc.Invoke(exps);
        }

        public Function(string keyWord,Func<IExpr[], EvalContext, IExpr> calculate,int quantityNumber=-1,Func<IExpr,string> exprStringSelector=null, uint[] selfCalculate = null, bool canprearg = false)
        {
            Keyword = keyWord;
            EvaluateFunc = calculate;
            CanPreparameter = canprearg;
            QuantityNumber = quantityNumber;
            SelfCalculate = selfCalculate;
            toExprString = new Func<IExpr[], string>((IExpr[] args) =>
            {
                return $"{Keyword}({String.Join(",", args.Select(exprStringSelector ?? ((IExpr exp) => exp.ToString())))})";
            });
        }
    }
}
