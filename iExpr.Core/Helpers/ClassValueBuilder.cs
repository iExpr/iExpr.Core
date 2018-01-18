using iExpr.Evaluators;
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
        public bool IsReadOnly { set; get; } = false;
    }

    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public class ClassCtorMethod : Attribute
    {
        public int ArgumentCount { set; get; } = -1;
        public bool IsSelfCalculate { set; get; } = false;
    }
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ClassFieldAttribute : Attribute
    {
        public string Name { set; get; } = null;
        public bool IsReadOnly { set; get; } = false;
    }

    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Enum| AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class CanClassValueAttribute : Attribute
    {
        public string Name { set; get; } = null;
        public bool CanChangeMember { get; set; } = false;
        public bool SelfToString { get; set; } = false;
    }

    public static class ClassValueBuilder
    {
        public static PreClassValue BuildStaticAndCtor(Type type)
        {
            if (!Attribute.IsDefined(type, typeof(CanClassValueAttribute))) throw new ArgumentException("The object can't be built.");
            PreClassValue pc = new PreClassValue();
            var pa = (CanClassValueAttribute)Attribute.GetCustomAttribute(type, typeof(CanClassValueAttribute));
            pc.ClassName = pa.Name ?? type.Name;
            pc.CanChangeMember = pa.CanChangeMember;
            foreach (var v in type.GetConstructors())
            {
                if (!Attribute.IsDefined(v, typeof(ClassCtorMethod))) continue;
                var am = (ClassCtorMethod)Attribute.GetCustomAttribute(v, typeof(ClassCtorMethod));
                var ctor = new PreFunctionValue(pc.ClassName, (args, cal) =>
                {
                    var r = v.Invoke(new object[] { args, cal });
                    return BuildObject(r);
                    /*if (r is IExpr) return (IExpr)r;
                    else return new ConcreteValue(r);*/
                }, am.ArgumentCount, am.IsSelfCalculate);
                pc.CtorMethod = ctor;
                break;
            }
            foreach (var v in type.GetMethods())
            {
                if (v.IsStatic == false) continue;
                if (!Attribute.IsDefined(v, typeof(ClassMethodAttribute))) continue;
                var am = (ClassMethodAttribute)Attribute.GetCustomAttribute(v, typeof(ClassMethodAttribute));
                pc.Add(am.Name ?? v.Name, new CollectionItemValue(new PreFunctionValue(am.Name ?? v.Name, (args, cal) =>
                {
                    var r = v.Invoke(null, new object[] { args, cal });
                    if (r is IExpr) return (IExpr)r;
                    else return new ConcreteValue(r);
                }, am.ArgumentCount, am.IsSelfCalculate), am.IsReadOnly));
            }
            foreach (var v in type.GetFields())
            {
                if (v.IsStatic == false) continue;
                if (!Attribute.IsDefined(v, typeof(ClassFieldAttribute))) continue;
                var am = (ClassFieldAttribute)Attribute.GetCustomAttribute(v, typeof(ClassFieldAttribute));
                pc.Add(am.Name ?? v.Name, new CollectionItemValue(v.GetValue(null), am.IsReadOnly));
            }
            foreach (var v in type.GetProperties())
            {
                if (v.CanRead == false) continue;
                var getm = v.GetMethod;
                if (getm.IsStatic == false) continue;
                if (!Attribute.IsDefined(v, typeof(ClassFieldAttribute))) continue;
                var am = (ClassFieldAttribute)Attribute.GetCustomAttribute(v, typeof(ClassFieldAttribute));
                pc.Add(am.Name ?? v.Name, new CollectionItemValue(getm.Invoke(null, null), am.IsReadOnly));
            }
            return pc;
        }

        public static PreClassValue BuildObject(object obj)
        {
            /*if(obj is IValue)
            {
                return BuildValue((IValue)obj);
            }*/
            var type = obj.GetType();
            if (!Attribute.IsDefined(type, typeof(CanClassValueAttribute))) throw new ArgumentException("The object can't be built.");
            PreClassValue pc = new PreClassValue();
            var pa = (CanClassValueAttribute)Attribute.GetCustomAttribute(type, typeof(CanClassValueAttribute));
            pc.ClassName = pa.Name ?? type.Name;
            pc.CanChangeMember = pa.CanChangeMember;
            if(pa.SelfToString)pc.ToStringFunc = obj.ToString;
            foreach (var v in type.GetMethods())
            {
                if (v.IsStatic == true) continue;
                if (!Attribute.IsDefined(v, typeof(ClassMethodAttribute))) continue;
                var am = (ClassMethodAttribute)Attribute.GetCustomAttribute(v, typeof(ClassMethodAttribute));
                var f = new PreFunctionValue(am.Name ?? v.Name, (args, cal) =>
                  {
                      var r = v.Invoke(obj, new object[] { args, cal });
                      if (r is IExpr) return (IExpr)r;
                      else return new ConcreteValue(r);
                  }, am.ArgumentCount, am.IsSelfCalculate);
                pc.Add(am.Name ?? v.Name, new CollectionItemValue(f, am.IsReadOnly));
            }
            foreach (var v in type.GetFields())
            {
                if (v.IsStatic == true) continue;
                if (!Attribute.IsDefined(v, typeof(ClassFieldAttribute))) continue;
                var am = (ClassFieldAttribute)Attribute.GetCustomAttribute(v, typeof(ClassFieldAttribute));
                pc.Add(am.Name??v.Name, new CollectionItemValue(v.GetValue(obj), am.IsReadOnly));
            }
            foreach (var v in type.GetProperties())
            {
                if (v.CanRead == false) continue;
                var getm = v.GetMethod;
                if (getm.IsStatic == true) continue;
                if (!Attribute.IsDefined(v, typeof(ClassFieldAttribute))) continue;
                var am = (ClassFieldAttribute)Attribute.GetCustomAttribute(v, typeof(ClassFieldAttribute));
                pc.Add(am.Name ?? v.Name, new CollectionItemValue(getm.Invoke(obj,null), am.IsReadOnly));
            }

            return pc;
        }

        public static PreFunctionValue BuildFunction(Func<FunctionArgument,EvalContext,object> func,string name,int argumentCount=-1,bool isSelfCal=false)
        {
            var f = new PreFunctionValue(name, (args, cal) =>
            {
                var r = func(args, cal);
                if (r is IExpr) return (IExpr)r;
                else return new ConcreteValue(r);
            }, argumentCount, isSelfCal);
            return f;
        }

        /*static PreClassValue BuildValue(IValue obj)
        {
            var type = obj.GetType();
            if (!Attribute.IsDefined(type, typeof(CanClassValueAttribute))) throw new ArgumentException("The object can't be built.");
            PreValueClassValue pc = new PreValueClassValue(obj);
            var pa = (CanClassValueAttribute)Attribute.GetCustomAttribute(type, typeof(CanClassValueAttribute));
            pc.ClassName = pa.Name ?? type.Name;
            pc.CanChangeMember = pa.CanChangeMember;
            foreach (var v in type.GetMethods())
            {
                if (!Attribute.IsDefined(v, typeof(ClassMethodAttribute))) continue;
                var am = (ClassMethodAttribute)Attribute.GetCustomAttribute(v, typeof(ClassMethodAttribute));
                var f = new PreFunctionValue(am.Name ?? v.Name, (args, cal) =>
                {
                    var r = v.Invoke(obj, new object[] { args, cal });
                    if (r is IExpr) return (IExpr)r;
                    else return new ConcreteValue(r);
                }, am.ArgumentCount, am.IsSelfCalculate);
                pc.Add(am.Name ?? v.Name, new CollectionItemValue(f, am.IsReadOnly));
            }
            foreach (var v in type.GetFields())
            {
                if (!Attribute.IsDefined(v, typeof(ClassFieldAttribute))) continue;
                var am = (ClassFieldAttribute)Attribute.GetCustomAttribute(v, typeof(ClassFieldAttribute));
                pc.Add(am.Name ?? v.Name, new CollectionItemValue(v.GetValue(obj), am.IsReadOnly));
            }
            foreach (var v in type.GetProperties())
            {
                if (!Attribute.IsDefined(v, typeof(ClassFieldAttribute))) continue;
                var am = (ClassFieldAttribute)Attribute.GetCustomAttribute(v, typeof(ClassFieldAttribute));
                pc.Add(am.Name ?? v.Name, new CollectionItemValue(v.GetValue(obj), am.IsReadOnly));
            }

            return pc;
        }*/
    }
}
