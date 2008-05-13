using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Emgu.Reflection
{
    public class Operation<T>
    {
        private MethodInfo _mi;

        public MethodInfo Method
        {
            get { return _mi; }
            set { _mi = value; }
        }

        private Object[] _parameters;

        public Object[] Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public Operation(MethodInfo mi, Object[] parameters)
        {
            _mi = mi;
            _parameters = parameters;
        }

        public Object ProcessMethod(T invoker)
        {
            Type ObjectType = typeof(T);
            return ObjectType.InvokeMember(_mi.Name, BindingFlags.InvokeMethod, null, invoker, _parameters);
        }

        public override string ToString()
        {
            return String.Format("{0}({1})",
                Method.Name,
                String.Join(", ", System.Array.ConvertAll<Object, String>(Parameters, System.Convert.ToString)));
        }

        public String ToCode(String instanceName)
        {
            String res = String.Format("{0}.{1}({2})",
                instanceName,
                Method.Name,
                String.Join(", ", System.Array.ConvertAll<Object, String>(Parameters, System.Convert.ToString)));
            return res;
        }
    }
}
