using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Emgu.Reflection
{
    /// <summary>
    /// An operation contains a MethodInfo and the methods parameters. It provides a way to invoke a specific method with the specific parameters. 
    /// </summary>
    /// <typeparam name="T">The type of instance this operation applies to</typeparam>
    public class Operation<T>
    {
        private MethodInfo _mi;

        /// <summary>
        /// The MethodInfo
        /// </summary>
        public MethodInfo Method
        {
            get { return _mi; }
            set { _mi = value; }
        }

        private Object[] _parameters;

        /// <summary>
        /// The parameters for this method
        /// </summary>
        public Object[] Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        /// <summary>
        /// Craete an operation using the specific method and parameters
        /// </summary>
        /// <param name="mi"></param>
        /// <param name="parameters"></param>
        public Operation(MethodInfo mi, Object[] parameters)
        {
            _mi = mi;
            _parameters = parameters;
        }

        /// <summary>
        /// Call the specific method with the specific parameters on the provided <paramref name="instance"/>
        /// </summary>
        /// <param name="instance">The instance to call the method</param>
        /// <returns></returns>
        public Object ProcessMethod(T instance)
        {
            return typeof(T).InvokeMember(_mi.Name, BindingFlags.InvokeMethod, null, instance, _parameters);
        }

        /// <summary>
        /// Represent this operation as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}({1})",
                Method.Name,
                String.Join(", ", System.Array.ConvertAll<Object, String>(Parameters, System.Convert.ToString)));
        }

        /// <summary>
        /// Represent this operation as code
        /// </summary>
        /// <param name="instanceName"></param>
        /// <returns></returns>
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
