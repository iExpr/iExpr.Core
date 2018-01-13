using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Values
{
    public static class BuiltinValues
    {
        public static readonly IValue Null = new ReadOnlyConcreteValue(null);
    }
}
