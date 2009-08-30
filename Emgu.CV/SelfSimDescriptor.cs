using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using System.Drawing;
using System.Diagnostics;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// SelfSimDescriptor
   /// </summary>
   public class SelfSimDescriptor : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static IntPtr CvSelfSimDescriptorCreate(
         int smallSize,
         int largeSize,
         int startDistanceBucket,
         int numberOfDistanceBuckets,
         int numberOfAngles);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvSelfSimDescriptorRelease(IntPtr descriptor);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvSelfSimDescriptorCompute(
         IntPtr descriptor,
         IntPtr image,
         IntPtr descriptors,
         Size winStride,
         IntPtr locations,
         int sizeOfLocation
         );

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static int CvSelfSimDescriptorGetDescriptorSize(IntPtr descriptor);
      #endregion

      /// <summary>
      /// 
      /// </summary>
      /// <param name="smallSize">Use 5 for default</param>
      /// <param name="largeSize">Use 41 for default</param>
      /// <param name="startDistanceBucket">Use 3 for default</param>
      /// <param name="numberOfDistanceBuckets">Use 7 for default</param>
      /// <param name="numberOfAngles">Use 20 for default</param>
      public SelfSimDescriptor(
         int smallSize,
         int largeSize,
         int startDistanceBucket,
         int numberOfDistanceBuckets,
         int numberOfAngles)
      {
         _ptr = CvSelfSimDescriptorCreate(smallSize, largeSize, startDistanceBucket, numberOfDistanceBuckets, numberOfAngles);
      }

      /// <summary>
      /// Get the size of the descriptor
      /// </summary>
      public int DescriptorSize
      {
         get
         {
            return CvSelfSimDescriptorGetDescriptorSize(_ptr);
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="image"></param>
      /// <param name="winStride"></param>
      /// <param name="locations"></param>
      /// <returns></returns>
      public float[] Compute(Image<Gray, Byte> image, Size winStride, Point[] locations)
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<float> seq = new Seq<float>(stor);
            GCHandle handle = GCHandle.Alloc(locations, GCHandleType.Pinned);
            CvSelfSimDescriptorCompute(_ptr, image, seq.Ptr, winStride, handle.AddrOfPinnedObject(), locations.Length);
            handle.Free();
            return seq.ToArray();
         }
      }

      /// <summary>
      /// Release all unmanaged memory associated with this descriptor
      /// </summary>
      protected override void DisposeObject()
      {
         CvSelfSimDescriptorRelease(_ptr);
      }
   }
}
