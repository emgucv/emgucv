//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

/*

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cvb
{
   /// <summary>
   /// Wrapper for the CvBlob detection functions.
   /// The Ptr property points to the label image of the cvb::cvLabel function.
   /// </summary>
   /// <remarks>Algorithm based on paper "A linear-time component-labeling algorithm using contour tracing technique" of Fu Chang, Chun-Jen Chen and Chi-Jen Lu.</remarks>
   public class CvBlobDetector : UnmanagedObject
   {
      static CvBlobDetector()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      private uint[,] _data;
      private GCHandle _dataHandle;

      private static int _sizeOfUInt32 = Toolbox.SizeOf<UInt32>();

      /// <summary>
      /// Detect blobs from input image.
      /// </summary>
      /// <param name="img">The input image</param>
      /// <param name="blobs">The storage for the detected blobs</param>
      /// <returns>Number of pixels that has been labeled.</returns>
      public uint Detect(Image<Gray, Byte> img, CvBlobs blobs)
      {
         Size size = img.Size;

         if (_data == null || _data.GetLength(0) != size.Height || _data.GetLength(1) != size.Width)
         {
            DisposeObject();

            _data = new UInt32[size.Height, size.Width];
            _dataHandle = GCHandle.Alloc(_data, GCHandleType.Pinned);
            _ptr = CvInvoke.cvCreateImageHeader(size, (CvEnum.IplDepth)(_sizeOfUInt32 * 8), 1);
            CvInvoke.cvSetData(_ptr, _dataHandle.AddrOfPinnedObject(), _sizeOfUInt32 * size.Width);
         }

         return cvbCvLabel(img, _ptr, blobs);
      }

      /// <summary>
      /// Calculates mean color of a blob in an image.
      /// </summary>
      /// <param name="blob">The blob.</param>
      /// <param name="originalImage">The original image</param>
      /// <returns>Average color</returns>
      public Bgr MeanColor(CvBlob blob, Image<Bgr, Byte> originalImage)
      {
         Bgr color = new Bgr();
         color.MCvScalar = cvbCvBlobMeanColor(blob, _ptr, originalImage);
         return color;
      }

      /// <summary>
      /// Blob rendering type
      /// </summary>
      [Flags]
      public enum BlobRenderType
      {
         /// <summary>
         /// Render each blog with a different color. 
         /// </summary>
         Color = 0x0001,
         /// <summary>
         /// Render centroid. 
         /// </summary>
         Centroid = 0x0002,
         /// <summary>
         /// Render bounding box. 
         /// </summary>
         BoundingBox = 0x0004,
         /// <summary>
         /// Render angle. 
         /// </summary>
         Angle = 0x0008,
         /// <summary>
         /// Print blob data to log out. 
         /// </summary>
         ToLog = 0x0010,
         /// <summary>
         /// Print blob data to std out. 
         /// </summary>
         ToStd = 0x0020,
         /// <summary>
         /// The default rendering type
         /// </summary>
         Default = Color | Centroid | BoundingBox | Angle
      }

      /// <summary>
      /// Draw the blobs on the image
      /// </summary>
      /// <param name="image">The binary mask.</param>
      /// <param name="blobs">The blobs.</param>
      /// <param name="type">Drawing type.</param>
      /// <param name="alpha">The alpha value. 1.0 for solid color and 0.0 for transparent</param>
      /// <returns>The images with the blobs drawn</returns>
      public Image<Bgr, Byte> DrawBlobs(Image<Gray, Byte> image, CvBlobs blobs, BlobRenderType type, double alpha)
      {
         Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);
         cvbCvRenderBlobs(Ptr, blobs, image, result, type, alpha);
         return result;
      }

      /// <summary>
      /// Get the binary mask for the blobs listed in the CvBlobs
      /// </summary>
      /// <param name="blobs">The blobs</param>
      /// <returns>The binary mask for the specific blobs</returns>
      public Image<Gray, Byte> DrawBlobsMask(CvBlobs blobs)
      {
         MIplImage img = (MIplImage)Marshal.PtrToStructure(Ptr, typeof(MIplImage));
         Image<Gray, Byte> mask = new Image<Gray, byte>(img.Width, img.Height);
         cvbCvFilterLabels(Ptr, mask, blobs);
         return mask;
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this Blob detector
      /// </summary>
      protected override void DisposeObject()
      {
         if (_data != null)
         {
            _dataHandle.Free();
            CvInvoke.cvReleaseImageHeader(ref _ptr);
            _data = null;
         }
      }

      internal struct BlobColor : IColor
      {
         public MCvScalar MCvScalar
         {
            get
            {
               throw new NotImplementedException();
            }
            set
            {
               throw new NotImplementedException();
            }
         }

         public int Dimension
         {
            get { return 8; }
         }
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static uint cvbCvLabel(IntPtr img, IntPtr imgOut, IntPtr blobs);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvbCvRenderBlobs(IntPtr labelMask, IntPtr blobs, IntPtr imgSource, IntPtr imgDest, Cvb.CvBlobDetector.BlobRenderType mode, double alpha);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static MCvScalar cvbCvBlobMeanColor(IntPtr blob, IntPtr imgLabel, IntPtr img);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvbCvFilterLabels(IntPtr imgIn, IntPtr imgOut, IntPtr blobs);
   }
}
*/