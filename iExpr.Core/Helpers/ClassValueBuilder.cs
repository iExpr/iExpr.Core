using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace iExpr.Helpers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ClassMethodAttribute : Attribute
    {
        public string Name { set; get; } = null;
        public int ArgumentCount { set; get; } = -1;
        public bool IsSelfCalculate { set; get; } = false;
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ClassFieldAttribute : Attribute
    {
        public string Name { set; get; } = null;
    }

    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Enum| AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class CanClassValueAttribute : Attribute
    {
        public string Name { set; get; } = null;
        public bool CanChangeMember { get; set; } = false;
    }

    public static class ClassValueBuilder
    {
        public static PreClassValue Build(object obj)
        {
            var type = obj.GetType();
            return Build(obj.GetType(),null);
        }

        public static PreClassValue Build(Type type,object obj=null)
        {
            if (!Attribute.IsDefined(type, typeof(CanClassValueAttribute))) throw new ArgumentException("The object can't be built.");
            PreClassValue pc = new PreClassValue();
            var pa = (CanClassValueAttribute)Attribute.GetCustomAttribute(type, typeof(CanClassValueAttribute));
            pc.ClassName = pa.Name ?? type.Name;
            pc.CanChangeMember = pa.CanChangeMember;
            foreach (var v in type.GetMethods())
            {
                if (!Attribute.IsDefined(v, typeof(ClassMethodAttribute))) continue;
                var am = (ClassMethodAttribute)Attribute.GetCustomAttribute(v, typeof(ClassMethodAttribute));
                pc.Add(am.Name ?? v.Name, new CollectionItemValue(new PreFunctionValue(am.Name??v.Name, (args, cal) =>
                {
                    var r = v.Invoke(obj, new object[] { args, cal });
                    if (r is IExpr) return (IExpr)r;
                    else return new ConcreteValue(r);
                },am.ArgumentCount,am.IsSelfCalculate)));
            }
            foreach (var v in type.GetFields())
            {
                if (!Attribute.IsDefined(v, typeof(ClassFieldAttribute))) continue;
                var am = (ClassFieldAttribute)Attribute.GetCustomAttribute(v, typeof(ClassFieldAttribute));
                pc.Add(am.Name??v.Name, new CollectionItemValue(v.GetValue(obj)));
            }
            foreach (var v in type.GetProperties())
            {
                if (!Attribute.IsDefined(v, typeof(ClassFieldAttribute))) continue;
                var am = (ClassFieldAttribute)Attribute.GetCustomAttribute(v, typeof(ClassFieldAttribute));
                pc.Add(am.Name ?? v.Name, new CollectionItemValue(v.GetValue(obj)));
            }
            return pc;
        }
    }
}
