//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.Util;
using Emgu.CV.Util;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Emgu.CV.OpenCL
{
   /// <summary>
   /// A OclMat, use the generic version if possible. 
   /// </summary>
   public class OclMat : UnmanagedObject
   {
      /// <summary>
      /// Create an empty OclMat
      /// </summary>
      public OclMat()
      {
         _ptr = OclInvoke.OclMatCreateDefault();
      }

      /// <summary>
      /// Release the unmanaged memory associated with this GpuMat
      /// </summary>
      protected override void DisposeObject()
      {
         OclInvoke.OclMatRelease(ref _ptr);
      }

      /// <summary>
      /// Check if the GpuMat is Empty
      /// </summary>
      public bool IsEmpty
      {
         get
         {
            return OclInvoke.OclMatIsEmpty(_ptr);
         }
      }

      /// <summary>
      /// Check if the GpuMat is Continuous
      /// </summary>
      public bool IsContinuous
      {
         get
         {
            return OclInvoke.OclMatIsContinuous(_ptr);
         }
      }

      /// <summary>
      /// Get the GpuMat size:
      /// width == number of columns, height == number of rows
      /// </summary>
      public Size Size
      {
         get { return OclInvoke.OclMatGetSize(_ptr); }
      }

      /// <summary>
      /// Get the number of channels in the GpuMat
      /// </summary>
      public int NumberOfChannels
      {
         get { return OclInvoke.OclMatGetChannels(_ptr); }
      }
   }

   /// <summary>
   /// Similar to CvArray but use OpenCL for processing
   /// </summary>
   /// <typeparam name="TDepth">The type of element in the matrix</typeparam>
   public class OclMat<TDepth> : OclMat, IEquatable<OclMat<TDepth>>
      where TDepth : new()
   {
      /// <summary>
      /// Create a OclMat from the unmanaged pointer
      /// </summary>
      /// <param name="ptr">The unmanaged pointer to the OclMat</param>
      public OclMat(IntPtr ptr)
      {
         _ptr = ptr;
      }

      /// <summary>
      /// Create an empty GpuMat
      /// </summary>
      public OclMat()
         : base()
      {
      }

      /// <summary>
      /// Create a GpuMat of the specified size
      /// </summary>
      /// <param name="rows">The number of rows (height)</param>
      /// <param name="cols">The number of columns (width)</param>
      /// <param name="channels">The number of channels</param>
      public OclMat(int rows, int cols, int channels)
      {
         int matType = CvInvoke.CV_MAKETYPE((int)CvToolbox.GetMatrixDepth(typeof(TDepth)), channels);
         _ptr = OclInvoke.OclMatCreate(rows, cols, matType);
      }

      /// <summary>
      /// Create an OclMat from an CvArray of the same depth type
      /// </summary>
      /// <param name="arr">The CvArry to be converted to OclMat</param>
      public OclMat(CvArray<TDepth> arr)
      {
         _ptr = OclInvoke.OclMatCreateFromArr(arr);
      }

      /// <summary>
      /// Pefroms blocking upload data to OclMat
      /// </summary>
      /// <param name="arr">The CvArray to be uploaded to OclMat</param>
      public void Upload(CvArray<TDepth> arr)
      {
         OclInvoke.OclMatUpload(_ptr, arr);
      }

      /// <summary>
      /// Downloads data from device to host memory. Blocking calls
      /// </summary>
      /// <param name="arr">The destination CvArray where the OclMat data will be downloaded to.</param>
      public void Download(CvArray<TDepth> arr)
      {
         OclInvoke.OclMatDownload(_ptr, arr);
      }

      
      /// <summary>
      /// Returns the min / max location and values for the image
      /// </summary>
      /// <param name="maxLocations">The maximum locations for each channel </param>
      /// <param name="maxValues">The maximum values for each channel</param>
      /// <param name="minLocations">The minimum locations for each channel</param>
      /// <param name="minValues">The minimum values for each channel</param>
      public void MinMax(out double[] minValues, out double[] maxValues, out Point[] minLocations, out Point[] maxLocations)
      {
         minValues = new double[NumberOfChannels];
         maxValues = new double[NumberOfChannels];
         minLocations = new Point[NumberOfChannels];
         maxLocations = new Point[NumberOfChannels];

         double minVal = 0, maxVal = 0;
         Point minLoc = new Point(), maxLoc = new Point();
         if (NumberOfChannels == 1)
         {
            OclInvoke.MinMaxLoc(Ptr, ref minVal, ref maxVal, ref minLoc, ref maxLoc, IntPtr.Zero);
            minValues[0] = minVal; maxValues[0] = maxVal;
            minLocations[0] = minLoc; maxLocations[0] = maxLoc;
         }
         else
         {
            OclMat<TDepth>[] channels = Split();
            try
            {
               for (int i = 0; i < NumberOfChannels; i++)
               {
                  OclInvoke.MinMaxLoc(channels[i], ref minVal, ref maxVal, ref minLoc, ref maxLoc, IntPtr.Zero);
                  minValues[i] = minVal; maxValues[i] = maxVal;
                  minLocations[i] = minLoc; maxLocations[i] = maxLoc;
               }
            }
            finally
            {
               foreach (OclMat<TDepth> mat in channels) mat.Dispose();
            }
         }
      }

      ///<summary> 
      ///Split current Image into an array of gray scale images where each element 
      ///in the array represent a single color channel of the original image
      ///</summary>
      ///<param name="oclMats"> 
      ///An array of single channel OclMat where each item
      ///in the array represent a single channel of the original OclMat 
      ///</param>
      public void SplitInto(OclMat<TDepth>[] oclMats)
      {
         Debug.Assert(NumberOfChannels == oclMats.Length, "Number of channels does not agrees with the length of gpuMats");

         if (NumberOfChannels == 1)
         {
            //If single channel, return a copy
            OclInvoke.Copy(_ptr, oclMats[0], IntPtr.Zero);
         }
         else
         {
            //handle multiple channels
            Size size = Size;
            IntPtr[] ptrs = new IntPtr[oclMats.Length];
            for (int i = 0; i < ptrs.Length; i++)
            {
               Debug.Assert(oclMats[i].Size == size, "Size mismatch");
               ptrs[i] = oclMats[i].Ptr;
            }
            GCHandle handle = GCHandle.Alloc(ptrs, GCHandleType.Pinned);
            OclInvoke.Split(_ptr, handle.AddrOfPinnedObject());
            handle.Free();
         }
      }

      ///<summary> 
      ///Split current OclMat into an array of single channel OclMat where each element 
      ///in the array represent a single channel of the original OclMat
      ///</summary>
      ///<returns> 
      ///An array of single channel OclMat where each element  
      ///in the array represent a single channel of the original OclMat 
      ///</returns>
      public OclMat<TDepth>[] Split()
      {
         OclMat<TDepth>[] result = new OclMat<TDepth>[NumberOfChannels];
         Size size = Size;
         for (int i = 0; i < result.Length; i++)
         {
            result[i] = new OclMat<TDepth>(size.Height, size.Width, 1);
         }

         SplitInto(result);
         return result;
      }

      /// <summary>
      /// Makes multi-channel array out of several single-channel arrays
      /// </summary>
      ///<param name="gpuMats"> 
      ///An array of single channel OclMat where each item
      ///in the array represent a single channel of the GpuMat 
      ///</param>
      public void MergeFrom(OclMat<TDepth>[] gpuMats)
      {
         Debug.Assert(NumberOfChannels == gpuMats.Length, "Number of channels does not agrees with the length of gpuMats");
         //If single channel, perform a copy
         if (NumberOfChannels == 1)
         {
               OclInvoke.Copy(gpuMats[0].Ptr, _ptr, IntPtr.Zero);
         }

         //handle multiple channels
         Size size = Size;
         IntPtr[] ptrs = new IntPtr[gpuMats.Length];
         for (int i = 0; i < gpuMats.Length; i++)
         {
            Debug.Assert(gpuMats[i].Size == size, "Size mismatch");
            ptrs[i] = gpuMats[i].Ptr;
         }
         GCHandle handle = GCHandle.Alloc(ptrs, GCHandleType.Pinned);
         OclInvoke.Merge(handle.AddrOfPinnedObject(), _ptr);
         handle.Free();
      }

      public bool Equals(OclMat<TDepth> other)
      {
         throw new NotImplementedException();
      }
   }
}
