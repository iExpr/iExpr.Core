using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr
{
    public class ModifierToken : ExprToken
    {
        public Guid ID { get; private set; }

        public string Content { get; private set; }

        protected ModifierToken() { }

        public static ModifierToken Create(Guid id,string content)
        {
            var v = new ModifierToken();
            v.ID = id;v.Content = content;
            return v;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ModifierToken);
        }

        public override bool Equals(IExpr _other)
        {
            var other = _other as ModifierToken;
            return other != null &&
                   ID.Equals(other.ID) &&
                   Content == other.Content;
        }

        public override int GetHashCode()
        {
            var hashCode = 163328450;
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Content);
            return hashCode;
        }

        public static bool operator ==(ModifierToken token1, ModifierToken token2)
        {
            return EqualityComparer<ModifierToken>.Default.Equals(token1, token2);
        }

        public static bool operator !=(ModifierToken token1, ModifierToken token2)
        {
            return !(token1 == token2);
        }
    }
}
