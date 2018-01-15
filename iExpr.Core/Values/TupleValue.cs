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
    public abstract class TupleValueBase : CollectionValue, IIndexableValue
    {
        public abstract IExpr Index(FunctionArgument args, EvalContext cal);
    }

    /// <summary>
    /// 元组值
    /// </summary>
    public class TupleValue : TupleValueBase
    {
        /// <summary>
        /// 元组内容
        /// </summary>
        private CollectionItemValue[] Contents { get; set; }

        protected override IEnumerable<CollectionItemValue> _Contents { get => Contents; }

        public override int Count => Contents == null ? 0 : Contents.Length;

        public IValue this[int index]
        {
            get
            {
                return Contents[index];
            }
            set
            {
                Contents[index].Value = value;
            }
        }

        public override IExpr Index(FunctionArgument args, EvalContext cal)
        {
            if (args.Indexs?.Length != 1)
                throw new EvaluateException("The index content is invalid.");
            var ind = cal.GetValue<int>(cal.Evaluate(args.Indexs[0]));
            //int ind = ConcreteValueHelper.GetValue<int>(pind);//TODO: Check for null
            return this[ind];
        }


        public override string ToString()
        {
            if (Contents == null) return "()";
            return $"({String.Join(",", Contents.Select(x => x?.ToString()))})";
        }

        public override void Reset(IEnumerable<IValue> vals=null)
        {
            if (vals == null) Contents = null;
            Contents = vals.Select(x => new CollectionItemValue(x)).ToArray();
        }

        public override CollectionValue CreateNew()
        {
            return new TupleValue();
        }

        public TupleValue()
        {

        }

        public TupleValue(IEnumerable<IValue> exprs) : this()
        {
            Reset(exprs);
        }

        public override bool Equals(IExpr other)
        {
            if (!(other is TupleValue)) return false;
            return _Contents == (other as TupleValue)._Contents;
        }

        public override bool Contains(IValue item)
        {
            //object it = cal.GetValue(item);
            foreach(var v in Contents)
            {
                if (v.Equals(item)) return true;
            }
            return false;
        }

        public override bool Equals(IValue other)
        {
            return this.ToString() == other.ToString();
        }
    }
}
