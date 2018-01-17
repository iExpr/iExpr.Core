using iExpr.Values;
using System.Collections.Generic;

namespace iExpr.Parsers
{
    public class ConstantList : Dictionary<string, ConstantToken>
    {
        //Dictionary<string, ConstantToken> sid = new Dictionary<string, ConstantToken>();

        public void Add(params ConstantToken[] val)
        {
            foreach (var v in val)
            {
                this.Add(v.ID, v);
            }
        }

        public void AddFunction(PreFunctionValue func)
        {
            this.Add(new ConstantToken(func.Keyword, func));
        }

        public void AddClassValue(PreClassValue cla,bool isstatic)
        {
            if(isstatic)this.Add(new ConstantToken(cla.ClassName, cla));
            else if(cla.CtorMethod!=null)
            {
                this.AddFunction(cla.CtorMethod);
            }
        }
    }
}
