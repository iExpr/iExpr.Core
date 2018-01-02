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
        public IExpr[] Contents { get; set; }
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
                Contents[index] = value;
            }
        }
        

        public override string ToValueString()
        {
            if (Contents == null) return "()";
            return $"({String.Join(",", Contents.Select(x => x?.ToExprString()))})";
        }

        public override void Reset(IEnumerable<IExpr> vals=null)
        {
            Contents = vals?.ToArray();
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
    }
}
