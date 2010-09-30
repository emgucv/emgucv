using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Geodetic;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.Util;
using System.Drawing;
using System.Diagnostics;

namespace Emgu.CV.Tiff
{

   internal static partial class TIFFInvoke
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr tiffWriterOpen(
         [MarshalAs(CvInvoke.StringMarshalType)]
         string fileSpec);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void tiffWriterClose(ref IntPtr pTiff);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void tiffWriteGeoTag(IntPtr pTiff, IntPtr modelTiepoint, IntPtr ModelPixelScale);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void tiffWriteImage(IntPtr pTiff, IntPtr image);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void tiffWriteImageInfo(IntPtr pTiff, int bitsPerSample, int samplesPerPixel);
      #endregion
   }

   /// <summary>
   /// A class that can be used for writing geotiff
   /// </summary>
   public class TiffWriter<TColor, TDepth> : UnmanagedObject
      where TColor : struct, IColor
      where TDepth : new()
   {

      /// <summary>
      /// Create a tiff writer to save an image
      /// </summary>
      /// <param name="fileName">The file name to be saved</param>
      public TiffWriter(String fileName)
      {
         _ptr = TIFFInvoke.tiffWriterOpen(fileName);
         TIFFInvoke.tiffWriteImageInfo(_ptr, Image<TColor, TDepth>.SizeOfElement * 8, new TColor().Dimension);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="image"></param>
      public virtual void WriteImage(Image<TColor, TDepth> image)
      {
         if (image is Image<Gray, Byte>)
         {
            TIFFInvoke.tiffWriteImage(_ptr, image.Ptr);
         }
         else if (image is Image<Bgra, Byte>)
         {
            //swap the B and R channel since geotiff assume RGBA for 4 channels image of depth Byte
            using (Image<Bgra, Byte> clone = (image as Image<Bgra, Byte>).Clone())
            using (Image<Gray, Byte> b = clone[0])
            using (Image<Gray, Byte> r = clone[2])
            {
               clone[2] = b;
               clone[0] = r;
               TIFFInvoke.tiffWriteImage(_ptr, clone.Ptr);
            }
         }
         else if (image is Image<Bgr, Byte>)
         {
            //swap the B and R channel since geotiff assume RGB for 3 channels image of depth Byte
            using (Image<Bgr, Byte> clone = (image as Image<Bgr, Byte>).Clone())
            using (Image<Gray, Byte> b = clone[0])
            using (Image<Gray, Byte> r = clone[2])
            {
               clone[2] = b;
               clone[0] = r;
               TIFFInvoke.tiffWriteImage(_ptr, clone);
            }
         }
         else
         {
            throw new NotImplementedException("The specific image type is not supported");
         }
      }

      /// <summary>
      /// Write the geo information into the tiff file
      /// </summary>
      /// <param name="originCoordinate">The coordinate of the origin. To be specific, this is the coordinate of the upper left corner of the pixel in the origin</param>
      /// <param name="pixelResolution">The resolution of the pixel in meters</param>
      /// <param name="imageSize">The size of the image</param>
      public void WriteGeoTag(GeodeticCoordinate originCoordinate, Size imageSize, MCvPoint2D64f pixelResolution)
      {
         GeodeticCoordinate lowerRight = TransformationWGS84.NED2Geodetic(
            new MCvPoint3D64f(pixelResolution.x * imageSize.Height, pixelResolution.y * imageSize.Width, 0.0),
            originCoordinate);
         MCvPoint3D64f res = new MCvPoint3D64f(
            (lowerRight.Longitude - originCoordinate.Longitude) * (180.0 / Math.PI) / imageSize.Width,
            (lowerRight.Latitude - originCoordinate.Latitude) * (180.0 / Math.PI) / imageSize.Height,
            0.0);

         double latitude = lowerRight.Latitude * (180.0 / Math.PI);
         double longitude = lowerRight.Longitude * (180.0 / Math.PI);
         double[] ModelTiepoint = { 
            0, 0, 0, 
            longitude, latitude, 0 };
         double[] ModelPixelScale = { pixelResolution.x, pixelResolution.y, 0.0 };
      }

      /// <summary>
      /// Write the geo information into the tiff file
      /// </summary>
      /// <param name="modelTiepoint"></param>
      /// <param name="modelPixelScale"></param>
      public void WriteGeoTag(double[] modelTiepoint, double[] modelPixelScale)
      {
         Debug.Assert(modelTiepoint.Length == 6, "Model Tiepoint should have length of 6");
         Debug.Assert(modelPixelScale.Length == 3, "Model Pixel Scale should have length of 3");

         GCHandle tiepointHandle = GCHandle.Alloc(modelTiepoint, GCHandleType.Pinned);
         GCHandle pixelScaleHandle = GCHandle.Alloc(modelPixelScale, GCHandleType.Pinned);

         TIFFInvoke.tiffWriteGeoTag(_ptr, tiepointHandle.AddrOfPinnedObject(), pixelScaleHandle.AddrOfPinnedObject());

         tiepointHandle.Free();
         pixelScaleHandle.Free();
      }

      /// <summary>
      /// Release the writer and write all data on to disk.
      /// </summary>
      protected override void DisposeObject()
      {
         TIFFInvoke.tiffWriterClose(ref _ptr);
      }
   }
}
