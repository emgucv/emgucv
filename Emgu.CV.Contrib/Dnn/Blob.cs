//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !(__IOS__ || UNITY_IPHONE || NETFX_CORE)
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.Dnn
{
   /// <summary>
   /// This class provides methods for continuous n-dimensional CPU and GPU array processing.
   /// </summary>
   public class Blob : UnmanagedObject
   {
      internal Blob(IntPtr blobPtr)
      {
         _ptr = blobPtr;
      }

      /// <summary>
      /// Constucts 4-dimensional blob (so-called batch) from image or array of images.
      /// </summary>
      /// <param name="image">2-dimensional multi-channel or 3-dimensional single-channel image (or array of images)</param>
      /// <param name="dstCn">specify size of second axis of ouptut blob</param>
      public Blob(IInputArray image, int dstCn = -1)
      {
         using (InputArray iaImage = image.GetInputArray())
            _ptr = ContribInvoke.cveDnnBlobCreateFromInputArray(iaImage, dstCn);
      }

      /// <summary>
      /// Returns reference to Mat, containing blob data.
      /// </summary>
      /// <returns>Reference to Mat, containing blob data.</returns>
      public Mat MatRef()
      {
         Mat m = new Mat();
         ContribInvoke.cveDnnBlobMatRef(_ptr, m);
         return m;
      }

      /// <summary>
      /// Release the unmanaged memory associated with this Blob
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            ContribInvoke.cveDnnBlobRelease(ref _ptr);
         }
      }

      /// <summary>
      /// Returns number of blob dimensions.
      /// </summary>
      public int Dims
      {
         get { return ContribInvoke.cveDnnBlobDims(_ptr); }
      }

      /// <summary>
      /// Returns size of the second axis blob.
      /// </summary>
      public int Channels
      {
         get { return ContribInvoke.cveDnnBlobChannels(_ptr); }
      }

      /// <summary>
      /// Returns size of the fourth axis blob.
      /// </summary>
      public int Cols
      {
         get { return ContribInvoke.cveDnnBlobCols(_ptr); }
      }

      /// <summary>
      /// Returns size of the first axis blob.
      /// </summary>
      public int Num
      {
         get { return ContribInvoke.cveDnnBlobNum(_ptr); }
      }

      /// <summary>
      /// Returns size of the thrid axis blob. 
      /// </summary>
      public int Rows
      {
         get { return ContribInvoke.cveDnnBlobRows(_ptr); }
      }
   }
}

namespace Emgu.CV
{
   public static partial class ContribInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveDnnBlobCreateFromInputArray(IntPtr image, int dstCn);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveDnnBlobMatRef(IntPtr blob, IntPtr outMat);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveDnnBlobRelease(ref IntPtr blob);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cveDnnBlobDims(IntPtr blob);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cveDnnBlobChannels(IntPtr blob);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cveDnnBlobCols(IntPtr blob);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cveDnnBlobNum(IntPtr blob);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cveDnnBlobRows(IntPtr blob);
   }
}

#endif