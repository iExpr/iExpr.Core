using iExpr.Calculators;
using iExpr.Helpers;
using iExpr.Parser;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iExpr.Operations
{
    public abstract class CoreEnvironmentProvider : EnvironmentProvider
    {
        

        protected CoreEnvironmentProvider()
        {
            base.Operations = new OperationList();
            base.Operations.Add(new IOperation[]
            {
                CoreOperations.Lambda,
                    CoreOperations.Dot,
                  CoreOperations.List,
                  CoreOperations.Set,
                  CoreOperations.Tuple,
                  CoreOperations.Value,
                  CoreOperations.Length
            });
        }


    }
}
