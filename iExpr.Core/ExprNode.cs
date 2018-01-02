using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr
{
    /// <summary>
    /// 表达式树结点
    /// </summary>
    public class ExprNode : IExpr
    {
        private IExpr[] children1;

        /// <summary>
        /// 根运算
        /// </summary>
        public IOperation Operation { get; set; }

        /// <summary>
        /// 子结点
        /// </summary>
        public IExpr[] Children { get; set; }

        public ExprNode(IOperation opt,params IExpr[] children)
        {
            Children = children;
            Operation = opt;
        }

        public ExprNode(IOperation opt, IExpr child0,params IExpr[] children)
        {
            List<IExpr> l = new List<IExpr>
            {
                child0
            }; l.AddRange(children);
            Children = l.ToArray();
            Operation = opt;
        }



        public string ToExprString()
        {
            return Operation.ToExprString(Children);
        }

        public override string ToString()
        {
            return ToExprString();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ExprNode);
        }

        public bool Equals(IExpr _other)
        {
            var other = _other as ExprNode;
            return other != null && other.ToExprString() == this.ToExprString();
        }

        public override int GetHashCode()
        {
            var hashCode = 1895487624;
            hashCode = hashCode * -1521134295 + EqualityComparer<IOperation>.Default.GetHashCode(Operation);
            hashCode = hashCode * -1521134295 + EqualityComparer<IExpr[]>.Default.GetHashCode(Children);
            return hashCode;
        }

        public static bool operator ==(ExprNode node1, ExprNode node2)
        {
            return EqualityComparer<ExprNode>.Default.Equals(node1, node2);
        }

        public static bool operator !=(ExprNode node1, ExprNode node2)
        {
            return !(node1 == node2);
        }
    }
}
