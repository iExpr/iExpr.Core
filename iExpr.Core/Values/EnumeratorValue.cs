using iExpr.Evaluators;
using iExpr.Exceptions;
using iExpr.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Values
{
    public class EnumeratorValue : IAccessibleValue, IEnumerableValue
    {
        ClassValue clv = null;

        public bool IsConstant => true;

        public virtual EvalContext Context { get; protected set; }

        public virtual IValue Current { get; set; }

        protected FunctionValue _Next; 

        public virtual IValue Next { get; set; }

        public EnumeratorValue(EvalContext context=null)
        {
            Context = context;
            clv = new ClassValue(context);
            Context?.Variables.Set(BuiltinValues.THIS, this);
        }

        public EnumeratorValue(IValue current,IValue next, EvalContext context=null) :this(context)
        {
            Next = next;
            Current = current;
        }

        public virtual IExpr Access(string id)
        {
            switch (id)
            {
                case "current":
                    return Current;
                case "next":
                    return Next;
                default:
                    return clv.Access(id);
                    //throw new EvaluateException("can't access the id.");
            }
        }

        public virtual bool Equals(IExpr other)
        {
            return false;
        }

        public virtual bool Equals(IValue other)
        {
            return false;
        }

        public virtual IEnumerator<IValue> GetEnumerator()
        {
            var arg = new FunctionArgument();
            var func = Context.GetValue<FunctionValue>(Next);
            while (Context.GetValue<bool>(func.Call(arg, Context))){
                var x = OperationHelper.GetValue(Current);
                if (x is IValue)
                    yield return (IValue)x;
                else yield return new ConcreteValue(x);
            }
        }
        
        public override string ToString()
        {
            return $"<enumerator value>";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
