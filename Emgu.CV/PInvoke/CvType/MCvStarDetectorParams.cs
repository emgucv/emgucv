using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// CvStarDetectorParams
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvStarDetectorParams
   {
      /// <summary>
      /// 
      /// </summary>
      public int MaxSize;
      /// <summary>
      /// 
      /// </summary>
      public int ResponseThreshold;
      /// <summary>
      /// 
      /// </summary>
      public int LineThresholdProjected;
      /// <summary>
      /// 
      /// </summary>
      public int LineThresholdBinarized;
      /// <summary>
      /// 
      /// </summary>
      public int SuppressNonmaxSize;

      /// <summary>
      /// Get the default star detector parameters
      /// </summary>
      /// <returns>The default star detector parameters</returns>
      public static MCvStarDetectorParams GetDefaultParameter()
      {
         MCvStarDetectorParams param = new MCvStarDetectorParams();
         param.MaxSize = 45;
         param.ResponseThreshold = 30;
         param.LineThresholdProjected = 10;
         param.LineThresholdBinarized = 8;
         param.SuppressNonmaxSize = 5;

         return param;
      }
   }
}
