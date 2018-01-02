﻿using iExpr.Calculators;
using iExpr.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iExpr.Helpers
{

    
    /// <summary>
    /// 提供简单的集成环境
    /// </summary>
    public class ExprWorker
    {
        /// <summary>
        /// 解析器
        /// </summary>
        public ExprBuilder Builder { get; set; }

        /// <summary>
        /// 运算器
        /// </summary>
        public EvalEnvironment EvalEnvironment { get; set; }

        /// <summary>
        /// 变量列表
        /// </summary>
        public VariableValueProvider Variables { get => EvalEnvironment.Variables; set => EvalEnvironment.Variables = value; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="syms">符号列表</param>
        /// <param name="nullValue">空值的转换值</param>
        public ExprWorker(EnvironmentProvider syms,IExpr nullValue,Calculators.ExprEvaluator evaluator)
        {
            Builder = new ExprBuilder() { Symbols = syms };

            EvalEnvironment = new EvalEnvironment() { Evaluator = evaluator,Constants=syms.Constants };
        }

        /// <summary>
        /// 解析表达式
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public IExpr Build(string expr)
        {
            return Builder.GetExpr(expr);
        }

        public EvalContext CreateContext()
        {
            return EvalEnvironment.CreateContext();
        }

    }
}