using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace iExpr.Values
{
    public class PreClassValue : ClassValue
    {
        public string ClassName { get; internal protected set; }

        public bool CanChangeMember { get; internal protected set; }

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
    }
}
