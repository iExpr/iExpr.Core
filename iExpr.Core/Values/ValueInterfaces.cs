using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Values
{
    public interface IContainsValue : IValue
    {
        bool Contains(IValue item);
    }
}
