//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvScalar 
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvScalar : ICodeGenerable, IEquatable<MCvScalar>
   {
      /// <summary>
      /// The scalar value
      /// </summary>
      public double V0;
      /// <summary>
      /// The scalar value
      /// </summary>
      public double V1;
      /// <summary>
      /// The scalar value
      /// </summary>
      public double V2;
      /// <summary>
      /// The scalar value
      /// </summary>
      public double V3;

      /// <summary>
      /// The scalar values as a vector (of size 4)
      /// </summary>
      /// <returns>The scalar values as an array</returns>
      public double[] ToArray()
      {
         return new double[4] { V0, V1, V2, V3 };
      }

      /// <summary>
      /// Create a new MCvScalar structure using the specific values
      /// </summary>
      /// <param name="v0">v0</param>
      public MCvScalar(double v0)
      {
         this.V0 = v0;
         V1 = 0;
         V2 = 0;
         V3 = 0;
      }

      /// <summary>
      /// Create a new MCvScalar structure using the specific values
      /// </summary>
      /// <param name="v0">v0</param>
      /// <param name="v1">v1</param>
      public MCvScalar(double v0, double v1)
      {
         this.V0 = v0;
         this.V1 = v1;
         V2 = 0;
         V3 = 0;
      }

      /// <summary>
      /// Create a new MCvScalar structure using the specific values
      /// </summary>
      /// <param name="v0">v0</param>
      /// <param name="v1">v1</param>
      /// <param name="v2">v2</param>
      public MCvScalar(double v0, double v1, double v2)
      {
         this.V0 = v0;
         this.V1 = v1;
         this.V2 = v2;
         V3 = 0;
      }

      /// <summary>
      /// Create a new MCvScalar structure using the specific values
      /// </summary>
      /// <param name="v0">v0</param>
      /// <param name="v1">v1</param>
      /// <param name="v2">v2</param>
      /// <param name="v3">v3</param>
      public MCvScalar(double v0, double v1, double v2, double v3)
      {
         this.V0 = v0;
         this.V1 = v1;
         this.V2 = v2;
         this.V3 = v3;
      }

      #region ICodeGenerable Members
      /// <summary>
      /// Return the code to generate this MCvScalar from specific language
      /// </summary>
      /// <param name="language">The programming language to generate code from</param>
      /// <returns>The code to generate this MCvScalar from specific language</returns>
      public string ToCode(Emgu.Util.TypeEnum.ProgrammingLanguage language)
      {
         return (language == Emgu.Util.TypeEnum.ProgrammingLanguage.CSharp || language == Emgu.Util.TypeEnum.ProgrammingLanguage.CPlusPlus) ?
            String.Format("new MCvScalar({0}, {1}, {2}, {3})", V0, V1, V2, V3) :
            ToString();
      }

      #endregion

      #region IEquatable<MCvScalar> Members
      /// <summary>
      /// Return true if the two MCvScalar equals
      /// </summary>
      /// <param name="other">The other MCvScalar to compare with</param>
      /// <returns>true if the two MCvScalar equals</returns>
      public bool Equals(MCvScalar other)
      {
         return V0 == other.V0 
            && V1 == other.V1 
            && V2 == other.V2 
            && V3 == other.V3;
      }
      #endregion
   }
}
