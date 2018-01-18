using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;
using iExpr.Evaluators;
using System.Data;
using iExpr.Helpers;

namespace iExpr.Values
{
    public abstract class ListValueBase : CollectionValue, IList<IValue>, IIndexableValue
    {
        public abstract IValue this[int index] { get; set; }

        public abstract bool IsReadOnly { get; }

        public abstract void Add(IValue item);
        public abstract void Clear();
        public abstract void CopyTo(IValue[] array, int arrayIndex);
        public abstract IExpr Index(FunctionArgument args, EvalContext cal);
        public abstract int IndexOf(IValue item);
        public abstract void Insert(int index, IValue item);
        public abstract bool Remove(IValue item);
        public abstract void RemoveAt(int index);
    }


    /// <summary>
    /// 列表值
    /// </summary>
    public class ListValue : ListValueBase
    {
        protected List<CollectionItemValue> Contents = new List<CollectionItemValue>();

        protected override IEnumerable<CollectionItemValue> _Contents { get => Contents; }

        public override bool IsReadOnly => ((ICollection<CollectionItemValue>)Contents).IsReadOnly;

        public override IValue this[int index]
        {
            get => this.Contents[index];
            set => this.Contents[index].Value = value;//TODO: Attetion to this.
        }

        public override IExpr Index(FunctionArgument args, EvalContext cal)
        {
            if (args.Indexs?.Length != 1)
                ExceptionHelper.RaiseIndexFailed(this, args);
            var ind = cal.GetValue<int>(cal.Evaluate(args.Indexs[0]));
            //int ind = ConcreteValueHelper.GetValue<int>(pind);//TODO: Check for null
            return this[ind];
        }

        public override int Count => Contents.Count;

        public override string ToString()
        {
            if (Contents == null) return "[]";
            return $"[{String.Join(",", Contents.Select(x => x.Value?.ToString()))}]";
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

        public override int IndexOf(IValue item)
        {
            //var it = cal.GetValue(item);
            //if (item is ConcreteValue) it = (item as ConcreteValue).Value as IValue;
            for (int i = 0; i < Contents.Count; i++)
            {
                if (Contents[i].Equals(item)) return i;
            }
            return -1;
        }

        CollectionItemValue GetItemValue(object item)
        {
            foreach (var x in Contents)
                if (x.Value == item) return x;
            return null;
        }

        public override void Insert(int index, IValue item)
        {
            Contents.Insert(index, new CollectionItemValue(item));
        }

        public override void RemoveAt(int index)
        {
            Contents.RemoveAt(index);
        }

        public override void Add(IValue item)
        {
            Contents.Add(new CollectionItemValue(item));
        }

        public override void Clear()
        {
            Contents.Clear();
        }

        public override bool Contains(IValue item)
        {
            return IndexOf(item) != -1;
        }

        public override void CopyTo(IValue[] array, int arrayIndex)
        {
            //throw new Exceptions.UndefinedExecuteException();

            new List<IValue>((this.Contents.Select(x=>(IValue)x))).CopyTo(array, arrayIndex);
        }

        public override bool Remove(IValue item)
        {
            var id = GetItemValue(item);if (id == null) return true;
            return Contents.Remove(id);
        }
    }
}
