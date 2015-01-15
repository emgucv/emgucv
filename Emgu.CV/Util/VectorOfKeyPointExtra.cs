//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV.Util
{
   public partial class VectorOfKeyPoint : Emgu.Util.UnmanagedObject, IInputArray, IOutputArray, IInputOutputArray
   {
 
      /// <summary>
      /// Remove keypoints within borderPixels of an image edge.
      /// </summary>
      /// <param name="imageSize">Image size</param>
      /// <param name="borderSize">Border size in pixel</param>
      public void FilterByImageBorder(Size imageSize, int borderSize)
      {
         VectorOfKeyPointFilterByImageBorder(Ptr, imageSize, borderSize);
      }

      /// <summary>
      /// Remove keypoints of sizes out of range.
      /// </summary>
      /// <param name="minSize">Minimum size</param>
      /// <param name="maxSize">Maximum size</param>
      public void FilterByKeypointSize(float minSize, float maxSize)
      {
         VectorOfKeyPointFilterByKeypointSize(Ptr, minSize, maxSize);
      }

      /// <summary>
      /// Remove keypoints from some image by mask for pixels of this image.
      /// </summary>
      /// <param name="mask">The mask</param>
      public void FilterByPixelsMask(Image<Gray, Byte> mask)
      {
         VectorOfKeyPointFilterByPixelsMask(Ptr, mask);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyPointFilterByImageBorder(IntPtr keypoints, Size imageSize, int borderSize);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyPointFilterByKeypointSize(IntPtr keypoints, float minSize, float maxSize);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyPointFilterByPixelsMask(IntPtr keypoints, IntPtr mask);

   }
}
