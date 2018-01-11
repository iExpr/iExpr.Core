using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace iExpr.Values
{
    /// <summary>
    /// 集合值
    /// </summary>
    public class SetValue : CollectionValue,ISet<IValue>
    {
        /// <summary>
        /// 集合内容
        /// </summary>
        private HashSet<IValue> values = new HashSet<IValue>();

        protected override IEnumerable<IValue> _Contents { get => this; }

        public bool IsReadOnly => ((ICollection<CollectionItemValue>)values).IsReadOnly;

        public override int Count => values.Count;

        public override string ToString()
        {
            if (values == null) return "{}";
            return $"{{{String.Join(",", values.Select(x => x?.ToString()))}}}";
        }

        public override void Reset(IEnumerable<IValue> vals = null)
        {
            this.Clear();
            foreach (var v in vals) this.Add(v);
        }

        public override CollectionValue CreateNew()
        {
            return new SetValue();
        }

        public SetValue()
        {

        }

        public SetValue(IEnumerable<IValue> exprs) : this()
        {
            Reset(exprs);
        }

        public override bool Equals(IExpr other)
        {
            if (!(other is SetValue)) return false;
            return this.ToString() == (other as SetValue).ToString();
        }

        public bool Add(IValue item)
        {
            return ((ISet<IValue>)values).Add(item);
        }

        public void ExceptWith(IEnumerable<IValue> other)
        {
            ((ISet<IValue>)values).ExceptWith(other);
        }

        public void IntersectWith(IEnumerable<IValue> other)
        {
            ((ISet<IValue>)values).IntersectWith(other);
        }

        public bool IsProperSubsetOf(IEnumerable<IValue> other)
        {
            return ((ISet<IValue>)values).IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<IValue> other)
        {
            return ((ISet<IValue>)values).IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<IValue> other)
        {
            return ((ISet<IValue>)values).IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<IValue> other)
        {
            return ((ISet<IValue>)values).IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<IValue> other)
        {
            return ((ISet<IValue>)values).Overlaps(other);
        }

        public bool SetEquals(IEnumerable<IValue> other)
        {
            return ((ISet<IValue>)values).SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<IValue> other)
        {
            ((ISet<IValue>)values).SymmetricExceptWith(other);
        }

        public void UnionWith(IEnumerable<IValue> other)
        {
            ((ISet<IValue>)values).UnionWith(other);
        }

        void ICollection<IValue>.Add(IValue item)
        {
            ((ISet<IValue>)values).Add(item);
        }

        public void Clear()
        {
            ((ISet<IValue>)values).Clear();
        }

        public bool Contains(IValue item)
        {
            return ((ISet<IValue>)values).Contains(item);
        }

        public void CopyTo(IValue[] array, int arrayIndex)
        {
            ((ISet<IValue>)values).CopyTo(array, arrayIndex);
        }

        public bool Remove(IValue item)
        {
            return ((ISet<IValue>)values).Remove(item);
        }
    }
}
