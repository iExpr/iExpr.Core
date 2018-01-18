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
        protected iExpr.Helpers.ExtendAccessibleValueHelper access = null;

        void init()
        {
            access = new iExpr.Helpers.ExtendAccessibleValueHelper(false, this);
            access.Add("current", new ConcreteValue(null));
            access.Add("next", new ConcreteValue(null));
        }

        public virtual bool IsCertain => true;

        public virtual EvalContext Context { get; protected set; }

        public virtual IValue Current { get => (IValue)access["current"]; set => access["current"] = value; }

        public virtual IValue Next { get => (IValue)access["next"]; set => access["next"]=value; }

        public EnumeratorValue(EvalContext context=null)
        {
            Context = context;
            Context?.Variables.Set(BuiltinValues.THIS, this);
            init();
        }

        public EnumeratorValue(IValue current,IValue next, EvalContext context=null) :this(context)
        {
            Next = next;
            Current = current;
        }

        public virtual bool Equals(IExpr other)
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

        public virtual IExpr Access(string id)
        {
            return ((IAccessibleValue)access).Access(id);
        }

        public virtual IDictionary<string, IExpr> GetMembers()
        {
            return ((IAccessibleValue)access).GetMembers();
        }
    }
}
