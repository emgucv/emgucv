using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV 
{
   /// <summary>
   /// OpenCV's PatchGenerator
   /// </summary>
   public struct PatchGenerator
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvPatchGeneratorInit(ref PatchGenerator pg);
      #endregion

      /// <summary>
      /// 
      /// </summary>
      public double BackgroundMin;
      /// <summary>
      /// 
      /// </summary>
      public double BackgroundMax;
      /// <summary>
      /// 
      /// </summary>
      public double NoiseRange;
      /// <summary>
      /// 
      /// </summary>
      [MarshalAs(UnmanagedType.I1)]
      public bool RandomBlur;
      /// <summary>
      /// 
      /// </summary>
      public double LambdaMin;
      /// <summary>
      /// 
      /// </summary>
      public double LambdaMax;
      /// <summary>
      /// 
      /// </summary>
      public double ThetaMin;
      /// <summary>
      /// 
      /// </summary>
      public double ThetaMax;
      /// <summary>
      /// 
      /// </summary>
      public double PhiMin;
      /// <summary>
      /// 
      /// </summary>
      public double PhiMax;

      /// <summary>
      /// Set the parameters to default value
      /// </summary>
      public void SetDefaultParameters()
      {
         CvPatchGeneratorInit(ref this);
      }
   }
}
