using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr
{
    public class ConstantToken : VariableToken
    {
        public IValue Value { get; protected set; }

        public ConstantToken(string id, IValue val, ModifierToken attached0, params ModifierToken[] attached) : this(id,val)
        {
            List<ModifierToken> l = new List<ModifierToken>
            {
                attached0
            }; l.AddRange(attached);
            Attached = l.ToArray();
        }

        public ConstantToken(string id, IValue val, params ModifierToken[] attached) : this(id,val)
        {
            Attached = attached;
        }

        public ConstantToken(string id,IValue val)
        {
            ID = id;
            Value = val;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ConstantToken);
        }

        public override bool Equals(IExpr _other)
        {
            var other = _other as ConstantToken;
            return other != null && other.ToString() == this.ToString();
        }

        public override int GetHashCode()
        {
            var hashCode = 1895487624;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            return hashCode;
        }

        public static bool operator ==(ConstantToken node1, ConstantToken node2)
        {
            return EqualityComparer<ConstantToken>.Default.Equals(node1, node2);
        }

        public static bool operator !=(ConstantToken node1, ConstantToken node2)
        {
            return !(node1 == node2);
        }
    }
}
