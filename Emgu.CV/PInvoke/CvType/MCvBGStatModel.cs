using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapper to the CvBGStatModel
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvBGStatModel
   {
      /// <summary>
      /// type of BG model
      /// </summary>
      public int type;

      /// <summary>
      /// 
      /// </summary>
      public IntPtr CvReleaseBGStatModel;

      /// <summary>
      /// 
      /// </summary>
      public IntPtr CvUpdateBGStatModel;

      /// <summary>
      /// 8UC3 reference background image
      /// </summary>
      public IntPtr background;

      /// <summary>
      /// 8UC1 foreground image
      /// </summary>
      public IntPtr foreground;

      /// <summary>
      /// 8UC3 reference background image, can be null
      /// </summary>
      public IntPtr layers;

      /// <summary>
      /// can be zero
      /// </summary>
      public int layer_count;

      /// <summary>
      /// storage for foreground_regions
      /// </summary>
      public IntPtr storage;

      /// <summary>
      /// foreground object contours
      /// </summary>
      public IntPtr foreground_regions;

   }
}
