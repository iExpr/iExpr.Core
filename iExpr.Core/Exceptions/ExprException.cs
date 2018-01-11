using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exceptions
{
    public abstract class ExprException: Exception
    {
        public ExprException() : base()//调用基类的构造器
        {
        }
        public ExprException(string message) : base(message)//调用基类的构造器
        {
        }
        public ExprException(string message, Exception innerException) : base(message, innerException)//调用基类的构造器
        { }
    }
}
