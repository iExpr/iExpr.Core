﻿using iExpr.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr
{
    /// <summary>
    /// 变量值
    /// </summary>
    public class VariableToken : ExprToken,IValue
    {
        /// <summary>
        /// 变量标识符
        /// </summary>
        public string ID { get; set; }

        public ModifierToken[] Attached { get; set; }

        public virtual bool IsCertain => false;

        public VariableToken(string id)
        {
            ID = id;
        }

        public VariableToken(string id, ModifierToken attached0,params ModifierToken[] attached) : this(id)
        {
            List<ModifierToken> l = new List<ModifierToken>
            {
                attached0
            }; l.AddRange(attached);
            Attached = l.ToArray() ;
        }

        public VariableToken(string id, params ModifierToken[] attached) : this(id)
        {
            Attached = attached;
        }

        public VariableToken()
        {

        }


        public override string ToString()//TODO: ModifierToken
        {
            return ID;
            
        }

        public override bool Equals(object other)
        {
            if (other is IValue) return Equals((IValue)other);
            else return false;
        }

        public override bool Equals(IExpr _other)
        {
            var other = _other as VariableToken;
            return other != null && other.ToString() == this.ToString();
        }

        public override int GetHashCode()
        {
            var hashCode = 1895487624;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            return hashCode;
        }

        public static bool operator ==(VariableToken node1, VariableToken node2)
        {
            return EqualityComparer<VariableToken>.Default.Equals(node1, node2);
        }

        public static bool operator !=(VariableToken node1, VariableToken node2)
        {
            return !(node1 == node2);
        }
    }
}
