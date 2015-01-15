//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Reflection;
using Emgu.Util;
using Emgu.Util.TypeEnum;
using Emgu.CV.Reflection;

namespace Emgu.CV.UI
{
   /// <summary>
   /// An operation contains the MethodInfo and the methods parameters. It provides a way to invoke a specific method with the specific parameters. 
   /// </summary>
   public class Operation: ICodeGenerable
   {
      private MethodInfo _mi;
      private Object[] _parameters;

      /// <summary>
      /// The MethodInfo
      /// </summary>
      public MethodInfo Method
      {
         get { return _mi; }
         set { _mi = value; }
      }

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

         #region get the generic types
         Type[] types = new Type[_mi.GetGenericArguments().Length];
         for (int i = 0; i < types.Length; i++)
         {
            types[i] = (_parameters[i] as GenericParameter).SelectedType;
         }
         #endregion

         #region get the non-generic parameters
         Object[] param = new object[_parameters.Length - types.Length];
         Array.Copy(_parameters, types.Length, param, 0, param.Length);
         #endregion

         return _mi.MakeGenericMethod(types).Invoke(instance, param); //m.DeclaringType.InvokeMember(m.Name, BindingFlags.InvokeMethod, null, instance, param);
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
      public String ToCode(ProgrammingLanguage language)
      {

         Type[] genericArguments = _mi.GetGenericArguments();

         Object[] genericParameters = new object[genericArguments.Length];
         Object[] nonGenericParameters = new object[_parameters.Length - genericParameters.Length];
         Array.Copy(_parameters, genericParameters, genericParameters.Length);
         Array.Copy(_parameters, genericParameters.Length, nonGenericParameters, 0, nonGenericParameters.Length);

         String genericArgString =
            genericArguments.Length > 0 ?
                  String.Join(",", Array.ConvertAll<Object, String>(genericParameters,
                     delegate(Object t)
                     {
                        return (t as GenericParameter).SelectedType.Name;
                     }))
            : string.Empty;
         

         String res = String.Empty;
         switch (language)
         {
            case ProgrammingLanguage.CSharp:
               if (genericArguments.Length > 0)
                  genericArgString = String.Format("<{0}>", genericArgString);
               res = String.Format("{0}.{1}{2}({3})",
                   "{instance}",
                   Method.Name,
                   genericArgString,
                   String.Join(", ", System.Array.ConvertAll<Object, String>(nonGenericParameters, delegate(Object p) { return ParameterToCode(p, language); })));
               break;
            case ProgrammingLanguage.CPlusPlus:
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

      private static String ParameterToCode(Object parameter, ProgrammingLanguage language)
      {
         ICodeGenerable gen = parameter as ICodeGenerable;
         return gen == null ? Convert.ToString(parameter) :
            gen.ToCode(language);
      }
   }
}
