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
    public class SetValue : CollectionValue
    {
        /// <summary>
        /// 集合内容
        /// </summary>
        public HashSet<IExpr> Contents { get; set; }
        protected override IEnumerable<IExpr> _Contents { get => Contents; }

        public override int Count => Contents == null ? 0 : Contents.Count;

        public override string ToValueString()
        {
            if (Contents == null) return "{}";
            return $"{{{String.Join(",", Contents.Select(x => x?.ToExprString()))}}}";
        }

        public override void Reset(IEnumerable<IExpr> vals = null)
        {
            Contents = new HashSet<IExpr>(vals);
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
        
    }
}
