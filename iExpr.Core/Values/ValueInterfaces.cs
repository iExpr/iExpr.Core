using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Values
{
    public interface IContainsValue : IValue
    {
        bool Contains(IValue item);
    }

    public interface IEnumerableValue : IValue, IEnumerable<IValue>
    {
    }

    public interface ICountableValue : IValue
    {
        int Count { get; }
    }
}
