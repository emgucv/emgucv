using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Emgu.Util
{
   /// <summary>
   /// An operation contains the MethodInfo and the methods parameters. It provides a way to invoke a specific method with the specific parameters. 
   /// </summary>
   /// <typeparam name="T">The type of instance this operation applies to</typeparam>
   public class Operation<T> : ICodeGenerable
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
         if (language == TypeEnum.ProgrammingLanguage.CSharp)
         {
            res = String.Format("{0}.{1}({2})",
                "{instance}",
                Method.Name,
                String.Join(", ", System.Array.ConvertAll<Object, String>(Parameters, delegate(Object p) { return  ParameterToCode(p, language);} )));
         }
         else if (language == TypeEnum.ProgrammingLanguage.CPlusPlus)
         {
            res = String.Format("{0}->{1}({2})",
                "{instance}",
                Method.Name,
                String.Join(", ", System.Array.ConvertAll<Object, String>(Parameters, delegate(Object p) { return  ParameterToCode(p, language);} )));
         }
         return res;
      }

      private String ParameterToCode(Object Parameters, TypeEnum.ProgrammingLanguage language)
      {
         ICodeGenerable gen = Parameters as ICodeGenerable;
         return gen == null ? System.Convert.ToString(Parameters) :
            gen.ToCode(language);
      }

   }
}
