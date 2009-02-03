using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Emgu.Util
{
   /// <summary>
   /// An operation contains the MethodInfo and the methods parameters. It provides a way to invoke a specific method with the specific parameters. 
   /// </summary>
   public class Operation: ICodeGenerable
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
      /// <param name="mi">The method info</param>
      /// <param name="parameters">The parameters for this method</param>
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
      public Object InvokeMethod(Object instance)
      {
         if (!_mi.ContainsGenericParameters)
            return _mi.DeclaringType.InvokeMember(_mi.Name, BindingFlags.InvokeMethod, null, instance, _parameters);

         Type[] types = new Type[_mi.GetGenericArguments().Length];
         for (int i = 0; i < types.Length; i++)
         {
            String typeName = ((String)_parameters[i]).Split('|')[1].Split(':')[0];
            if ((types[i] = Type.GetType(typeName)) == null)
            {
               typeName = String.Format("{0},{1}", typeName, typeName.Substring(0, typeName.LastIndexOf(".")));
               types[i] = Type.GetType(typeName);
            }
         }
         MethodInfo m = _mi.MakeGenericMethod(types);
         Object[] param = new object[_parameters.Length - types.Length];
         Array.Copy(_parameters, types.Length, param, 0, param.Length);
         return m.Invoke(instance, param); //m.DeclaringType.InvokeMember(m.Name, BindingFlags.InvokeMethod, null, instance, param);
      }

      /// <summary>
      /// Represent this operation as a string
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         return String.Format(
             System.Globalization.CultureInfo.CurrentCulture,
             "{0}({1})",
             Method.Name,
             String.Join(", ", System.Array.ConvertAll<Object, String>(Parameters, System.Convert.ToString)));
      }

      /// <summary>
      /// Represent this operation as code
      /// </summary>
      /// <returns></returns>
      public String ToCode(TypeEnum.ProgrammingLanguage language)
      {
         String res = String.Empty;

         String genericArgString = string.Empty;
         Type[] genericArguments = _mi.GetGenericArguments();

         Object[] genericParameters = new object[genericArguments.Length];
         Object[] nonGenericParameters = new object[_parameters.Length - genericParameters.Length];
         Array.Copy(_parameters, genericParameters, genericParameters.Length);
         Array.Copy(_parameters, genericParameters.Length, nonGenericParameters, 0, nonGenericParameters.Length);

         if (genericArguments.Length > 0)
         {
            genericArgString = String.Join(",", Array.ConvertAll<Object, String>(genericParameters, 
               delegate(Object t)
               {
                  return (t as String).Split('|')[1].Split(':')[0];
               }));
         }

         switch (language)
         {
            case TypeEnum.ProgrammingLanguage.CSharp:
               if (genericArguments.Length > 0)
                  genericArgString = String.Format("<{0}>", genericArgString);
               res = String.Format("{0}.{1}{2}({3})",
                   "{instance}",
                   Method.Name,
                   genericArgString,
                   String.Join(", ", System.Array.ConvertAll<Object, String>(nonGenericParameters, delegate(Object p) { return ParameterToCode(p, language); })));
               break;
            case TypeEnum.ProgrammingLanguage.CPlusPlus:
               if (genericArguments.Length > 0)
                  genericArgString = String.Format("<{0}>", genericArgString);
               res = String.Format("{0}->{1}{2}({3})",
                   "{instance}",
                   Method.Name,
                   genericArgString,
                   String.Join(", ", System.Array.ConvertAll<Object, String>(nonGenericParameters, delegate(Object p) { return ParameterToCode(p, language); })));
               break;
         }
         return res;
      }

      private static String ParameterToCode(Object parameter, TypeEnum.ProgrammingLanguage language)
      {
         ICodeGenerable gen = parameter as ICodeGenerable;
         return gen == null ? System.Convert.ToString(parameter) :
            gen.ToCode(language);
      }

   }
}
