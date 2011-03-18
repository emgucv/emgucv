//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   internal class RTreeClassifierExtern
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvRTreeClassifierCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvRTreeClassifierRelease(IntPtr classifier);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvRTreeClassifierTrain(
         IntPtr classifier,
         IntPtr trainImage,
         IntPtr trainPoints,
         int numberOfPoints,
         ref UInt64 rng,
         int numTrees, int depth,
         int views, IntPtr reducedNumDim,
         int numQuantBits);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int CvRTreeClassifierGetSigniture(
         IntPtr classifier,
         IntPtr image,
         ref Point point,
         int patchSize,
         IntPtr signiture);


      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int CvRTreeClassifierGetNumClasses(IntPtr classifier);
      #endregion
   }

   /// <summary>
   /// The Calonder classifier
   /// </summary>
   public class RTreeClassifier<TColor> : UnmanagedObject where TColor : struct, IColor
   {
      /// <summary>
      /// Create an Calonder classifier
      /// </summary>
      public RTreeClassifier()
      {
         _ptr = RTreeClassifierExtern.CvRTreeClassifierCreate();
      }

      /// <summary>
      /// Train the calonder classifier with the specific images
      /// </summary>
      /// <param name="trainImage">The traning image</param>
      /// <param name="keypoints">The keypoints on this image</param>
      /// <param name="numTrees">Use 48 for default</param>
      /// <param name="depth">Use 9 for default</param>
      /// <param name="views">Use 5000 for default</param>
      /// <param name="reducedNumDim">use 176 for default</param>
      /// <param name="numQuantBits">Use 4 for default</param>
      public void Train(
      Image<TColor, Byte> trainImage,
      Point[] keypoints,
      int numTrees, int depth,
      int views, int reducedNumDim,
      int numQuantBits)
      {
         Debug.Assert(reducedNumDim <= keypoints.Length, "ReducedNumDim should be smaller or equals the number of keypoints");

         Random r = new Random();
         UInt64 rng = (UInt64)r.Next();
         GCHandle handle = GCHandle.Alloc(keypoints, GCHandleType.Pinned);
         RTreeClassifierExtern.CvRTreeClassifierTrain(
            _ptr,
            trainImage,
            handle.AddrOfPinnedObject(),
            keypoints.Length,
            ref rng,
            numTrees,
            depth,
            views,
            new IntPtr(reducedNumDim),
            numQuantBits);

         handle.Free();
      }

      /// <summary>
      /// Release the unmanaged resource associated with this classifier
      /// </summary>
      protected override void DisposeObject()
      {
         RTreeClassifierExtern.CvRTreeClassifierRelease(_ptr);
      }

      /// <summary>
      /// Get the number of class used in this classifier
      /// </summary>
      public int NumberOfClasses
      {
         get
         {
            return RTreeClassifierExtern.CvRTreeClassifierGetNumClasses(_ptr);
         }
      }

      /// <summary>
      /// Compute the signiture from the given location on the image
      /// </summary>
      /// <param name="image">The image to compute signiture from</param>
      /// <param name="keypoint">The location of the key point</param>
      /// <param name="patchSize">The size of the patch</param>
      /// <returns>null if the signiture cannot be computed, otherwise the signiture itself is returned.</returns>
      public float[] GetSigniture(Image<TColor, Byte> image, Point keypoint, int patchSize)
      {
         float[] result = new float[NumberOfClasses];
         GCHandle handle = GCHandle.Alloc(result, GCHandleType.Pinned);
         int count = RTreeClassifierExtern.CvRTreeClassifierGetSigniture(_ptr, image, ref keypoint, patchSize, handle.AddrOfPinnedObject());
         handle.Free();
         return count == 0 ? null : result;
      }
   }
}
