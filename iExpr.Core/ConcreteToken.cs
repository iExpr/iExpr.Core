using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr
{

    
    /// <summary>
    /// 具体值
    /// </summary>
    public class ConcreteToken : ExprToken
    {
        public static readonly ConcreteToken Null = new ConcreteToken(null);

        /// <summary>
        /// 判断是否是常量值
        /// </summary>
        public bool IsConstant
        {
            get
            {
                switch (Value)
                {
                    case IExpr exp:
                        if (exp is ConcreteToken) return (exp as ConcreteToken).IsConstant;
                        else return false;
                    case IValue val:
                        return val.IsConstant;
                    default:
                        return true;
                }
            }
        }

        public ConcreteToken(object val)
        {
            Value = val;
        }

        public ConcreteToken() : this(null) { }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
        

        public override string ToExprString()
        {
            if (Value == null) return "";
            if (Value is IValue) return ((IValue)Value).ToValueString();
            if (Value is string)
            {
                return $"@\"{Value.ToString()}\"";
            }
            return Value.ToString();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ConcreteToken);
        }

        public override bool Equals(IExpr _other)
        {
            var other = _other as ConcreteToken;
            return other != null && other.ToExprString() == this.ToExprString();
        }

        public override int GetHashCode()
        {
            var hashCode = 1895487624;
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Value);
            return hashCode;
        }

        public static bool operator ==(ConcreteToken node1, ConcreteToken node2)
        {
            return EqualityComparer<ConcreteToken>.Default.Equals(node1, node2);
        }

        public static bool operator !=(ConcreteToken node1, ConcreteToken node2)
        {
            return !(node1 == node2);
        }
    }
}
