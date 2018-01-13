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
        private HashSet<CollectionItemValue> values = new HashSet<CollectionItemValue>();

        protected override IEnumerable<CollectionItemValue> _Contents { get => values; }

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
            return values.Add(new CollectionItemValue(item));
        }

        IEnumerable<CollectionItemValue> getitems(IEnumerable<IValue> other)
        {
            return (other.Select(x => (x is CollectionItemValue) ? (CollectionItemValue)x : new CollectionItemValue(x)));
        }

        public void ExceptWith(IEnumerable<IValue> other)
        {
            values.ExceptWith(getitems(other));
        }

        public void IntersectWith(IEnumerable<IValue> other)
        {
            values.IntersectWith(getitems(other));
        }

        public bool IsProperSubsetOf(IEnumerable<IValue> other)
        {
            return values.IsProperSubsetOf(getitems(other));
        }

        public bool IsProperSupersetOf(IEnumerable<IValue> other)
        {
            return values.IsProperSupersetOf(getitems(other));
        }

        public bool IsSubsetOf(IEnumerable<IValue> other)
        {
            return values.IsSubsetOf(getitems(other));
        }

        public bool IsSupersetOf(IEnumerable<IValue> other)
        {
            return values.IsSupersetOf(getitems(other));
        }

        public bool Overlaps(IEnumerable<IValue> other)
        {
            return values.Overlaps(getitems(other));
        }

        public bool SetEquals(IEnumerable<IValue> other)
        {
            return values.SetEquals(getitems(other));
        }

        public void SymmetricExceptWith(IEnumerable<IValue> other)
        {
            values.SymmetricExceptWith(getitems(other));
        }

        public void UnionWith(IEnumerable<IValue> other)
        {
            values.UnionWith(getitems(other));
        }

        public void Clear()
        {
            values.Clear();
        }

        public bool Contains(IValue item)
        {
            return values.Contains(item);
        }

        public void CopyTo(IValue[] array, int arrayIndex)
        {
            throw new Exceptions.UndefinedExecuteException();
        }

        CollectionItemValue GetItemValue(object item)
        {
            foreach (var x in values)
                if (x.Value == item) return x;
            return null;
        }

        public bool Remove(IValue item)
        {
            var id = GetItemValue(item); if (id == null) return true;
            return values.Remove(id);
        }

        void ICollection<IValue>.Add(IValue item)
        {
            this.Add(item);
        }
    }
}
