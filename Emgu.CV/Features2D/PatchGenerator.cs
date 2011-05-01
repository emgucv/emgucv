//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// OpenCV's PatchGenerator
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct PatchGenerator
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [MarshalAs(CvInvoke.BoolMarshalType)]
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
