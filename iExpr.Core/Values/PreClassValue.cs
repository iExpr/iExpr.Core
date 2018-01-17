using iExpr.Evaluators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace iExpr.Values
{
    /*
    public class PreValueClassValue : PreClassValue, IHasValue
    {
        public IValue Value { get; protected set; }

        internal PreValueClassValue(IValue val)
        {
            Value = val;
            base.ToStringFunc = val.ToString;
        }

        public override bool IsConstant => Value.IsConstant;

        object IHasValue.Value => Value is ConcreteValue ? (Value as ConcreteValue).Value : Value;

        public override bool Equals(object obj)
        {
            return Value.Equals(obj);
        }

        public override bool Equals(IExpr other)
        {
            return Value.Equals(other);
        }

        public override bool Equals(IValue other)
        {
            return Value.Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
    */

    public class PreClassValue : ClassValue
    {
        public string ClassName { get; internal protected set; }

        public bool CanChangeMember { get; internal protected set; }

        public PreFunctionValue CtorMethod { get; internal protected set; }

        public Func<string> ToStringFunc { get; internal protected set; }

        public override IExpr Access(string id)
        {
            if (this.ContainsKey(id)) return this[id];
            else if (CanChangeMember)
            {
                var r = new CollectionItemValue(BuiltinValues.Null);
                this.Add(id, r);
                return r;
            }
            else
            {
                throw new EvaluateException("can't access the id.");
            }
        }

        internal PreClassValue() { }

        public override string ToString()
        {
            if (ToStringFunc != null) return ToStringFunc();
            return $"<class value named {ClassName}>";
        }
    }
}
