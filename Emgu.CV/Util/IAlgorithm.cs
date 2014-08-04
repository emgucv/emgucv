//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Util;

namespace Emgu.CV
{
   /// <summary>
   /// This is the algorithm class
   /// </summary>
   public interface IAlgorithm
   {
      /// <summary>
      /// Return the pointer to the algorithm object
      /// </summary>
      /// <returns>The pointer to the algorithm object</returns>
      IntPtr AlgorithmPtr { get; }
   }

   /// <summary>
   /// The type of the parameter
   /// </summary>
   public enum ParamType
   {
      /// <summary>
      /// Int32 
      /// </summary>
      Int32 = 0,
      /// <summary>
      /// Boolean
      /// </summary>
      Boolean = 1,
      /// <summary>
      /// Double
      /// </summary>
      Double = 2,
      /// <summary>
      /// String
      /// </summary>
      String = 3,
      /// <summary>
      /// Mat
      /// </summary>
      Mat = 4,
      /// <summary>
      /// VectorOfMat
      /// </summary>
      VectorOfMat = 5,
      /// <summary>
      /// Algorithm
      /// </summary>
      Algorithm = 6,
      /// <summary>
      /// Float
      /// </summary>
      Float = 7,
      /// <summary>
      /// UInt32
      /// </summary>
      UInt32 = 8,
      /// <summary>
      /// UInt64
      /// </summary>
      UInt64 = 9,
      /// <summary>
      /// Unsigned char
      /// </summary>
      UChar = 11
   }

   /// <summary>
   /// The definition of the parameter.
   /// </summary>
   public class ParamDef
   {
      /// <summary>
      /// The name of the parameter
      /// </summary>
      public String Name;
      /// <summary>
      /// The type of the parameter
      /// </summary>
      public ParamType Type;
      /// <summary>
      /// Help documents on the parameter
      /// </summary>
      public String Help;
   }

   /// <summary>
   /// Extension methods to the IAlgorithm interface
   /// </summary>
   public static class AlgorithmExtensions
   {
      /// <summary>
      /// Returns the algorithm parameter
      /// </summary>
      /// <param name="algorithm">The algorithm</param>
      /// <param name="name">The name of the parameter</param>
      /// <returns>The value of the parameter</returns>
      public static int GetInt(this IAlgorithm algorithm, String name)
      {
         using (CvString cs = new CvString(name))
         {
            return CvInvoke.cveAlgorithmGetInt(algorithm.AlgorithmPtr, cs);
         }
      }

      /// <summary>
      /// Sets the algorithm parameter
      /// </summary>
      /// <param name="algorithm">The algorithm</param>
      /// <param name="name">The name of the parameter</param>
      /// <param name="value">The value of the parameter</param>
      public static void SetInt(this IAlgorithm algorithm, String name, int value)
      {
         using (CvString cs = new CvString(name))
         {
            CvInvoke.cveAlgorithmSetInt(algorithm.AlgorithmPtr, cs, value);
         }
      }

      /// <summary>
      /// Returns the algorithm parameter
      /// </summary>
      /// <param name="algorithm">The algorithm</param>
      /// <param name="name">The name of the parameter</param>
      /// <returns>The value of the parameter</returns>
      public static double GetDouble(this IAlgorithm algorithm, String name)
      {
         using (CvString cs = new CvString(name))
         {
            return CvInvoke.cveAlgorithmGetDouble(algorithm.AlgorithmPtr, cs);
         }
      }

      /// <summary>
      /// Sets the algorithm parameter
      /// </summary>
      /// <param name="algorithm">The algorithm</param>
      /// <param name="name">The name of the parameter</param>
      /// <param name="value">The value of the parameter</param>
      public static void SetDouble(this IAlgorithm algorithm, String name, double value)
      {
         using (CvString cs = new CvString(name))
         {
            CvInvoke.cveAlgorithmSetDouble(algorithm.AlgorithmPtr, cs, value);
         }
      }

      /// <summary>
      /// Returns the algorithm parameter
      /// </summary>
      /// <param name="algorithm">The algorithm</param>
      /// <param name="name">The name of the parameter</param>
      /// <returns>The value of the parameter</returns>
      public static String GetString(this IAlgorithm algorithm, String name)
      {
         using (CvString cs = new CvString(name))
         using (CvString result = new CvString())
         {
            CvInvoke.cveAlgorithmGetString(algorithm.AlgorithmPtr, cs, result);
            return result.ToString();
         }
      }

      /// <summary>
      /// Sets the algorithm parameter
      /// </summary>
      /// <param name="algorithm">The algorithm</param>
      /// <param name="name">The name of the parameter</param>
      /// <param name="value">The value of the parameter</param>
      public static void SetString(this IAlgorithm algorithm, String name, String value)
      {
         using (CvString cs = new CvString(name))
         using (CvString v = new CvString(value))
         {
            CvInvoke.cveAlgorithmSetString(algorithm.AlgorithmPtr, cs, v);
         }
      }

      /// <summary>
      /// Get the list of parameter definitions
      /// </summary>
      /// <param name="algorithm">The algorithm to retrieve the parameter list from</param>
      /// <returns>The list of parameter definitions</returns>
      public static ParamDef[] GetParams(this IAlgorithm algorithm)
      {
         using (VectorOfCvString names = new VectorOfCvString())
         using (VectorOfInt types = new VectorOfInt())
         using (VectorOfCvString helps = new VectorOfCvString())
         {
            CvInvoke.cveAlgorithmGetParams(algorithm.AlgorithmPtr, names, types, helps);
            ParamDef[] results = new ParamDef[names.Size];
            for (int i = 0; i < results.Length; i++)
            {
               ParamDef t = new ParamDef();
               using (CvString n = names[i])
               using (CvString h = helps[i])
               {
                  t.Name = n.ToString();
                  t.Type = (ParamType) types[i];
                  t.Help = h.ToString();
               }
               results[i] = t;
            }
            return results;
         }
      }

      /// <summary>
      /// Get the list of algorithms available from opencv
      /// </summary>
      public static String[] AlgorithmList
      {
         get
         {
            using (VectorOfCvString v = new VectorOfCvString())
            {
               CvInvoke.cveAlgorithmGetList(v);
               String[] results = new String[v.Size];
               for (int i = 0; i < results.Length; i++)
               {
                  using (CvString s = v[i])
                  {
                     results[i] = s.ToString();
                  }
               }
               return results;
            }
         }
      }
   }

   public partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cveAlgorithmGetInt(IntPtr algorithm, IntPtr name);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveAlgorithmSetInt(IntPtr algorithm, IntPtr name, int value);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static double cveAlgorithmGetDouble(IntPtr algorithm, IntPtr name);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveAlgorithmSetDouble(IntPtr algorithm, IntPtr name, double value);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveAlgorithmGetString(IntPtr algorithm, IntPtr name, IntPtr result);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveAlgorithmSetString(IntPtr algorithm, IntPtr name, IntPtr result);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveAlgorithmGetParams(IntPtr algorithm, IntPtr names, IntPtr types, IntPtr helps);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveAlgorithmGetList(IntPtr names);

   }
}
