using iExpr.Exceptions;
using iExpr.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Values
{
    public class CollectionItemValue : ConcreteValue
    {
        public override bool IsCertain => base.IsCertain;

        public bool IsReadOnly { get; protected set; } = false;

        public override object Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if(IsReadOnly) throw new Exceptions.EvaluateException("Can't change the read-only value");
                object v = null;
                if (value is IExpr) v = OperationHelper.GetValue((IExpr)value);
                else v = value;
                base.Value = v;
                //else throw new ExprException("The item of colletion must be a expr (IExpr).");
            }
        }

        public CollectionItemValue(object val)
        {
            Value = val;
            if (val is ReadOnlyConcreteValue) IsReadOnly = true;
        }

        public CollectionItemValue(object val, bool isreadonly) : this(val)
        {
            IsReadOnly = isreadonly;
        }
    }

    public abstract class CollectionValue: IContainsValue, ICountableValue, IEnumerableValue, IEnumerable<CollectionItemValue>
    {
        protected abstract IEnumerable<CollectionItemValue> _Contents { get; }

        public abstract void Reset(IEnumerable<IValue> vals = null);

        public abstract bool Contains(IValue item);

        /// <summary>
        /// 创建一个同类型的新对象（用于拷贝等）
        /// </summary>
        /// <returns></returns>
        public abstract CollectionValue CreateNew();

        public abstract int Count
        {
            get;
        }

        public virtual bool IsCertain
        {
            get
            {
                foreach (var v in _Contents)
                {
                    if (v == null) return true;
                    switch (v)
                    {
                        case IValue c:
                            if (c.IsCertain == false) return false;
                            break;
                        default:
                            return false;
                    }
                }
                return true;
            }
        }

        public IEnumerator<CollectionItemValue> GetEnumerator()
        {
            return _Contents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Contents.GetEnumerator();
        }

        public abstract bool Equals(IExpr other);

        IEnumerator<IValue> IEnumerable<IValue>.GetEnumerator()
        {
            foreach (var v in _Contents)
                yield return (v.Value is IValue) ? (IValue)v.Value : v;
        }
    }
}
