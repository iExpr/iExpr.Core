using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Helpers
{
    public class ExtendAccessibleValueHelper : Dictionary<string, IExpr>, IAccessibleValue
    {
        public ExtendAccessibleValueHelper(bool canChangeMember, object parent)
        {
            CanChangeMember = canChangeMember;
            Parent = parent;
        }

        public bool IsCertain => true;

        public bool CanChangeMember { get; set; }

        public object Parent { get; set; }

        public IExpr Access(string id)
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
                ExceptionHelper.RaiseAccessFailed(Parent, id);
                return default;
            }
        }

        public bool Equals(IExpr other)
        {
            return ((object)this).Equals(other);
        }

        public void Add(PreFunctionValue func)
        {
            this.Add(func.Keyword, func);
        }

        public void Add(PreClassValue func)
        {
            this.Add(func.ClassName, func);
        }

        public IDictionary<string, IExpr> GetMembers()
        {
            return (IDictionary<string, IExpr>)this;
        }
    }
}
