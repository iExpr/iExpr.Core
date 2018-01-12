using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace iExpr.Values
{
    /// <summary>
    /// 元组值
    /// </summary>
    public class TupleValue : CollectionValue
    {
        /// <summary>
        /// 元组内容
        /// </summary>
        private IExpr[] Contents { get; set; }

        protected override IEnumerable<IExpr> _Contents { get => Contents; }

        public override int Count => Contents == null ? 0 : Contents.Length;

        public IExpr this[int index]
        {
            get
            {
                return Contents[index];
            }
            set
            {
                ((CollectionItemValue)Contents[index]).Value = value;
            }
        }
        

        public override string ToString()
        {
            if (Contents == null) return "()";
            return $"({String.Join(",", Contents.Select(x => x?.ToString()))})";
        }

        public override void Reset(IEnumerable<IExpr> vals=null)
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

        public TupleValue(IEnumerable<IExpr> exprs) : this()
        {
            Reset(exprs);
            
        }

        public override bool Equals(IExpr other)
        {
            if (!(other is TupleValue)) return false;
            return _Contents == (other as TupleValue)._Contents;
        }
    }
}
