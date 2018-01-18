using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace iExpr.Values
{
    public abstract class SetValueBase : CollectionValue, ISet<IValue>
    {
        public abstract bool IsReadOnly { get; }

        public abstract bool Add(IValue item);
        public abstract void Clear();
        public abstract void CopyTo(IValue[] array, int arrayIndex);
        public abstract void ExceptWith(IEnumerable<IValue> other);
        public abstract void IntersectWith(IEnumerable<IValue> other);
        public abstract bool IsProperSubsetOf(IEnumerable<IValue> other);
        public abstract bool IsProperSupersetOf(IEnumerable<IValue> other);
        public abstract bool IsSubsetOf(IEnumerable<IValue> other);
        public abstract bool IsSupersetOf(IEnumerable<IValue> other);
        public abstract bool Overlaps(IEnumerable<IValue> other);
        public abstract bool Remove(IValue item);
        public abstract bool SetEquals(IEnumerable<IValue> other);
        public abstract void SymmetricExceptWith(IEnumerable<IValue> other);
        public abstract void UnionWith(IEnumerable<IValue> other);

        void ICollection<IValue>.Add(IValue item)
        {
            this.Add(item);
        }
    }


    /// <summary>
    /// 集合值
    /// </summary>
    public class SetValue : SetValueBase
    {
        /// <summary>
        /// 集合内容
        /// </summary>
        private HashSet<CollectionItemValue> Contents = new HashSet<CollectionItemValue>();

        protected override IEnumerable<CollectionItemValue> _Contents { get => Contents; }

        public override bool IsReadOnly => ((ICollection<CollectionItemValue>)Contents).IsReadOnly;

        public override int Count => Contents.Count;

        public override string ToString()
        {
            if (Contents == null) return "{}";
            return $"{{{String.Join(",", Contents.Select(x => x?.ToString()))}}}";
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

        public override bool Add(IValue item)
        {
            return Contents.Add(new CollectionItemValue(item));
        }

        IEnumerable<CollectionItemValue> getitems(IEnumerable<IValue> other)
        {
            return (other.Select(x => (x is CollectionItemValue) ? (CollectionItemValue)x : new CollectionItemValue(x)));
        }

        public override void ExceptWith(IEnumerable<IValue> other)
        {
            Contents.ExceptWith(getitems(other));
        }

        public override void IntersectWith(IEnumerable<IValue> other)
        {
            Contents.IntersectWith(getitems(other));
        }

        public override bool IsProperSubsetOf(IEnumerable<IValue> other)
        {
            return Contents.IsProperSubsetOf(getitems(other));
        }

        public override bool IsProperSupersetOf(IEnumerable<IValue> other)
        {
            return Contents.IsProperSupersetOf(getitems(other));
        }

        public override bool IsSubsetOf(IEnumerable<IValue> other)
        {
            return Contents.IsSubsetOf(getitems(other));
        }

        public override bool IsSupersetOf(IEnumerable<IValue> other)
        {
            return Contents.IsSupersetOf(getitems(other));
        }

        public override bool Overlaps(IEnumerable<IValue> other)
        {
            return Contents.Overlaps(getitems(other));
        }

        public override bool SetEquals(IEnumerable<IValue> other)
        {
            return Contents.SetEquals(getitems(other));
        }

        public override void SymmetricExceptWith(IEnumerable<IValue> other)
        {
            Contents.SymmetricExceptWith(getitems(other));
        }

        public override void UnionWith(IEnumerable<IValue> other)
        {
            Contents.UnionWith(getitems(other));
        }

        public override void Clear()
        {
            Contents.Clear();
        }

        public override bool Contains(IValue item)
        {
            return Contents.Contains(item);
        }

        public override void CopyTo(IValue[] array, int arrayIndex)
        {
            new HashSet<IValue>((this.Contents.Select(x => (IValue)x))).CopyTo(array, arrayIndex);
        }

        CollectionItemValue GetItemValue(object item)
        {
            foreach (var x in Contents)
                if (x.Value == item) return x;
            return null;
        }

        public override bool Remove(IValue item)
        {
            var id = GetItemValue(item); if (id == null) return true;
            return Contents.Remove(id);
        }
    }
}
