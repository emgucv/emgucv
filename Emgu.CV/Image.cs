//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

#if ANDROID
using Bitmap = Android.Graphics.Bitmap;
#elif IOS
using MonoTouch.UIKit;
#elif NETFX_CORE
#else
using System.Drawing.Imaging;
#endif

using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
#if !NETFX_CORE
using System.Security.Permissions;
#endif
using Emgu.CV.Features2D;
using Emgu.CV.Reflection;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// An Image is a wrapper to IplImage of OpenCV. 
   /// </summary>
   /// <typeparam name="TColor">Color type of this image (either Gray, Bgr, Bgra, Hsv, Hls, Lab, Luv, Xyz, Ycc, Rgb or Rbga)</typeparam>
   /// <typeparam name="TDepth">Depth of this image (either Byte, SByte, Single, double, UInt16, Int16 or Int32)</typeparam>
#if !NETFX_CORE
   [Serializable]
#endif
   public partial class Image<TColor, TDepth>
      : CvArray<TDepth>, IImage, IEquatable<Image<TColor, TDepth>>
      where TColor : struct, IColor
      where TDepth : new()
   {
      private ImageDataReleaseMode _imageDataReleaseMode;

      private TDepth[, ,] _array;

      /// <summary>
      /// The dimension of color
      /// </summary>
      private static readonly int _numberOfChannels = new TColor().Dimension;

      #region constructors
      ///<summary>
      ///Create an empty Image
      ///</summary>
      protected Image()
      {
      }

      /// <summary>
      /// Create image from the specific multi-dimensional data, where the 1st dimesion is # of rows (height), the 2nd dimension is # cols (width) and the 3rd dimension is the channel
      /// </summary>
      /// <param name="data">The multi-dimensional data where the 1st dimension is # of rows (height), the 2nd dimension is # cols (width) and the 3rd dimension is the channel </param>
      public Image(TDepth[, ,] data)
      {
         Data = data;
      }

      /// <summary>
      /// Create an Image from unmanaged data. 
      /// </summary>
      /// <param name="width">The width of the image</param>
      /// <param name="height">The height of the image</param>
      /// <param name="stride">Size of aligned image row in bytes</param>
      /// <param name="scan0">Pointer to aligned image data, <b>where each row should be 4-align</b> </param>
      /// <remarks>The caller is responsible for allocating and freeing the block of memory specified by the scan0 parameter, however, the memory should not be released until the related Image is released. </remarks>
      public Image(int width, int height, int stride, IntPtr scan0)
      {
         MapDataToImage(width, height, stride, scan0);
      }

      internal void MapDataToImage(int width, int height, int stride, IntPtr scan0)
      {
         _ptr = CvInvoke.cvCreateImageHeader(new Size(width, height), CvDepth, NumberOfChannels);
         MIplImage iplImage = MIplImage;
         iplImage.imageData = scan0;
         iplImage.widthStep = stride;
         Marshal.StructureToPtr(iplImage, _ptr, false);
      }

      /// <summary>
      /// Read image from a file
      /// </summary>
      /// <param name="fileName">the name of the file that contains the image</param>
      public Image(String fileName)
      {
#if NETFX_CORE
         IntPtr ptr = CvInvoke.cvLoadImage(fileName, CvEnum.LOAD_IMAGE_TYPE.CV_LOAD_IMAGE_ANYCOLOR | CvEnum.LOAD_IMAGE_TYPE.CV_LOAD_IMAGE_ANYDEPTH);
         if (ptr == IntPtr.Zero)
            throw new NullReferenceException(String.Format("Unable to load image from file \"{0}\".", fileName));
         try
         {
            LoadImageFromIplImagePtr(ptr);
         } finally
         {
            CvInvoke.cvReleaseImage(ref ptr);
         }
#else

         FileInfo fi = new FileInfo(fileName);
         if (!fi.Exists)
         {
            throw new FileNotFoundException(String.Format("The file {0} could not be not found.", fileName), fileName);
         }

         String extension = fi.Extension.ToLower();
#if IOS
         //Open CV's libpng doesn't seem to be able to handle png in iOS
         //Use UIImage to load png
         if (extension.Equals(".png"))
         {
            using (UIImage tmp = UIImage.FromFile(fileName))
            {
               AllocateData((int)tmp.Size.Height, (int)tmp.Size.Width, NumberOfChannels);
               ConvertFromCGImage(tmp.CGImage);
            }
            return;
         }
#else
         if ((this is Image<Bgra, Byte> && (extension.Equals(".png"))
            || extension.Equals(".tiff") || extension.Equals(".tif")))
         {
            //Open CV is unable to load the alpha channel of the png file, 
            //It is also not able to load some tiff formatted file correctly
            //use Bitmap to load it correctly.
            LoadFileUsingBitmap(fi);
         } else
#endif
         try
         {
#if ANDROID
            int rotation = 0;
            Android.Media.ExifInterface exif = new Android.Media.ExifInterface(fi.FullName);
            int orientation = exif.GetAttributeInt(Android.Media.ExifInterface.TagOrientation, int.MinValue);
            switch (orientation)
            {
               case (int)Android.Media.Orientation.Rotate270:
                  rotation = 270;
                  break;
               case (int)Android.Media.Orientation.Rotate180:
                  rotation = 180;
                  break;
               case (int)Android.Media.Orientation.Rotate90:
                  rotation = 90;
                  break;
            }
            if (rotation == 0)
               LoadImageUsingOpenCV(fi);
            else
            {
               using(Bitmap bmp = Android.Graphics.BitmapFactory.DecodeFile(fi.FullName))
               {
                  Android.Graphics.Matrix matrix = new Android.Graphics.Matrix();
                  matrix.PostRotate(rotation);
                  using (Bitmap rotated = Bitmap.CreateBitmap(bmp, 0, 0, bmp.Width, bmp.Height, matrix, true))
                  {
                     //manually disposed sooner such that memory is released.
                     bmp.Dispose();
                     Bitmap = rotated;
                  }
               }
            }
#else
            LoadImageUsingOpenCV(fi);
#endif
         } catch (TypeInitializationException e)
         {
            //possibly Exception in CvInvoke's static constructor.
            throw e;
         } catch (Exception e)
         {
#if IOS
            using (UIImage tmp = UIImage.FromFile(fileName))
            {
               AllocateData((int)tmp.Size.Height, (int)tmp.Size.Width, NumberOfChannels);
               ConvertFromCGImage(tmp.CGImage);
            }
#else
            //give Bitmap a try
            //and if it cannot load the image, exception will be thrown
            LoadFileUsingBitmap(fi);
#endif
         }
#endif
      }

#if IOS || NETFX_CORE
#else
      /// <summary>
      /// Load the specific file using Bitmap
      /// </summary>
      /// <param name="file"></param>
      private void LoadFileUsingBitmap(FileInfo file)
      {
#if ANDROID
         using (Bitmap bmp = Android.Graphics.BitmapFactory.DecodeFile(file.FullName))
#else
         using (Bitmap bmp = new Bitmap(file.FullName))
#endif
            Bitmap = bmp;

      }
#endif

#if !NETFX_CORE
      /// <summary>
      /// Load the specific file using OpenCV
      /// </summary>
      /// <param name="file"></param>
      private void LoadImageUsingOpenCV(FileInfo file)
      {
         IntPtr ptr = CvInvoke.cvLoadImage(file.FullName, CvEnum.LOAD_IMAGE_TYPE.CV_LOAD_IMAGE_ANYCOLOR | CvEnum.LOAD_IMAGE_TYPE.CV_LOAD_IMAGE_ANYDEPTH);
         if (ptr == IntPtr.Zero)
            throw new NullReferenceException(String.Format("Unable to load image from file \"{0}\".", file.FullName));

         try
         {
            LoadImageFromIplImagePtr(ptr);
         } finally
         {
            CvInvoke.cvReleaseImage(ref ptr);
         }
      }
#endif

#if IOS || NETFX_CORE
#else
      /// <summary>
      /// Obtain the image from the specific Bitmap
      /// </summary>
      /// <param name="bmp">The bitmap which will be converted to the image</param>
      public Image(Bitmap bmp)
      {
         Bitmap = bmp;
      }
#endif

      ///<summary>
      ///Create a blank Image of the specified width, height and color.
      ///</summary>
      ///<param name="width">The width of the image</param>
      ///<param name="height">The height of the image</param>
      ///<param name="value">The initial color of the image</param>
      public Image(int width, int height, TColor value)
         : this(width, height)
      {
         //int n1 = MIplImage.nSize;
         SetValue(value);
         //int n2 = MIplImage.nSize;
         //int nDiff = n2 - n1;
      }

      ///<summary>
      ///Create a blank Image of the specified width and height. 
      ///</summary>
      ///<param name="width">The width of the image</param>
      ///<param name="height">The height of the image</param>
      public Image(int width, int height)
      {
         AllocateData(height, width, NumberOfChannels);
      }

      /// <summary>
      /// Create a blank Image of the specific size
      /// </summary>
      /// <param name="size">The size of the image</param>
      public Image(Size size)
         : this(size.Width, size.Height)
      {
      }

      /// <summary>
      /// Get or Set the data for this matrix. The Get function has O(1) complexity. The Set function make a copy of the data
      /// </summary>
      /// <remarks>
      /// If the image contains Byte and width is not a multiple of 4. The second dimension of the array might be larger than the Width of this image.  
      /// This is necessary since the length of a row need to be 4 align for OpenCV optimization. 
      /// The Set function always make a copy of the specific value. If the image contains Byte and width is not a multiple of 4. The second dimension of the array created might be larger than the Width of this image.  
      /// </remarks>
      public TDepth[, ,] Data
      {
         get
         {
            return _array;
         }
         set
         {
            Debug.Assert(value != null, "Data cannot be set to null");
            Debug.Assert(value.GetLength(2) == NumberOfChannels, "The number of channels must equal");
            AllocateData(value.GetLength(0), value.GetLength(1), NumberOfChannels);
            int rows = value.GetLength(0);
            int valueRowLength = value.GetLength(1) * value.GetLength(2);
            int arrayRowLength = _array.GetLength(1) * _array.GetLength(2);
            for (int i = 0; i < rows; i++)
               Array.Copy(value, i * valueRowLength, _array, i * arrayRowLength, valueRowLength);
         }
      }

      /// <summary>
      /// Allocate data for the array
      /// </summary>
      /// <param name="rows">The number of rows</param>
      /// <param name="cols">The number of columns</param>
      /// <param name="numberOfChannels">The number of channels of this image</param>
      protected override void AllocateData(int rows, int cols, int numberOfChannels)
      {
         DisposeObject();
         Debug.Assert(!_dataHandle.IsAllocated, "Handle should be free");

         _ptr = CvInvoke.cvCreateImageHeader(new Size(cols, rows), CvDepth, numberOfChannels);
         _imageDataReleaseMode = ImageDataReleaseMode.ReleaseHeaderOnly;

         GC.AddMemoryPressure(StructSize.MIplImage);

         Debug.Assert(MIplImage.align == 4, "Only 4 align is supported at this moment");

         if (typeof(TDepth) == typeof(Byte) && (cols & 3) != 0 && (numberOfChannels & 3) != 0)
         {   //if the managed data isn't 4 aligned, make it so
            _array = new TDepth[rows, (cols & (~3)) + 4, numberOfChannels];
         } else
         {
            _array = new TDepth[rows, cols, numberOfChannels];
         }

         _dataHandle = GCHandle.Alloc(_array, GCHandleType.Pinned);
         //int n1 = MIplImage.nSize;
         CvInvoke.cvSetData(_ptr, _dataHandle.AddrOfPinnedObject(), _array.GetLength(1) * _array.GetLength(2) * SizeOfElement);
         //int n2 = MIplImage.nSize;
         //int nDiff = n2 - n1;
      }

      ///<summary>
      ///Create a multi-channel image from multiple gray scale images
      ///</summary>
      ///<param name="channels">The image channels to be merged into a single image</param>
      public Image(Image<Gray, TDepth>[] channels)
      {
         Debug.Assert(NumberOfChannels == channels.Length);
         AllocateData(channels [0].Height, channels [0].Width, NumberOfChannels);

         if (NumberOfChannels == 1)
         {
            //if this image only have a single channel
            CvInvoke.cvCopy(channels [0].Ptr, Ptr, IntPtr.Zero);
         } else
         {
            int channelsCount = channels.Length;
            CvInvoke.cvMerge(
               channelsCount > 0 ? channels [0].Ptr : IntPtr.Zero,
               channelsCount > 1 ? channels [1].Ptr : IntPtr.Zero,
               channelsCount > 2 ? channels [2].Ptr : IntPtr.Zero,
               channelsCount > 3 ? channels [3].Ptr : IntPtr.Zero,
               Ptr);
         }
      }
      #endregion

#if !NETFX_CORE
      #region Implement ISerializable interface
      /// <summary>
      /// Constructor used to deserialize runtime serialized object
      /// </summary>
      /// <param name="info">The serialization info</param>
      /// <param name="context">The streaming context</param>
      public Image(SerializationInfo info, StreamingContext context)
      {
         DeserializeObjectData(info, context);
         ROI = (Rectangle)info.GetValue("Roi", typeof(Rectangle));
      }

      /// <summary>
      /// A function used for runtime serialization of the object
      /// </summary>
      /// <param name="info">Serialization info</param>
      /// <param name="context">streaming context</param>
      [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         if (IsROISet)
         {
            Rectangle roi = ROI;
            ROI = Rectangle.Empty;
            base.GetObjectData(info, context);
            ROI = roi;
            info.AddValue("Roi", roi);
         } else
         {
            base.GetObjectData(info, context);
            info.AddValue("Roi", ROI);
         }
      }
      #endregion
#endif

      #region Image Properties

      /// <summary>
      /// The IplImage structure
      /// </summary>
      public MIplImage MIplImage
      {
         get
         {
            return (MIplImage)Marshal.PtrToStructure(Ptr, typeof(MIplImage));
         }
      }

      ///<summary> 
      /// Get or Set the region of interest for this image. To clear the ROI, set it to System.Drawing.Rectangle.Empty
      ///</summary>
      public Rectangle ROI
      {
         set
         {
            if (value.Equals(Rectangle.Empty))
            {
               //reset the image ROI
               CvInvoke.cvResetImageROI(Ptr);
            } else
            {   //set the image ROI to the specific value
               CvInvoke.cvSetImageROI(Ptr, value);
            }
         }
         get
         {
            //return the image ROI
            return CvInvoke.cvGetImageROI(Ptr);
         }
      }

      /// <summary>
      /// Get the number of channels for this image
      /// </summary>
      public override int NumberOfChannels
      {
         get
         {
            return _numberOfChannels;
         }
      }

      /// <summary>
      /// Get the underneath managed array
      /// </summary>
      public override Array ManagedArray
      {
         get { return _array; }
         set
         {
            TDepth[, ,] data = value as TDepth[, ,];
            if (data == null)
               throw new InvalidCastException(String.Format("Cannot convert ManagedArray to type of {0}[,,].", typeof(TDepth).ToString()));
            Data = data;
         }
      }

      /// <summary>
      /// Get the equivalent opencv depth type for this image
      /// </summary>
      public static CvEnum.IPL_DEPTH CvDepth
      {
         get
         {
            Type typeOfDepth = typeof(TDepth);

            if (typeOfDepth == typeof(Single))
               return CvEnum.IPL_DEPTH.IPL_DEPTH_32F;
            else if (typeOfDepth == typeof(Byte))
               return CvEnum.IPL_DEPTH.IPL_DEPTH_8U;
            else if (typeOfDepth == typeof(Double))
               return CvEnum.IPL_DEPTH.IPL_DEPTH_64F;
            else if (typeOfDepth == typeof(SByte))
               return Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_8S;
            else if (typeOfDepth == typeof(UInt16))
               return Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_16U;
            else if (typeOfDepth == typeof(Int16))
               return Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_16S;
            else if (typeOfDepth == typeof(Int32))
               return Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32S;
            else
#if NETFX_CORE
               throw new NotImplementedException("Unsupported image depth");
#else
               throw new NotImplementedException(Properties.StringTable.UnsupportedImageDepth);
#endif
         }
      }

      ///<summary> 
      ///Indicates if the region of interest has been set
      ///</summary> 
      public bool IsROISet
      {
         get
         {
            return Marshal.ReadIntPtr(Ptr, ImageConstants.RoiOffset) != IntPtr.Zero;
         }
      }

      /// <summary>
      /// Get the average value on this image
      /// </summary>
      /// <returns>The average color of the image</returns>
      public TColor GetAverage()
      {
         return GetAverage(null);
      }

      /// <summary>
      /// Get the average value on this image, using the specific mask
      /// </summary>
      /// <param name="mask">The mask for find the average value</param>
      /// <returns>The average color of the masked area</returns>
      public TColor GetAverage(Image<Gray, Byte> mask)
      {
         TColor res = new TColor();
         res.MCvScalar = CvInvoke.cvAvg(Ptr, mask == null ? IntPtr.Zero : mask.Ptr);
         return res;
      }

      /// <summary>Get the sum for each color channel </summary>
      /// <returns>The sum for each color channel</returns>
      public TColor GetSum()
      {
         TColor res = new TColor();
         res.MCvScalar = CvInvoke.cvSum(Ptr);
         return res;
      }
      #endregion

      #region Coping and Filling
      /// <summary>
      /// Set every pixel of the image to the specific color 
      /// </summary>
      /// <param name="color">The color to be set</param>
      public void SetValue(TColor color)
      {
         SetValue(color.MCvScalar);
      }

      /// <summary>
      /// Set every pixel of the image to the specific color, using a mask
      /// </summary>
      /// <param name="color">The color to be set</param>
      /// <param name="mask">The mask for setting color</param>
      public void SetValue(TColor color, Image<Gray, Byte> mask)
      {
         SetValue(color.MCvScalar, mask);
      }

      /// <summary>
      /// Copy the masked area of this image to destination
      /// </summary>
      /// <param name="dest">the destination to copy to</param>
      /// <param name="mask">the mask for copy</param>
      public void Copy(Image<TColor, TDepth> dest, Image<Gray, Byte> mask)
      {
         CvInvoke.cvCopy(Ptr, dest.Ptr, mask == null ? IntPtr.Zero : mask.Ptr);
      }

      ///<summary> 
      /// Make a copy of the image using a mask, if ROI is set, only copy the ROI 
      /// </summary> 
      /// <param name="mask">the mask for coping</param>
      ///<returns> A copy of the image</returns>
      public Image<TColor, TDepth> Copy(Image<Gray, Byte> mask)
      {
         Image<TColor, TDepth> res = new Image<TColor, TDepth>(Size);
         Copy(res, mask);
         return res;
      }

      /// <summary>
      /// Make a copy of the specific ROI (Region of Interest) from the image
      /// </summary>
      /// <param name="roi">The roi to be copied</param>
      /// <returns>The roi region on the image</returns>
      public Image<TColor, TDepth> Copy(Rectangle roi)
      {
         Rectangle currentRoi = ROI; //cache the current roi
         Image<TColor, TDepth> res = new Image<TColor, TDepth>(roi.Size);
         ROI = roi;
         CvInvoke.cvCopy(Ptr, res.Ptr, IntPtr.Zero);
         ROI = currentRoi; //reset the roi
         return res;
      }

      /// <summary>
      /// Get a copy of the boxed region of the image
      /// </summary>
      /// <param name="box">The boxed region of the image</param>
      /// <returns>A copy of the boxed region of the image</returns>
      public Image<TColor, TDepth> Copy(MCvBox2D box)
      {
         PointF[] srcCorners = box.GetVertices();

         PointF[] destCorners = new PointF[] {
            new PointF(0, box.size.Height - 1),
            new PointF(0, 0),
            new PointF(box.size.Width - 1, 0)};

         using (RotationMatrix2D<double> rot = CameraCalibration.GetAffineTransform(srcCorners, destCorners))
         {
            Image<TColor, TDepth> res = new Image<TColor, TDepth>((int)box.size.Width, (int)box.size.Height);
            CvInvoke.cvWarpAffine(Ptr, res.Ptr, rot.Ptr, (int)Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR | (int)Emgu.CV.CvEnum.WARP.CV_WARP_DEFAULT, new MCvScalar());

            //TODO: Find out why cvGetQuadrangleSubPix do not return the expected output.
            //CvInvoke.cvGetQuadrangleSubPix(Ptr, res.Ptr, rot.Ptr); 
            return res;
         }
      }

      ///<summary> Make a copy of the image, if ROI is set, only copy the ROI</summary>
      ///<returns> A copy of the image</returns>
      public Image<TColor, TDepth> Copy()
      {
         return Copy(null);
      }

      /// <summary> 
      /// Create an image of the same size
      /// </summary>
      /// <remarks>The initial pixel in the image equals zero</remarks>
      /// <returns> The image of the same size</returns>
      public Image<TColor, TDepth> CopyBlank()
      {
         return new Image<TColor, TDepth>(Size);
      }

      /// <summary>
      /// Make a clone of the current image. All image data as well as the COI and ROI are cloned
      /// </summary>
      /// <returns>A clone of the current image. All image data as well as the COI and ROI are cloned</returns>
      public Image<TColor, TDepth> Clone()
      {
         int coi = CvInvoke.cvGetImageCOI(Ptr); //get the COI for current image
         Rectangle roi = ROI; //get the ROI for current image

         CvInvoke.cvSetImageCOI(Ptr, 0); //clear COI for current image
         ROI = Rectangle.Empty; // clear ROI for current image

         #region create a clone of the current image with the same COI and ROI
         Image<TColor, TDepth> res = Copy();
         CvInvoke.cvSetImageCOI(res.Ptr, coi);
         res.ROI = roi;
         #endregion

         CvInvoke.cvSetImageCOI(Ptr, coi); //reset the COI for the current image
         ROI = roi; // reset the ROI for the current image

         return res;
      }

      /// <summary>
      /// Get a subimage which image data is shared with the current image.
      /// </summary>
      /// <param name="rect">The rectangle area of the sub-image</param>
      /// <returns>A subimage which image data is shared with the current image</returns>
      public Image<TColor, TDepth> GetSubRect(Rectangle rect)
      {
         Image<TColor, TDepth> subRect = new Image<TColor, TDepth>();
         subRect._array = _array;

         GC.AddMemoryPressure(StructSize.MIplImage); //This pressure will be released once the result image is disposed. 

         subRect._ptr = CvInvoke.cvGetImageSubRect(_ptr, ref rect);
         return subRect;
      }

      #endregion

      #region Drawing functions
      ///<summary> Draw an Rectangle of the specific color and thickness </summary>
      ///<param name="rect"> The rectangle to be drawn</param>
      ///<param name="color"> The color of the rectangle </param>
      ///<param name="thickness"> If thickness is less than 1, the rectangle is filled up </param>
      public virtual void Draw(Rectangle rect, TColor color, int thickness)
      {
         /*
         CvInvoke.cvRectangle(
             Ptr,
             rect.Location,
             Point.Add(rect.Location, rect.Size),
             color.MCvScalar,
             (thickness <= 0) ? -1 : thickness,
             CvEnum.LINE_TYPE.EIGHT_CONNECTED,
             0);*/
         CvInvoke.cvRectangleR(
            Ptr,
            rect, color.MCvScalar,
            thickness,
            CvEnum.LINE_TYPE.EIGHT_CONNECTED,
            0);
      }

      ///<summary> Draw a 2D Cross using the specific color and thickness </summary>
      ///<param name="cross"> The 2D Cross to be drawn</param>
      ///<param name="color"> The color of the cross </param>
      ///<param name="thickness"> Must be &gt; 0 </param>
      public void Draw(Cross2DF cross, TColor color, int thickness)
      {
#if !NETFX_CORE
         Debug.Assert(thickness > 0, Properties.StringTable.ThicknessShouldBeGreaterThanZero);
#endif
         if (thickness > 0)
         {
            Draw(cross.Horizontal, color, thickness);
            Draw(cross.Vertical, color, thickness);
         }
      }
      ///<summary> Draw a line segment using the specific color and thickness </summary>
      ///<param name="line"> The line segment to be drawn</param>
      ///<param name="color"> The color of the line segment </param>
      ///<param name="thickness"> The thickness of the line segment </param>
      public virtual void Draw(LineSegment2DF line, TColor color, int thickness)
      {
#if !NETFX_CORE
         Debug.Assert(thickness > 0, Properties.StringTable.ThicknessShouldBeGreaterThanZero);
#endif
         if (thickness > 0)
            CvInvoke.cvLine(
                Ptr,
                Point.Round(line.P1),
                Point.Round(line.P2),
                color.MCvScalar,
                thickness,
                CvEnum.LINE_TYPE.EIGHT_CONNECTED,
                0);
      }

      ///<summary> Draw a line segment using the specific color and thickness </summary>
      ///<param name="line"> The line segment to be drawn</param>
      ///<param name="color"> The color of the line segment </param>
      ///<param name="thickness"> The thickness of the line segment </param>
      public virtual void Draw(LineSegment2D line, TColor color, int thickness)
      {
#if !NETFX_CORE
         Debug.Assert(thickness > 0, Properties.StringTable.ThicknessShouldBeGreaterThanZero);
#endif
         if (thickness > 0)
            CvInvoke.cvLine(
                Ptr,
                line.P1,
                line.P2,
                color.MCvScalar,
                thickness,
                CvEnum.LINE_TYPE.EIGHT_CONNECTED,
                0);
      }

      ///<summary> Draw a convex polygon using the specific color and thickness </summary>
      ///<param name="polygon"> The convex polygon to be drawn</param>
      ///<param name="color"> The color of the triangle </param>
      ///<param name="thickness"> If thickness is less than 1, the triangle is filled up </param>
      public virtual void Draw(IConvexPolygonF polygon, TColor color, int thickness)
      {
         Point[] vertices =
#if NETFX_CORE
            Extensions.
#else
            Array.
#endif
            ConvertAll<PointF, Point>(polygon.GetVertices(), Point.Round);

         if (thickness > 0)
            DrawPolyline(vertices, true, color, thickness);
         else
         {
            FillConvexPoly(vertices, color);
         }
      }

      /// <summary>
      /// Fill the convex polygon with the specific color
      /// </summary>
      /// <param name="pts">The array of points that define the convex polygon</param>
      /// <param name="color">The color to fill the polygon with</param>
      public void FillConvexPoly(Point[] pts, TColor color)
      {
         CvInvoke.cvFillConvexPoly(Ptr, pts, pts.Length, color.MCvScalar, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, 0);
      }

      /// <summary>
      /// Draw the polyline defined by the array of 2D points
      /// </summary>
      /// <param name="pts">A polyline defined by its point</param>
      /// <param name="isClosed">if true, the last line segment is defined by the last point of the array and the first point of the array</param>
      /// <param name="color">the color used for drawing</param>
      /// <param name="thickness">the thinkness of the line</param>
      public void DrawPolyline(Point[] pts, bool isClosed, TColor color, int thickness)
      {
         DrawPolyline(new Point[][] { pts }, isClosed, color, thickness);
      }

      /// <summary>
      /// Draw the polylines defined by the array of array of 2D points
      /// </summary>
      /// <param name="pts">An array of polylines each represented by an array of points</param>
      /// <param name="isClosed">if true, the last line segment is defined by the last point of the array and the first point of the array</param>
      /// <param name="color">the color used for drawing</param>
      /// <param name="thickness">the thinkness of the line</param>
      public void DrawPolyline(Point[][] pts, bool isClosed, TColor color, int thickness)
      {
         if (thickness > 0)
         {
            GCHandle[] handles = new GCHandle[pts.Length];
            IntPtr[] ptrs = new IntPtr[pts.Length];
            int[] lengths = new int[ptrs.Length];
            for (int i = 0; i < pts.Length; i++)
            {
               handles[i] = GCHandle.Alloc(pts[i], GCHandleType.Pinned);
               ptrs[i] = handles[i].AddrOfPinnedObject();
               lengths[i] = pts[i].Length;
            }

            CvInvoke.cvPolyLine(
               Ptr,
               ptrs,
               lengths,
               pts.Length,
               isClosed,
               color.MCvScalar,
               thickness,
               Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED,
               0);

            foreach (GCHandle h in handles)
               h.Free();
         }
      }

        ///<summary> Draw a Circle of the specific color and thickness </summary>
        ///<param name="circle"> The circle to be drawn</param>
        ///<param name="color"> The color of the circle </param>
        ///<param name="thickness"> If thickness is less than 1, the circle is filled up </param>
        public virtual void Draw(CircleF circle, TColor color, int thickness)
        {
            CvInvoke.cvCircle(
             Ptr,
             Point.Round(circle.Center),
             (int)circle.Radius,
             color.MCvScalar,
             (thickness <= 0) ? -1 : thickness,
             CvEnum.LINE_TYPE.EIGHT_CONNECTED,
             0);
        }

        ///<summary> Draw a Ellipse of the specific color and thickness </summary>
        ///<param name="ellipse"> The ellipse to be draw</param>
        ///<param name="color"> The color of the ellipse </param>
        ///<param name="thickness"> If thickness is less than 1, the ellipse is filled up </param>
        public void Draw(Ellipse ellipse, TColor color, int thickness)
        {
            /*
         CvInvoke.cvEllipse(
             Ptr,
             Point.Round(ellipse.MCvBox2D.center),
             new Size(((int)ellipse.MCvBox2D.size.Width) >> 1, ((int)ellipse.MCvBox2D.size.Height) >> 1),
             ellipse.MCvBox2D.angle,
             0.0,
             360.0,
             color.MCvScalar,
             (thickness <= 0) ? -1 : thickness,
             CvEnum.LINE_TYPE.EIGHT_CONNECTED,
             0);*/
            CvInvoke.cvEllipseBox(Ptr, ellipse.MCvBox2D, color.MCvScalar, thickness, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, 0);
        }

        /// <summary>
        /// Draw the text using the specific font on the image
        /// </summary>
        /// <param name="message">The text message to be draw</param>
        /// <param name="font">The font used for drawing</param>
        /// <param name="bottomLeft">The location of the bottom left corner of the font</param>
        /// <param name="color">The color of the text</param>
        public virtual void Draw(String message, ref MCvFont font, Point bottomLeft, TColor color)
        {
            CvInvoke.cvPutText(
             Ptr,
             message,
             bottomLeft,
             ref font,
             color.MCvScalar);
        }

        /// <summary>
        /// Draws contour outlines in the image if thickness&gt;=0 or fills area bounded by the contours if thickness&lt;0
        /// </summary>
        /// <param name="c">Pointer to the contour</param>
        /// <param name="color">Color of the contour</param>
        /// <param name="thickness">Thickness of lines the contours are drawn with. If it is negative, the contour interiors are drawn</param>
        public void Draw(Seq<Point> c, TColor color, int thickness)
        {
            Draw(c, color, color, 0, thickness, Point.Empty);
        }

        /// <summary>
        /// Draws contour outlines in the image if thickness&gt;=0 or fills area bounded by the contours if thickness&lt;0
        /// </summary>
        /// <param name="c">Pointer to the first contour</param>
        /// <param name="externalColor">Color of the external contours</param>
        /// <param name="holeColor">Color of internal contours (holes). </param>
        /// <param name="maxLevel">
        /// Maximal level for drawn contours.
        /// If 0, only contour is drawn.
        /// If 1, the contour and all contours after it on the same level are drawn.
        /// If 2, all contours after and all contours one level below the contours are drawn, etc. If the value is negative, the function does not draw the contours following after contour but draws child contours of contour up to abs(maxLevel)-1 level
        /// </param>
        /// <param name="thickness">Thickness of lines the contours are drawn with. If it is negative, the contour interiors are drawn</param>
        public void Draw(Seq<Point> c, TColor externalColor, TColor holeColor, int maxLevel, int thickness)
        {
            Draw(c, externalColor, holeColor, maxLevel, thickness, Point.Empty);
        }

        /// <summary>
        /// Draws contour outlines in the image if thickness&gt;=0 or fills area bounded by the contours if thickness&lt;0
        /// </summary>
        /// <param name="c">Pointer to the first contour</param>
        /// <param name="externalColor">Color of the external contours</param>
        /// <param name="holeColor">Color of internal contours (holes). </param>
        /// <param name="maxLevel">
        /// Maximal level for drawn contours.
        /// If 0, only contour is drawn.
        /// If 1, the contour and all contours after it on the same level are drawn.
        /// If 2, all contours after and all contours one level below the contours are drawn, etc. If the value is negative, the function does not draw the contours following after contour but draws child contours of contour up to abs(maxLevel)-1 level
        /// </param>
        /// <param name="thickness">Thickness of lines the contours are drawn with. If it is negative, the contour interiors are drawn</param>
        /// <param name="offset">Shift all the point coordinates by the specified value. It is useful in case if the contours retrived in some image ROI and then the ROI offset needs to be taken into account during the rendering</param>
        public void Draw(Seq<Point> c, TColor externalColor, TColor holeColor, int maxLevel, int thickness, Point offset)
        {
            CvInvoke.cvDrawContours(
          Ptr,
          c.Ptr,
          externalColor.MCvScalar,
          holeColor.MCvScalar,
          maxLevel,
          thickness,
          CvEnum.LINE_TYPE.EIGHT_CONNECTED,
          offset);
        }
      #endregion

      #region Object Detection
      #region Haar detection
        /// <summary>
        /// Detect HaarCascade object in the current image, using predefined parameters
        /// </summary>
        /// <param name="haarObj">The object to be detected</param>
        /// <returns>The objects detected, one array per channel</returns>
        [Obsolete("Use HaarCascade.Detect function instead. This function will be removed in the next release")]
        public MCvAvgComp[][] DetectHaarCascade(HaarCascade haarObj)
        {
            return DetectHaarCascade(haarObj, 1.1, 3, CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(0, 0));
        }

        /// <summary>
        /// Finds rectangular regions in the given image that are likely to contain objects the cascade has been trained for and returns those regions as a sequence of rectangles. The function scans the image several times at different scales (see cvSetImagesForHaarClassifierCascade). Each time it considers overlapping regions in the image and applies the classifiers to the regions using cvRunHaarClassifierCascade. It may also apply some heuristics to reduce number of analyzed regions, such as Canny prunning. After it has proceeded and collected the candidate rectangles (regions that passed the classifier cascade), it groups them and returns a sequence of average rectangles for each large enough group. The default parameters (scale_factor=1.1, min_neighbors=3, flags=0) are tuned for accurate yet slow object detection. For a faster operation on real video images the settings are: scale_factor=1.2, min_neighbors=2, flags=CV_HAAR_DO_CANNY_PRUNING, min_size=&lt;minimum possible face size&gt; (for example, ~1/4 to 1/16 of the image area in case of video conferencing).
        /// </summary>
        /// <param name="haarObj">Haar classifier cascade in internal representation</param>
        /// <param name="scaleFactor">The factor by which the search window is scaled between the subsequent scans, for example, 1.1 means increasing window by 10%</param>
        /// <param name="minNeighbors">Minimum number (minus 1) of neighbor rectangles that makes up an object. All the groups of a smaller number of rectangles than min_neighbors-1 are rejected. If min_neighbors is 0, the function does not any grouping at all and returns all the detected candidate rectangles, which may be useful if the user wants to apply a customized grouping procedure</param>
        /// <param name="flag">Mode of operation. Currently the only flag that may be specified is CV_HAAR_DO_CANNY_PRUNING. If it is set, the function uses Canny edge detector to reject some image regions that contain too few or too much edges and thus can not contain the searched object. The particular threshold values are tuned for face detection and in this case the pruning speeds up the processing.</param>
        /// <param name="minSize">Minimum window size. By default, it is set to the size of samples the classifier has been trained on (~20x20 for face detection)</param>
        /// <returns>The objects detected, one array per channel</returns>
        [Obsolete("Use HaarCascade.Detect function instead. This function will be removed in the next release")]
        public MCvAvgComp[][] DetectHaarCascade(HaarCascade haarObj, double scaleFactor, int minNeighbors, CvEnum.HAAR_DETECTION_TYPE flag, Size minSize)
        {
            using (MemStorage stor = new MemStorage())
            {
                Func<IImage, int, MCvAvgComp[]> detector =
                delegate(IImage img, int channel)
                {
                    IntPtr objects = CvInvoke.cvHaarDetectObjects(
                       img.Ptr,
                       haarObj.Ptr,
                       stor.Ptr,
                       scaleFactor,
                       minNeighbors,
                       flag,
                       minSize, 
                       Size.Empty);

                    if (objects == IntPtr.Zero)
                        return new MCvAvgComp[0];

                    Seq<MCvAvgComp> rects = new Seq<MCvAvgComp>(objects, stor);
                    return rects.ToArray();
                };

                MCvAvgComp[][] res = ForEachDuplicateChannel(detector);
                return res;
            }
        }
      #endregion

      #region Hough line and circles
        ///<summary>
        ///Apply Probabilistic Hough transform to find line segments.
        ///The current image must be a binary image (eg. the edges as a result of the Canny edge detector)
        ///</summary>
        ///<param name="rhoResolution">Distance resolution in pixel-related units.</param>
        ///<param name="thetaResolution">Angle resolution measured in radians</param>
        ///<param name="threshold">A line is returned by the function if the corresponding accumulator value is greater than threshold</param>
        ///<param name="minLineWidth">Minimum width of a line</param>
        ///<param name="gapBetweenLines">Minimum gap between lines</param>
        ///<returns>The line segments detected for each of the channels</returns>
        public LineSegment2D[][] HoughLinesBinary(double rhoResolution, double thetaResolution, int threshold, double minLineWidth, double gapBetweenLines)
        {
            using (MemStorage stor = new MemStorage())
            {
                return ForEachDuplicateChannel<LineSegment2D[]>(
               delegate(IImage img, int channel)
                {
                    IntPtr lines = CvInvoke.cvHoughLines2(
                     img.Ptr,
                     stor.Ptr,
                     CvEnum.HOUGH_TYPE.CV_HOUGH_PROBABILISTIC,
                     rhoResolution,
                     thetaResolution,
                     threshold,
                     minLineWidth,
                     gapBetweenLines);
                    Seq<LineSegment2D> segments = new Seq<LineSegment2D>(lines, stor);
                    return segments.ToArray();
                }
                );
            }
        }

        ///<summary>
        /// Apply Canny Edge Detector follows by Probabilistic Hough transform to find line segments in the image
        ///</summary>
        ///<param name="cannyThreshold"> The threshhold to find initial segments of strong edges</param>
        ///<param name="cannyThresholdLinking"> The threshold used for edge Linking</param>
        ///<param name="rhoResolution">Distance resolution in pixel-related units.</param>
        ///<param name="thetaResolution">Angle resolution measured in radians</param>
        ///<param name="threshold">A line is returned by the function if the corresponding accumulator value is greater than threshold</param>
        ///<param name="minLineWidth">Minimum width of a line</param>
        ///<param name="gapBetweenLines">Minimum gap between lines</param>
        ///<returns>The line segments detected for each of the channels</returns>
        public LineSegment2D[][] HoughLines(double cannyThreshold, double cannyThresholdLinking, double rhoResolution, double thetaResolution, int threshold, double minLineWidth, double gapBetweenLines)
        {
            using (Image<Gray, Byte> canny = Canny(cannyThreshold, cannyThresholdLinking))
            {
                return canny.HoughLinesBinary(
               rhoResolution,
               thetaResolution,
               threshold,
               minLineWidth,
               gapBetweenLines);
            }
        }

        ///<summary>
        ///First apply Canny Edge Detector on the current image,
        ///then apply Hough transform to find circles
        ///</summary>
        ///<param name="cannyThreshold">The higher threshold of the two passed to Canny edge detector (the lower one will be twice smaller).</param>
        ///<param name="accumulatorThreshold">Accumulator threshold at the center detection stage. The smaller it is, the more false circles may be detected. Circles, corresponding to the larger accumulator values, will be returned first</param>
        ///<param name="dp">Resolution of the accumulator used to detect centers of the circles. For example, if it is 1, the accumulator will have the same resolution as the input image, if it is 2 - accumulator will have twice smaller width and height, etc</param>
        ///<param name="minRadius">Minimal radius of the circles to search for</param>
        ///<param name="maxRadius">Maximal radius of the circles to search for</param>
        ///<param name="minDist">Minimum distance between centers of the detected circles. If the parameter is too small, multiple neighbor circles may be falsely detected in addition to a true one. If it is too large, some circles may be missed</param>
        ///<returns>The circle detected for each of the channels</returns>
        public CircleF[][] HoughCircles(TColor cannyThreshold, TColor accumulatorThreshold, double dp, double minDist, int minRadius, int maxRadius)
        {
            using (MemStorage stor = new MemStorage())
            {
                double[] cannyThresh = cannyThreshold.MCvScalar.ToArray();
                double[] accumulatorThresh = accumulatorThreshold.MCvScalar.ToArray();
                Func<IImage, int, CircleF[]> detector =
                delegate(IImage img, int channel)
                {
                    IntPtr circlesSeqPtr = CvInvoke.cvHoughCircles(
                       img.Ptr,
                       stor.Ptr,
                       CvEnum.HOUGH_TYPE.CV_HOUGH_GRADIENT,
                       dp,
                       minDist,
                       cannyThresh[channel],
                       accumulatorThresh[channel],
                       minRadius,
                       maxRadius);

                    Seq<CircleF> cirSeq = new Seq<CircleF>(circlesSeqPtr, stor);
                    return cirSeq.ToArray();
                };
                return ForEachDuplicateChannel(detector);
            }
        }
      #endregion

      #region Contour detection
        /// <summary>
        /// Find a list of contours using simple approximation method.
        /// </summary>
        /// <returns>
        /// Contour if there is any;
        /// null if no contour is found
        /// </returns>
        public Contour<Point> FindContours()
        {
            return FindContours(CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, CvEnum.RETR_TYPE.CV_RETR_LIST);
        }

        /// <summary>
        /// Find contours
        /// </summary>
        /// <param name="method">The type of approximation method</param>
        /// <param name="type">The retrival type</param>
        /// <returns>
        /// Contour if there is any;
        /// null if no contour is found
        /// </returns>
        public Contour<Point> FindContours(CvEnum.CHAIN_APPROX_METHOD method, CvEnum.RETR_TYPE type)
        {
            return FindContours(method, type, new MemStorage());
        }

        /// <summary>
        /// Find contours using the specific memory storage
        /// </summary>
        /// <param name="method">The type of approximation method</param>
        /// <param name="type">The retrival type</param>
        /// <param name="stor">The storage used by the sequences</param>
        /// <returns>
        /// Contour if there is any;
        /// null if no contour is found
        /// </returns>
        public Contour<Point> FindContours(CvEnum.CHAIN_APPROX_METHOD method, CvEnum.RETR_TYPE type, MemStorage stor)
        {
            if (method == Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_CODE)
            {
                //TODO: wrap CvChain and add code here
#if NETFX_CORE
               throw new NotImplementedException("Not implmented");
#else
                throw new NotImplementedException(Properties.StringTable.NotImplemented);
#endif
            }

            using (Image<TColor, TDepth> imagecopy = Copy()) //since cvFindContours modifies the content of the source, we need to make a clone
            {
                IntPtr seq = IntPtr.Zero;
                CvInvoke.cvFindContours(
                imagecopy.Ptr,
                stor.Ptr,
                ref seq,
                StructSize.MCvContour,
                type,
                method,
                Point.Empty);

                return (seq == IntPtr.Zero) ? null : new Contour<Point>(seq, stor);
            }
        }
      #endregion
      #endregion

      #region Indexer
        /// <summary>
        /// Get or Set the specific channel of the current image.
        /// For Get operation, a copy of the specific channel is returned.
        /// For Set operation, the specific channel is copied to this image.
        /// </summary>
        /// <param name="channel">The channel to get from the current image, zero based index</param>
        /// <returns>The specific channel of the current image</returns>
        public Image<Gray, TDepth> this[int channel]
        {
            get
            {
                Image<Gray, TDepth> imageChannel = new Image<Gray, TDepth>(Size);
                int coi = CvInvoke.cvGetImageCOI(Ptr);
                CvInvoke.cvSetImageCOI(Ptr, channel + 1);
                CvInvoke.cvCopy(Ptr, imageChannel.Ptr, IntPtr.Zero);
                CvInvoke.cvSetImageCOI(Ptr, coi);
                return imageChannel;
            }
            set
            {
                IntPtr[] channels = new IntPtr[4];
                channels[channel] = value.Ptr;
                CvInvoke.cvCvtPlaneToPix(channels[0], channels[1], channels[2], channels[3], Ptr);
            }
        }

        /// <summary>
        /// Get or Set the color in the <paramref name="row"/>th row (y direction) and <paramref name="column"/>th column (x direction)
        /// </summary>
        /// <param name="row">The zero-based row (y direction) of the pixel </param>
        /// <param name="column">The zero-based column (x direction) of the pixel</param>
        /// <returns>The color in the specific <paramref name="row"/> and <paramref name="column"/></returns>
        public TColor this[int row, int column]
        {
            get
            {
                TColor res = new TColor();
                res.MCvScalar = CvInvoke.cvGet2D(Ptr, row, column);
                return res;
            }
            set
            {
                CvInvoke.cvSet2D(Ptr, row, column, value.MCvScalar);
            }
        }

        /// <summary>
        /// Get or Set the color in the <paramref name="location"/>
        /// </summary>
        /// <param name="location">the location of the pixel </param>
        /// <returns>the color in the <paramref name="location"/></returns>
        public TColor this[Point location]
        {
            get
            {
                return this[location.Y, location.X];
            }
            set
            {
                this[location.Y, location.X] = value;
            }
        }
      #endregion

      #region utilities
        /// <summary>
        /// Return parameters based on ROI
        /// </summary>
        /// <param name="ptr">The Pointer to the IplImage</param>
        /// <param name="start">The address of the pointer that point to the start of the Bytes taken into consideration ROI</param>
        /// <param name="elementCount">ROI.Width * ColorType.Dimension</param>
        /// <param name="byteWidth">The number of bytes in a row taken into consideration ROI</param>
        /// <param name="rows">The number of rows taken into consideration ROI</param>
        /// <param name="widthStep">The width step required to jump to the next row</param>
        protected static void RoiParam(IntPtr ptr, out Int64 start, out int rows, out int elementCount, out int byteWidth, out int widthStep)
        {
            MIplImage ipl = (MIplImage)Marshal.PtrToStructure(ptr, typeof(MIplImage));
            start = ipl.imageData.ToInt64();
            widthStep = ipl.widthStep;

            if (ipl.roi != IntPtr.Zero)
            {
                Rectangle rec = CvInvoke.cvGetImageROI(ptr);
                elementCount = rec.Width * ipl.nChannels;
                byteWidth = ((int)ipl.depth >> 3) * elementCount;

                start += rec.Y * widthStep
                    + ((int)ipl.depth >> 3) * rec.X;
                rows = rec.Height;
            } else
            {
                byteWidth = widthStep;
                elementCount = ipl.width * ipl.nChannels;
                rows = ipl.height;
            }
        }

        /// <summary>
        /// Apply convertor and compute result for each channel of the image.
        /// </summary>
        /// <remarks>
        /// For single channel image, apply converter directly.
        /// For multiple channel image, set the COI for the specific channel before appling the convertor
        /// </remarks>
        /// <typeparam name="TResult">The return type</typeparam>
        /// <param name="conv">The converter such that accept the IntPtr of a single channel IplImage, and image channel index which returning result of type R</param>
        /// <returns>An array which contains result for each channel</returns>
        private TResult[] ForEachChannel<TResult>(Func<IntPtr, int, TResult> conv)
        {
            TResult[] res = new TResult[NumberOfChannels];
            if (NumberOfChannels == 1)
                res[0] = conv(Ptr, 0);
            else
            {
                for (int i = 0; i < NumberOfChannels; i++)
                {
                    CvInvoke.cvSetImageCOI(Ptr, i + 1);
                    res[i] = conv(Ptr, i);
                }
                CvInvoke.cvSetImageCOI(Ptr, 0);
            }
            return res;
        }


        /// <summary>
        /// Apply convertor and compute result for each channel of the image, for single channel image, apply converter directly, for multiple channel image, make a copy of each channel to a temperary image and apply the convertor
        /// </summary>
        /// <param name="action">The converter such that accept the IntPtr of a single channel IplImage, and image channel index which returning result of type R</param>
        /// <returns>An array which contains result for each channel</returns>
        private void ForEachDuplicateChannel(Action<IImage, int> action)
        {
            if (NumberOfChannels == 1)
                action(this, 0);
            else
            {
                int oldCOI = CvInvoke.cvGetImageCOI(Ptr);
                using (Image<Gray, TDepth> tmp = new Image<Gray, TDepth>(Size))
                    for (int i = 0; i < NumberOfChannels; i++)
                    {
                        CvInvoke.cvSetImageCOI(Ptr, i + 1);
                        CvInvoke.cvCopy(Ptr, tmp.Ptr, IntPtr.Zero);
                        action(tmp, i);
                    }
                CvInvoke.cvSetImageCOI(Ptr, oldCOI);
            }
        }

        /// <summary>
        /// Apply convertor and compute result for each channel of the image, for single channel image, apply converter directly, for multiple channel image, make a copy of each channel to a temperary image and apply the convertor
        /// </summary>
        /// <typeparam name="TReturn">The return type</typeparam>
        /// <param name="conv">The converter such that accept the IntPtr of a single channel IplImage, and image channel index which returning result of type R</param>
        /// <returns>An array which contains result for each channel</returns>
        private TReturn[] ForEachDuplicateChannel<TReturn>(Func<IImage, int, TReturn> conv)
        {
            TReturn[] res = new TReturn[NumberOfChannels];
            if (NumberOfChannels == 1)
                res[0] = conv(this, 0);
            else
            {
                int oldCOI = CvInvoke.cvGetImageCOI(Ptr);
                using (Image<Gray, TDepth> tmp = new Image<Gray, TDepth>(Size))
                    for (int i = 0; i < NumberOfChannels; i++)
                    {
                        CvInvoke.cvSetImageCOI(Ptr, i + 1);
                        CvInvoke.cvCopy(Ptr, tmp.Ptr, IntPtr.Zero);
                        res[i] = conv(tmp, i);
                    }

                CvInvoke.cvSetImageCOI(Ptr, oldCOI);
            }
            return res;
        }

        /// <summary>
        /// If the image has only one channel, apply the action directly on the IntPtr of this image and <paramref name="dest"/>,
        /// otherwise, make copy each channel of this image to a temperary one, apply action on it and another temperory image and copy the resulting image back to image2
        /// </summary>
        /// <typeparam name="TOtherDepth">The type of the depth of the <paramref name="dest"/> image</typeparam>
        /// <param name="act">The function which acepts the src IntPtr, dest IntPtr and index of the channel as input</param>
        /// <param name="dest">The destination image</param>
        private void ForEachDuplicateChannel<TOtherDepth>(Action<IntPtr, IntPtr, int> act, Image<TColor, TOtherDepth> dest)
            where TOtherDepth : new()
        {
            if (NumberOfChannels == 1)
                act(Ptr, dest.Ptr, 0);
            else
            {
                int sourceCOI = CvInvoke.cvGetImageCOI(Ptr);
                int destCOI = CvInvoke.cvGetImageCOI(dest);
                using (Image<Gray, TDepth> tmp1 = new Image<Gray, TDepth>(Size))
                using (Image<Gray, TOtherDepth> tmp2 = new Image<Gray, TOtherDepth>(dest.Size))
                {
                    for (int i = 0; i < NumberOfChannels; i++)
                    {
                        CvInvoke.cvSetImageCOI(Ptr, i + 1);
                        CvInvoke.cvSetImageCOI(dest.Ptr, i + 1);
                        CvInvoke.cvCopy(Ptr, tmp1.Ptr, IntPtr.Zero);
                        act(tmp1.Ptr, tmp2.Ptr, i);
                        CvInvoke.cvCopy(tmp2.Ptr, dest.Ptr, IntPtr.Zero);
                    }
                }
                CvInvoke.cvSetImageCOI(Ptr, sourceCOI);
                CvInvoke.cvSetImageCOI(dest.Ptr, destCOI);
            }
        }
      #endregion

      #region Gradient, Edges and Features
        /// <summary>
        /// Calculates the image derivative by convolving the image with the appropriate kernel
        /// The Sobel operators combine Gaussian smoothing and differentiation so the result is more or less robust to the noise. Most often, the function is called with (xorder=1, yorder=0, aperture_size=3) or (xorder=0, yorder=1, aperture_size=3) to calculate first x- or y- image derivative.
        /// </summary>
        /// <param name="xorder">Order of the derivative x</param>
        /// <param name="yorder">Order of the derivative y</param>
        /// <param name="apertureSize">Size of the extended Sobel kernel, must be 1, 3, 5 or 7. In all cases except 1, aperture_size xaperture_size separable kernel will be used to calculate the derivative.</param>
        /// <returns>The result of the sobel edge detector</returns>
        [ExposableMethod(Exposable = true, Category = "Gradients, Edges")]
        public Image<TColor, Single> Sobel(int xorder, int yorder, int apertureSize)
        {
            Image<TColor, Single> res = new Image<TColor, float>(Size);
            CvInvoke.cvSobel(Ptr, res.Ptr, xorder, yorder, apertureSize);
            return res;
        }

        /// <summary>
        /// Calculates Laplacian of the source image by summing second x- and y- derivatives calculated using Sobel operator.
        /// Specifying aperture_size=1 gives the fastest variant that is equal to convolving the image with the following kernel:
        ///
        /// |0  1  0|
        /// |1 -4  1|
        /// |0  1  0|
        /// </summary>
        /// <param name="apertureSize">Aperture size </param>
        /// <returns>The Laplacian of the image</returns>
        [ExposableMethod(Exposable = true, Category = "Gradients, Edges")]
        public Image<TColor, Single> Laplace(int apertureSize)
        {
            Image<TColor, Single> res = new Image<TColor, float>(Size);
            CvInvoke.cvLaplace(Ptr, res.Ptr, apertureSize);
            return res;
        }

        ///<summary> Find the edges on this image and marked them in the returned image.</summary>
        ///<param name="thresh"> The threshhold to find initial segments of strong edges</param>
        ///<param name="threshLinking"> The threshold used for edge Linking</param>
        ///<returns> The edges found by the Canny edge detector</returns>
        [ExposableMethod(Exposable = true, Category = "Gradients, Edges")]
        public Image<Gray, Byte> Canny(double thresh, double threshLinking)
        {
            return Canny(thresh, threshLinking, 3);
        }

        ///<summary> Find the edges on this image and marked them in the returned image.</summary>
        ///<param name="thresh"> The threshhold to find initial segments of strong edges</param>
        ///<param name="threshLinking"> The threshold used for edge Linking</param>
        ///<param name="apertureSize">The aperture size, use 3 for default</param>
        ///<returns> The edges found by the Canny edge detector</returns>
        public Image<Gray, Byte> Canny(double thresh, double threshLinking, int apertureSize)
        {
            Image<Gray, Byte> res = new Image<Gray, Byte>(Size);
            CvInvoke.cvCanny(this, res, thresh, threshLinking, apertureSize);
            return res;
        }

      #region SURF
        /// <summary>
        /// Finds robust features in the image (basic descriptor is returned in this case). For each feature it returns its location, size, orientation and optionally the descriptor, basic or extended. The function can be used for object tracking and localization, image stitching etc
        /// </summary>
        /// <param name="param">The SURF parameters</param>
        /// <returns>The SURF features</returns>
        public SURFFeature[] ExtractSURF(ref MCvSURFParams param)
        {
            return ExtractSURF(null, ref param);
        }

        /// <summary>
        /// Finds robust features in the image (basic descriptor is returned in this case). For each feature it returns its location, size, orientation and optionally the descriptor, basic or extended. The function can be used for object tracking and localization, image stitching etc
        /// </summary>
        /// <param name="mask">The optional input 8-bit mask, can be null if not needed. The features are only found in the areas that contain more than 50% of non-zero mask pixels</param>
        /// <param name="param">The SURF parameters</param>
        /// <returns>The SURF features</returns>
        public SURFFeature[] ExtractSURF(Image<Gray, Byte> mask, ref MCvSURFParams param)
        {
            using (MemStorage stor = new MemStorage())
            {
                IntPtr descriptorPtr = new IntPtr();
                IntPtr keypointsPtr = new IntPtr();

                CvInvoke.cvExtractSURF(
               Ptr, mask == null ? IntPtr.Zero : mask.Ptr,
               ref keypointsPtr,
               ref descriptorPtr,
               stor.Ptr,
               param,
               0);
                Seq<MCvSURFPoint> keypoints = new Seq<MCvSURFPoint>(keypointsPtr, stor);

                MCvSURFPoint[] surfPoints = keypoints.ToArray();

                SURFFeature[] res = new SURFFeature[surfPoints.Length];

                int elementsInDescriptor = (param.Extended == 0) ? 64 : 128;

                for (int i = 0; i < res.Length; i++)
                {
                    float[] descriptor = new float[elementsInDescriptor];
                    Marshal.Copy(CvInvoke.cvGetSeqElem(descriptorPtr, i), descriptor, 0, elementsInDescriptor);
                    res[i] = new SURFFeature(ref surfPoints[i], descriptor);
                }

                return res;
            }
        }

      #endregion

        /// <summary>
        /// Get the star keypoints from this image
        /// </summary>
        /// <param name="param">The Star Detector parameters</param>
        /// <returns>The Star keypoints in this image</returns>
        public MCvStarKeypoint[] GetStarKeypoints(ref MCvStarDetectorParams param)
        {
            using (MemStorage stor = new MemStorage())
            {
                IntPtr keyPointsPtr = CvInvoke.cvGetStarKeypoints(_ptr, stor.Ptr, param);
                Seq<MCvStarKeypoint> keyPoints = new Seq<MCvStarKeypoint>(keyPointsPtr, stor);
                return keyPoints.ToArray();
            }
        }

        /// <summary>
        /// Finds corners with big eigenvalues in the image.
        /// </summary>
        /// <remarks>The function first calculates the minimal eigenvalue for every source image pixel using cvCornerMinEigenVal function and stores them in eig_image. Then it performs non-maxima suppression (only local maxima in 3x3 neighborhood remain). The next step is rejecting the corners with the minimal eigenvalue less than quality_level?max(eig_image(x,y)). Finally, the function ensures that all the corners found are distanced enough one from another by considering the corners (the most strongest corners are considered first) and checking that the distance between the newly considered feature and the features considered earlier is larger than min_distance. So, the function removes the features than are too close to the stronger features</remarks>
        /// <param name="maxFeaturesPerChannel">The maximum features to be detected per channel</param>
        /// <param name="qualityLevel">Multiplier for the maxmin eigenvalue; specifies minimal accepted quality of image corners</param>
        /// <param name="minDistance">Limit, specifying minimum possible distance between returned corners; Euclidian distance is used. </param>
        /// <param name="blockSize">Size of the averaging block, passed to underlying cvCornerMinEigenVal or cvCornerHarris used by the function</param>
        /// <returns>The good features for each channel</returns>
        public PointF[][] GoodFeaturesToTrack(int maxFeaturesPerChannel, double qualityLevel, double minDistance, int blockSize)
        {
            return GoodFeaturesToTrack(maxFeaturesPerChannel, qualityLevel, minDistance, blockSize, false, 0);
        }

        /// <summary>
        /// Finds corners with big eigenvalues in the image.
        /// </summary>
        /// <remarks>The function first calculates the minimal eigenvalue for every source image pixel using cvCornerMinEigenVal function and stores them in eig_image. Then it performs non-maxima suppression (only local maxima in 3x3 neighborhood remain). The next step is rejecting the corners with the minimal eigenvalue less than quality_level?max(eig_image(x,y)). Finally, the function ensures that all the corners found are distanced enough one from another by considering the corners (the most strongest corners are considered first) and checking that the distance between the newly considered feature and the features considered earlier is larger than min_distance. So, the function removes the features than are too close to the stronger features</remarks>
        /// <param name="maxFeaturesPerChannel">The maximum features to be detected per channel</param>
        /// <param name="qualityLevel">Multiplier for the maxmin eigenvalue; specifies minimal accepted quality of image corners</param>
        /// <param name="minDistance">Limit, specifying minimum possible distance between returned corners; Euclidian distance is used. </param>
        /// <param name="blockSize">Size of the averaging block, passed to underlying cvCornerMinEigenVal or cvCornerHarris used by the function</param>
        /// <param name="k">Free parameter of Harris detector. If provided, Harris operator (cvCornerHarris) is used instead of default cvCornerMinEigenVal. </param>
        /// <returns>The good features for each channel</returns>
        public PointF[][] GoodFeaturesToTrack(int maxFeaturesPerChannel, double qualityLevel, double minDistance, int blockSize, double k)
        {
            return GoodFeaturesToTrack(maxFeaturesPerChannel, qualityLevel, minDistance, blockSize, true, k);
        }

        /// <summary>
        /// Finds corners with big eigenvalues in the image.
        /// </summary>
        /// <remarks>The function first calculates the minimal eigenvalue for every source image pixel using cvCornerMinEigenVal function and stores them in eig_image. Then it performs non-maxima suppression (only local maxima in 3x3 neighborhood remain). The next step is rejecting the corners with the minimal eigenvalue less than quality_level?max(eig_image(x,y)). Finally, the function ensures that all the corners found are distanced enough one from another by considering the corners (the most strongest corners are considered first) and checking that the distance between the newly considered feature and the features considered earlier is larger than min_distance. So, the function removes the features than are too close to the stronger features</remarks>
        /// <param name="maxFeaturesPerChannel">The maximum features to be detected per channel</param>
        /// <param name="qualityLevel">Multiplier for the maxmin eigenvalue; specifies minimal accepted quality of image corners</param>
        /// <param name="minDistance">Limit, specifying minimum possible distance between returned corners; Euclidian distance is used. </param>
        /// <param name="blockSize">Size of the averaging block, passed to underlying cvCornerMinEigenVal or cvCornerHarris used by the function</param>
        /// <param name="useHarris">If nonzero, Harris operator (cvCornerHarris) is used instead of default cvCornerMinEigenVal</param>
        /// <param name="k">Free parameter of Harris detector; used only if use_harris = true </param>
        /// <returns>The good features for each channel</returns>
        public PointF[][] GoodFeaturesToTrack(int maxFeaturesPerChannel, double qualityLevel, double minDistance, int blockSize, bool useHarris, double k)
        {
            using (Image<Gray, Single> eigImage = new Image<Gray, float>(Width, Height))
            using (Image<Gray, Single> tmpImage = new Image<Gray, float>(Width, Height))
            {
                return ForEachDuplicateChannel<PointF[]>(
               delegate(IImage img, int channel)
                {
                  int cornercount = maxFeaturesPerChannel;
                  PointF[] pts = new PointF[maxFeaturesPerChannel];
                  GCHandle handle = GCHandle.Alloc(pts, GCHandleType.Pinned);
                  CvInvoke.cvGoodFeaturesToTrack(
                      img.Ptr,
                      eigImage.Ptr,
                      tmpImage.Ptr,
                      handle.AddrOfPinnedObject(),
                      ref cornercount,
                      qualityLevel,
                      minDistance,
                      IntPtr.Zero,
                      blockSize,
                      useHarris ? 1 : 0,
                      k);
                  handle.Free();
                  Array.Resize(ref pts, cornercount);
                  return pts;
               });
         }
      }

      /// <summary>
      /// Iterates to find the sub-pixel accurate location of corners, or radial saddle points
      /// </summary>
      /// <param name="corners">Coordinates of the input corners, the values will be modified by this function call</param>
      /// <param name="win">Half sizes of the search window. For example, if win=(5,5) then 5*2+1 x 5*2+1 = 11 x 11 search window is used</param>
      /// <param name="zeroZone">Half size of the dead region in the middle of the search zone over which the summation in formulae below is not done. It is used sometimes to avoid possible singularities of the autocorrelation matrix. The value of (-1,-1) indicates that there is no such size</param>
      /// <param name="criteria">Criteria for termination of the iterative process of corner refinement. That is, the process of corner position refinement stops either after certain number of iteration or when a required accuracy is achieved. The criteria may specify either of or both the maximum number of iteration and the required accuracy</param>
      /// <returns>Refined corner coordinates</returns>
      public void FindCornerSubPix(
         PointF[][] corners,
         Size win,
         Size zeroZone,
         MCvTermCriteria criteria)
      {
         ForEachDuplicateChannel(delegate(IImage img, int channel)
             {
                PointF[] ptsForCurrentChannel = corners[channel];
                CvInvoke.cvFindCornerSubPix(
                   img.Ptr,
                   ptsForCurrentChannel,
                   ptsForCurrentChannel.Length,
                   win,
                   zeroZone,
                   criteria);
             });
      }

      #endregion

      #region Matching
      /// <summary>
      /// The function slids through image, compares overlapped patches of size wxh with templ using the specified method and return the comparison results 
      /// </summary>
      /// <param name="template">Searched template; must be not greater than the source image and the same data type as the image</param>
      /// <param name="method">Specifies the way the template must be compared with image regions </param>
      /// <returns>The comparison result: width = this.Width - template.Width + 1; height = this.Height - template.Height + 1 </returns>
      public Image<Gray, Single> MatchTemplate(Image<TColor, TDepth> template, CvEnum.TM_TYPE method)
      {
         Image<Gray, Single> res = new Image<Gray, Single>(Width - template.Width + 1, Height - template.Height + 1);
         CvInvoke.cvMatchTemplate(Ptr, template.Ptr, res.Ptr, method);
         return res;
      }
      #endregion

      #region Object Tracking
      /// <summary>
      /// Updates snake in order to minimize its total energy that is a sum of internal energy that depends on contour shape (the smoother contour is, the smaller internal energy is) and external energy that depends on the energy field and reaches minimum at the local energy extremums that correspond to the image edges in case of image gradient.
      /// </summary>
      /// <param name="contour">Some existing contour</param>
      /// <param name="alpha">Weight[s] of continuity energy, single float or array of length floats, one per each contour point</param>
      /// <param name="beta">Weight[s] of curvature energy, similar to alpha.</param>
      /// <param name="gamma">Weight[s] of image energy, similar to alpha.</param>
      /// <param name="windowSize">Size of neighborhood of every point used to search the minimum, both win.width and win.height must be odd</param>
      /// <param name="tc">Termination criteria. The parameter criteria.epsilon is used to define the minimal number of points that must be moved during any iteration to keep the iteration process running. If at some iteration the number of moved points is less than criteria.epsilon or the function performed criteria.max_iter iterations, the function terminates. </param>
      /// <param name="storage">The memory storage used by the resulting sequence</param>
      /// <returns>The snake[d] contour</returns>
      public Contour<Point> Snake(Seq<Point> contour, float alpha, float beta, float gamma, Size windowSize, MCvTermCriteria tc, MemStorage storage)
      {
         int count = contour.Total;

         Point[] points = new Point[count];
         GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
         CvInvoke.cvCvtSeqToArray(contour.Ptr, handle.AddrOfPinnedObject(), MCvSlice.WholeSeq);
         CvInvoke.cvSnakeImage(
             Ptr,
             handle.AddrOfPinnedObject(),
             count,
             new float[1] { alpha },
             new float[1] { beta },
             new float[1] { gamma },
             1,
             windowSize,
             tc,
             true);

         Contour<Point> rSeq = new Contour<Point>(storage);

         CvInvoke.cvSeqPushMulti(rSeq.Ptr, handle.AddrOfPinnedObject(), count, Emgu.CV.CvEnum.BACK_OR_FRONT.FRONT);
         handle.Free();

         return rSeq;
      }

      /// <summary>
      /// Updates snake in order to minimize its total energy that is a sum of internal energy that depends on contour shape (the smoother contour is, the smaller internal energy is) and external energy that depends on the energy field and reaches minimum at the local energy extremums that correspond to the image edges in case of image gradient.
      /// </summary>
      /// <param name="contour">Some existing contour. It's value will be update by this function</param>
      /// <param name="alpha">Weight[s] of continuity energy, single float or array of length floats, one per each contour point</param>
      /// <param name="beta">Weight[s] of curvature energy, similar to alpha.</param>
      /// <param name="gamma">Weight[s] of image energy, similar to alpha.</param>
      /// <param name="windowSize">Size of neighborhood of every point used to search the minimum, both win.width and win.height must be odd</param>
      /// <param name="tc">Termination criteria. The parameter criteria.epsilon is used to define the minimal number of points that must be moved during any iteration to keep the iteration process running. If at some iteration the number of moved points is less than criteria.epsilon or the function performed criteria.max_iter iterations, the function terminates. </param>
      /// <param name="calculateGradiant">If true, the function calculates gradient magnitude for every image pixel and considers it as the energy field, otherwise the input image itself is considered</param>
      public void Snake(Point[] contour, float alpha, float beta, float gamma, Size windowSize, MCvTermCriteria tc, bool calculateGradiant)
      {
         CvInvoke.cvSnakeImage(
             Ptr,
             contour,
             contour.Length,
             new float[1] { alpha },
             new float[1] { beta },
             new float[1] { gamma },
             1,
             windowSize,
             tc,
             calculateGradiant ? 1 : 0);
      }
      #endregion

      #region Logic
      #region And Methods
      ///<summary> Perform an elementwise AND operation with another image and return the result</summary>
      ///<param name="img2">The second image for the AND operation</param>
      ///<returns> The result of the AND operation</returns>
      public Image<TColor, TDepth> And(Image<TColor, TDepth> img2)
      {
         Image<TColor, TDepth> res = new Image<TColor, TDepth>(Size);
         CvInvoke.cvAnd(Ptr, img2.Ptr, res.Ptr, IntPtr.Zero);
         return res;
      }

      ///<summary> 
      ///Perform an elementwise AND operation with another image, using a mask, and return the result
      ///</summary>
      ///<param name="img2">The second image for the AND operation</param>
      ///<param name="mask">The mask for the AND operation</param>
      ///<returns> The result of the AND operation</returns>
      public Image<TColor, TDepth> And(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
      {
         Image<TColor, TDepth> res = new Image<TColor, TDepth>(Size);
         CvInvoke.cvAnd(Ptr, img2.Ptr, res.Ptr, mask.Ptr);
         return res;
      }

      ///<summary> Perform an binary AND operation with some color</summary>
      ///<param name="val">The color for the AND operation</param>
      ///<returns> The result of the AND operation</returns>
      public Image<TColor, TDepth> And(TColor val)
      {
         Image<TColor, TDepth> res = new Image<TColor, TDepth>(Size);
         CvInvoke.cvAndS(Ptr, val.MCvScalar, res.Ptr, IntPtr.Zero);
         return res;
      }

      ///<summary> Perform an binary AND operation with some color using a mask</summary>
      ///<param name="val">The color for the AND operation</param>
      ///<param name="mask">The mask for the AND operation</param>
      ///<returns> The result of the AND operation</returns>
      public Image<TColor, TDepth> And(TColor val, Image<Gray, Byte> mask)
      {
         Image<TColor, TDepth> res = new Image<TColor, TDepth>(Size);
         CvInvoke.cvAndS(Ptr, val.MCvScalar, res.Ptr, mask.Ptr);
         return res;
      }
      #endregion

      #region Or Methods
      ///<summary> Perform an elementwise OR operation with another image and return the result</summary>
      ///<param name="img2">The second image for the OR operation</param>
      ///<returns> The result of the OR operation</returns>
      public Image<TColor, TDepth> Or(Image<TColor, TDepth> img2)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvOr(Ptr, img2.Ptr, res.Ptr, IntPtr.Zero);
         return res;
      }
      ///<summary> Perform an elementwise OR operation with another image, using a mask, and return the result</summary>
      ///<param name="img2">The second image for the OR operation</param>
      ///<param name="mask">The mask for the OR operation</param>
      ///<returns> The result of the OR operation</returns>
      public Image<TColor, TDepth> Or(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvOr(Ptr, img2.Ptr, res.Ptr, mask.Ptr);
         return res;
      }

      ///<summary> Perform an elementwise OR operation with some color</summary>
      ///<param name="val">The value for the OR operation</param>
      ///<returns> The result of the OR operation</returns>
      [ExposableMethod(Exposable = true, Category = "Logic")]
      public Image<TColor, TDepth> Or(TColor val)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvOrS(Ptr, val.MCvScalar, res.Ptr, IntPtr.Zero);
         return res;
      }
      ///<summary> Perform an elementwise OR operation with some color using a mask</summary>
      ///<param name="val">The color for the OR operation</param>
      ///<param name="mask">The mask for the OR operation</param>
      ///<returns> The result of the OR operation</returns>
      public Image<TColor, TDepth> Or(TColor val, Image<Gray, Byte> mask)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvOrS(Ptr, val.MCvScalar, res.Ptr, mask.Ptr);
         return res;
      }
      #endregion

      #region Xor Methods
      ///<summary> Perform an elementwise XOR operation with another image and return the result</summary>
      ///<param name="img2">The second image for the XOR operation</param>
      ///<returns> The result of the XOR operation</returns>
      public Image<TColor, TDepth> Xor(Image<TColor, TDepth> img2)
      {
         return Xor(img2, null);
      }

      /// <summary>
      /// Perform an elementwise XOR operation with another image, using a mask, and return the result
      /// </summary>
      /// <param name="img2">The second image for the XOR operation</param>
      /// <param name="mask">The mask for the XOR operation</param>
      /// <returns>The result of the XOR operation</returns>
      public Image<TColor, TDepth> Xor(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvXor(Ptr, img2.Ptr, res.Ptr, mask);
         return res;
      }

      /// <summary> 
      /// Perform an binary XOR operation with some color
      /// </summary>
      /// <param name="val">The value for the XOR operation</param>
      /// <returns> The result of the XOR operation</returns>
      [ExposableMethod(Exposable = true, Category = "Logic")]
      public Image<TColor, TDepth> Xor(TColor val)
      {
         return Xor(val, null);
      }

      /// <summary>
      /// Perform an binary XOR operation with some color using a mask
      /// </summary>
      /// <param name="val">The color for the XOR operation</param>
      /// <param name="mask">The mask for the XOR operation</param>
      /// <returns> The result of the XOR operation</returns>
      public Image<TColor, TDepth> Xor(TColor val, Image<Gray, Byte> mask)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvXorS(Ptr, val.MCvScalar, res.Ptr, mask);
         return res;
      }
      #endregion

      ///<summary> 
      ///Compute the complement image
      ///</summary>
      ///<returns> The complement image</returns>
      public Image<TColor, TDepth> Not()
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvNot(Ptr, res.Ptr);
         return res;
      }
      #endregion

      #region Comparison
      ///<summary> Find the elementwise maximum value </summary>
      ///<param name="img2">The second image for the Max operation</param>
      ///<returns> An image where each pixel is the maximum of <i>this</i> image and the parameter image</returns>
      public Image<TColor, TDepth> Max(Image<TColor, TDepth> img2)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvMax(Ptr, img2.Ptr, res.Ptr);
         return res;
      }

      ///<summary> Find the elementwise maximum value </summary>
      ///<param name="value">The value to compare with</param>
      ///<returns> An image where each pixel is the maximum of <i>this</i> image and <paramref name="value"/></returns>
      public Image<TColor, TDepth> Max(double value)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvMaxS(Ptr, value, res.Ptr);
         return res;
      }

      ///<summary> Find the elementwise minimum value </summary>
      ///<param name="img2">The second image for the Min operation</param>
      ///<returns> An image where each pixel is the minimum of <i>this</i> image and the parameter image</returns>
      public Image<TColor, TDepth> Min(Image<TColor, TDepth> img2)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvMin(Ptr, img2.Ptr, res.Ptr);
         return res;
      }

      ///<summary> Find the elementwise minimum value </summary>
      ///<param name="value">The value to compare with</param>
      ///<returns> An image where each pixel is the minimum of <i>this</i> image and <paramref name="value"/></returns>
      public Image<TColor, TDepth> Min(double value)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvMinS(Ptr, value, res.Ptr);
         return res;
      }

      ///<summary>Checks that image elements lie between two scalars</summary>
      ///<param name="lower"> The inclusive lower limit of color value</param>
      ///<param name="higher"> The inclusive upper limit of color value</param>
      ///<returns> res[i,j] = 255 if <paramref name="lower"/> &lt;= this[i,j] &lt;= <paramref name="higher"/>, 0 otherwise</returns>
      [ExposableMethod(Exposable = true, Category = "Logic")]
      public Image<Gray, Byte> InRange(TColor lower, TColor higher)
      {
         Image<Gray, Byte> res = new Image<Gray, Byte>(Size);
         CvInvoke.cvInRangeS(Ptr, lower.MCvScalar, higher.MCvScalar, res.Ptr);
         return res;
      }

      ///<summary>Checks that image elements lie between values defined by two images of same size and type</summary>
      ///<param name="lower"> The inclusive lower limit of color value</param>
      ///<param name="higher"> The inclusive upper limit of color value</param>
      ///<returns> res[i,j] = 255 if <paramref name="lower"/>[i,j] &lt;= this[i,j] &lt;= <paramref name="higher"/>[i,j], 0 otherwise</returns>
      public Image<Gray, Byte> InRange(Image<TColor, TDepth> lower, Image<TColor, TDepth> higher)
      {
         Image<Gray, Byte> res = new Image<Gray, Byte>(Size);
         CvInvoke.cvInRange(Ptr, lower, higher, res);
         return res;
      }

      /// <summary>
      /// Compare the current image with <paramref name="img2"/> and returns the comparison mask
      /// </summary>
      /// <param name="img2">The other image to compare with</param>
      /// <param name="cmpType">The comparison type</param>
      /// <returns>The result of the comparison as a mask</returns>
      public Image<TColor, Byte> Cmp(Image<TColor, TDepth> img2, CvEnum.CMP_TYPE cmpType)
      {
         Size size = Size;
         Image<TColor, Byte> res = new Image<TColor, byte>(size);

         if (NumberOfChannels == 1)
         {
            CvInvoke.cvCmp(Ptr, img2.Ptr, res.Ptr, cmpType);
         }
         else
         {
            using (Image<Gray, TDepth> src1 = new Image<Gray, TDepth>(size))
            using (Image<Gray, TDepth> src2 = new Image<Gray, TDepth>(size))
            using (Image<Gray, Byte> dest = new Image<Gray, Byte>(size))
               for (int i = 0; i < NumberOfChannels; i++)
               {
                  CvInvoke.cvSetImageCOI(Ptr, i + 1);
                  CvInvoke.cvSetImageCOI(img2.Ptr, i + 1);
                  CvInvoke.cvCopy(Ptr, src1.Ptr, IntPtr.Zero);
                  CvInvoke.cvCopy(img2.Ptr, src2.Ptr, IntPtr.Zero);

                  CvInvoke.cvCmp(src1.Ptr, src2.Ptr, dest.Ptr, cmpType);

                  CvInvoke.cvSetImageCOI(res.Ptr, i + 1);
                  CvInvoke.cvCopy(dest.Ptr, res.Ptr, IntPtr.Zero);
               }
            CvInvoke.cvSetImageCOI(Ptr, 0);
            CvInvoke.cvSetImageCOI(img2.Ptr, 0);
            CvInvoke.cvSetImageCOI(res.Ptr, 0);
         }

         return res;
      }

      /// <summary>
      /// Compare the current image with <paramref name="value"/> and returns the comparison mask
      /// </summary>
      /// <param name="value">The value to compare with</param>
      /// <param name="comparisonType">The comparison type</param>
      /// <returns>The result of the comparison as a mask</returns>
      [ExposableMethod(Exposable = true, Category = "Logic")]
      public Image<TColor, Byte> Cmp(double value, CvEnum.CMP_TYPE comparisonType)
      {
         Size size = Size;
         Image<TColor, Byte> res = new Image<TColor, byte>(size);

         if (NumberOfChannels == 1)
         {
            CvInvoke.cvCmpS(Ptr, value, res.Ptr, comparisonType);
         }
         else
         {
            ForEachDuplicateChannel<Byte>(
               delegate(IntPtr img1, IntPtr img2, int channel)
               {
                  CvInvoke.cvCmpS(img1, value, img2, comparisonType);
               },
               res);
         }

         return res;
      }

      /// <summary>
      /// Compare two images, returns true if the each of the pixels are equal, false otherwise
      /// </summary>
      /// <param name="img2">The other image to compare with</param>
      /// <returns>true if the each of the pixels for the two images are equal, false otherwise</returns>
      public bool Equals(Image<TColor, TDepth> img2)
      {
         //true if the references are equal
         if (Object.ReferenceEquals(this, img2)) return true;

         //false if size are not equal
         if (!Size.Equals(img2.Size)) return false;

         using (Image<TColor, TDepth> neqMask = new Image<TColor, TDepth>(Size))
         {
            CvInvoke.cvXor(_ptr, img2.Ptr, neqMask.Ptr, IntPtr.Zero);
            if (NumberOfChannels == 1)
               return CvInvoke.cvCountNonZero(neqMask) == 0;
            else
            {
               IntPtr singleChannel = Marshal.AllocHGlobal(StructSize.MCvMat);
               try
               {
                  CvInvoke.cvReshape(neqMask, singleChannel, 1, 0);
                  return CvInvoke.cvCountNonZero(singleChannel) == 0;
               }
               finally
               {
                  Marshal.FreeHGlobal(singleChannel);
               }
            }
         }
      }
      #endregion

      /*
      #region Discrete Transforms
      /// <summary>
      /// performs forward or inverse transform of 1D or 2D floating-point array
      /// </summary>
      /// <param name="type">Transformation flags</param>
      /// <param name="nonzeroRows">Number of nonzero rows to in the source array (in case of forward 2d transform), or a number of rows of interest in the destination array (in case of inverse 2d transform). If the value is negative, zero, or greater than the total number of rows, it is ignored. The parameter can be used to speed up 2d convolution/correlation when computing them via DFT</param>
      /// <returns>The result of DFT</returns>
      [ExposableMethod(Exposable = true, Category = "Discrete Transforms")]
      public Image<TColor, Single> DFT(CvEnum.CV_DXT type, int nonzeroRows)
      {
         Image<TColor, Single> res = new Image<TColor, float>(Width, Height);
         CvInvoke.cvDFT(Ptr, res.Ptr, type, nonzeroRows);
         return res;
      }

      /// <summary>
      /// performs forward or inverse transform of 2D floating-point image
      /// </summary>
      /// <param name="type">Transformation flags</param>
      /// <returns>The result of DFT</returns>
      public Image<TColor, Single> DFT(CvEnum.CV_DXT type)
      {
         return DFT(type, 0);
      }

      /// <summary>
      /// performs forward or inverse transform of 2D floating-point image
      /// </summary>
      /// <param name="type">Transformation flags</param>
      /// <returns>The result of DCT</returns>
      [ExposableMethod(Exposable = true, Category = "Discrete Transforms")]
      public Image<TColor, Single> DCT(CvEnum.CV_DCT_TYPE type)
      {
         Image<TColor, Single> res = new Image<TColor, float>(Width, Height);
         CvInvoke.cvDCT(Ptr, res.Ptr, type);
         return res;
      }
      #endregion
      */
      #region Segmentation
      /// <summary>
      /// Use grabcut to perform background foreground segmentation.
      /// </summary>
      /// <param name="rect">The initial rectangle region for the foreground</param>
      /// <param name="iteration">The number of iterations to run GrabCut</param>
      /// <returns>The background foreground mask where 2 indicates background and 3 indicates foreground</returns>
      public Image<Gray, Byte> GrabCut(Rectangle rect, int iteration)
      {
         Image<Gray, Byte> mask = new Image<Gray, byte>(Size);
         using (Matrix<double> bgdModel = new Matrix<double>(1, 13 * 5))
         using (Matrix<double> fgdModel = new Matrix<double>(1, 13 * 5))
         {
            CvInvoke.CvGrabCut(Ptr, mask.Ptr, ref rect, bgdModel, fgdModel, 0, Emgu.CV.CvEnum.GRABCUT_INIT_TYPE.INIT_WITH_RECT);
            CvInvoke.CvGrabCut(Ptr, mask.Ptr, ref rect, bgdModel, fgdModel, iteration, Emgu.CV.CvEnum.GRABCUT_INIT_TYPE.EVAL);
         }
         return mask;
      }
      #endregion

      #region Arithmatic
      #region Subtraction methods
      ///<summary> Elementwise subtract another image from the current image </summary>
      ///<param name="img2">The second image to be subtracted from the current image</param>
      ///<returns> The result of elementwise subtracting img2 from the current image</returns>
      public Image<TColor, TDepth> Sub(Image<TColor, TDepth> img2)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvSub(Ptr, img2.Ptr, res.Ptr, IntPtr.Zero);
         return res;
      }

      ///<summary> Elementwise subtract another image from the current image, using a mask</summary>
      ///<param name="img2">The image to be subtracted from the current image</param>
      ///<param name="mask">The mask for the subtract operation</param>
      ///<returns> The result of elementwise subtrating img2 from the current image, using the specific mask</returns>
      public Image<TColor, TDepth> Sub(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvSub(Ptr, img2.Ptr, res.Ptr, mask.Ptr);
         return res;
      }

      ///<summary> Elementwise subtract a color from the current image</summary>
      ///<param name="val">The color value to be subtracted from the current image</param>
      ///<returns> The result of elementwise subtracting color 'val' from the current image</returns>
      [ExposableMethod(Exposable = true, Category = "Math")]
      public Image<TColor, TDepth> Sub(TColor val)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvSubS(Ptr, val.MCvScalar, res.Ptr, IntPtr.Zero);
         return res;
      }

      /// <summary>
      /// result = val - this
      /// </summary>
      /// <param name="val">the value which subtract this image</param>
      /// <returns>val - this</returns>
      [ExposableMethod(Exposable = true, Category = "Math")]
      public Image<TColor, TDepth> SubR(TColor val)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvSubRS(Ptr, val.MCvScalar, res.Ptr, IntPtr.Zero);
         return res;
      }

      /// <summary>
      /// result = val - this, using a mask
      /// </summary>
      /// <param name="val">The value which subtract this image</param>
      /// <param name="mask">The mask for subtraction</param>
      /// <returns>val - this, with mask</returns>
      public Image<TColor, TDepth> SubR(TColor val, Image<Gray, Byte> mask)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvSubRS(Ptr, val.MCvScalar, res.Ptr, mask.Ptr);
         return res;
      }
      #endregion

      #region Addition methods
      ///<summary> Elementwise add another image with the current image </summary>
      ///<param name="img2">The image to be added to the current image</param>
      ///<returns> The result of elementwise adding img2 to the current image</returns>
      public Image<TColor, TDepth> Add(Image<TColor, TDepth> img2)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvAdd(Ptr, img2.Ptr, res.Ptr, IntPtr.Zero);
         return res;
      }
      ///<summary> Elementwise add <paramref name="img2"/> with the current image, using a mask</summary>
      ///<param name="img2">The image to be added to the current image</param>
      ///<param name="mask">The mask for the add operation</param>
      ///<returns> The result of elementwise adding img2 to the current image, using the specific mask</returns>
      public Image<TColor, TDepth> Add(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvAdd(Ptr, img2.Ptr, res.Ptr, mask.Ptr);
         return res;
      }
      ///<summary> Elementwise add a color <paramref name="val"/> to the current image</summary>
      ///<param name="val">The color value to be added to the current image</param>
      ///<returns> The result of elementwise adding color <paramref name="val"/> from the current image</returns>
      [ExposableMethod(Exposable = true, Category = "Math")]
      public Image<TColor, TDepth> Add(TColor val)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvAddS(Ptr, val.MCvScalar, res.Ptr, IntPtr.Zero);
         return res;
      }
      #endregion

      #region Multiplication methods
      ///<summary> Elementwise multiply another image with the current image and the <paramref name="scale"/></summary>
      ///<param name="img2">The image to be elementwise multiplied to the current image</param>
      ///<param name="scale">The scale to be multiplied</param>
      ///<returns> this .* img2 * scale </returns>
      public Image<TColor, TDepth> Mul(Image<TColor, TDepth> img2, double scale)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvMul(Ptr, img2.Ptr, res.Ptr, scale);
         return res;
      }

      ///<summary> Elementwise multiply <paramref name="img2"/> with the current image</summary>
      ///<param name="img2">The image to be elementwise multiplied to the current image</param>
      ///<returns> this .* img2 </returns>
      public Image<TColor, TDepth> Mul(Image<TColor, TDepth> img2)
      {
         return Mul(img2, 1.0);
      }

      ///<summary> Elementwise multiply the current image with <paramref name="scale"/></summary>
      ///<param name="scale">The scale to be multiplied</param>
      ///<returns> The scaled image </returns>
      [ExposableMethod(Exposable = true, Category = "Math")]
      public Image<TColor, TDepth> Mul(double scale)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvConvertScale(Ptr, res.Ptr, scale, 0.0);
         return res;
      }
      #endregion

      /// <summary>
      /// Accumulate <paramref name="img2"/> to the current image using the specific mask
      /// </summary>
      /// <param name="img2">The image to be added to the current image</param>
      /// <param name="mask">the mask</param>
      public void Acc(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
      {
         CvInvoke.cvAcc(img2.Ptr, Ptr, mask.Ptr);
      }

      /// <summary>
      /// Accumulate <paramref name="img2"/> to the current image using the specific mask
      /// </summary>
      /// <param name="img2">The image to be added to the current image</param>
      public void Acc(Image<TColor, TDepth> img2)
      {
         CvInvoke.cvAcc(img2.Ptr, Ptr, IntPtr.Zero);
      }

      ///<summary> 
      ///Return the weighted sum such that: res = this * alpha + img2 * beta + gamma
      ///</summary>
      ///<param name="img2">img2 in: res = this * alpha + img2 * beta + gamma </param>
      ///<param name="alpha">alpha in: res = this * alpha + img2 * beta + gamma</param>
      ///<param name="beta">beta in: res = this * alpha + img2 * beta + gamma</param>
      ///<param name="gamma">gamma in: res = this * alpha + img2 * beta + gamma</param>
      ///<returns>this * alpha + img2 * beta + gamma</returns>
      public Image<TColor, TDepth> AddWeighted(Image<TColor, TDepth> img2, double alpha, double beta, double gamma)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvAddWeighted(Ptr, alpha, img2.Ptr, beta, gamma, res.Ptr);
         return res;
      }

      ///<summary> 
      /// Update Running Average. <i>this</i> = (1-alpha)*<i>this</i> + alpha*img
      ///</summary>
      ///<param name="img">Input image, 1- or 3-channel, Byte or Single (each channel of multi-channel image is processed independently). </param>
      ///<param name="alpha">the weight of <paramref name="img"/></param>
      public void RunningAvg(Image<TColor, TDepth> img, double alpha)
      {
         RunningAvg(img, alpha, null);
      }

      ///<summary> 
      /// Update Running Average. <i>this</i> = (1-alpha)*<i>this</i> + alpha*img, using the mask
      ///</summary>
      ///<param name="img">Input image, 1- or 3-channel, Byte or Single (each channel of multi-channel image is processed independently). </param>
      ///<param name="alpha">The weight of <paramref name="img"/></param>
      ///<param name="mask">The mask for the running average</param>
      public void RunningAvg(Image<TColor, TDepth> img, double alpha, Image<Gray, Byte> mask)
      {
         CvInvoke.cvRunningAvg(img.Ptr, Ptr, alpha, mask == null ? IntPtr.Zero : mask.Ptr);
      }

      ///<summary> 
      ///Computes absolute different between <i>this</i> image and the other image
      ///</summary>
      ///<param name="img2">The other image to compute absolute different with</param>
      ///<returns> The image that contains the absolute different value</returns>
      public Image<TColor, TDepth> AbsDiff(Image<TColor, TDepth> img2)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvAbsDiff(Ptr, img2.Ptr, res.Ptr);
         return res;
      }

      ///<summary> 
      ///Computes absolute different between <i>this</i> image and the specific color
      ///</summary>
      ///<param name="color">The color to compute absolute different with</param>
      ///<returns> The image that contains the absolute different value</returns>
      [ExposableMethod(Exposable = true, Category = "Math")]
      public Image<TColor, TDepth> AbsDiff(TColor color)
      {
         Image<TColor, TDepth> res = new Image<TColor, TDepth>(Size);
         CvInvoke.cvAbsDiffS(Ptr, res.Ptr, color.MCvScalar);
         return res;
      }
      #endregion

      #region Math Functions
      /// <summary>
      /// Raises every element of input array to p
      /// dst(I)=src(I)^p, if p is integer
      /// dst(I)=abs(src(I))^p, otherwise
      /// </summary>
      /// <param name="power">The exponent of power</param>
      /// <returns>The power image</returns>
      [ExposableMethod(Exposable = true, Category = "Math")]
      public Image<TColor, TDepth> Pow(double power)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvPow(Ptr, res.Ptr, power);
         return res;
      }

      /// <summary>
      /// Calculates exponent of every element of input array:
      /// dst(I)=exp(src(I))
      /// </summary>
      /// <remarks>Maximum relative error is ~7e-6. Currently, the function converts denormalized values to zeros on output.</remarks>
      /// <returns>The exponent image</returns>
      [ExposableMethod(Exposable = true, Category = "Math")]
      public Image<TColor, TDepth> Exp()
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvExp(Ptr, res.Ptr);
         return res;
      }

      /// <summary>
      /// Calculates natural logarithm of absolute value of every element of input array
      /// </summary>
      /// <returns>Natural logarithm of absolute value of every element of input array</returns>
      [ExposableMethod(Exposable = true, Category = "Math")]
      public Image<TColor, TDepth> Log()
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvLog(Ptr, res.Ptr);
         return res;
      }
      #endregion

      #region Sampling, Interpolation and Geometrical Transforms
      ///<summary> Sample the pixel values on the specific line segment </summary>
      ///<param name="line"> The line to obtain samples</param>
      ///<returns>The values on the (Eight-connected) line </returns>
      public TDepth[,] Sample(LineSegment2D line)
      {
         return Sample(line, Emgu.CV.CvEnum.CONNECTIVITY.EIGHT_CONNECTED);
      }

      /// <summary>
      /// Sample the pixel values on the specific line segment
      /// </summary>
      /// <param name="line">The line to obtain samples</param>
      /// <param name="type">The sampling type</param>
      /// <returns>The values on the line, the first dimension is the index of the point, the second dimension is the index of color channel</returns>
      public TDepth[,] Sample(LineSegment2D line, CvEnum.CONNECTIVITY type)
      {
         int size = type == Emgu.CV.CvEnum.CONNECTIVITY.EIGHT_CONNECTED ?
            Math.Max(Math.Abs(line.P2.X - line.P1.X), Math.Abs(line.P2.Y - line.P1.Y))
            : Math.Abs(line.P2.X - line.P1.X) + Math.Abs(line.P2.Y - line.P1.Y);

         TDepth[,] data = new TDepth[size, NumberOfChannels];
         GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
         CvInvoke.cvSampleLine(
             Ptr,
             line.P1,
             line.P2,
             handle.AddrOfPinnedObject(),
             type);
         handle.Free();
         return data;
      }

      /// <summary>
      /// Scale the image to the specific size 
      /// </summary>
      /// <param name="width">The width of the returned image.</param>
      /// <param name="height">The height of the returned image.</param>
      /// <param name="interpolationType">The type of interpolation</param>
      /// <returns>The resized image</returns>
      [ExposableMethod(Exposable = true)]
      public Image<TColor, TDepth> Resize(int width, int height, CvEnum.INTER interpolationType)
      {
         Image<TColor, TDepth> imgScale = new Image<TColor, TDepth>(width, height);
         CvInvoke.cvResize(Ptr, imgScale.Ptr, interpolationType);
         return imgScale;
      }

      /// <summary>
      /// Scale the image to the specific size
      /// </summary>
      /// <param name="width">The width of the returned image.</param>
      /// <param name="height">The height of the returned image.</param>
      /// <param name="interpolationType">The type of interpolation</param>
      /// <param name="preserveScale">if true, the scale is preservered and the resulting image has maximum width(height) possible that is &lt;= <paramref name="width"/> (<paramref name="height"/>), if false, this function is equaivalent to Resize(int width, int height)</param>
      /// <returns>The resized image</returns>
      public Image<TColor, TDepth> Resize(int width, int height, CvEnum.INTER interpolationType, bool preserveScale)
      {
         return preserveScale ?
            Resize(Math.Min((double)width / Width, (double)height / Height), interpolationType)
            : Resize(width, height, interpolationType);
      }

      /// <summary>
      /// Scale the image to the specific size: width *= scale; height *= scale  
      /// </summary>
      /// <param name="scale">The scale to resize</param>
      /// <param name="interpolationType">The type of interpolation</param>
      /// <returns>The scaled image</returns>
      [ExposableMethod(Exposable = true)]
      public Image<TColor, TDepth> Resize(double scale, CvEnum.INTER interpolationType)
      {
         return Resize(
             (int)(Width * scale),
             (int)(Height * scale),
             interpolationType);
      }

      /// <summary>
      /// Rotate the image the specified angle cropping the result to the original size
      /// </summary>
      /// <param name="angle">The angle of rotation in degrees.</param>
      /// <param name="background">The color with wich to fill the background</param>   
      /// <returns>The image rotates by the specific angle</returns>
      public Image<TColor, TDepth> Rotate(double angle, TColor background)
      {
         return Rotate(angle, background, true);
      }

      /// <summary>
      /// Transforms source image using the specified matrix
      /// </summary>
      /// <param name="mapMatrix">2x3 transformation matrix</param>
      /// <param name="interpolationType">Interpolation type</param>
      /// <param name="warpType">Warp type</param>
      /// <param name="backgroundColor">A value used to fill outliers</param>
      /// <returns>The result of the transformation</returns>
      public Image<TColor, TDepth> WarpAffine<TMapDepth>(Matrix<TMapDepth> mapMatrix, CvEnum.INTER interpolationType, CvEnum.WARP warpType, TColor backgroundColor)
         where TMapDepth : new()
      {
         return WarpAffine(mapMatrix, Width, Height, interpolationType, warpType, backgroundColor);
      }

      /// <summary>
      /// Transforms source image using the specified matrix
      /// </summary>
      /// <param name="mapMatrix">2x3 transformation matrix</param>
      /// <param name="width">The width of the resulting image</param>
      /// <param name="height">the height of the resulting image</param>
      /// <param name="interpolationType">Interpolation type</param>
      /// <param name="warpType">Warp type</param>
      /// <param name="backgroundColor">A value used to fill outliers</param>
      /// <typeparam name="TMapDepth">The depth type of <paramref name="mapMatrix"/>, should be either float or double</typeparam>
      /// <returns>The result of the transformation</returns>
      public Image<TColor, TDepth> WarpAffine<TMapDepth>(Matrix<TMapDepth> mapMatrix, int width, int height, CvEnum.INTER interpolationType, CvEnum.WARP warpType, TColor backgroundColor)
         where TMapDepth : new()
      {
         Image<TColor, TDepth> res = new Image<TColor, TDepth>(width, height);
         CvInvoke.cvWarpAffine(Ptr, res.Ptr, mapMatrix.Ptr, (int)interpolationType | (int)warpType, backgroundColor.MCvScalar);
         return res;
      }

      /// <summary>
      /// Transforms source image using the specified matrix
      /// </summary>
      /// <param name="mapMatrix">3x3 transformation matrix</param>
      /// <param name="interpolationType">Interpolation type</param>
      /// <param name="warpType">Warp type</param>
      /// <param name="backgroundColor">A value used to fill outliers</param>
      /// <typeparam name="TMapDepth">The depth type of <paramref name="mapMatrix"/>, should be either float or double</typeparam>
      /// <returns>The result of the transformation</returns>
      public Image<TColor, TDepth> WarpPerspective<TMapDepth>(Matrix<TMapDepth> mapMatrix, CvEnum.INTER interpolationType, CvEnum.WARP warpType, TColor backgroundColor)
         where TMapDepth : new()
      {
         return WarpPerspective(mapMatrix, Width, Height, interpolationType, warpType, backgroundColor);
      }

      /// <summary>
      /// Transforms source image using the specified matrix
      /// </summary>
      /// <param name="mapMatrix">3x3 transformation matrix</param>
      /// <param name="width">The width of the resulting image</param>
      /// <param name="height">the height of the resulting image</param>
      /// <param name="interpolationType">Interpolation type</param>
      /// <param name="warpType">Warp type</param>
      /// <param name="backgroundColor">A value used to fill outliers</param>
      /// <typeparam name="TMapDepth">The depth type of <paramref name="mapMatrix"/>, should be either float or double</typeparam>
      /// <returns>The result of the transformation</returns>
      public Image<TColor, TDepth> WarpPerspective<TMapDepth>(Matrix<TMapDepth> mapMatrix, int width, int height, CvEnum.INTER interpolationType, CvEnum.WARP warpType, TColor backgroundColor)
         where TMapDepth : new()
      {
         Image<TColor, TDepth> res = new Image<TColor, TDepth>(width, height);
         CvInvoke.cvWarpPerspective(Ptr, res.Ptr, mapMatrix.Ptr, (int)interpolationType | (int)warpType, backgroundColor.MCvScalar);
         return res;
      }

      /// <summary>
      /// Rotate this image the specified <paramref name="angle"/>
      /// </summary>
      /// <param name="angle">The angle of rotation in degrees.</param>
      /// <param name="background">The color with wich to fill the background</param>
      /// <param name="crop">If set to true the image is cropped to its original size, possibly losing corners information. If set to false the result image has different size than original and all rotation information is preserved</param>
      /// <returns>The rotated image</returns>
      [ExposableMethod(Exposable = true, Category = "Transform")]
      public Image<TColor, TDepth> Rotate(double angle, TColor background, bool crop)
      {
         Size size = Size;
         PointF center = new PointF(size.Width * 0.5f, size.Height * 0.5f);
         return Rotate(angle, center, CvEnum.INTER.CV_INTER_CUBIC, background, crop);
      }

      /// <summary>
      /// Rotate this image the specified <paramref name="angle"/>
      /// </summary>
      /// <param name="angle">The angle of rotation in degrees. Positive means clockwise.</param>
      /// <param name="background">The color with wich to fill the background</param>
      /// <param name="crop">If set to true the image is cropped to its original size, possibly losing corners information. If set to false the result image has different size than original and all rotation information is preserved</param>
      /// <param name="center">The center of rotation</param>
      /// <param name="interpolationMethod">The intepolation method</param>
      /// <returns>The rotated image</returns>
      public Image<TColor, TDepth> Rotate(double angle, PointF center, CvEnum.INTER interpolationMethod, TColor background, bool crop)
      {
         if (crop)
         {
            using (RotationMatrix2D<float> rotationMatrix = new RotationMatrix2D<float>(center, -angle, 1))
               return WarpAffine(rotationMatrix, interpolationMethod, Emgu.CV.CvEnum.WARP.CV_WARP_FILL_OUTLIERS, background);
         }
         else
         {
            Size dstImgSize;
            using (RotationMatrix2D<float> rotationMatrix = RotationMatrix2D<float>.CreateRotationMatrix(center, -angle, Size, out dstImgSize))
            {
               return WarpAffine(rotationMatrix, dstImgSize.Width, dstImgSize.Height, interpolationMethod, Emgu.CV.CvEnum.WARP.CV_WARP_FILL_OUTLIERS, background);
            }
         }
      }

      ///<summary>
      /// Convert the image to log polar, simulating the human foveal vision
      /// </summary>
      /// <param name="center">The transformation center, where the output precision is maximal</param>
      /// <param name="magnitude">Magnitude scale parameter</param>
      /// <param name="interpolationType">interpolation type</param>
      /// <param name="warpType">Warp type</param>
      /// <returns>The converted image</returns>
      [ExposableMethod(Exposable = true, Category = "Transform")]
      public Image<TColor, TDepth> LogPolar(PointF center, double magnitude, CvEnum.INTER interpolationType, CvEnum.WARP warpType)
      {
         Image<TColor, TDepth> imgPolar = CopyBlank();
         int flags = (int)interpolationType | (int)warpType;
         CvInvoke.cvLogPolar(Ptr, imgPolar.Ptr, center, magnitude, flags);
         return imgPolar;
      }
      #endregion

      #region Image color and depth conversion
      ///<summary> Convert the current image to the specific color and depth </summary>
      ///<typeparam name="TOtherColor"> The type of color to be converted to </typeparam>
      ///<typeparam name="TOtherDepth"> The type of pixel depth to be converted to </typeparam>
      ///<returns> Image of the specific color and depth </returns>
      [ExposableMethod(
         Exposable = true,
         Category = "Conversion",
         GenericParametersOptions = new Type[] {
            typeof(Bgr), typeof(Bgra), typeof(Gray), typeof(Hsv), typeof(Hls), typeof(Lab), typeof(Luv), typeof(Xyz), typeof(Ycc),
            typeof(Single), typeof(Byte), typeof(Double)},
         GenericParametersOptionSizes = new int[] { 9, 3 }
         )]
      public Image<TOtherColor, TOtherDepth> Convert<TOtherColor, TOtherDepth>()
         where TOtherColor : struct, IColor
         where TOtherDepth : new()
      {
         Image<TOtherColor, TOtherDepth> res = new Image<TOtherColor, TOtherDepth>(Size);
         res.ConvertFrom(this);
         return res;
      }

      /// <summary>
      /// Convert the source image to the current image, if the size are different, the current image will be a resized version of the srcImage. 
      /// </summary>
      /// <typeparam name="TSrcColor">The color type of the source image</typeparam>
      /// <typeparam name="TSrcDepth">The color depth of the source image</typeparam>
      /// <param name="srcImage">The sourceImage</param>
      public void ConvertFrom<TSrcColor, TSrcDepth>(Image<TSrcColor, TSrcDepth> srcImage)
         where TSrcColor : struct, IColor
         where TSrcDepth : new()
      {
         if (!Size.Equals(srcImage.Size))
         {  //if the size of the source image do not match the size of the current image
            using (Image<TSrcColor, TSrcDepth> tmp = srcImage.Resize(Width, Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR))
            {
               ConvertFrom(tmp);
               return;
            }
         }

         if (typeof(TColor) == typeof(TSrcColor))
         {
            #region same color
            if (typeof(TDepth) == typeof(TSrcDepth))
            {   //same depth
               CvInvoke.cvCopy(srcImage.Ptr, Ptr, IntPtr.Zero);
            }
            else
            {
               //different depth
               //int channelCount = NumberOfChannels;
               {
                  if (typeof(TDepth) == typeof(Byte) && typeof(TSrcDepth) != typeof(Byte))
                  {
                     double[] minVal, maxVal;
                     Point[] minLoc, maxLoc;
                     srcImage.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);
                     double min = minVal[0];
                     double max = maxVal[0];
                     for (int i = 1; i < minVal.Length; i++)
                     {
                        min = Math.Min(min, minVal[i]);
                        max = Math.Max(max, maxVal[i]);
                     }
                     double scale = 1.0, shift = 0.0;
                     if (max > 255.0 || min < 0)
                     {
                        scale = (max == min) ? 0.0 : 255.0 / (max - min);
                        shift = (scale == 0) ? min : -min * scale;
                     }

                     CvInvoke.cvConvertScaleAbs(srcImage.Ptr, Ptr, scale, shift);
                  }
                  else
                  {
                     CvInvoke.cvConvertScale(srcImage.Ptr, Ptr, 1.0, 0.0);
                  }
               }
            }
            #endregion
         }
         else
         {
            #region different color
            if (typeof(TDepth) == typeof(TSrcDepth))
            {   //same depth
               ConvertColor(srcImage.Ptr, Ptr, typeof(TSrcColor), typeof(TColor), Size);
            }
            else
            {   //different depth
               using (Image<TSrcColor, TDepth> tmp = srcImage.Convert<TSrcColor, TDepth>()) //convert depth
                  ConvertColor(tmp.Ptr, Ptr, typeof(TSrcColor), typeof(TColor), Size);
            }
            #endregion
         }
      }

      private static void ConvertColor(IntPtr src, IntPtr dest, Type srcColor, Type destColor, Size size)
      {
         try
         {
            // if the direct conversion exist, apply the conversion
            CvInvoke.cvCvtColor(src, dest, CvToolbox.GetColorCvtCode(srcColor, destColor));
         }
         catch
         {
            try
            {
               //if a direct conversion doesn't exist, apply a two step conversion
               using (Image<Bgr, TDepth> tmp = new Image<Bgr, TDepth>(size))
               {
                  CvInvoke.cvCvtColor(src, tmp.Ptr, CvToolbox.GetColorCvtCode(srcColor, typeof(Bgr)));
                  CvInvoke.cvCvtColor(tmp.Ptr, dest, CvToolbox.GetColorCvtCode(typeof(Bgr), destColor));
               }
            }
            catch
            {
               throw new NotSupportedException(String.Format(
                  "Convertion from Image<{0}, {1}> to Image<{2}, {3}> is not supported by OpenCV",
                  srcColor.ToString(),
                  typeof(TDepth).ToString(),
                  destColor.ToString(),
                  typeof(TDepth).ToString()));
            }
         }
      }

      ///<summary> Convert the current image to the specific depth, at the same time scale and shift the values of the pixel</summary>
      ///<param name="scale"> The value to be multipled with the pixel </param>
      ///<param name="shift"> The value to be added to the pixel</param>
      /// <typeparam name="TOtherDepth"> The type of depth to convert to</typeparam>
      ///<returns> Image of the specific depth, val = val * scale + shift </returns>
      public Image<TColor, TOtherDepth> ConvertScale<TOtherDepth>(double scale, double shift)
                  where TOtherDepth : new()
      {
         Image<TColor, TOtherDepth> res = new Image<TColor, TOtherDepth>(Width, Height);

         if (typeof(TOtherDepth) == typeof(Byte))
            CvInvoke.cvConvertScaleAbs(Ptr, res.Ptr, scale, shift);
         else
            CvInvoke.cvConvertScale(Ptr, res.Ptr, scale, shift);

         return res;
      }
      #endregion

#if IOS || NETFX_CORE
#else
      //#region Conversion with Bitmap
      /// <summary>
      /// The Get property provide a more efficient way to convert Image&lt;Gray, Byte&gt;, Image&lt;Bgr, Byte&gt; and Image&lt;Bgra, Byte&gt; into Bitmap
      /// such that the image data is <b>shared</b> with Bitmap. 
      /// If you change the pixel value on the Bitmap, you change the pixel values on the Image object as well!
      /// For other types of image this property has the same effect as ToBitmap()
      /// <b>Take extra caution not to use the Bitmap after the Image object is disposed</b>
      /// The Set property convert the bitmap to this Image type.
      /// </summary>
      public virtual Bitmap Bitmap
      {
         get
         {
#if ANDROID
            return ToBitmap();
#else
            IntPtr scan0;
            int step;
            Size size;
            CvInvoke.cvGetRawData(Ptr, out scan0, out step, out size);

            if (this is Image<Gray, Byte>)
            {   //Grayscale of Bytes
               Bitmap bmp = new Bitmap(
                   size.Width,
                   size.Height,
                   step,
                   PixelFormat.Format8bppIndexed,
                   scan0
                   );

               bmp.Palette = CvToolbox.GrayscalePalette;

               return bmp;
            }
            // Mono in Linux doesn't support scan0 constructure with Format24bppRgb, use ToBitmap instead
            // See https://bugzilla.novell.com/show_bug.cgi?id=363431
            // TODO: check mono buzilla Bug 363431 to see when it will be fixed 
            else if (
               Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Windows &&
               Platform.ClrType == Emgu.Util.TypeEnum.ClrType.DotNet &&
               this is Image<Bgr, Byte>)
            {   //Bgr byte    
               return new Bitmap(
                   size.Width,
                   size.Height,
                   step,
                   PixelFormat.Format24bppRgb,
                   scan0);
            }
            else if (this is Image<Bgra, Byte>)
            {   //Bgra byte
               return new Bitmap(
                   size.Width,
                   size.Height,
                   step,
                   PixelFormat.Format32bppArgb,
                   scan0);
            }
            /*
            //PixelFormat.Format16bppGrayScale is not supported in .NET
            else if (typeof(TColor) == typeof(Gray) && typeof(TDepth) == typeof(UInt16))
            {
               return new Bitmap(
                  size.width,
                  size.height,
                  step,
                  PixelFormat.Format16bppGrayScale;
                  scan0);
            }*/
            else
            {  //default handler
               return ToBitmap();
            }
#endif
         }
         set
         {
#if ANDROID
            #region reallocate memory if necessary
            Size size = new Size(value.Width, value.Height);
            if (Ptr == IntPtr.Zero)
            {
               AllocateData(size.Height, size.Width, NumberOfChannels);
            } else if (!Size.Equals(size))
            {
               DisposeObject();
               AllocateData(size.Height, size.Width, NumberOfChannels);
            }
            #endregion

            Bitmap.Config config = value.GetConfig();
            if (config.Equals(Bitmap.Config.Argb8888))
            {
               using (BitmapArgb8888Image bi = new BitmapArgb8888Image(value))
               {
                  ConvertFrom(bi);
               }
            } else if (config.Equals(Bitmap.Config.Rgb565))
            {
               int[] values = new int[size.Width * size.Height];
               value.GetPixels(values, 0, size.Width, 0, 0, size.Width, size.Height);
               GCHandle handle = GCHandle.Alloc(values, GCHandleType.Pinned);
               using (Image<Bgra, Byte> bgra = new Image<Bgra, byte>(size.Width, size.Height, size.Width * 4, handle.AddrOfPinnedObject()))
               {
                  ConvertFrom(bgra);
               }
               handle.Free();
            }
            else
            {
               throw new NotImplementedException(String.Format("Coping from Bitmap of {0} is not implemented", config));
            }
#else
            #region reallocate memory if necessary
            Size size = value.Size;
            if (Ptr == IntPtr.Zero)
            {
               AllocateData(size.Height, size.Width, NumberOfChannels);
            } else if (!Size.Equals(size))
            {
               DisposeObject();
               AllocateData(size.Height, size.Width, NumberOfChannels);
            }
            #endregion

            switch (value.PixelFormat)
            {
               case PixelFormat.Format32bppRgb:
                  if (this is Image<Bgr, Byte>)
                  {
                     BitmapData data = value.LockBits(
                        new Rectangle(Point.Empty, value.Size),
                        ImageLockMode.ReadOnly,
                        value.PixelFormat);

                     using (Image<Bgra, Byte> mat = new Image<Bgra, Byte>(value.Width, value.Height, data.Stride, data.Scan0))
                     {
                        for (int i = 0; i < 3; i++)
                        {
                           CvInvoke.cvSetImageCOI(Ptr, i + 1);
                           CvInvoke.cvSetImageCOI(mat, i + 1);
                           CvInvoke.cvCopy(mat, Ptr, IntPtr.Zero);
                        }
                        CvInvoke.cvSetImageCOI(Ptr, 0);
                     }

                     value.UnlockBits(data);
                  }
                  else
                  {
                     using (Image<Bgr, Byte> tmp = new Image<Bgr, byte>(value))
                        ConvertFrom(tmp);
                  }
                  break;
               case PixelFormat.Format32bppArgb:
                  if (this is Image<Bgra, Byte>)
                     CopyFromBitmap(value);
                  else
                  {
                     BitmapData data = value.LockBits(
                        new Rectangle(Point.Empty, value.Size),
                        ImageLockMode.ReadOnly,
                        value.PixelFormat);
                     using (Image<Bgra, Byte> tmp = new Image<Bgra, byte>(value.Width, value.Height, data.Stride, data.Scan0))
                        ConvertFrom(tmp);
                     value.UnlockBits(data);
                  }
                  break;
               case PixelFormat.Format8bppIndexed:
                  if (this is Image<Bgra, Byte>)
                  {
                     Matrix<Byte> bTable, gTable, rTable, aTable;
                     CvToolbox.ColorPaletteToLookupTable(value.Palette, out bTable, out gTable, out rTable, out aTable);
                     BitmapData data = value.LockBits(
                        new Rectangle(Point.Empty, value.Size),
                        ImageLockMode.ReadOnly,
                        value.PixelFormat);
                     using (Image<Gray, Byte> indexValue = new Image<Gray, byte>(value.Width, value.Height, data.Stride, data.Scan0))
                     {
                        using (Image<Gray, Byte> b = indexValue.CopyBlank())
                        using (Image<Gray, Byte> g = indexValue.CopyBlank())
                        using (Image<Gray, Byte> r = indexValue.CopyBlank())
                        using (Image<Gray, Byte> a = indexValue.CopyBlank())
                        {
                           CvInvoke.cvLUT(indexValue.Ptr, b.Ptr, bTable.Ptr);
                           CvInvoke.cvLUT(indexValue.Ptr, g.Ptr, gTable.Ptr);
                           CvInvoke.cvLUT(indexValue.Ptr, r.Ptr, rTable.Ptr);
                           CvInvoke.cvLUT(indexValue.Ptr, a.Ptr, aTable.Ptr);
                           CvInvoke.cvMerge(b.Ptr, g.Ptr, r.Ptr, a.Ptr, Ptr);
                        }
                     }
                     value.UnlockBits(data);
                     bTable.Dispose(); gTable.Dispose(); rTable.Dispose(); aTable.Dispose();
                  }
                  else
                  {
                     using (Image<Bgra, Byte> tmp = new Image<Bgra, byte>(value))
                        ConvertFrom(tmp);
                  }
                  break;
               case PixelFormat.Format24bppRgb:
                  if (this is Image<Bgr, Byte>)
                     CopyFromBitmap(value);
                  else
                  {
                     BitmapData data = value.LockBits(
                        new Rectangle(Point.Empty, value.Size),
                        ImageLockMode.ReadOnly,
                        value.PixelFormat);
                     using (Image<Bgr, Byte> tmp = new Image<Bgr, byte>(value.Width, value.Height, data.Stride, data.Scan0))
                        ConvertFrom(tmp);
                     value.UnlockBits(data);
                  }
                  break;
               case PixelFormat.Format1bppIndexed:
                  if (this is Image<Gray, Byte>)
                  {
                     int rows = size.Height;
                     int cols = size.Width;
                     BitmapData data = value.LockBits(
                         new Rectangle(Point.Empty, size),
                         ImageLockMode.ReadOnly,
                         value.PixelFormat);

                     int fullByteCount = cols >> 3;
                     int partialBitCount = cols & 7;

                     int mask = 1 << 7;

                     Int64 srcAddress = data.Scan0.ToInt64();
                     Byte[, ,] imagedata = Data as Byte[, ,];

                     Byte[] row = new byte[fullByteCount + (partialBitCount == 0 ? 0 : 1)];

                     int v = 0;
                     for (int i = 0; i < rows; i++, srcAddress += data.Stride)
                     {
                        Marshal.Copy((IntPtr)srcAddress, row, 0, row.Length);

                        for (int j = 0; j < cols; j++, v <<= 1)
                        {
                           if ((j & 7) == 0)
                           {  //fetch the next byte 
                              v = row[j >> 3];
                           }
                           imagedata[i, j, 0] = (v & mask) == 0 ? (Byte)0 : (Byte)255;
                        }
                     }
                  }
                  else
                  {
                     using (Image<Gray, Byte> tmp = new Image<Gray, Byte>(value))
                        ConvertFrom(tmp);
                  }
                  break;
               default:
            #region Handle other image type
                  /*
				               Bitmap bgraImage = new Bitmap(value.Width, value.Height, PixelFormat.Format32bppArgb);
				               using (Graphics g = Graphics.FromImage(bgraImage))
				               {
				                  g.DrawImageUnscaled(value, 0, 0, value.Width, value.Height);
				               }
				               Bitmap = bgraImage;*/
                  using (Image<Bgra, Byte> tmp1 = new Image<Bgra, Byte>(value.Size))
                  {
                     Byte[, ,] data = tmp1.Data;
                     for (int i = 0; i < value.Width; i++)
                        for (int j = 0; j < value.Height; j++)
                        {
                           Color color = value.GetPixel(i, j);
                           data[j, i, 0] = color.B;
                           data[j, i, 1] = color.G;
                           data[j, i, 2] = color.R;
                           data[j, i, 3] = color.A;
                        }

                     ConvertFrom<Bgra, Byte>(tmp1);
                  }
            #endregion
                  break;
            }
#endif
         }
      }

#if ANDROID
#else
      /// <summary>
      /// Utility function for Bitmap Set property
      /// </summary>
      /// <param name="bmp"></param>
      private void CopyFromBitmap(Bitmap bmp)
      {
         BitmapData data = bmp.LockBits(
             new Rectangle(Point.Empty, bmp.Size),
             ImageLockMode.ReadOnly,
             bmp.PixelFormat);

         using (Matrix<TDepth> mat = new Matrix<TDepth>(bmp.Height, bmp.Width, NumberOfChannels, data.Scan0, data.Stride))
            CvInvoke.cvCopy(mat.Ptr, Ptr, IntPtr.Zero);

         bmp.UnlockBits(data);
      }
#endif

      /// <summary> 
      /// Convert this image into Bitmap, the pixel values are copied over to the Bitmap
      /// </summary>
      /// <remarks> For better performance on Image&lt;Gray, Byte&gt; and Image&lt;Bgr, Byte&gt;, consider using the Bitmap property </remarks>
      /// <returns> This image in Bitmap format, the pixel data are copied over to the Bitmap</returns>
      public Bitmap ToBitmap()
      {
#if ANDROID
         return ToBitmap(Android.Graphics.Bitmap.Config.Argb8888);
#else
         Type typeOfColor = typeof(TColor);
         Type typeofDepth = typeof(TDepth);

         PixelFormat format = PixelFormat.Undefined;

         if (typeOfColor == typeof(Gray)) // if this is a gray scale image
         {
            format = PixelFormat.Format8bppIndexed;
         }
         else if (typeOfColor == typeof(Bgra)) //if this is Bgra image
         {
            format = PixelFormat.Format32bppArgb;
         }
         else if (typeOfColor == typeof(Bgr))  //if this is a Bgr Byte image
         {  
            format = PixelFormat.Format24bppRgb;
         }
         else
         {
            using (Image<Bgr, Byte> temp = Convert<Bgr, Byte>())
               return temp.ToBitmap();
         }

         if (typeof(TDepth) == typeof(Byte))
         {
            Size size = Size;
            Bitmap bmp = new Bitmap(size.Width, size.Height, format);
            BitmapData data = bmp.LockBits(
                new Rectangle(Point.Empty, size),
                ImageLockMode.WriteOnly,
                format);
            //using (Matrix<Byte> m = new Matrix<byte>(size.Height, size.Width, data.Scan0, data.Stride))
            using (Image<TColor, Byte> m = new Image<TColor, Byte>(size.Width, size.Height, data.Stride, data.Scan0))
               CvInvoke.cvCopy(Ptr, m.Ptr, IntPtr.Zero);

            bmp.UnlockBits(data);

            if (format == PixelFormat.Format8bppIndexed)
               bmp.Palette = CvToolbox.GrayscalePalette;
            return bmp;
         }
         else
         {
            using (Image<TColor, Byte> temp = Convert<TColor, Byte>())
               return temp.ToBitmap();
         }
#endif
      }

      ///<summary> Create a Bitmap image of certain size</summary>
      ///<param name="width">The width of the bitmap</param>
      ///<param name="height"> The height of the bitmap</param>
      ///<returns> This image in Bitmap format of the specific size</returns>
      public Bitmap ToBitmap(int width, int height)
      {
         using (Image<TColor, TDepth> scaledImage = Resize(width, height, CvEnum.INTER.CV_INTER_LINEAR))
            return scaledImage.ToBitmap();
      }
     // #endregion
#endif

      #region Pyramids
      ///<summary>
      /// Performs downsampling step of Gaussian pyramid decomposition. 
      /// First it convolves <i>this</i> image with the specified filter and then downsamples the image 
      /// by rejecting even rows and columns.
      ///</summary>
      ///<returns> The downsampled image</returns>
      [ExposableMethod(Exposable = true, Category = "Pyramids")]
      public Image<TColor, TDepth> PyrDown()
      {
         Image<TColor, TDepth> res = new Image<TColor, TDepth>(Width >> 1, Height >> 1);
         CvInvoke.cvPyrDown(Ptr, res.Ptr, CvEnum.FILTER_TYPE.CV_GAUSSIAN_5x5);
         return res;
      }

      ///<summary>
      /// Performs up-sampling step of Gaussian pyramid decomposition. 
      /// First it upsamples <i>this</i> image by injecting even zero rows and columns and then convolves 
      /// result with the specified filter multiplied by 4 for interpolation. 
      /// So the resulting image is four times larger than the source image.
      ///</summary>
      ///<returns> The upsampled image</returns>
      [ExposableMethod(Exposable = true, Category = "Pyramids")]
      public Image<TColor, TDepth> PyrUp()
      {
         Image<TColor, TDepth> res = new Image<TColor, TDepth>(Width << 1, Height << 1);
         CvInvoke.cvPyrUp(Ptr, res.Ptr, CvEnum.FILTER_TYPE.CV_GAUSSIAN_5x5);
         return res;
      }

      /// <summary>
      /// Compute the image pyramid
      /// </summary>
      /// <param name="maxLevel">The number of level's for the pyramid; Level 0 referes to the current image, level n is computed by calling the PyrDown() function on level n-1</param>
      /// <returns>The image pyramid</returns>
      public Image<TColor, TDepth>[] BuildPyramid(int maxLevel)
      {
         Debug.Assert(maxLevel >= 0, "The pyramid should have at lease maxLevel of 0");
         Image<TColor, TDepth>[] pyr = new Image<TColor, TDepth>[maxLevel + 1];
         pyr[0] = this;

         for (int i = 1; i <= maxLevel; i++)
            pyr[i] = pyr[i - 1].PyrDown();

         return pyr;
      }
      #endregion

      #region Special Image Transforms
      ///<summary> Use inpaint to recover the intensity of the pixels which location defined by <paramref>mask</paramref> on <i>this</i> image </summary>
      ///<param name="mask">The inpainting mask. Non-zero pixels indicate the area that needs to be inpainted</param>
      ///<param name="radius">The radius of circular neighborhood of each point inpainted that is considered by the algorithm</param>
      ///<returns> The inpainted image </returns>
      public Image<TColor, TDepth> InPaint(Image<Gray, Byte> mask, double radius)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvInpaint(Ptr, mask.Ptr, res.Ptr, radius, CvEnum.INPAINT_TYPE.CV_INPAINT_TELEA);
         return res;
      }
      #endregion

      #region Morphological Operations
      /// <summary>
      /// Perform advanced morphological transformations using erosion and dilation as basic operations.
      /// </summary>
      /// <param name="element">Structuring element</param>
      /// <param name="operation">Type of morphological operation</param>
      /// <param name="iterations">Number of times erosion and dilation are applied</param>
      /// <returns>The result of the morphological operation</returns>
      public Image<TColor, TDepth> MorphologyEx(StructuringElementEx element, CvEnum.CV_MORPH_OP operation, int iterations)
      {
         Image<TColor, TDepth> res = CopyBlank();

         //For MOP_GRADIENT, a temperary buffer is required
         Image<TColor, TDepth> buffer =
            (operation == Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_GRADIENT)
            ? new Image<TColor, TDepth>(Size)
            : null;

         CvInvoke.cvMorphologyEx(
            Ptr, res.Ptr,
            buffer == null ? IntPtr.Zero : buffer.Ptr,
            element == null ? IntPtr.Zero : element.Ptr,
            operation,
            iterations);

         //release the temporary buffer if it is created
         if (buffer != null) buffer.Dispose();

         return res;
      }

      /// <summary>
      /// Perform inplace advanced morphological transformations using erosion and dilation as basic operations.
      /// </summary>
      /// <param name="element">Structuring element</param>
      /// <param name="operation">Type of morphological operation</param>
      /// <param name="iterations">Number of times erosion and dilation are applied</param>
      public void _MorphologyEx(StructuringElementEx element, CvEnum.CV_MORPH_OP operation, int iterations)
      {
         Image<TColor, TDepth> temp =
            (operation == Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_GRADIENT
            || operation == Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_TOPHAT
            || operation == Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_BLACKHAT) ?
            CopyBlank()
            : null;

         CvInvoke.cvMorphologyEx(
            Ptr,
            Ptr,
            temp == null ? IntPtr.Zero : temp.Ptr,
            element == null ? IntPtr.Zero : element.Ptr,
            operation,
            iterations);

         if (temp != null) temp.Dispose();
      }

      /// <summary>
      /// Erodes <i>this</i> image using a 3x3 rectangular structuring element.
      /// Erosion are applied several (iterations) times
      /// </summary>
      /// <param name="iterations">The number of erode iterations</param>
      /// <returns> The eroded image</returns>
      public Image<TColor, TDepth> Erode(int iterations)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvErode(Ptr, res.Ptr, IntPtr.Zero, iterations);
         return res;
      }

      /// <summary>
      /// Dilates <i>this</i> image using a 3x3 rectangular structuring element.
      /// Dilation are applied several (iterations) times
      /// </summary>
      /// <param name="iterations">The number of dilate iterations</param>
      /// <returns> The dialated image</returns>
      public Image<TColor, TDepth> Dilate(int iterations)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvDilate(Ptr, res.Ptr, IntPtr.Zero, iterations);
         return res;
      }

      /// <summary>
      /// Erodes <i>this</i> image inplace using a 3x3 rectangular structuring element.
      /// Erosion are applied several (iterations) times
      /// </summary>
      /// <param name="iterations">The number of erode iterations</param>
      [ExposableMethod(Exposable = true, Category = "Morphology")]
      public void _Erode(int iterations)
      {
         CvInvoke.cvErode(Ptr, Ptr, IntPtr.Zero, iterations);
      }

      /// <summary>
      /// Dilates <i>this</i> image inplace using a 3x3 rectangular structuring element.
      /// Dilation are applied several (iterations) times
      /// </summary>
      /// <param name="iterations">The number of dilate iterations</param>
      [ExposableMethod(Exposable = true, Category = "Morphology")]
      public void _Dilate(int iterations)
      {
         CvInvoke.cvDilate(Ptr, Ptr, IntPtr.Zero, iterations);
      }
      #endregion

      #region generic operations
      /// <summary> 
      /// perform an generic action based on each element of the image
      /// </summary>
      /// <param name="action">The action to be applied to each element of the image</param>
      public void Action(Action<TDepth> action)
      {
         int cols1 = Width * new TColor().Dimension;

         int step1;
         IntPtr start;
         Size roiSize;
         CvInvoke.cvGetRawData(Ptr, out start, out step1, out roiSize);
         Int64 data1 = start.ToInt64();
         int width1 = SizeOfElement * cols1;

         using (PinnedArray<TDepth> row1 = new PinnedArray<TDepth>(cols1))
            for (int row = 0; row < Height; row++, data1 += step1)
            {
               Toolbox.memcpy(row1.AddrOfPinnedObject(), new IntPtr(data1), width1);
               foreach (TDepth v in row1.Array)
                  action(v);
            }
      }

      /// <summary>
      /// Perform an generic operation based on the elements of the two images
      /// </summary>
      /// <typeparam name="TOtherDepth">The depth of the second image</typeparam>
      /// <param name="img2">The second image to perform action on</param>
      /// <param name="action">An action such that the first parameter is the a single channel of a pixel from the first image, the second parameter is the corresponding channel of the correspondind pixel from the second image </param>
      public void Action<TOtherDepth>(Image<TColor, TOtherDepth> img2, Action<TDepth, TOtherDepth> action)
                  where TOtherDepth : new()
      {
         Debug.Assert(Size.Equals(img2.Size));

         Int64 data1;
         int height1, cols1, width1, step1;
         RoiParam(Ptr, out data1, out height1, out cols1, out width1, out step1);

         Int64 data2;
         int height2, cols2, width2, step2;
         RoiParam(img2.Ptr, out data2, out height2, out cols2, out width2, out step2);

         TDepth[] row1 = new TDepth[cols1];
         TOtherDepth[] row2 = new TOtherDepth[cols1];
         GCHandle handle1 = GCHandle.Alloc(row1, GCHandleType.Pinned);
         GCHandle handle2 = GCHandle.Alloc(row2, GCHandleType.Pinned);

         for (int row = 0; row < height1; row++, data1 += step1, data2 += step2)
         {
            Toolbox.memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
            Toolbox.memcpy(handle2.AddrOfPinnedObject(), (IntPtr)data2, width2);
            for (int col = 0; col < cols1; action(row1[col], row2[col]), col++) ;
         }
         handle1.Free();
         handle2.Free();
      }

      /// <summary> 
      /// Compute the element of a new image based on the value as well as the x and y positions of each pixel on the image
      /// </summary> 
      public Image<TColor, TOtherDepth> Convert<TOtherDepth>(Func<TDepth, int, int, TOtherDepth> converter)
         where TOtherDepth : new()
      {
         Image<TColor, TOtherDepth> res = new Image<TColor, TOtherDepth>(Width, Height);

         int nchannel = MIplImage.nChannels;

         Int64 data1;
         int height1, cols1, width1, step1;
         RoiParam(Ptr, out data1, out height1, out cols1, out width1, out step1);

         Int64 data2;
         int height2, cols2, width2, step2;
         RoiParam(res.Ptr, out data2, out height2, out cols2, out width2, out step2);

         TDepth[] row1 = new TDepth[cols1];
         TOtherDepth[] row2 = new TOtherDepth[cols1];
         GCHandle handle1 = GCHandle.Alloc(row1, GCHandleType.Pinned);
         GCHandle handle2 = GCHandle.Alloc(row2, GCHandleType.Pinned);

         for (int row = 0; row < height1; row++, data1 += step1, data2 += step2)
         {
            Toolbox.memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
            for (int col = 0; col < cols1; row2[col] = converter(row1[col], row, col / nchannel), col++) ;
            Toolbox.memcpy((IntPtr)data2, handle2.AddrOfPinnedObject(), width2);
         }
         handle1.Free();
         handle2.Free();
         return res;
      }

      ///<summary> Compute the element of the new image based on element of this image</summary> 
      public Image<TColor, TOtherDepth> Convert<TOtherDepth>(Converter<TDepth, TOtherDepth> converter)
         where TOtherDepth : new()
      {
         Image<TColor, TOtherDepth> res = new Image<TColor, TOtherDepth>(Size);

         Int64 data1;
         int height1, cols1, width1, step1;
         RoiParam(Ptr, out data1, out height1, out cols1, out width1, out step1);

         Int64 data2;
         int height2, cols2, width2, step2;
         RoiParam(res.Ptr, out data2, out height2, out cols2, out width2, out step2);

         TDepth[] row1 = new TDepth[cols1];
         TOtherDepth[] row2 = new TOtherDepth[cols1];

         GCHandle handle1 = GCHandle.Alloc(row1, GCHandleType.Pinned);
         GCHandle handle2 = GCHandle.Alloc(row2, GCHandleType.Pinned);
         for (int row = 0; row < height1; row++, data1 += step1, data2 += step2)
         {
            Toolbox.memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
            for (int col = 0; col < cols1; row2[col] = converter(row1[col]), col++) ;
            Toolbox.memcpy((IntPtr)data2, handle2.AddrOfPinnedObject(), width2);
         }
         handle1.Free();
         handle2.Free();
         return res;
      }

      ///<summary> Compute the element of the new image based on the elements of the two image</summary>
      public Image<TColor, TDepth3> Convert<TDepth2, TDepth3>(Image<TColor, TDepth2> img2, Func<TDepth, TDepth2, TDepth3> converter)
         where TDepth2 : new()
         where TDepth3 : new()
      {
         Debug.Assert(Size.Equals(img2.Size), "Image size do not match");

         Image<TColor, TDepth3> res = new Image<TColor, TDepth3>(Width, Height);

         Int64 data1;
         int height1, cols1, width1, step1;
         RoiParam(Ptr, out data1, out height1, out cols1, out width1, out step1);

         Int64 data2;
         int height2, cols2, width2, step2;
         RoiParam(img2.Ptr, out data2, out height2, out cols2, out width2, out step2);

         Int64 data3;
         int height3, cols3, width3, step3;
         RoiParam(res.Ptr, out data3, out height3, out cols3, out width3, out step3);

         TDepth[] row1 = new TDepth[cols1];
         TDepth2[] row2 = new TDepth2[cols1];
         TDepth3[] row3 = new TDepth3[cols1];
         GCHandle handle1 = GCHandle.Alloc(row1, GCHandleType.Pinned);
         GCHandle handle2 = GCHandle.Alloc(row2, GCHandleType.Pinned);
         GCHandle handle3 = GCHandle.Alloc(row3, GCHandleType.Pinned);

         for (int row = 0; row < height1; row++, data1 += step1, data2 += step2, data3 += step3)
         {
            Toolbox.memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
            Toolbox.memcpy(handle2.AddrOfPinnedObject(), (IntPtr)data2, width2);
            for (int col = 0; col < cols1; row3[col] = converter(row1[col], row2[col]), col++) ;
            Toolbox.memcpy((IntPtr)data3, handle3.AddrOfPinnedObject(), width3);
         }

         handle1.Free();
         handle2.Free();
         handle3.Free();

         return res;
      }

      ///<summary> Compute the element of the new image based on the elements of the three image</summary>
      public Image<TColor, TDepth4> Convert<TDepth2, TDepth3, TDepth4>(Image<TColor, TDepth2> img2, Image<TColor, TDepth3> img3, Func<TDepth, TDepth2, TDepth3, TDepth4> converter)
         where TDepth2 : new()
         where TDepth3 : new()
         where TDepth4 : new()
      {
         Debug.Assert(Size.Equals(img2.Size) && Size.Equals(img3.Size), "Image size do not match");

         Image<TColor, TDepth4> res = new Image<TColor, TDepth4>(Width, Height);

         Int64 data1;
         int height1, cols1, width1, step1;
         RoiParam(Ptr, out data1, out height1, out cols1, out width1, out step1);

         Int64 data2;
         int height2, cols2, width2, step2;
         RoiParam(img2.Ptr, out data2, out height2, out cols2, out width2, out step2);

         Int64 data3;
         int height3, cols3, width3, step3;
         RoiParam(img3.Ptr, out data3, out height3, out cols3, out width3, out step3);

         Int64 data4;
         int height4, cols4, width4, step4;
         RoiParam(res.Ptr, out data4, out height4, out cols4, out width4, out step4);

         TDepth[] row1 = new TDepth[cols1];
         TDepth2[] row2 = new TDepth2[cols1];
         TDepth3[] row3 = new TDepth3[cols1];
         TDepth4[] row4 = new TDepth4[cols1];
         GCHandle handle1 = GCHandle.Alloc(row1, GCHandleType.Pinned);
         GCHandle handle2 = GCHandle.Alloc(row2, GCHandleType.Pinned);
         GCHandle handle3 = GCHandle.Alloc(row3, GCHandleType.Pinned);
         GCHandle handle4 = GCHandle.Alloc(row4, GCHandleType.Pinned);

         for (int row = 0; row < height1; row++, data1 += step1, data2 += step2, data3 += step3, data4 += step4)
         {
            Toolbox.memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
            Toolbox.memcpy(handle2.AddrOfPinnedObject(), (IntPtr)data2, width2);
            Toolbox.memcpy(handle3.AddrOfPinnedObject(), (IntPtr)data3, width3);

            for (int col = 0; col < cols1; row4[col] = converter(row1[col], row2[col], row3[col]), col++) ;

            Toolbox.memcpy((IntPtr)data4, handle4.AddrOfPinnedObject(), width4);
         }
         handle1.Free();
         handle2.Free();
         handle3.Free();
         handle4.Free();

         return res;
      }

      ///<summary> Compute the element of the new image based on the elements of the four image</summary>
      public Image<TColor, TDepth5> Convert<TDepth2, TDepth3, TDepth4, TDepth5>(Image<TColor, TDepth2> img2, Image<TColor, TDepth3> img3, Image<TColor, TDepth4> img4, Func<TDepth, TDepth2, TDepth3, TDepth4, TDepth5> converter)
         where TDepth2 : new()
         where TDepth3 : new()
         where TDepth4 : new()
         where TDepth5 : new()
      {
         Debug.Assert(Size.Equals(img2.Size) && Size.Equals(img3.Size) && Size.Equals(img4.Size), "Image size do not match");

         Image<TColor, TDepth5> res = new Image<TColor, TDepth5>(Width, Height);

         Int64 data1;
         int height1, cols1, width1, step1;
         RoiParam(Ptr, out data1, out height1, out cols1, out width1, out step1);

         Int64 data2;
         int height2, cols2, width2, step2;
         RoiParam(img2.Ptr, out data2, out height2, out cols2, out width2, out step2);

         Int64 data3;
         int height3, cols3, width3, step3;
         RoiParam(img3.Ptr, out data3, out height3, out cols3, out width3, out step3);

         Int64 data4;
         int height4, cols4, width4, step4;
         RoiParam(img4.Ptr, out data4, out height4, out cols4, out width4, out step4);

         Int64 data5;
         int height5, cols5, width5, step5;
         RoiParam(res.Ptr, out data5, out height5, out cols5, out width5, out step5);

         TDepth[] row1 = new TDepth[cols1];
         TDepth2[] row2 = new TDepth2[cols1];
         TDepth3[] row3 = new TDepth3[cols1];
         TDepth4[] row4 = new TDepth4[cols1];
         TDepth5[] row5 = new TDepth5[cols1];
         GCHandle handle1 = GCHandle.Alloc(row1, GCHandleType.Pinned);
         GCHandle handle2 = GCHandle.Alloc(row2, GCHandleType.Pinned);
         GCHandle handle3 = GCHandle.Alloc(row3, GCHandleType.Pinned);
         GCHandle handle4 = GCHandle.Alloc(row4, GCHandleType.Pinned);
         GCHandle handle5 = GCHandle.Alloc(row5, GCHandleType.Pinned);

         for (int row = 0; row < height1; row++, data1 += step1, data2 += step2, data3 += step3, data4 += step4, data5 += step5)
         {
            Toolbox.memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
            Toolbox.memcpy(handle2.AddrOfPinnedObject(), (IntPtr)data2, width2);
            Toolbox.memcpy(handle3.AddrOfPinnedObject(), (IntPtr)data3, width3);
            Toolbox.memcpy(handle4.AddrOfPinnedObject(), (IntPtr)data4, width4);

            for (int col = 0; col < cols1; row5[col] = converter(row1[col], row2[col], row3[col], row4[col]), col++) ;
            Toolbox.memcpy((IntPtr)data5, handle5.AddrOfPinnedObject(), width5);
         }
         handle1.Free();
         handle2.Free();
         handle3.Free();
         handle4.Free();
         handle5.Free();

         return res;
      }
      #endregion

      #region Implment UnmanagedObject
      /// <summary>
      /// Release all unmanaged memory associate with the image
      /// </summary>
      protected override void DisposeObject()
      {
         base.DisposeObject();

         if (_ptr != IntPtr.Zero)
         {
            if (_imageDataReleaseMode == ImageDataReleaseMode.ReleaseHeaderOnly)
            {
               CvInvoke.cvReleaseImageHeader(ref _ptr);
               Debug.Assert(_ptr == IntPtr.Zero);
               GC.RemoveMemoryPressure(StructSize.MIplImage);
            }
            else //ImageDataReleaseMode.ReleaseIplImage
            {
               CvInvoke.cvReleaseImage(ref _ptr);
               Debug.Assert(_ptr == IntPtr.Zero);
            }
         }

         _array = null;
      }
      #endregion

      #region Operator overload

      /// <summary>
      /// Perform an elementwise AND operation on the two images
      /// </summary>
      /// <param name="img1">The first image to AND</param>
      /// <param name="img2">The second image to AND</param>
      /// <returns>The result of the AND operation</returns>
      public static Image<TColor, TDepth> operator &(Image<TColor, TDepth> img1, Image<TColor, TDepth> img2)
      {
         return img1.And(img2);
      }

      /// <summary>
      /// Perform an elementwise AND operation using an images and a color
      /// </summary>
      /// <param name="img1">The first image to AND</param>
      /// <param name="val">The color to AND</param>
      /// <returns>The result of the AND operation</returns>
      public static Image<TColor, TDepth> operator &(Image<TColor, TDepth> img1, double val)
      {
         TColor color = new TColor();
         color.MCvScalar = new MCvScalar(val, val, val, val);
         return img1.And(color);
      }

      /// <summary>
      /// Perform an elementwise AND operation using an images and a color
      /// </summary>
      /// <param name="img1">The first image to AND</param>
      /// <param name="val">The color to AND</param>
      /// <returns>The result of the AND operation</returns>
      public static Image<TColor, TDepth> operator &(double val, Image<TColor, TDepth> img1)
      {
         TColor color = new TColor();
         color.MCvScalar = new MCvScalar(val, val, val, val);
         return img1.And(color);
      }

      /// <summary>
      /// Perform an elementwise AND operation using an images and a color
      /// </summary>
      /// <param name="img1">The first image to AND</param>
      /// <param name="val">The color to AND</param>
      /// <returns>The result of the AND operation</returns>
      public static Image<TColor, TDepth> operator &(Image<TColor, TDepth> img1, TColor val)
      {
         return img1.And(val);
      }

      /// <summary>
      /// Perform an elementwise AND operation using an images and a color
      /// </summary>
      /// <param name="img1">The first image to AND</param>
      /// <param name="val">The color to AND</param>
      /// <returns>The result of the AND operation</returns>
      public static Image<TColor, TDepth> operator &(TColor val, Image<TColor, TDepth> img1)
      {
         return img1.And(val);
      }

      /// <summary> Perform an elementwise OR operation with another image and return the result</summary>
      /// <param name="img1">The first image to apply bitwise OR operation</param>
      /// <param name="img2">The second image to apply bitwise OR operation</param>
      /// <returns> The result of the OR operation</returns>
      public static Image<TColor, TDepth> operator |(Image<TColor, TDepth> img1, Image<TColor, TDepth> img2)
      {
         return img1.Or(img2);
      }

      ///<summary> 
      /// Perform an binary OR operation with some color
      /// </summary>
      ///<param name="img1">The image to OR</param>
      ///<param name="val"> The color to OR</param>
      ///<returns> The result of the OR operation</returns>
      public static Image<TColor, TDepth> operator |(Image<TColor, TDepth> img1, double val)
      {
         TColor color = new TColor();
         color.MCvScalar = new MCvScalar(val, val, val, val);
         return img1.Or(color);
      }

      ///<summary> 
      /// Perform an binary OR operation with some color
      /// </summary>
      ///<param name="img1">The image to OR</param>
      ///<param name="val"> The color to OR</param>
      ///<returns> The result of the OR operation</returns>
      public static Image<TColor, TDepth> operator |(double val, Image<TColor, TDepth> img1)
      {
         return img1 | val;
      }

      ///<summary> 
      /// Perform an binary OR operation with some color
      /// </summary>
      ///<param name="img1">The image to OR</param>
      ///<param name="val"> The color to OR</param>
      ///<returns> The result of the OR operation</returns>
      public static Image<TColor, TDepth> operator |(Image<TColor, TDepth> img1, TColor val)
      {
         return img1.Or(val);
      }

      ///<summary> 
      /// Perform an binary OR operation with some color
      /// </summary>
      ///<param name="img1">The image to OR</param>
      ///<param name="val"> The color to OR</param>
      ///<returns> The result of the OR operation</returns>
      public static Image<TColor, TDepth> operator |(TColor val, Image<TColor, TDepth> img1)
      {
         return img1.Or(val);
      }

      ///<summary>Compute the complement image</summary>
      ///<param name="image">The image to be inverted</param>
      ///<returns>The complement image</returns>
      public static Image<TColor, TDepth> operator ~(Image<TColor, TDepth> image)
      {
         return image.Not();
      }

      /// <summary>
      /// Elementwise add <paramref name="img1"/> with <paramref name="img2"/>
      /// </summary>
      /// <param name="img1">The first image to be added</param>
      /// <param name="img2">The second image to be added</param>
      /// <returns>The sum of the two images</returns>
      public static Image<TColor, TDepth> operator +(Image<TColor, TDepth> img1, Image<TColor, TDepth> img2)
      {
         return img1.Add(img2);
      }

      /// <summary>
      /// Elementwise add <paramref name="img1"/> with <paramref name="val"/>
      /// </summary>
      /// <param name="img1">The image to be added</param>
      /// <param name="val">The value to be added</param>
      /// <returns>The images plus the color</returns>
      public static Image<TColor, TDepth> operator +(double val, Image<TColor, TDepth> img1)
      {
         return img1 + val;
      }

      /// <summary>
      /// Elementwise add <paramref name="image"/> with <paramref name="value"/>
      /// </summary>
      /// <param name="image">The image to be added</param>
      /// <param name="value">The value to be added</param>
      /// <returns>The images plus the color</returns>
      public static Image<TColor, TDepth> operator +(Image<TColor, TDepth> image, double value)
      {
         TColor color = new TColor();
         color.MCvScalar = new MCvScalar(value, value, value, value);
         return image.Add(color);
      }

      /// <summary>
      /// Elementwise add <paramref name="image"/> with <paramref name="value"/>
      /// </summary>
      /// <param name="image">The image to be added</param>
      /// <param name="value">The color to be added</param>
      /// <returns>The images plus the color</returns>
      public static Image<TColor, TDepth> operator +(Image<TColor, TDepth> image, TColor value)
      {
         return image.Add(value);
      }

      /// <summary>
      /// Elementwise add <paramref name="image"/> with <paramref name="value"/>
      /// </summary>
      /// <param name="image">The image to be added</param>
      /// <param name="value">The color to be added</param>
      /// <returns>The images plus the color</returns>
      public static Image<TColor, TDepth> operator +(TColor value, Image<TColor, TDepth> image)
      {
         return image.Add(value);
      }

      /// <summary>
      /// Elementwise subtract another image from the current image
      /// </summary>
      /// <param name="image1">The image to be subtracted</param>
      /// <param name="image2">The second image to be subtracted from <paramref name="image1"/></param>
      /// <returns> The result of elementwise subtracting img2 from <paramref name="image1"/> </returns>
      public static Image<TColor, TDepth> operator -(Image<TColor, TDepth> image1, Image<TColor, TDepth> image2)
      {
         return image1.Sub(image2);
      }

      /// <summary>
      /// Elementwise subtract another image from the current image
      /// </summary>
      /// <param name="image">The image to be subtracted</param>
      /// <param name="value">The color to be subtracted</param>
      /// <returns> The result of elementwise subtracting <paramred name="val"/> from <paramref name="image"/> </returns>
      public static Image<TColor, TDepth> operator -(Image<TColor, TDepth> image, TColor value)
      {
         return image.Sub(value);
      }

      /// <summary>
      /// Elementwise subtract another image from the current image
      /// </summary>
      /// <param name="image">The image to be subtracted</param>
      /// <param name="value">The color to be subtracted</param>
      /// <returns> <paramred name="val"/> - <paramref name="image"/> </returns>
      public static Image<TColor, TDepth> operator -(TColor value, Image<TColor, TDepth> image)
      {
         return image.SubR(value);
      }

      /// <summary>
      /// <paramred name="val"/> - <paramref name="image"/>
      /// </summary>
      /// <param name="image">The image to be subtracted</param>
      /// <param name="value">The value to be subtracted</param>
      /// <returns> <paramred name="val"/> - <paramref name="image"/> </returns>
      public static Image<TColor, TDepth> operator -(double value, Image<TColor, TDepth> image)
      {
         TColor color = new TColor();
         color.MCvScalar = new MCvScalar(value, value, value, value);
         return image.SubR(color);
      }

      /// <summary>
      /// Elementwise subtract another image from the current image
      /// </summary>
      /// <param name="image">The image to be subtracted</param>
      /// <param name="value">The value to be subtracted</param>
      /// <returns> <paramref name="image"/> - <paramred name="val"/>   </returns>
      public static Image<TColor, TDepth> operator -(Image<TColor, TDepth> image, double value)
      {
         TColor color = new TColor();
         color.MCvScalar = new MCvScalar(value, value, value, value);
         return image.Sub(color);
      }

      /// <summary>
      ///  <paramref name="image"/> * <paramref name="scale"/>
      /// </summary>
      /// <param name="image">The image</param>
      /// <param name="scale">The multiplication scale</param>
      /// <returns><paramref name="image"/> * <paramref name="scale"/></returns>
      public static Image<TColor, TDepth> operator *(Image<TColor, TDepth> image, double scale)
      {
         return image.Mul(scale);
      }

      /// <summary>
      ///   <paramref name="scale"/>*<paramref name="image"/>
      /// </summary>
      /// <param name="image">The image</param>
      /// <param name="scale">The multiplication scale</param>
      /// <returns><paramref name="scale"/>*<paramref name="image"/></returns>
      public static Image<TColor, TDepth> operator *(double scale, Image<TColor, TDepth> image)
      {
         return image.Mul(scale);
      }

      /// <summary>
      /// Perform the convolution with <paramref name="kernel"/> on <paramref name="image"/>
      /// </summary>
      /// <param name="image">The image</param>
      /// <param name="kernel">The kernel</param>
      /// <returns>Result of the convolution</returns>
      public static Image<TColor, Single> operator *(Image<TColor, TDepth> image, ConvolutionKernelF kernel)
      {
         return image.Convolution(kernel);
      }

      /// <summary>
      ///  <paramref name="image"/> / <paramref name="scale"/>
      /// </summary>
      /// <param name="image">The image</param>
      /// <param name="scale">The division scale</param>
      /// <returns><paramref name="image"/> / <paramref name="scale"/></returns>
      public static Image<TColor, TDepth> operator /(Image<TColor, TDepth> image, double scale)
      {
         return image.Mul(1.0 / scale);
      }

      /// <summary>
      ///   <paramref name="scale"/> / <paramref name="image"/>
      /// </summary>
      /// <param name="image">The image</param>
      /// <param name="scale">The scale</param>
      /// <returns><paramref name="scale"/> / <paramref name="image"/></returns>
      public static Image<TColor, TDepth> operator /(double scale, Image<TColor, TDepth> image)
      {
         Image<TColor, TDepth> res = image.CopyBlank();
         CvInvoke.cvDiv(IntPtr.Zero, image.Ptr, res.Ptr, scale);
         return res;
      }

      #endregion

      #region Filters
      /// <summary>
      /// Summation over a pixel param1 x param2 neighborhood with subsequent scaling by 1/(param1 x param2)
      /// </summary>
      /// <param name="width">The width of the window</param>
      /// <param name="height">The height of the window</param>
      /// <returns>The result of blur</returns>
      public Image<TColor, TDepth> SmoothBlur(int width, int height)
      {
         return SmoothBlur(width, height, true);
      }

      /// <summary>
      /// Summation over a pixel param1 x param2 neighborhood. If scale is true, the result is subsequent scaled by 1/(param1 x param2)
      /// </summary>
      /// <param name="width">The width of the window</param>
      /// <param name="height">The height of the window</param>
      /// <param name="scale">If true, the result is subsequent scaled by 1/(param1 x param2)</param>
      /// <returns>The result of blur</returns>
      [ExposableMethod(Exposable = true, Category = "Smoothing")]
      public Image<TColor, TDepth> SmoothBlur(int width, int height, bool scale)
      {
         Emgu.CV.CvEnum.SMOOTH_TYPE type = scale ? Emgu.CV.CvEnum.SMOOTH_TYPE.CV_BLUR : Emgu.CV.CvEnum.SMOOTH_TYPE.CV_BLUR_NO_SCALE;
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvSmooth(Ptr, res.Ptr, type, width, height, 0.0, 0.0);
         return res;
      }

      /// <summary>
      /// Finding median of <paramref name="size"/>x<paramref name="size"/> neighborhood 
      /// </summary>
      /// <param name="size">The size (width &amp; height) of the window</param>
      /// <returns>The result of mediam smooth</returns>
      [ExposableMethod(Exposable = true, Category = "Smoothing")]
      public Image<TColor, TDepth> SmoothMedian(int size)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvSmooth(Ptr, res.Ptr, Emgu.CV.CvEnum.SMOOTH_TYPE.CV_MEDIAN, size, size, 0, 0);
         return res;
      }

      /// <summary>
      /// Applying bilateral 3x3 filtering
      /// </summary>
      /// <param name="colorSigma">Color sigma</param>
      /// <param name="spaceSigma">Space sigma</param>
      /// <param name="kernelSize">The size of the bilatral kernel</param>
      /// <returns>The result of bilateral smooth</returns>
      [ExposableMethod(Exposable = true, Category = "Smoothing")]
      public Image<TColor, TDepth> SmoothBilatral(int kernelSize, int colorSigma, int spaceSigma)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvSmooth(Ptr, res.Ptr, Emgu.CV.CvEnum.SMOOTH_TYPE.CV_BILATERAL, kernelSize, kernelSize, colorSigma, spaceSigma);
         return res;
      }

      #region Gaussian Smooth
      ///<summary> Perform Gaussian Smoothing in the current image and return the result </summary>
      ///<param name="kernelSize"> The size of the Gaussian kernel (<paramref name="kernelSize"/> x <paramref name="kernelSize"/>)</param>
      ///<returns> The smoothed image</returns>
      public Image<TColor, TDepth> SmoothGaussian(int kernelSize)
      {
         return SmoothGaussian(kernelSize, 0, 0, 0);
      }

      ///<summary> Perform Gaussian Smoothing in the current image and return the result </summary>
      ///<param name="kernelWidth"> The width of the Gaussian kernel</param>
      ///<param name="kernelHeight"> The height of the Gaussian kernel</param>
      ///<param name="sigma1"> The standard deviation of the Gaussian kernel in the horizontal dimwnsion</param>
      ///<param name="sigma2"> The standard deviation of the Gaussian kernel in the vertical dimwnsion</param>
      ///<returns> The smoothed image</returns>
      [ExposableMethod(Exposable = true, Category = "Smoothing")]
      public Image<TColor, TDepth> SmoothGaussian(int kernelWidth, int kernelHeight, double sigma1, double sigma2)
      {
         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvSmooth(Ptr, res.Ptr, CvEnum.SMOOTH_TYPE.CV_GAUSSIAN, kernelWidth, kernelHeight, sigma1, sigma2);
         return res;
      }

      ///<summary> Perform Gaussian Smoothing inplace for the current image </summary>
      ///<param name="kernelSize"> The size of the Gaussian kernel (<paramref name="kernelSize"/> x <paramref name="kernelSize"/>)</param>
      public void _SmoothGaussian(int kernelSize)
      {
         _SmoothGaussian(kernelSize, 0, 0, 0);
      }

      ///<summary> Perform Gaussian Smoothing inplace for the current image </summary>
      ///<param name="kernelWidth"> The width of the Gaussian kernel</param>
      ///<param name="kernelHeight"> The height of the Gaussian kernel</param>
      ///<param name="sigma1"> The standard deviation of the Gaussian kernel in the horizontal dimwnsion</param>
      ///<param name="sigma2"> The standard deviation of the Gaussian kernel in the vertical dimwnsion</param>
      public void _SmoothGaussian(int kernelWidth, int kernelHeight, double sigma1, double sigma2)
      {
         CvInvoke.cvSmooth(Ptr, Ptr, CvEnum.SMOOTH_TYPE.CV_GAUSSIAN, kernelWidth, kernelHeight, sigma1, sigma2);
      }

      ///<summary> 
      ///Performs a convolution using the specific <paramref name="kernel"/> 
      ///</summary>
      ///<param name="kernel">The convolution kernel</param>
      ///<returns>The result of the convolution</returns>
      public Image<TColor, Single> Convolution(ConvolutionKernelF kernel)
      {
         Image<TColor, Single> floatImage =
            (typeof(TDepth) == typeof(Single)) ?
            this as Image<TColor, Single>
            : Convert<TColor, Single>();

         Image<TColor, Single> res = new Image<TColor, Single>(Size);
         ForEachDuplicateChannel(
            delegate(IntPtr srcFloat, IntPtr dest, int channel)
            {
               //perform the convolution operation
               CvInvoke.cvFilter2D(
                   srcFloat,
                   dest,
                   kernel.Ptr,
                   kernel.Center);
            },
            res);

         if (!object.ReferenceEquals(floatImage, this))
            floatImage.Dispose();

         return res;
      }

      /// <summary>
      /// Calculates integral images for the source image
      /// </summary>
      /// <returns>The integral image</returns>
      public Image<TColor, double> Integral()
      {
         Image<TColor, double> sum = new Image<TColor, double>(Width + 1, Height + 1);
         CvInvoke.cvIntegral(Ptr, sum.Ptr, IntPtr.Zero, IntPtr.Zero);
         return sum;
      }

      /// <summary>
      /// Calculates integral images for the source image
      /// </summary>
      /// <param name="sum">The integral image</param>
      /// <param name="squareSum">The integral image for squared pixel values</param>
      /// <returns>The integral image</returns>
      public void Integral(out Image<TColor, double> sum, out Image<TColor, double> squareSum)
      {
         sum = new Image<TColor, double>(Width + 1, Height + 1);
         squareSum = new Image<TColor, double>(Width + 1, Height + 1);
         CvInvoke.cvIntegral(Ptr, sum.Ptr, squareSum.Ptr, IntPtr.Zero);
      }

      /// <summary>
      /// Calculates one or more integral images for the source image
      /// </summary>
      /// <param name="sum">The integral image</param>
      /// <param name="squareSum">The integral image for squared pixel values</param>
      /// <param name="titledSum">The integral for the image rotated by 45 degrees</param>
      public void Integral(out Image<TColor, double> sum, out Image<TColor, double> squareSum, out Image<TColor, double> titledSum)
      {
         sum = new Image<TColor, double>(Width + 1, Height + 1);
         squareSum = new Image<TColor, double>(Width + 1, Height + 1);
         titledSum = new Image<TColor, double>(Width + 1, Height + 1);
         CvInvoke.cvIntegral(Ptr, sum.Ptr, squareSum.Ptr, titledSum.Ptr);
      }
      #endregion

      #region Threshold methods
      /// <summary>
      /// Transforms grayscale image to binary image. 
      /// Threshold calculated individually for each pixel. 
      /// For the method CV_ADAPTIVE_THRESH_MEAN_C it is a mean of <paramref name="blockSize"/> x <paramref name="blockSize"/> pixel
      /// neighborhood, subtracted by param1. 
      /// For the method CV_ADAPTIVE_THRESH_GAUSSIAN_C it is a weighted sum (gaussian) of <paramref name="blockSize"/> x <paramref name="blockSize"/> pixel neighborhood, subtracted by param1.
      /// </summary>
      /// <param name="maxValue">Maximum value to use with CV_THRESH_BINARY and CV_THRESH_BINARY_INV thresholding types</param>
      /// <param name="adaptiveType">Adaptive_method </param>
      /// <param name="thresholdType">Thresholding type. must be one of CV_THRESH_BINARY, CV_THRESH_BINARY_INV  </param>
      /// <param name="blockSize">The size of a pixel neighborhood that is used to calculate a threshold value for the pixel: 3, 5, 7, ... </param>
      /// <param name="param1">Constant subtracted from mean or weighted mean. It may be negative. </param>
      /// <returns>The result of the adaptive threshold</returns>
      [ExposableMethod(Exposable = true, Category = "Threshold")]
      public Image<TColor, TDepth> ThresholdAdaptive(
         TColor maxValue,
         CvEnum.ADAPTIVE_THRESHOLD_TYPE adaptiveType,
         CvEnum.THRESH thresholdType,
         int blockSize,
         TColor param1)
      {
         double[] max = maxValue.MCvScalar.ToArray();
         double[] p1 = param1.MCvScalar.ToArray();
         Image<TColor, TDepth> result = CopyBlank();
         ForEachDuplicateChannel<TDepth>(
            delegate(IntPtr src, IntPtr dst, int channel)
            {
               CvInvoke.cvAdaptiveThreshold(src, dst, max[channel], adaptiveType, thresholdType, blockSize, p1[channel]);
            },
            result);
         return result;
      }

      ///<summary> 
      ///the base threshold method shared by public threshold functions 
      ///</summary>
      private void ThresholdBase(Image<TColor, TDepth> dest, TColor threshold, TColor maxValue, CvEnum.THRESH threshType)
      {
         double[] t = threshold.MCvScalar.ToArray();
         double[] m = maxValue.MCvScalar.ToArray();
         ForEachDuplicateChannel<TDepth>(
            delegate(IntPtr src, IntPtr dst, int channel)
            {
               CvInvoke.cvThreshold(src, dst, t[channel], m[channel], threshType);
            },
            dest);
      }

      /// <summary> Threshold the image such that: dst(x,y) = src(x,y), if src(x,y)>threshold;  0, otherwise </summary>
      /// <param name="threshold">The threshold value</param>
      /// <returns> dst(x,y) = src(x,y), if src(x,y)>threshold;  0, otherwise </returns>
      public Image<TColor, TDepth> ThresholdToZero(TColor threshold)
      {
         Image<TColor, TDepth> res = CopyBlank();
         ThresholdBase(res, threshold, new TColor(), CvEnum.THRESH.CV_THRESH_TOZERO);
         return res;
      }

      /// <summary> 
      /// Threshold the image such that: dst(x,y) = 0, if src(x,y)>threshold;  src(x,y), otherwise 
      /// </summary>
      /// <param name="threshold">The threshold value</param>
      /// <returns>The image such that: dst(x,y) = 0, if src(x,y)>threshold;  src(x,y), otherwise</returns>
      public Image<TColor, TDepth> ThresholdToZeroInv(TColor threshold)
      {
         Image<TColor, TDepth> res = CopyBlank();
         ThresholdBase(res, threshold, new TColor(), CvEnum.THRESH.CV_THRESH_TOZERO_INV);
         return res;
      }

      /// <summary>
      /// Threshold the image such that: dst(x,y) = threshold, if src(x,y)>threshold; src(x,y), otherwise 
      /// </summary>
      /// <param name="threshold">The threshold value</param>
      /// <returns>The image such that: dst(x,y) = threshold, if src(x,y)>threshold; src(x,y), otherwise</returns>
      public Image<TColor, TDepth> ThresholdTrunc(TColor threshold)
      {
         Image<TColor, TDepth> res = CopyBlank();
         ThresholdBase(res, threshold, new TColor(), CvEnum.THRESH.CV_THRESH_TRUNC);
         return res;
      }

      /// <summary> 
      /// Threshold the image such that: dst(x,y) = max_value, if src(x,y)>threshold; 0, otherwise 
      /// </summary>
      /// <returns>The image such that: dst(x,y) = max_value, if src(x,y)>threshold; 0, otherwise </returns>
      public Image<TColor, TDepth> ThresholdBinary(TColor threshold, TColor maxValue)
      {
         Image<TColor, TDepth> res = CopyBlank();
         ThresholdBase(res, threshold, maxValue, CvEnum.THRESH.CV_THRESH_BINARY);
         return res;
      }

      /// <summary> Threshold the image such that: dst(x,y) = 0, if src(x,y)>threshold;  max_value, otherwise </summary>
      /// <param name="threshold">The threshold value</param>
      /// <param name="maxValue">The maximum value of the pixel on the result</param>
      /// <returns>The image such that: dst(x,y) = 0, if src(x,y)>threshold;  max_value, otherwise</returns>
      public Image<TColor, TDepth> ThresholdBinaryInv(TColor threshold, TColor maxValue)
      {
         Image<TColor, TDepth> res = CopyBlank();
         ThresholdBase(res, threshold, maxValue, CvEnum.THRESH.CV_THRESH_BINARY_INV);
         return res;
      }

      /// <summary> Threshold the image inplace such that: dst(x,y) = src(x,y), if src(x,y)>threshold;  0, otherwise </summary>
      /// <param name="threshold">The threshold value</param>
      [ExposableMethod(Exposable = true, Category = "Threshold")]
      public void _ThresholdToZero(TColor threshold)
      {
         ThresholdBase(this, threshold, new TColor(), CvEnum.THRESH.CV_THRESH_TOZERO);
      }

      /// <summary> Threshold the image inplace such that: dst(x,y) = 0, if src(x,y)>threshold;  src(x,y), otherwise </summary>
      /// <param name="threshold">The threshold value</param>
      [ExposableMethod(Exposable = true, Category = "Threshold")]
      public void _ThresholdToZeroInv(TColor threshold)
      {
         ThresholdBase(this, threshold, new TColor(), CvEnum.THRESH.CV_THRESH_TOZERO_INV);
      }

      /// <summary> Threshold the image inplace such that: dst(x,y) = threshold, if src(x,y)>threshold; src(x,y), otherwise </summary>
      /// <param name="threshold">The threshold value</param>
      [ExposableMethod(Exposable = true, Category = "Threshold")]
      public void _ThresholdTrunc(TColor threshold)
      {
         ThresholdBase(this, threshold, new TColor(), CvEnum.THRESH.CV_THRESH_TRUNC);
      }

      /// <summary> Threshold the image inplace such that: dst(x,y) = max_value, if src(x,y)>threshold; 0, otherwise </summary>
      /// <param name="threshold">The threshold value</param>
      /// <param name="maxValue">The maximum value of the pixel on the result</param>
      [ExposableMethod(Exposable = true, Category = "Threshold")]
      public void _ThresholdBinary(TColor threshold, TColor maxValue)
      {
         ThresholdBase(this, threshold, maxValue, CvEnum.THRESH.CV_THRESH_BINARY);
      }

      /// <summary> Threshold the image inplace such that: dst(x,y) = 0, if src(x,y)>threshold;  max_value, otherwise </summary>
      /// <param name="threshold">The threshold value</param>
      /// <param name="maxValue">The maximum value of the pixel on the result</param>
      [ExposableMethod(Exposable = true, Category = "Threshold")]
      public void _ThresholdBinaryInv(TColor threshold, TColor maxValue)
      {
         ThresholdBase(this, threshold, maxValue, CvEnum.THRESH.CV_THRESH_BINARY_INV);
      }
      #endregion
      #endregion

      #region Statistic
      /// <summary>
      /// Calculates the average value and standard deviation of array elements, independently for each channel
      /// </summary>
      /// <param name="average">The avg color</param>
      /// <param name="sdv">The standard deviation for each channel</param>
      /// <param name="mask">The operation mask</param>
      public void AvgSdv(out TColor average, out MCvScalar sdv, Image<Gray, Byte> mask)
      {
         average = new TColor();
         MCvScalar avgScalar = new MCvScalar();
         sdv = new MCvScalar();

         CvInvoke.cvAvgSdv(Ptr, ref avgScalar, ref sdv, mask == null ? IntPtr.Zero : mask.Ptr);
         average.MCvScalar = avgScalar;
      }

      /// <summary>
      /// Calculates the average value and standard deviation of array elements, independently for each channel
      /// </summary>
      /// <param name="avg">The avg color</param>
      /// <param name="sdv">The standard deviation for each channel</param>
      public void AvgSdv(out TColor avg, out MCvScalar sdv)
      {
         AvgSdv(out avg, out sdv, null);
      }

      /// <summary>
      /// Count the non Zero elements for each channel
      /// </summary>
      /// <returns>Count the non Zero elements for each channel</returns>
      public int[] CountNonzero()
      {
         return
             ForEachChannel<int>(delegate(IntPtr channel, int channelNumber)
             {
                return CvInvoke.cvCountNonZero(channel);
             });
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
         //int[] minIdx = new int[2], maxIdx = new int[2];
         if (NumberOfChannels == 1)
         {
            CvInvoke.cvMinMaxLoc(Ptr, ref minVal, ref maxVal, ref minLoc, ref maxLoc, IntPtr.Zero);
            //CvInvoke.CvMinMaxIdx(Ptr, out minVal, out maxVal, minIdx, maxIdx, IntPtr.Zero);
            minValues[0] = minVal; maxValues[0] = maxVal;
            //minLoc.X = minIdx[1]; minLoc.Y = minIdx[0];
            //maxLoc.X = maxIdx[1]; maxLoc.Y = maxIdx[0];
            minLocations[0] = minLoc; maxLocations[0] = maxLoc;
            
         }
         else
         {
            for (int i = 0; i < NumberOfChannels; i++)
            {
               CvInvoke.cvSetImageCOI(Ptr, i + 1);
               CvInvoke.cvMinMaxLoc(Ptr, ref minVal, ref maxVal, ref minLoc, ref maxLoc, IntPtr.Zero);
               //CvInvoke.CvMinMaxIdx(Ptr, out minVal, out maxVal, minIdx, maxIdx, IntPtr.Zero);
               minValues[i] = minVal; maxValues[i] = maxVal;
               //minLoc.X = minIdx[1]; minLoc.Y = minIdx[0];
               //maxLoc.X = maxIdx[1]; maxLoc.Y = maxIdx[0];
               minLocations[i] = minLoc; maxLocations[i] = maxLoc;
            }
            CvInvoke.cvSetImageCOI(Ptr, 0);
         }
      }
      #endregion

      #region Image Flipping

      ///<summary> Return a flipped copy of the current image</summary>
      ///<param name="flipType">The type of the flipping</param>
      ///<returns> The flipped copy of <i>this</i> image </returns>
      public Image<TColor, TDepth> Flip(CvEnum.FLIP flipType)
      {
         if (flipType == Emgu.CV.CvEnum.FLIP.NONE) return Copy();

         Image<TColor, TDepth> res = CopyBlank();
         CvInvoke.cvFlip(Ptr, res.Ptr, flipType);
         return res;
      }

      ///<summary> Inplace flip the image</summary>
      ///<param name="flipType">The type of the flipping</param>
      ///<returns> The flipped copy of <i>this</i> image </returns>
      [ExposableMethod(Exposable = true, Category = "Transform")]
      public void _Flip(CvEnum.FLIP flipType)
      {
         if (flipType != Emgu.CV.CvEnum.FLIP.NONE)
         {
            CvInvoke.cvFlip(
               Ptr,
               IntPtr.Zero,
               flipType);
         }
      }
      #endregion

      #region various

      /// <summary>
      /// Concate the current image with another image vertically.
      /// </summary>
      /// <param name="otherImage">The other image to concate</param>
      /// <returns>A new image that is the vertical concatening of this image and <paramref name="otherImage"/></returns>
      public Image<TColor, TDepth> ConcateVertical(Image<TColor, TDepth> otherImage)
      {
         Image<TColor, TDepth> res = new Image<TColor, TDepth>(Math.Max(Width, otherImage.Width), Height + otherImage.Height);
         res.ROI = ROI;
         CvInvoke.cvCopy(Ptr, res.Ptr, IntPtr.Zero);
         Rectangle rect = otherImage.ROI;
         rect.Y += Height;
         res.ROI = rect;
         CvInvoke.cvCopy(otherImage.Ptr, res.Ptr, IntPtr.Zero);
         res.ROI = Rectangle.Empty;
         return res;
      }

      /// <summary>
      /// Concate the current image with another image horizontally. 
      /// </summary>
      /// <param name="otherImage">The other image to concate</param>
      /// <returns>A new image that is the horizontal concatening of this image and <paramref name="otherImage"/></returns>
      public Image<TColor, TDepth> ConcateHorizontal(Image<TColor, TDepth> otherImage)
      {
         Image<TColor, TDepth> res = new Image<TColor, TDepth>(Width + otherImage.Width, Math.Max(Height, otherImage.Height));
         res.ROI = ROI;
         CvInvoke.cvCopy(Ptr, res.Ptr, IntPtr.Zero);
         Rectangle rect = otherImage.ROI;
         rect.X += Width;
         res.ROI = rect;
         CvInvoke.cvCopy(otherImage.Ptr, res.Ptr, IntPtr.Zero);
         res.ROI = Rectangle.Empty;
         return res;
      }

      /// <summary>
      /// Calculates spatial and central moments up to the third order and writes them to moments. The moments may be used then to calculate gravity center of the shape, its area, main axises and various shape characteristics including 7 Hu invariants.
      /// </summary>
      /// <param name="binary">If the flag is true, all the zero pixel values are treated as zeroes, all the others are treated as 1's</param>
      /// <returns>spatial and central moments up to the third order</returns>
      public MCvMoments GetMoments(bool binary)
      {
         MCvMoments m = new MCvMoments();
         CvInvoke.cvMoments(Ptr, ref m, binary ? 1 : 0);
         return m;
      }

      /// <summary>
      /// Gamma corrects this image inplace. The image must have a depth type of Byte.
      /// </summary>
      /// <param name="gamma">The gamma value</param>
      [ExposableMethod(Exposable = true)]
      public void _GammaCorrect(double gamma)
      {
         Image<TColor, Byte> img = this as Image<TColor, Byte>;
         if (img == null)
            throw new NotImplementedException("Gamma correction only implemented for Image of Byte as Depth");

         Byte[,] gammaLUT = new Byte[256, 1];
         for (int i = 0; i < 256; i++)
            gammaLUT[i, 0] = (Byte)(Math.Pow(i / 255.0, gamma) * 255.0);

         using (Matrix<Byte> lut = new Matrix<byte>(gammaLUT))
         {
            Matrix<Byte> lookupTable;
            if (lut.NumberOfChannels == 1)
               lookupTable = lut;
            else
            {
               lookupTable = new Matrix<byte>(lut.Rows, lut.Cols, NumberOfChannels);

               CvInvoke.cvMerge(
                  lut.Ptr,
                  NumberOfChannels > 1 ? lut.Ptr : IntPtr.Zero,
                  NumberOfChannels > 2 ? lut.Ptr : IntPtr.Zero,
                  NumberOfChannels > 3 ? lut.Ptr : IntPtr.Zero,
                  lookupTable.Ptr);
            }

            CvInvoke.cvLUT(Ptr, Ptr, lookupTable.Ptr);

            if (!object.ReferenceEquals(lut, lookupTable))
               lookupTable.Dispose();
         }
      }

      ///<summary> 
      ///Split current Image into an array of gray scale images where each element 
      ///in the array represent a single color channel of the original image
      ///</summary>
      ///<returns> 
      ///An array of gray scale images where each element  
      ///in the array represent a single color channel of the original image 
      ///</returns>
      public Image<Gray, TDepth>[] Split()
      {
         //If single channel, return a copy
         if (NumberOfChannels == 1) return new Image<Gray, TDepth>[] { Copy() as Image<Gray, TDepth> };

         //handle multiple channels
         Image<Gray, TDepth>[] res = new Image<Gray, TDepth>[NumberOfChannels];
         IntPtr[] a = new IntPtr[4];
         Size size = Size;
         for (int i = 0; i < NumberOfChannels; i++)
         {
            res[i] = new Image<Gray, TDepth>(size);
            a[i] = res[i].Ptr;
         }

         CvInvoke.cvSplit(Ptr, a[0], a[1], a[2], a[3]);

         return res;
      }

      /// <summary>
      /// Save this image to the specific file. 
      /// </summary>
      /// <param name="fileName">The name of the file to be saved to</param>
      /// <remarks>The image format is chosen depending on the filename extension, see cvLoadImage. Only 8-bit single-channel or 3-channel (with 'BGR' channel order) images can be saved using this function. If the format, depth or channel order is different, use cvCvtScale and cvCvtColor to convert it before saving, or use universal cvSave to save the image to XML or YAML format.</remarks>
      public override void Save(String fileName)
      {
         try
         {
            base.Save(fileName); //save the image using OpenCV
         }
         catch (Exception e)
         {
#if IOS || NETFX_CORE
            throw e;
#elif ANDROID
            FileInfo fileInfo = new FileInfo(fileName);
            using (Bitmap bmp = Bitmap)
            using (FileStream fs = fileInfo.Open(FileMode.Append, FileAccess.Write))
            {
               String extension = fileInfo.Extension.ToLower();
               Debug.Assert(extension.Substring(0, 1).Equals("."));
               switch (extension)
               {
                  case ".jpg":
                  case ".jpeg":
                     bmp.Compress(Bitmap.CompressFormat.Jpeg, 90, fs);
                     break;
                  case ".png":
                     bmp.Compress(Bitmap.CompressFormat.Png, 90, fs);
                     break;
                  default:
                     throw new NotImplementedException(String.Format("Saving to {0} format is not supported", extension));
               }
            }
#else 
            //Saving with OpenCV fails
            //Try to save the image using .NET's Bitmap class
            using (Bitmap bmp = Bitmap)
            {
               String extension = Path.GetExtension(fileName).ToLower();
               switch (extension)
               {
                  case ".jpg":
                  case ".jpeg":
                     bmp.Save(fileName, ImageFormat.Jpeg);
                     break;
                  case ".bmp":
                     bmp.Save(fileName, ImageFormat.Bmp);
                     break;
                  case ".png":
                     bmp.Save(fileName, ImageFormat.Png);
                     break;
                  case ".tiff":
                  case ".tif":
                     bmp.Save(fileName, ImageFormat.Tiff);
                     break;
                  case ".gif":
                     bmp.Save(fileName, ImageFormat.Gif);
                     break;
                  default:
                     throw new NotImplementedException(String.Format("Saving to {0} format is not supported", extension));
               }
            }
#endif
         }
      }

      /// <summary>
      /// The algorithm inplace normalizes brightness and increases contrast of the image.
      /// For color images, a HSV representation of the image is first obtained and the V (value) channel is histogram normalized
      /// </summary>
      [ExposableMethod(Exposable = true)]
      public void _EqualizeHist()
      {
         if (NumberOfChannels == 1) //Gray scale image
         {
            CvInvoke.cvEqualizeHist(Ptr, Ptr);
         }
         else //Color image
         {
            //Get an hsv representation of this image
            Image<Hsv, TDepth> hsv = this as Image<Hsv, TDepth> ?? Convert<Hsv, TDepth>();

            //equalize the V (value) channel
            using (Image<Gray, TDepth> v = new Image<Gray, TDepth>(Size))
            {
               CvInvoke.cvSetImageCOI(hsv.Ptr, 3);
               CvInvoke.cvCopy(hsv.Ptr, v.Ptr, IntPtr.Zero);
               v._EqualizeHist();
               CvInvoke.cvCopy(v.Ptr, hsv.Ptr, IntPtr.Zero);
               CvInvoke.cvSetImageCOI(hsv.Ptr, 0);
            }

            if (!Object.ReferenceEquals(this, hsv))
            {
               ConvertFrom(hsv);
               hsv.Dispose();
            }
         }
      }
      #endregion

      #region IImage
      IImage[] IImage.Split()
      {
         return
#if NETFX_CORE
            Extensions.
#else
            Array.
#endif
            ConvertAll<Image<Gray, TDepth>, IImage>(
               Split(),
               delegate(Image<Gray, TDepth> img) { return (IImage)img; });
      }
      #endregion

      #region ICloneable Members

      object ICloneable.Clone()
      {
         return Clone();
      }

      #endregion

      /// <summary>
      /// This function load the image data from the iplImage pointer
      /// </summary>
      /// <param name="iplImage">The pointer to the iplImage</param>
      private void LoadImageFromIplImagePtr(IntPtr iplImage)
      {
         MIplImage mptr = (MIplImage)Marshal.PtrToStructure(iplImage, typeof(MIplImage));
         Size size = new Size(mptr.width, mptr.height);

         //Allocate data in mamanged memory
         AllocateData(size.Height, size.Width, NumberOfChannels);

         if (mptr.nChannels == 1)
         {  //Grayscale image;
            switch (mptr.depth)
            {
               case CvEnum.IPL_DEPTH.IPL_DEPTH_8U:
                  using (Image<Gray, Byte> tmp = new Image<Gray, byte>(mptr.width, mptr.height, mptr.widthStep, mptr.imageData))
                     ConvertFrom(tmp);
                  break;
               case CvEnum.IPL_DEPTH.IPL_DEPTH_16U:
                  using (Image<Gray, UInt16> tmp = new Image<Gray, ushort>(mptr.width, mptr.height, mptr.widthStep, mptr.imageData))
                     ConvertFrom(tmp);
                  break;
               case CvEnum.IPL_DEPTH.IPL_DEPTH_32F:
                  using (Image<Gray, float> tmp = new Image<Gray, float>(mptr.width, mptr.height, mptr.widthStep, mptr.imageData))
                     ConvertFrom(tmp);
                  break;
               case CvEnum.IPL_DEPTH.IPL_DEPTH_64F:
                  using (Image<Gray, double> tmp = new Image<Gray, double>(mptr.width, mptr.height, mptr.widthStep, mptr.imageData))
                     ConvertFrom(tmp);
                  break;
               default:
                  throw new NotImplementedException(String.Format("Loading of {0}, {1} channel image is not implemented.", mptr.depth, mptr.nChannels));
            }
         }
         else if (mptr.nChannels == 3)
         {  //BGR image
            switch (mptr.depth)
            {
               case CvEnum.IPL_DEPTH.IPL_DEPTH_8U:
                  using (Image<Bgr, Byte> tmp = new Image<Bgr, byte>(mptr.width, mptr.height, mptr.widthStep, mptr.imageData))
                     ConvertFrom(tmp);
                  break;
               case CvEnum.IPL_DEPTH.IPL_DEPTH_16U:
                  using (Image<Bgr, UInt16> tmp = new Image<Bgr, ushort>(mptr.width, mptr.height, mptr.widthStep, mptr.imageData))
                     ConvertFrom(tmp);
                  break;
               case CvEnum.IPL_DEPTH.IPL_DEPTH_32F:
                  using (Image<Bgr, float> tmp = new Image<Bgr, float>(mptr.width, mptr.height, mptr.widthStep, mptr.imageData))
                     ConvertFrom(tmp);
                  break;
               case CvEnum.IPL_DEPTH.IPL_DEPTH_64F:
                  using (Image<Bgr, double> tmp = new Image<Bgr, double>(mptr.width, mptr.height, mptr.widthStep, mptr.imageData))
                     ConvertFrom(tmp);
                  break;
               default:
                  throw new NotImplementedException(String.Format("Loading of {0}, {1} channel image is not implemented.", mptr.depth, mptr.nChannels));
            }
         }
         else
         {
            throw new NotImplementedException(String.Format("Loading of {0}, {1} channel image is not implemented.", mptr.depth, mptr.nChannels));
         }
      }

      /// <summary>
      /// Get the managed image from an unmanaged IplImagePointer
      /// </summary>
      /// <param name="iplImage">The pointer to the iplImage</param>
      /// <returns>The managed image from the iplImage pointer</returns>
      public static Image<TColor, TDepth> FromIplImagePtr(IntPtr iplImage)
      {
         Image<TColor, TDepth> result = new Image<TColor, TDepth>();
         result.LoadImageFromIplImagePtr(iplImage);
         return result;
      }

      /// <summary>
      /// Decode the image from opencv supported image data. 
      /// </summary>
      /// <param name="rawData">The byte array that repesent the image in opencv supported format. e.g. this can be a stream of the jpeg file</param>
      /// <returns>The image from decoding the rawData</returns>
      public static Image<TColor, TDepth> FromRawImageData(byte[] rawData)
      {
         if (typeof(TColor) == typeof(Bgr) && typeof(TDepth) == typeof(byte))
         {
            Image<TColor, TDepth> result = new Image<TColor, TDepth>();
            result._ptr = CvInvoke.cvDecodeImage(rawData, CvEnum.LOAD_IMAGE_TYPE.CV_LOAD_IMAGE_COLOR);
            if (result._ptr == IntPtr.Zero)
               throw new Exception("Unable to decode image data.");
            result._imageDataReleaseMode = ImageDataReleaseMode.ReleaseIplImage;
            return result;
         }
         else
         {
            using (Image<Bgr, Byte> tmp = Image<Bgr, Byte>.FromRawImageData(rawData))
            {
               return tmp.Convert<TColor, TDepth>();
            }
         }
      }

      /// <summary>
      /// Get the jpeg representation of the image
      /// </summary>
      /// <returns>An byte array that contains the image as jpeg data</returns>
      public byte[] ToJpegData()
      {
         IntPtr mat = CvInvoke.cvEncodeImage(".jpg", Ptr, IntPtr.Zero);
         MCvMat cvMat = (MCvMat) Marshal.PtrToStructure(mat, typeof(MCvMat));
         byte[] data = new byte[cvMat.rows * cvMat.cols];
         Marshal.Copy(cvMat.data, data, 0, data.Length);
         CvInvoke.cvReleaseMat(ref mat);
         return data;
      }

      ///<summary> 
      /// Get the size of the array
      ///</summary>
      public override System.Drawing.Size Size
      {
         get
         {
            //TODO: this override should not be necessary if cvGetSize is working correctly, need to check when this will be fixed.
            MIplImage iplImage = MIplImage;
            if (iplImage.roi != IntPtr.Zero)
            {
               return ROI.Size;
            }
            else
            {
               return new Size(iplImage.width, iplImage.height);
            }
         }
      }
   }

   /// <summary>
   /// Constants used by the image class
   /// </summary>
   internal static class ImageConstants
   {
      /// <summary>
      /// Offset of roi
      /// </summary>
      public static readonly int RoiOffset = (int)Marshal.OffsetOf(typeof(MIplImage), "roi");
   }

   internal enum ImageDataReleaseMode
   {
      ReleaseHeaderOnly,
      ReleaseIplImage
   }
}


