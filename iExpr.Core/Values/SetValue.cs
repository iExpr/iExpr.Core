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
        private HashSet<IExpr> values = new HashSet<IExpr>();

        protected override IEnumerable<IExpr> _Contents { get => this; }

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
            return ((ISet<IExpr>)values).Add(item);
        }

        public void ExceptWith(IEnumerable<IExpr> other)
        {
            ((ISet<IExpr>)values).ExceptWith(other);
        }

        public void IntersectWith(IEnumerable<IExpr> other)
        {
            ((ISet<IExpr>)values).IntersectWith(other);
        }

        public bool IsProperSubsetOf(IEnumerable<IExpr> other)
        {
            return ((ISet<IExpr>)values).IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<IExpr> other)
        {
            return ((ISet<IExpr>)values).IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<IExpr> other)
        {
            return ((ISet<IExpr>)values).IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<IExpr> other)
        {
            return ((ISet<IExpr>)values).IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<IExpr> other)
        {
            return ((ISet<IExpr>)values).Overlaps(other);
        }

        public bool SetEquals(IEnumerable<IExpr> other)
        {
            return ((ISet<IExpr>)values).SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<IExpr> other)
        {
            ((ISet<IExpr>)values).SymmetricExceptWith(other);
        }

        public void UnionWith(IEnumerable<IExpr> other)
        {
            ((ISet<IExpr>)values).UnionWith(other);
        }

        void ICollection<IExpr>.Add(IExpr item)
        {
            ((ISet<IExpr>)values).Add(item);
        }

        public void Clear()
        {
            ((ISet<IExpr>)values).Clear();
        }

        public bool Contains(IExpr item)
        {
            return ((ISet<IExpr>)values).Contains(item);
        }

        public void CopyTo(IExpr[] array, int arrayIndex)
        {
            ((ISet<IExpr>)values).CopyTo(array, arrayIndex);
        }

        public bool Remove(IExpr item)
        {
            return ((ISet<IExpr>)values).Remove(item);
        }
    }
}
