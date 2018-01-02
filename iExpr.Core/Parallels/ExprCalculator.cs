using System;
using System.Collections.Generic;
using System.Text;
using iExpr.Calculators;
using iExpr.Helpers;

namespace iExpr.Parallels
{
    /// <summary>
    /// 单变量计算环境
    /// </summary>
    public class OneVariableExprCalculator : Calculators.ExprEvaluator
    {
        /*public static OneVariableExprCalculator GetFromExprCalculator(ExprCalculator cal,string vid=null)
        {
            if (cal is OneVariableExprCalculator) return (OneVariableExprCalculator)cal.GetChild();
            return new OneVariableExprCalculator() { NullValue = cal.NullValue, Parent = cal };
        }*/

        object val;
        IExpr exprVal;

        /// <summary>
        /// 变量值
        /// </summary>
        public object Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
                if (val is IExpr)
                    exprVal = Evaluate((IExpr)val);

                else exprVal = ConcreteValueHelper.BuildValue(val);
            }
        }

        public OneVariableExprCalculator():base(false)
        {
            //Variables = null;
        }

        /*public override Calculators.ExprCalculator GetChild()
        {
            return new OneVariableExprCalculator() { NullValue = this.NullValue,Value=this.Value, Parent = this };
        }

        protected override IExpr CalculateVariable(VariableValue expr)
        {
            return exprVal;
        }*/
    }
}
