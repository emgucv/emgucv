/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Mean Shift Blob tracker parameters
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvBlobTrackerParamMS
   {
      /// <summary>
      /// 
      /// </summary>
      public int noOfSigBits;
      /// <summary>
      /// 
      /// </summary>
      public int appearance_profile;
      /// <summary>
      /// MS profile
      /// </summary>
      public CvEnum.BlobtrackerMsProfile meanshift_profile;
      /// <summary>
      /// 
      /// </summary>
      public float sigma;
   }
}
*/