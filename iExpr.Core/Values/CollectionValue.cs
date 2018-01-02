using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Values
{
    public abstract class CollectionValue: IValue,IEnumerable<IExpr>
    {
        protected abstract IEnumerable<IExpr> _Contents { get; }

        public abstract void Reset(IEnumerable<IExpr> vals = null);

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
                        case ConcreteToken c:
                            if (c.IsConstant == false) return false;
                            break;
                        default:
                            return false;
                    }
                }
                return true;
            }
        }

        public IEnumerator<IExpr> GetEnumerator()
        {
            return _Contents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Contents.GetEnumerator();
        }

        public override string ToString()
        {
            return ToValueString();
        }

        public abstract string ToValueString();
    }
}
