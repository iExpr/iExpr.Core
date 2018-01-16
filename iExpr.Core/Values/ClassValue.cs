using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Values
{
    public class ClassValue : Dictionary<string, CollectionItemValue>, IAccessibleValue
    {
        public bool IsConstant => true;

        public virtual IExpr Access(string id)
        {
            if (this.ContainsKey(id)) return this[id];
            else
            {
                var r = new CollectionItemValue(BuiltinValues.Null);
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
    }
}
