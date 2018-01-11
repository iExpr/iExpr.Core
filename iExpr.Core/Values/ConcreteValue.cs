using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Values
{
    /// <summary>
    /// 具体值
    /// </summary>
    public class ConcreteValue : IValue
    {
        /// <summary>
        /// 判断是否是常量值
        /// </summary>
        public bool IsConstant
        {
            get
            {
                switch (Value)
                {
                    case IValue val:
                        return val.IsConstant;
                    default:
                        return true;
                }
            }
        }

        public ConcreteValue(object val)
        {
            Value = val;
        }

        public ConcreteValue() : this(null) { }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        public override string ToString()
        {
            if (Value == null) return "";
            if (Value is IValue) return ((IValue)Value).ToString();
            if (Value is string)
            {
                return $"@\"{Value.ToString()}\"";
            }
            return Value.ToString();
        }

        public bool Equals(IExpr _other)
        {
            var other = _other as ConcreteValue;
            return other != null && other.ToString() == this.ToString();
        }

        public override int GetHashCode()
        {
            var hashCode = 1895487624;
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Value);
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj is IExpr) return Equals((IExpr)obj);
            else return false;
        }

        public static bool operator ==(ConcreteValue node1, ConcreteValue node2)
        {
            return EqualityComparer<ConcreteValue>.Default.Equals(node1, node2);
        }

        public static bool operator !=(ConcreteValue node1, ConcreteValue node2)
        {
            return !(node1 == node2);
        }
    }
}
