using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace iExpr.Values
{
    /// <summary>
    /// 列表值
    /// </summary>
    public class ListValue : CollectionValue,IList<IValue>
    {
        private List<CollectionItemValue> values = new List<CollectionItemValue>();

        protected override IEnumerable<CollectionItemValue> _Contents { get => values; }

        public bool IsReadOnly => ((ICollection<CollectionItemValue>)values).IsReadOnly;

        public IValue this[int index]
        {
            get => this.values[index];
            set => this.values[index].Value = value;//TODO: Attetion to this.
        }

        public override int Count => values.Count;

        public override string ToString()
        {
            if (values == null) return "[]";
            return $"[{String.Join(",", values.Select(x => x.Value?.ToString()))}]";
        }

        public override void Reset(IEnumerable<IValue> vals = null)
        {
            this.Clear();
            foreach (var v in vals) this.Add(v);
        }

        public override CollectionValue CreateNew()
        {
            return new ListValue();
        }

        public ListValue()
        {

        }

        public ListValue(IEnumerable<IValue> exprs):this()
        {
            Reset(exprs);
        }

        public override bool Equals(IExpr other)
        {
            if (!(other is ListValue)) return false;
            return this.ToString() == (other as ListValue).ToString();
        }

        public int IndexOf(IValue item)
        {
            if (item is ConcreteValue) item = (item as ConcreteValue).Value as IValue;
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].Value == item) return i;
            }
            return -1;
        }

        CollectionItemValue GetItemValue(object item)
        {
            foreach (var x in values)
                if (x.Value == item) return x;
            return null;
        }

        public void Insert(int index, IValue item)
        {
            values.Insert(index, new CollectionItemValue(item));
        }

        public void RemoveAt(int index)
        {
            values.RemoveAt(index);
        }

        public void Add(IValue item)
        {
            values.Add(new CollectionItemValue(item));
        }

        public void Clear()
        {
            values.Clear();
        }

        public bool Contains(IValue item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(IValue[] array, int arrayIndex)
        {
            //throw new Exceptions.UndefinedExecuteException();

            new List<IValue>((this.values.Select(x=>(IValue)x))).CopyTo(array, arrayIndex);
        }

        public bool Remove(IValue item)
        {
            var id = GetItemValue(item);if (id == null) return true;
            return values.Remove(id);
        }
    }
}
