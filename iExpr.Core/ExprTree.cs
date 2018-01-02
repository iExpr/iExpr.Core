using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr
{
    public class ExprTree
    {
        public IExpr Content { get; set; }

        public ExprTree(IExpr exp)
        {
            Content = exp;
        }

        public ExprTree() : this(null) { }
    }
}
