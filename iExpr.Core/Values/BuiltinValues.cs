using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Values
{
    public static class BuiltinValues
    {
        public static readonly IValue Null = new ReadOnlyConcreteValue(null);

        public static readonly IValue True = new ReadOnlyConcreteValue(true);

        public static readonly IValue False = new ReadOnlyConcreteValue(false);

        public const string THIS = "this";
    }
}
