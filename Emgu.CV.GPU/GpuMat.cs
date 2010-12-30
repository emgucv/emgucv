using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Drawing;
using System.Diagnostics;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// Similar to CvArray but use GPU for processing
   /// </summary>
   /// <typeparam name="TDepth">The type of element in the matrix</typeparam>
   public class GpuMat<TDepth> : UnmanagedObject, IEquatable<GpuMat<TDepth>>
      where TDepth : new()
   {
      /// <summary>
      /// Create a GpuMat of the specified size
      /// </summary>
      /// <param name="rows">The number of rows (height)</param>
      /// <param name="cols">The number of columns (width)</param>
      /// <param name="channels">The number of channels</param>
      public GpuMat(int rows, int cols, int channels)
      {
         _ptr = GpuInvoke.gpuMatCreate(rows, cols, CvInvoke.CV_MAKETYPE((int)Util.GetMatrixDepth(typeof(TDepth)), channels));
      }

      /// <summary>
      /// Create a GpuMat of the specified size
      /// </summary>
      /// <param name="size">The size of the GpuMat</param>
      /// <param name="channels">The number of channels</param>
      public GpuMat(Size size, int channels)
         : this(size.Height, size.Width, channels)
      {
      }

      /// <summary>
      /// Create a GpuMat from an CvArray of the same depth type
      /// </summary>
      /// <param name="arr">The CvArry to be converted to GpuMat</param>
      public GpuMat(CvArray<TDepth> arr)
      {
         _ptr = GpuInvoke.gpuMatCreateFromArr(arr);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this GpuMat
      /// </summary>
      protected override void DisposeObject()
      {
         GpuInvoke.gpuMatRelease(ref _ptr);
      }

      /// <summary>
      /// Get the GpuMat size:
      /// width == number of columns, height == number of rows
      /// </summary>
      public Size Size
      {
         get { return GpuInvoke.gpuMatGetSize(_ptr); }
      }

      /// <summary>
      /// Get the number of channels in the GpuMat
      /// </summary>
      public int NumberOfChannels
      {
         get { return GpuInvoke.gpuMatGetChannels(_ptr); }
      }

      /// <summary>
      /// Pefroms blocking upload data to GpuMat
      /// </summary>
      /// <param name="arr">The CvArray to be uploaded to GpuMat</param>
      public void Upload(CvArray<TDepth> arr)
      {
         GpuInvoke.gpuMatUpload(_ptr, arr);
      }

      /// <summary>
      /// Downloads data from device to host memory. Blocking calls
      /// </summary>
      /// <param name="arr">The destination CvArray where the GpuMat data will be downloaded to.</param>
      public void Download(CvArray<TDepth> arr)
      {
         GpuInvoke.gpuMatDownload(_ptr, arr);
      }

      ///<summary> 
      ///Split current Image into an array of gray scale images where each element 
      ///in the array represent a single color channel of the original image
      ///</summary>
      ///<param name="gpuMats"> 
      ///An array of single channel GpuMat where each item
      ///in the array represent a single channel of the original GpuMat 
      ///</param>
      public void SplitInto(GpuMat<TDepth>[] gpuMats)
      {
         Debug.Assert(NumberOfChannels == gpuMats.Length, "Number of channels does not agrees with the length of gpuMats");
         //If single channel, return a copy
         if (NumberOfChannels == 1) GpuInvoke.gpuMatCopy(_ptr, gpuMats[0], IntPtr.Zero);

         //handle multiple channels
         Size size = Size;
         IntPtr[] ptrs = new IntPtr[gpuMats.Length];
         for (int i = 0; i < gpuMats.Length; i++)
         {
            Debug.Assert(gpuMats[i].Size == size, "Size mismatch");
            ptrs[i] = gpuMats[i].Ptr;
         }
         GCHandle handle = GCHandle.Alloc(ptrs, GCHandleType.Pinned);
         GpuInvoke.gpuMatSplit(_ptr, handle.AddrOfPinnedObject());
         handle.Free();
      }

      /// <summary>
      /// Makes multi-channel array out of several single-channel arrays
      /// </summary>
      ///<param name="gpuMats"> 
      ///An array of single channel GpuMat where each item
      ///in the array represent a single channel of the GpuMat 
      ///</param>
      public void MergeFrom(GpuMat<TDepth>[] gpuMats)
      {
         Debug.Assert(NumberOfChannels == gpuMats.Length, "Number of channels does not agrees with the length of gpuMats");
         //If single channel, perform a copy
         if (NumberOfChannels == 1) GpuInvoke.gpuMatCopy(gpuMats[0].Ptr, _ptr, IntPtr.Zero);

         //handle multiple channels
         Size size = Size;
         IntPtr[] ptrs = new IntPtr[gpuMats.Length];
         for (int i = 0; i < gpuMats.Length; i++)
         {
            Debug.Assert(gpuMats[i].Size == size, "Size mismatch");
            ptrs[i] = gpuMats[i].Ptr;
         }
         GCHandle handle = GCHandle.Alloc(ptrs, GCHandleType.Pinned);
         GpuInvoke.gpuMatMerge(handle.AddrOfPinnedObject(), _ptr);
         handle.Free();
      }

      ///<summary> 
      ///Split current GpuMat into an array of single channel GpuMat where each element 
      ///in the array represent a single channel of the original GpuMat
      ///</summary>
      ///<returns> 
      ///An array of single channel GpuMat where each element  
      ///in the array represent a single channel of the original GpuMat 
      ///</returns>
      public GpuMat<TDepth>[] Split()
      {
         GpuMat<TDepth>[] result = new GpuMat<TDepth>[NumberOfChannels];
         Size size = Size;
         for (int i = 0; i < result.Length; i++)
         {
            result[i] = new GpuMat<TDepth>(size, 1);
         }

         SplitInto(result);
         return result;
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

         if (NumberOfChannels == 1)
         {
            GpuInvoke.gpuMatMinMaxLoc(Ptr, ref minValues[0], ref maxValues[0], ref minLocations[0], ref maxLocations[0], IntPtr.Zero);
         }
         else
         {
            GpuMat<TDepth>[] channels = Split();
            try
            {
               for (int i = 0; i < NumberOfChannels; i++)
               {
                  GpuInvoke.gpuMatMinMaxLoc(Ptr, ref minValues[i], ref maxValues[i], ref minLocations[i], ref maxLocations[i], IntPtr.Zero);
               }
            }
            finally
            {
               foreach (GpuMat<TDepth> mat in channels) mat.Dispose();
            }
         }
      }

      /// <summary>
      /// Returns true if the two GpuMat equals
      /// </summary>
      /// <param name="other">The other GpuMat to be compares with</param>
      /// <returns>True if the two GpuMat equals</returns>
      public bool Equals(GpuMat<TDepth> other)
      {
         if (NumberOfChannels != other.NumberOfChannels || Size != other.Size) return false;

         using (GpuMat<TDepth> xor = new GpuMat<TDepth>(Size, NumberOfChannels))
         {
            GpuInvoke.gpuMatBitwiseXor(_ptr, other, xor, IntPtr.Zero);

            if (xor.NumberOfChannels == 1)
               return GpuInvoke.gpuMatCountNonZero(xor) == 0;
            else
            {
               GpuMat<TDepth>[] channels = xor.Split();
               try
               {
                  return Array.TrueForAll(channels, delegate(GpuMat<TDepth> gi) { return GpuInvoke.gpuMatCountNonZero(gi) == 0; });
               }
               finally
               {
                  foreach (GpuMat<TDepth> gi in channels) gi.Dispose();
               }
            }
         }
      }
   }
}
