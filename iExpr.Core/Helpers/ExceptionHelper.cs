using iExpr.Evaluators;
using iExpr.Exceptions;
using iExpr.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Helpers
{
    public static class ExceptionHelper
    {
        public static void RaiseUnrecognizedToken(Symbol expr,string message=null,Exception innerException = null)
        {
            throw new Exceptions.UnrecognizedTokenException(expr, message, innerException);
        }
        public static void RaiseExtraBracket(Symbol expr, string message = null, Exception innerException = null)
        {
            throw new Exceptions.ExtraBracketException(expr, message, innerException);
        }
        public static void RaiseUnexpectedExpression(Symbol expr, string message = null, Exception innerException = null)
        {
            throw new Exceptions.UnexpectedExpressionException(expr, message, innerException);
        }
        public static void RaiseIncompleteExpression(Symbol expr, string message = null, Exception innerException = null)
        {
            throw new Exceptions.IncompleteExpressionException(expr, message, innerException);
        }
        public static void RaiseRelatedExpressionNotFound(Symbol expr, string message = null, Exception innerException = null)
        {
            throw new Exceptions.RelatedExpressionNotFoundException(expr, message, innerException);
        }

        public static void RaiseWrongArgsNumber(object sender, int exp, int act, string message=null, Exception innerException=null, EvaluateExceptionType type = EvaluateExceptionType.None)
        {
            throw new Exceptions.WrongArgumentCountException(sender, exp, act, message, innerException, type);
        }
        public static void RaiseNotValue(object sender, object obj, string message=null, Exception innerException = null, EvaluateExceptionType type = EvaluateExceptionType.None)
        {
            throw new Exceptions.NotValueException(sender, obj, message, innerException, type);
        }
        public static void RaiseUncertainArgument(object sender, string message=null, Exception innerException = null, EvaluateExceptionType type = EvaluateExceptionType.None)
        {
            throw new Exceptions.UncertainArgumentException(sender, message, innerException, type);
        }
        

        public static void RaiseChangeReadOnlyValue(object sender, string message=null, Exception innerException = null, EvaluateExceptionType type = EvaluateExceptionType.None)
        {
            throw new Exceptions.ChangeReadOnlyValueException(sender, message, innerException, type);
        }

        public static void RaiseAccessFailed(object sender, string id,string message = null, Exception innerException = null, EvaluateExceptionType type = EvaluateExceptionType.None)
        {
            throw new Exceptions.AccessFailedException(sender, id,message, innerException, type);
        }

        public static void RaiseCallFailed(object sender, FunctionArgument args , string message = null, Exception innerException = null, EvaluateExceptionType type = EvaluateExceptionType.None)
        {
            throw new Exceptions.CallFailedException(sender, args,message, innerException, type);
        }

        public static void RaiseIndexFailed(object sender, FunctionArgument args, string message = null, Exception innerException = null, EvaluateExceptionType type = EvaluateExceptionType.None)
        {
            throw new Exceptions.IndexFailedException(sender, args, message, innerException, type);
        }

        public static void RaiseContentFailed(object sender, FunctionArgument args, string message = null, Exception innerException = null, EvaluateExceptionType type = EvaluateExceptionType.None)
        {
            throw new Exceptions.ContentFailedException(sender, args, message, innerException, type);
        }
        public static void RaiseInvalidExpressionFailed(object sender, IExpr expr, string message = null, Exception innerException = null, EvaluateExceptionType type = EvaluateExceptionType.None)
        {
            throw new Exceptions.InvalidExpressionException(sender, expr, message, innerException, type);
        }
        
    }
}
