using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exceptions
{

    public class EvaluateException : ExprException
    {

        public EvaluateException() : base()//调用基类的构造器
        {
        }
        public EvaluateException(string message) : base(message)//调用基类的构造器
        {
        }
        public EvaluateException(string message, Exception innerException) : base(message, innerException)//调用基类的构造器
        {
        }

        public override string Message
        {
            get
            {
                return $"Expr evaluating failed: {base.Message}.";  
            }
        }
    }
}
