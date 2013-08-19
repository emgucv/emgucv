//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV.Structure;
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
         : this(OclInvoke.OclMatCreateDefault())
      {
      }

      /// <summary>
      /// Create an OclMat from the unmanaged pointer
      /// </summary>
      /// <param name="ptr">The unmanaged pointer to the OclMat</param>
      internal OclMat(IntPtr ptr)
      {
         _ptr = ptr;
      }

      /// <summary>
      /// Release the unmanaged memory associated with this OclMat
      /// </summary>
      protected override void DisposeObject()
      {
         OclInvoke.OclMatRelease(ref _ptr);
      }

      /// <summary>
      /// Check if the OclMat is Empty
      /// </summary>
      public bool IsEmpty
      {
         get
         {
            return OclInvoke.OclMatIsEmpty(_ptr);
         }
      }

      /// <summary>
      /// Check if the OclMat is Continuous
      /// </summary>
      public bool IsContinuous
      {
         get
         {
            return OclInvoke.OclMatIsContinuous(_ptr);
         }
      }

      /// <summary>
      /// Get the OclMat type
      /// </summary>
      public int Type
      {
         get { return OclInvoke.OclMatGetType(_ptr); }
      }

      /// <summary>
      /// Get the OclMat size:
      /// width == number of columns, height == number of rows
      /// </summary>
      public Size Size
      {
         get { return OclInvoke.OclMatGetSize(_ptr); }
      }

      /// <summary>
      /// Get the number of rows
      /// </summary>
      public int Rows
      {
         get { return Size.Height; }
      }

      /// <summary>
      /// Get the number of columns
      /// </summary>
      public int Cols
      {
         get { return Size.Width; }
      }

      /// <summary>
      /// Get the OclMat size:
      /// width == wholcols, height == wholerows
      /// </summary>
      public Size WholeSize
      {
         get { return OclInvoke.OclMatGetWholeSize(_ptr); }
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
#region constructors
      /// <summary>
      /// Create an OclMat from the unmanaged pointer
      /// </summary>
      /// <param name="ptr">The unmanaged pointer to the OclMat</param>
      public OclMat(IntPtr ptr)
         :base(ptr)
      {
      }

      /// <summary>
      /// Create an empty OclMat
      /// </summary>
      public OclMat()
         : base()
      {
      }

      /// <summary>
      /// Create an OclMat of the specified size
      /// </summary>
      /// <param name="rows">The number of rows (height)</param>
      /// <param name="cols">The number of columns (width)</param>
      /// <param name="channels">The number of channels</param>
      public OclMat(int rows, int cols, int channels)
         : base(OclInvoke.OclMatCreate(
         rows, 
         cols, 
         CvInvoke.CV_MAKETYPE((int)CvToolbox.GetMatrixDepth(typeof(TDepth)), channels)))
      {
      }

      /// <summary>
      /// Create an OclMat of the specified size
      /// </summary>
      /// <param name="size">The size of the OclMat</param>
      /// <param name="channels">The number of channels</param>
      public OclMat(Size size, int channels)
         : this(size.Height, size.Width, channels)
      {
      }

      /// <summary>
      /// Create an OclMat from an CvArray of the same depth type
      /// </summary>
      /// <param name="arr">The CvArry to be converted to OclMat</param>
      public OclMat(CvArray<TDepth> arr)
         : base(OclInvoke.OclMatCreateFromArr(arr))
      {
      }
#endregion

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
         Debug.Assert(arr.Size.Equals(WholeSize), "Destination CvArray size does not match source OclMat wholesize");
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

      /// <summary>
      /// Changes shape of OclMat without copying data.
      /// </summary>
      /// <param name="newCn">New number of channels. newCn = 0 means that the number of channels remains unchanged.</param>
      /// <param name="newRows">New number of rows. newRows = 0 means that the number of rows remains unchanged unless it needs to be changed according to newCn value.</param>
      /// <returns>An OclMat of different shape</returns>
      public OclMat<TDepth> Reshape(int newCn, int newRows)
      {
         OclMat<TDepth> result = new OclMat<TDepth>();
         OclInvoke.OclMatReshape(this, result, newCn, newRows);
         return result;
      }

      /// <summary>
      /// Copies scalar value to every selected element of the destination OclMat:
      /// OclMat(I)=value if mask(I)!=0
      /// </summary>
      /// <param name="value">Fill value</param>
      /// <param name="mask">Operation mask, 8-bit single channel OclMat; specifies elements of destination array to be changed. Can be null if not used.</param>
      public void SetTo(MCvScalar value, OclMat<Byte> mask)
      {
         OclInvoke.OclMatSetTo(_ptr, value, mask);
      }

      /// <summary>
      /// Returns true if the two GpuMat equals
      /// </summary>
      /// <param name="other">The other GpuMat to be compares with</param>
      /// <returns>True if the two GpuMat equals</returns>
      public bool Equals(OclMat<TDepth> other)
      {
         if (NumberOfChannels != other.NumberOfChannels || Size != other.Size) return false;

         using (OclMat<TDepth> xor = new OclMat<TDepth>(Size, NumberOfChannels))
         {
            OclInvoke.BitwiseXor(_ptr, other, xor, IntPtr.Zero);

            if (NumberOfChannels == 3)
            {
               //we cannot apply count non-zeros on 3 channel oclMat because 3 channel oclMat actually contains 4 channels
               //The 4th channel may contains randome element and we only wish to check for the first 3 channels.
               //so we split up the channels and count one by one.
               OclMat[] channels = xor.Split();
               try
               {
                  for (int i = 0; i < channels.Length; i++)
                  {
                     int nonZero = OclInvoke.CountNonZero(channels[i]);
                     if (nonZero > 0)
                        return false;
                  }
                  return true;
               }
               finally
               {
                  for (int i = 0; i < channels.Length; i++)
                  {
                     channels[i].Dispose();
                     channels[i] = null;
                  }
               }
            }

            return OclInvoke.CountNonZero(xor) == 0;

         }         
      }

   }
}
