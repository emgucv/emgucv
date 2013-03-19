//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV.Util
{
   /// <summary>
   /// Wraped class of the C++ standard vector of MKeyPoint.
   /// </summary>
#if !NETFX_CORE
   [Serializable]
   public class VectorOfKeyPoint : Emgu.Util.UnmanagedObject, ISerializable
#else
   public class VectorOfKeyPoint : Emgu.Util.UnmanagedObject
#endif
   {

#if !NETFX_CORE
      /// <summary>
      /// Constructor used to deserialize runtime serialized object
      /// </summary>
      /// <param name="info">The serialization info</param>
      /// <param name="context">The streaming context</param>
      public VectorOfKeyPoint(SerializationInfo info, StreamingContext context)
         : this()
      {
         Push((MKeyPoint[])info.GetValue("KeyPoints", typeof(MKeyPoint[])));
      }
#endif

      /// <summary>
      /// Create an empty standard vector of KeyPoint
      /// </summary>
      public VectorOfKeyPoint()
      {
         _ptr = CvInvoke.VectorOfKeyPointCreate();
      }

      /// <summary>
      /// Create an standard vector of KeyPoint of the specific size
      /// </summary>
      /// <param name="size">The size of the vector</param>
      public VectorOfKeyPoint(int size)
      {
         _ptr = CvInvoke.VectorOfKeyPointCreateSize(size);
      }

      /// <summary>
      /// Push an array of value into the standard vector
      /// </summary>
      /// <param name="value">The value to be pushed to the vector</param>
      public void Push(MKeyPoint[] value)
      {
         if (value.Length > 0)
         {
            GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            CvInvoke.VectorOfKeyPointPushMulti(_ptr, handle.AddrOfPinnedObject(), value.Length);
            handle.Free();
         }
      }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public int Size
      {
         get
         {
            return CvInvoke.VectorOfKeyPointGetSize(_ptr);
         }
      }

      /// <summary>
      /// Clear the vector
      /// </summary>
      public void Clear()
      {
         CvInvoke.VectorOfKeyPointClear(_ptr);
      }

      /// <summary>
      /// The pointer to the first element on the vector. In case of an empty vector, IntPtr.Zero will be returned.
      /// </summary>
      public IntPtr StartAddress
      {
         get
         {
            return CvInvoke.VectorOfKeyPointGetStartAddress(_ptr);
         }
      }

      /// <summary>
      /// Convert the standard vector to an array of KeyPoint
      /// </summary>
      /// <returns>An array of KeyPoint</returns>
      public MKeyPoint[] ToArray()
      {
         MKeyPoint[] res = new MKeyPoint[Size];
         if (res.Length > 0)
         {
            GCHandle handle = GCHandle.Alloc(res, GCHandleType.Pinned);
            CvInvoke.VectorOfKeyPointCopyData(_ptr, handle.AddrOfPinnedObject());
            handle.Free();
         }
         return res;
      }
      /// <summary>
      /// Remove keypoints within borderPixels of an image edge.
      /// </summary>
      /// <param name="imageSize">Image size</param>
      /// <param name="borderSize">Border size in pixel</param>
      public void FilterByImageBorder(Size imageSize, int borderSize)
      {
         CvInvoke.VectorOfKeyPointFilterByImageBorder(Ptr, imageSize, borderSize);
      }

      /// <summary>
      /// Remove keypoints of sizes out of range.
      /// </summary>
      /// <param name="minSize">Minimum size</param>
      /// <param name="maxSize">Maximum size</param>
      public void FilterByKeypointSize(float minSize, float maxSize)
      {
         CvInvoke.VectorOfKeyPointFilterByKeypointSize(Ptr, minSize, maxSize);
      }

      /// <summary>
      /// Remove keypoints from some image by mask for pixels of this image.
      /// </summary>
      /// <param name="mask">The mask</param>
      public void FilterByPixelsMask(Image<Gray, Byte> mask)
      {
         CvInvoke.VectorOfKeyPointFilterByPixelsMask(Ptr, mask);
      }

      /// <summary>
      /// Get the item in the specific index
      /// </summary>
      /// <param name="index">The index</param>
      /// <returns>The item in the specific index</returns>
      public MKeyPoint this[int index]
      {
         get
         {
            MKeyPoint result = new MKeyPoint();
            CvInvoke.VectorOfKeyPointGetItem(_ptr, index, ref result);
            return result;
         }
      }

      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.VectorOfKeyPointRelease(_ptr);
      }

#if !NETFX_CORE
      /// <summary>
      /// A function used for runtime serialization of the object
      /// </summary>
      /// <param name="info">Serialization info</param>
      /// <param name="context">Streaming context</param>
      public void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.AddValue("KeyPoints", ToArray());
      }
#endif
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfKeyPointCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfKeyPointCreateSize(int size);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyPointRelease(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfKeyPointGetSize(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyPointCopyData(IntPtr v, IntPtr data);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfKeyPointGetStartAddress(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyPointPushMulti(IntPtr v, IntPtr values, int count);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyPointClear(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyPointFilterByImageBorder(IntPtr keypoints, Size imageSize, int borderSize);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyPointFilterByKeypointSize(IntPtr keypoints, float minSize, float maxSize);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyPointFilterByPixelsMask(IntPtr keypoints, IntPtr mask);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfKeyPointGetItem(IntPtr keypoints, int index, ref MKeyPoint keypoint);
   }
}