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
    public class ListValue : CollectionValue
    {
        /// <summary>
        /// 列表内容
        /// </summary>
        public List<IExpr> Contents { get; set; }
        protected override IEnumerable<IExpr> _Contents { get => Contents; }

        public override int Count => Contents == null ? 0 : Contents.Count;

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
            if (Contents == null) return "[]";
            return $"[{String.Join(",", Contents.Select(x => x?.ToExprString()))}]";
        }

        public override void Reset(IEnumerable<IExpr> vals = null)
        {
            Contents = new List<IExpr>(vals);
        }

        public override CollectionValue CreateNew()
        {
            return new ListValue();
        }

        public ListValue()
        {

        }

        public ListValue(IEnumerable<IExpr> exprs):this()
        {
            Reset(exprs);
        }

        
    }
}
