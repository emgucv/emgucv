/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// An ImageFeature contains a keypoint and its descriptor
   /// </summary>
   /// <typeparam name="TDescriptor">The type of data used for the descriptor. Either float or Byte</typeparam>
#if !NETFX_CORE
   [Serializable]
#endif
   public struct ImageFeature<TDescriptor>
      where TDescriptor: struct
   {
      private MKeyPoint _keyPoint;
      private TDescriptor[] _descriptor;

      /// <summary>
      /// The descriptor to the keypoint
      /// </summary>
      public TDescriptor[] Descriptor
      {
         get { return _descriptor; }
         set { _descriptor = value; }
      }

      /// <summary>
      /// The keypoint
      /// </summary>
      public MKeyPoint KeyPoint
      {
         get { return _keyPoint; }
         set { _keyPoint = value; }
      }

      /// <summary>
      /// Convert the raw keypoints and descriptors to ImageFeature
      /// </summary>
      /// <param name="keyPointsVec">The raw keypoints vector</param>
      /// <param name="descriptors">The raw descriptor matrix</param>
      /// <returns>An array of image features</returns>
      public static ImageFeature<TDescriptor>[] ConvertFromRaw(VectorOfKeyPoint keyPointsVec, Matrix<TDescriptor> descriptors)
      {
         if (keyPointsVec.Size == 0) return new ImageFeature<TDescriptor>[0];
         Debug.Assert(keyPointsVec.Size == descriptors.Rows, "Size of keypoints vector do not match the rows of the descriptors matrix.");
         int sizeOfdescriptor = descriptors.Cols;
         MKeyPoint[] keyPoints = keyPointsVec.ToArray();
         ImageFeature<TDescriptor>[] features = new ImageFeature<TDescriptor>[keyPoints.Length];
         MCvMat header = descriptors.MCvMat;
         long address = header.Data.ToInt64();
         int rowSizeInByte = sizeOfdescriptor * Marshal.SizeOf(typeof(TDescriptor));
         for (int i = 0; i < keyPoints.Length; i++, address += header.Step)
         {
            features[i].KeyPoint = keyPoints[i];
            TDescriptor[] desc = new TDescriptor[sizeOfdescriptor];
            GCHandle handler = GCHandle.Alloc(desc, GCHandleType.Pinned);
            Toolbox.memcpy(handler.AddrOfPinnedObject(), new IntPtr(address), rowSizeInByte);
            handler.Free();
            features[i].Descriptor = desc;
         }
         return features;
      }

      /// <summary>
      /// Convert the image features to keypoint vector and descriptor matrix
      /// </summary>
      public static void ConvertToRaw(ImageFeature<TDescriptor>[] features, out VectorOfKeyPoint keyPoints, out Matrix<TDescriptor> descriptors)
      {
         if (features.Length == 0)
         {
            keyPoints = null;
            descriptors = null;
            return;
         }
         keyPoints = new VectorOfKeyPoint();
         keyPoints.Push(
#if NETFX_CORE
            Extensions.
#else
            Array.
#endif
            ConvertAll<ImageFeature<TDescriptor>, MKeyPoint>(features, delegate(ImageFeature<TDescriptor> feature) { return feature.KeyPoint; }));

         descriptors = new Matrix<TDescriptor>(features.Length, features[0].Descriptor.Length);
         int descriptorLength = features[0].Descriptor.Length;
         int rowSizeInByte = descriptorLength * Marshal.SizeOf(typeof(TDescriptor));
         MCvMat header = descriptors.MCvMat;
         long address = header.Data.ToInt64();
         for (int i = 0; i < features.Length; i++, address += header.Step)
         {
            GCHandle handler = GCHandle.Alloc(features[i].Descriptor, GCHandleType.Pinned);
            Toolbox.memcpy(new IntPtr(address), handler.AddrOfPinnedObject(), rowSizeInByte);
            handler.Free();
         }
      }
   }
}
*/