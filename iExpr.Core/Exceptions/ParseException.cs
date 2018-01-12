using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exceptions
{
    public class ParseException : ExprException
    {

        public ParseException() : base()//调用基类的构造器
        {
        }
        public ParseException(string message,string expr=null) : base(message)//调用基类的构造器
        {
            FailedExpr = expr;
        }
        public ParseException(string message, Exception innerException,string expr=null) : base(message, innerException)//调用基类的构造器
        {
            FailedExpr = expr;
        }

        public string FailedExpr { get; set; }

        public override string Message
        {
            get
            {
                return $"Expr parse failed: {base.Message}. expr: {FailedExpr}";
            }
        }
    }
}
