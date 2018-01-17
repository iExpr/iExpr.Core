using iExpr.Evaluators;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Values
{
    internal class EnumeratorNextFunctionValue : PreFunctionValue
    {
        public PreEnumeratorValue Enumerator { get; private set; }

        static IExpr Execute(EnumeratorNextFunctionValue main, EvalContext cal)
        {
            return new ConcreteValue(main.Enumerator.Enumerator.MoveNext());
        }

        public EnumeratorNextFunctionValue(PreEnumeratorValue en) : base()
        {
            Enumerator = en;
            Keyword = "next";
            EvaluateFunc = (x, y) => { return Execute(this, y); };
        }
    }

    public class PreEnumeratorValue: EnumeratorValue, IEnumerableValue
    {
        public virtual IEnumerator<IValue> Enumerator { get; protected set; }

        EnumeratorNextFunctionValue next = null;

        public override IValue Current { get=> Enumerator.Current; set=>throw new Exceptions.UndefinedExecuteException(); }

        public override IValue Next { get=> next; set => throw new Exceptions.UndefinedExecuteException(); }

        public PreEnumeratorValue(IEnumerator<IValue> enumerator):base(null)
        {
            this.Enumerator = enumerator;
            next = new EnumeratorNextFunctionValue(this);
        }

        public PreEnumeratorValue(IEnumerable<IValue> enumerable) : this(enumerable.GetEnumerator())
        {
        }

        public override IEnumerator<IValue> GetEnumerator()
        {
            return Enumerator;
        }

        public override string ToString()
        {
            return $"<enumerator value>";
        }
    }
}
