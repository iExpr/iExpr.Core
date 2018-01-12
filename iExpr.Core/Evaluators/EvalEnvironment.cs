using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace iExpr.Evaluators
{
    /// <summary>
    /// 计算环境
    /// </summary>
    public abstract class EvalEnvironment
    {
        /// <summary>
        /// 变量列表
        /// </summary>
        public VariableValueProvider Variables { get; set; } = new VariableValueProvider();

        /// <summary>
        /// 运算提供者
        /// </summary>
        public IExprEvaluator Evaluator { get; protected set; }

        public virtual EvalContext CreateContext(CancellationTokenSource cancel=null)
        {
            var res = EvalContext.Create(cancel ?? new System.Threading.CancellationTokenSource());
            res.Evaluator = Evaluator;
            res.Variables = Variables;
            return res;
        }
    }
}
