using iExpr.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr
{
    public interface ICalculator
    {
        ExprCalculator Parent { get; set; }
        IExpr Calculate(IExpr expr);
    }
}
