//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
      public CudaStereoBM(int numberOfDisparities = 64, int blockSize = 19)
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
      public void FindStereoCorrespondence(IInputArray left, IInputArray right, IOutputArray disparity, Stream stream = null)
      {
         using (InputArray iaLeft = left.GetInputArray())
         using (InputArray iaRight = right.GetInputArray())
         using (OutputArray oaDisparity = disparity.GetOutputArray())
            CudaInvoke.cudaStereoBMFindStereoCorrespondence(_ptr, iaLeft, iaRight, oaDisparity, stream);
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