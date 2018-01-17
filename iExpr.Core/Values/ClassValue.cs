using iExpr.Evaluators;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Values
{
    public class ClassValue : Dictionary<string, CollectionItemValue>, IAccessibleValue
    {
        public virtual bool IsConstant => true;

        public virtual EvalContext Context { get; protected set; }

        public ClassValue(EvalContext context=null)
        {
            Context = context;
            Context?.Variables.Add(BuiltinValues.THIS, this);
        }

        public virtual IExpr Access(string id)
        {
            if (this.ContainsKey(id)) return this[id];
            else
            {
                var r = new CollectionItemValue(BuiltinValues.Null,false);
                this.Add(id, r);
                return r;
            }
        }

        public virtual bool Equals(IExpr other)
        {
            return ((object)this).Equals(other);
        }

        public virtual bool Equals(IValue other)
        {
            return ((object)this).Equals(other);
        }

        public override string ToString()
        {
            return $"<class value with {this.Count} member(s)>";
        }
    }
}
