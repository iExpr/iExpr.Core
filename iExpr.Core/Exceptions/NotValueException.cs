using iExpr.Evaluators;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exceptions
{
    public class ChangeReadOnlyValueException : EvaluateException
    {
        public ChangeReadOnlyValueException(object sender, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, type)
        {
        }

        public ChangeReadOnlyValueException(object sender, string message, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, type)
        {
        }

        public ChangeReadOnlyValueException(object sender, string message, Exception innerException, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, innerException, type)
        {
        }
    }

    public class AccessFailedException : EvaluateException
    {
        public string ID { get; private set; }

        public AccessFailedException(object sender,string id, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, type)
        {
        }

        public AccessFailedException(object sender, string id, string message, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, type)
        {
        }

        public AccessFailedException(object sender, string id, string message, Exception innerException, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, innerException, type)
        {
        }

        public override string Message
        {
            get
            {
                return $"Can't access id: {ID}. -{base.Message}";
            }
        }
    }

    public class CallFailedException : EvaluateException
    {
        public FunctionArgument Argument { get; private set; }

        public CallFailedException(object sender, FunctionArgument args, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, type)
        {
            Argument = args;
        }

        public CallFailedException(object sender, FunctionArgument args, string message, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, type)
        {
            Argument = args;
        }

        public CallFailedException(object sender, FunctionArgument args, string message, Exception innerException, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, innerException, type)
        {
            Argument = args;
        }

        public override string Message
        {
            get
            {
                return $"Can't call. -{base.Message}";
            }
        }
    }

    public class IndexFailedException : EvaluateException
    {
        public FunctionArgument Argument { get; private set; }

        public IndexFailedException(object sender, FunctionArgument args, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, type)
        {
            Argument = args;
        }

        public IndexFailedException(object sender, FunctionArgument args, string message, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, type)
        {
            Argument = args;
        }

        public IndexFailedException(object sender, FunctionArgument args, string message, Exception innerException, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, innerException, type)
        {
            Argument = args;
        }

        public override string Message
        {
            get
            {
                return $"Can't index. -{base.Message}";
            }
        }
    }

    public class ContentFailedException : EvaluateException
    {
        public FunctionArgument Argument { get; private set; }

        public ContentFailedException(object sender, FunctionArgument args, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, type)
        {
            Argument = args;
        }

        public ContentFailedException(object sender, FunctionArgument args, string message, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, type)
        {
            Argument = args;
        }

        public ContentFailedException(object sender, FunctionArgument args, string message, Exception innerException, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, innerException, type)
        {
            Argument = args;
        }

        public override string Message
        {
            get
            {
                return $"Can't content. -{base.Message}";
                
            }
        }
    }

    public class InvalidExpressionException : EvaluateException
    {
        public IExpr Expr { get; private set; }

        public InvalidExpressionException(object sender, IExpr expr,EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, type)
        {
            Expr = expr;
        }

        public InvalidExpressionException(object sender, IExpr expr, string message, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, type)
        {
            Expr = expr;
        }

        public InvalidExpressionException(object sender, IExpr expr, string message, Exception innerException, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, innerException, type)
        {
            Expr = expr;
        }

        

    }

    public class NotValueException : EvaluateException
    {
        public object Object { get; private set; }

        public NotValueException(object sender,object obj, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, type)
        {
            Object = obj;
        }

        public NotValueException(object sender, object obj, string message, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, type)
        {
            Object = obj;
        }

        public NotValueException(object sender, object obj, string message, Exception innerException, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, innerException, type)
        {
            Object = obj;
        }

        public override string Message
        {
            get
            {
                return $"The object is not a value: {Object}. -{base.Message}";
            }
        }
    }

    public class WrongArgumentCountException : EvaluateException
    {
        public int Expect { get; private set; }

        public int Actual { get; private set; }

        public WrongArgumentCountException(object sender, int exp,int act, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, type)
        {
            Expect = exp;
            Actual = act;
        }

        public WrongArgumentCountException(object sender, int exp, int act, string message, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, type)
        {
            Expect = exp;
            Actual = act;
        }

        public WrongArgumentCountException(object sender, int exp, int act, string message, Exception innerException, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, innerException, type)
        {
            Expect = exp;
            Actual = act;
        }

        public override string Message
        {
            get
            {
                return $"Unexpect argument count: expect {Expect} but actual {Actual}. -{base.Message}";
            }
        }
    }

    public class UncertainArgumentException : EvaluateException
    {
        public UncertainArgumentException(object sender, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, type)
        {
        }

        public UncertainArgumentException(object sender, string message, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, type)
        {
        }

        public UncertainArgumentException(object sender, string message, Exception innerException, EvaluateExceptionType type = EvaluateExceptionType.None) : base(sender, message, innerException, type)
        {
        }

        public override string Message
        {
            get
            {
                return $"Uncertain value. -{base.Message}";
            }
        }
    }
}
