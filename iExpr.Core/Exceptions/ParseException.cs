using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exceptions
{
    public class ParseException : ExprException
    {
        public ParseException(string expr,string message=null, Exception innerException=null) : base(message, innerException)//调用基类的构造器
        {
            FailedExpr = expr;
        }

        public string FailedExpr { get; private set; }

        public override string Message
        {
            get
            {
                return $"{base.Message} Failed expr: {FailedExpr}";
            }
        }
    }

    public class UnrecognizedTokenException : ParseException
    {
        public UnrecognizedTokenException(string expr, string message = null, Exception innerException = null) : base(expr, message, innerException)
        {
        }
    }

    public class ExtraBracketException : ParseException
    {
        public ExtraBracketException(string expr, string message = null, Exception innerException = null) : base(expr, message, innerException)
        {
        }
    }

    public class UnexpectedExpressionException : ParseException
    {
        public UnexpectedExpressionException(string expr, string message = null, Exception innerException = null) : base(expr, message, innerException)
        {
        }
    }

    public class RelatedExpressionNotFoundException : ParseException
    {
        public RelatedExpressionNotFoundException(string expr, string message = null, Exception innerException = null) : base(expr, message, innerException)
        {
        }
    }

    public class IncompleteExpressionException : ParseException
    {
        public IncompleteExpressionException(string expr, string message = null, Exception innerException = null) : base(expr, message, innerException)
        {
        }
    }
}
