using iExpr.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exceptions
{
    public class ParseException : ExprException
    {
        public ParseException(Symbol expr,string message=null, Exception innerException=null) : base(message, innerException)//调用基类的构造器
        {
            Symbol = expr;
        }

        public Symbol Symbol { get; protected set; }
    }

    public class UnrecognizedTokenException : ParseException
    {

        public override string Message
        {
            get
            {
                return $"Unrecognized token: {Symbol} [{Symbol.StartPosition}-{Symbol.EndPosition}].- {base.Message}";
            }
        }

        public UnrecognizedTokenException(Symbol s, string message = null, Exception innerException = null) : base(s, message, innerException)
        {
        }
    }

    public class ExtraBracketException : ParseException
    {

        public override string Message
        {
            get
            {
                return $"Unexpected bracket: {Symbol} [{Symbol.StartPosition}-{Symbol.EndPosition}].- {base.Message}";
            }
        }

        public ExtraBracketException(Symbol expr, string message = null, Exception innerException = null) : base(expr, message, innerException)
        {
        }
    }

    public class UnexpectedExpressionException : ParseException
    {
        public override string Message
        {
            get
            {
                return $"Unexpected expression: {Symbol} [{Symbol.StartPosition}-{Symbol.EndPosition}].- {base.Message}";
            }
        }

        public UnexpectedExpressionException(Symbol expr, string message = null, Exception innerException = null) : base(expr, message, innerException)
        {
        }
    }

    public class RelatedExpressionNotFoundException : ParseException
    {
        public override string Message
        {
            get
            {
                return $"Can't find related expression: {Symbol} [{Symbol.StartPosition}-{Symbol.EndPosition}].- {base.Message}";
            }
        }

        public RelatedExpressionNotFoundException(Symbol expr, string message = null, Exception innerException = null) : base(expr, message, innerException)
        {
        }
    }

    public class IncompleteExpressionException : ParseException
    {
        public override string Message
        {
            get
            {
                return $"Incomplete expression: {Symbol} [{Symbol.StartPosition}-{Symbol.EndPosition}].- {base.Message}";
            }
        }

        public IncompleteExpressionException(Symbol expr, string message = null, Exception innerException = null) : base(expr, message, innerException)
        {
        }
    }
}
