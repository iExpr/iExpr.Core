using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace iExpr.Calculators
{
    /// <summary>
    /// 计算环境
    /// </summary>
    public class EvalEnvironment
    {
        /// <summary>
        /// 变量列表
        /// </summary>
        public VariableValueProvider Variables { get; set; } = new VariableValueProvider();

        /// <summary>
        /// 常量集合
        /// </summary>
        public ConstantValueProvider Constants { get; set; }

        /// <summary>
        /// 空值
        /// </summary>
        public IExpr NullValue { get; set; }

        /// <summary>
        /// 运算提供者
        /// </summary>
        public ExprEvaluator Evaluator { get; set; }

        public EvalContext CreateContext(CancellationTokenSource cancel=null)
        {
            var res = EvalContext.Create(cancel ?? new System.Threading.CancellationTokenSource());
            res.NullValue = NullValue;
            res.Constants = Constants;
            res.Evaluator = Evaluator;
            res.Variables = Variables;
            return res;
        }
    }
}
