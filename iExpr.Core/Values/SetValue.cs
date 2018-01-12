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
    public class SetValue : CollectionValue,ISet<IExpr>
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

        public override void Reset(IEnumerable<IExpr> vals = null)
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

        public SetValue(IEnumerable<IExpr> exprs) : this()
        {
            Reset(exprs);
        }

        public override bool Equals(IExpr other)
        {
            if (!(other is SetValue)) return false;
            return this.ToString() == (other as SetValue).ToString();
        }

        public bool Add(IExpr item)
        {
            return values.Add(new CollectionItemValue(item));
        }

        IEnumerable<CollectionItemValue> getitems(IEnumerable<IExpr> other)
        {
            return (other.Select(x => (x is CollectionItemValue) ? (CollectionItemValue)x : new CollectionItemValue(x)));
        }

        public void ExceptWith(IEnumerable<IExpr> other)
        {
            values.ExceptWith(getitems(other));
        }

        public void IntersectWith(IEnumerable<IExpr> other)
        {
            values.IntersectWith(getitems(other));
        }

        public bool IsProperSubsetOf(IEnumerable<IExpr> other)
        {
            return values.IsProperSubsetOf(getitems(other));
        }

        public bool IsProperSupersetOf(IEnumerable<IExpr> other)
        {
            return values.IsProperSupersetOf(getitems(other));
        }

        public bool IsSubsetOf(IEnumerable<IExpr> other)
        {
            return values.IsSubsetOf(getitems(other));
        }

        public bool IsSupersetOf(IEnumerable<IExpr> other)
        {
            return values.IsSupersetOf(getitems(other));
        }

        public bool Overlaps(IEnumerable<IExpr> other)
        {
            return values.Overlaps(getitems(other));
        }

        public bool SetEquals(IEnumerable<IExpr> other)
        {
            return values.SetEquals(getitems(other));
        }

        public void SymmetricExceptWith(IEnumerable<IExpr> other)
        {
            values.SymmetricExceptWith(getitems(other));
        }

        public void UnionWith(IEnumerable<IExpr> other)
        {
            values.UnionWith(getitems(other));
        }

        public void Clear()
        {
            values.Clear();
        }

        public bool Contains(IExpr item)
        {
            return values.Contains(item);
        }

        public void CopyTo(IExpr[] array, int arrayIndex)
        {
            throw new Exceptions.UndefinedExecuteException();
        }

        CollectionItemValue GetItemValue(object item)
        {
            foreach (var x in values)
                if (x.Value == item) return x;
            return null;
        }

        public bool Remove(IExpr item)
        {
            var id = GetItemValue(item); if (id == null) return true;
            return values.Remove(id);
        }

        void ICollection<IExpr>.Add(IExpr item)
        {
            this.Add(item);
        }
    }
}
