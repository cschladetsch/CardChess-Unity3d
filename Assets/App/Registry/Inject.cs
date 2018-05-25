using System;
using System.Reflection;

namespace App.Registry
{
    public class Inject : Attribute
    {
        public object[] Args;
        public PropertyInfo PropertyInfo;
        public FieldInfo FieldInfo;
        public Type ValueType;
        public Inject(params object[] args)
        {
            Args = args;
        }
    }
}
