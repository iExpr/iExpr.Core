﻿using iExpr.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Values
{
    public sealed class CollectionItemValue : ConcreteValue
    {
        public override bool IsConstant => base.IsConstant;

        public override object Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                object v = null;
                if (value is ConcreteValue) v = (value as ConcreteValue).Value;
                else v = value;
                base.Value = v;
                //else throw new ExprException("The item of colletion must be a expr (IExpr).");
            }
        }

        public CollectionItemValue(object val)
        {
            Value = val;
        }
    }

    public abstract class CollectionValue: IValue,IEnumerable<CollectionItemValue>, IEnumerable<IValue>
    {
        protected abstract IEnumerable<CollectionItemValue> _Contents { get; }

        public abstract void Reset(IEnumerable<IValue> vals = null);

        /// <summary>
        /// 创建一个同类型的新对象（用于拷贝等）
        /// </summary>
        /// <returns></returns>
        public abstract CollectionValue CreateNew();

        public abstract int Count
        {
            get;
        }

        public bool IsConstant
        {
            get
            {
                foreach (var v in _Contents)
                {
                    if (v == null) return true;
                    switch (v)
                    {
                        case IValue c:
                            if (c.IsConstant == false) return false;
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
