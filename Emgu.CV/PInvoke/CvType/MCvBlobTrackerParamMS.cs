using System;
using System.Collections.Generic;
using System.Text;
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
      public CvEnum.BLOBTRACKER_MS_PROFILE meanshift_profile;
      /// <summary>
      /// 
      /// </summary>
      public float sigma;
   }
}
