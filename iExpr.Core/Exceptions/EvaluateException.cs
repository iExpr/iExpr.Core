using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exceptions
{
    public enum EvaluateExceptionType
    {
        None
    }

    public class EvaluateException : ExprException
    {
        public EvaluateExceptionType Type
        {
            get;protected set;
        }

        public object Sender
        {
            get;private set;
        }

        public EvaluateException(object sender,EvaluateExceptionType type= EvaluateExceptionType.None) : base()//调用基类的构造器
        {
            Type = type;
            Sender = sender;
        }
        public EvaluateException(object sender, string message, EvaluateExceptionType type = EvaluateExceptionType.None) : base(message)//调用基类的构造器
        {
            Type = type;
            Sender = sender;
        }
        public EvaluateException(object sender, string message, Exception innerException, EvaluateExceptionType type = EvaluateExceptionType.None) : base(message, innerException)//调用基类的构造器
        {
            Type = type;
            Sender = sender;
        }
    }
}
