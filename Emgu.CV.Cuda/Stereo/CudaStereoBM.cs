//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

 using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Use Block Matching algorithm to find stereo correspondence
   /// </summary>
   public class CudaStereoBM : UnmanagedObject
   {
      /*
      /// <summary>
      /// Preset type
      /// </summary>
      public enum PresetType
      {
         /// <summary>
         /// Basic
         /// </summary>
         BasicPreset = 0,
         /// <summary>
         /// prefilter xsobel
         /// </summary>
         PrefilterXSobel = 1
      };*/

      /// <summary>
      /// Create a stereoBM 
      /// </summary>
      /// <param name="numberOfDisparities">The number of disparities. Must be multiple of 8. Use 64 for default </param>
      /// <param name="blockSize">The SAD window size. Use 19 for default</param>
      public CudaStereoBM(int numberOfDisparities, int blockSize)
      {
         _ptr = CudaInvoke.cudaStereoBMCreate(numberOfDisparities, blockSize);
      }

      /// <summary>
      /// Computes disparity map for the input rectified stereo pair.
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="disparity">The disparity map</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void FindStereoCorrespondence(CudaImage<Gray, Byte> left, CudaImage<Gray, Byte> right, CudaImage<Gray, Byte> disparity, Stream stream)
      {
         CudaInvoke.cudaStereoBMFindStereoCorrespondence(_ptr, left.InputArrayPtr, right.InputArrayPtr, disparity.OutputArrayPtr, stream);
      }

      /// <summary>
      /// Release the stereo state and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaStereoBMRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaStereoBMCreate(int ndisparities, int blockSize);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaStereoBMFindStereoCorrespondence(IntPtr stereoBM, IntPtr left, IntPtr right, IntPtr disparity, IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaStereoBMRelease(ref IntPtr stereoBM);
   }
}