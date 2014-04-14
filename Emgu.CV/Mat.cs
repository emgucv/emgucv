//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.IO;

#if ANDROID
using Bitmap = Android.Graphics.Bitmap;
#elif IOS
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
#elif NETFX_CORE
#else
using System.Drawing.Imaging;
#endif

namespace Emgu.CV
{
   /// <summary>
   /// The equavailent of cv::Mat, should only be used if you know what you are doing.
   /// In most case you should use the Matrix class instead
   /// </summary>
   public partial class Mat : MatDataAllocator, IInputArray, IOutputArray, IInputOutputArray, IImage
   {
      internal IntPtr _inputArrayPtr;
      internal IntPtr _outputArrayPtr;
      internal IntPtr _inputOutputArrayPtr;

      internal bool _needDispose;

      internal Mat(IntPtr ptr, bool needDispose, bool useCustomMemAllocator = false)
      {
         _ptr = ptr;
         _needDispose = needDispose;
         /*
         if (useCustomMemAllocator)
         {
            InitActionPtr();
            _memoryAllocator = MatInvoke.cvMatUseCustomAllocator(_ptr, AllocateCallback, DeallocateCallback, _allocateDataActionPtr, _freeDataActionPtr);
         }*/
      }

      /// <summary>
      /// Create an empty cv::Mat
      /// </summary>
      public Mat()
         : this(MatInvoke.cvMatCreate(), true, true)
      {  
      }

      /// <summary>
      /// Create a mat of the specific type.
      /// </summary>
      /// <param name="rows">Number of rows in a 2D array.</param>
      /// <param name="cols">Number of columns in a 2D array.</param>
      /// <param name="type">Mat element type</param>
      /// <param name="channels">Number of channels</param>
      public Mat(int rows, int cols, CvEnum.DepthType type, int channels)
         : this()
      {
         Create(rows, cols, type, channels);
      }

      /// <summary>
      /// Create a Mat header from existing data
      /// </summary>
      /// <param name="rows">Number of rows in a 2D array.</param>
      /// <param name="cols">Number of columns in a 2D array.</param>
      /// <param name="type">Mat element type</param>
      /// <param name="channels">Number of channels</param>
      /// <param name="data">Pointer to the user data. Matrix constructors that take data and step parameters do not allocate matrix data. Instead, they just initialize the matrix header that points to the specified data, which means that no data is copied. This operation is very efficient and can be used to process external data using OpenCV functions. The external data is not automatically deallocated, so you should take care of it.</param>
      /// <param name="step">Number of bytes each matrix row occupies. The value should include the padding bytes at the end of each row, if any.</param>
      public Mat(int rows, int cols, CvEnum.DepthType type, int channels, IntPtr data, int step)
         : this(MatInvoke.cvMatCreateWithData(rows, cols, CvInvoke.MakeType(type, channels), data, new IntPtr(step)), true, true)
      {
      }

      /// <summary>
      /// Load the Mat from file
      /// </summary>
      /// <param name="fileName">The name of the file</param>
      /// <param name="loadType">File loading method</param>
      public Mat(String fileName, CvEnum.LoadImageType loadType)
         : this(MatInvoke.cvMatCreate(), true, false)
      {
         CvInvoke.cveImread(fileName, loadType, this);
      }

      public Mat(Mat mat, Rectangle roi)
         :this(MatInvoke.cvMatCreateFromRect(mat.Ptr, ref roi), true, true)
      {  
      }

      /// <summary>
      /// Convert this Mat to UMat
      /// </summary>
      /// <param name="access">Access type</param>
      /// <returns>The UMat</returns>
      public UMat ToUMat(CvEnum.AccessType access)
      {
         return new UMat(MatInvoke.cvMatGetUMat(Ptr, access), true);
      }

      /// <summary>
      /// Allocates new array data if needed.
      /// </summary>
      /// <param name="rows">New number of rows.</param>
      /// <param name="cols">New number of columns.</param>
      /// <param name="type">New matrix element depth type.</param>
      /// <param name="channels">New matrix number of channels</param>
      public void Create(int rows, int cols, CvEnum.DepthType type, int channels)
      {
         MatInvoke.cvMatCreateData(_ptr, rows, cols, CvInvoke.MakeType(type, channels));
      }

      /// <summary>
      /// The size of this matrix
      /// </summary>
      public Size Size
      {
         get
         {
            return MatInvoke.cvMatGetSize(_ptr);
         }
      }

      /// <summary>
      /// The number of rows
      /// </summary>
      public int Rows
      {
         get
         {
            return Size.Height;
         }
      }

      /// <summary>
      /// The number of columns
      /// </summary>
      public int Cols
      {
         get
         {
            return Size.Width;
         }
      }

      /// <summary>
      /// Pointer to the begining of the raw data
      /// </summary>
      public IntPtr DataPointer
      {
         get
         {
            return MatInvoke.cvMatGetDataPointer(_ptr);
         }
      }

      /// <summary>
      /// Step
      /// </summary>
      public int Step
      {
         get
         {
            return (int)MatInvoke.cvMatGetStep(_ptr);
         }
      }

      /// <summary>
      /// Number of channels
      /// </summary>
      public int NumberOfChannels
      {
         get
         {
            return (int)MatInvoke.cvMatGetChannels(_ptr);
         }
      }

      /// <summary>
      /// Depth type
      /// </summary>
      public CvEnum.DepthType Depth
      {
         get
         {
            return (CvEnum.DepthType)MatInvoke.cvMatGetDepth(_ptr);
         }
      }

      /// <summary>
      /// The size of the elements in this matrix
      /// </summary>
      public int ElementSize
      {
         get
         {
            return MatInvoke.cvMatGetElementSize(_ptr);
         }
      }

      /// <summary>
      /// Copy the data in this cv::Mat to a CvArray
      /// </summary>
      /// <param name="m">The input array to copy to</param>
      /// <param name="mask">Operation mask. Its non-zero elements indicate which matrix elements need to be copied.</param>
      public void CopyTo(IOutputArray m, IInputArray mask = null)
      {
         MatInvoke.cvMatCopyTo(Ptr, m.OutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr);
      }

      /// <summary>
      /// Converts an array to another data type with optional scaling.
      /// </summary>
      /// <param name="m">Output matrix; if it does not have a proper size or type before the operation, it is reallocated.</param>
      /// <param name="rtype">Desired output matrix type or, rather, the depth since the number of channels are the same as the input has; if rtype is negative, the output matrix will have the same type as the input.</param>
      /// <param name="alpha">Optional scale factor.</param>
      /// <param name="beta">Optional delta added to the scaled values.</param>
      public void ConvertTo(IOutputArray m, CvEnum.DepthType rtype, double alpha = 1.0, double beta = 0.0)
      {
         MatInvoke.cvMatConvertTo(Ptr, m.OutputArrayPtr, rtype, alpha, beta);
      }

      /// <summary>
      /// Indicates if this cv::Mat is empty
      /// </summary>
      public bool IsEmpty
      {
         get
         {
            return MatInvoke.cvMatIsEmpty(_ptr);
         }
      }

      /// <summary>
      /// Changes the shape and/or the number of channels of a 2D matrix without copying the data.
      /// </summary>
      /// <param name="cn">New number of channels. If the parameter is 0, the number of channels remains the same.</param>
      /// <param name="rows">New number of rows. If the parameter is 0, the number of rows remains the same.</param>
      /// <returns>A new mat header that has different shape</returns>
      public Mat Reshape(int cn, int rows = 0)
      {
         return new Mat(MatInvoke.cvMatReshape(Ptr, cn, rows), true, false);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this object.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose && _ptr != IntPtr.Zero)
            MatInvoke.cvMatRelease(ref _ptr);

         if (_inputArrayPtr != IntPtr.Zero)
            CvInvoke.cveInputArrayRelease(ref _inputArrayPtr);

         if (_outputArrayPtr != IntPtr.Zero)
            CvInvoke.cveOutputArrayRelease(ref _outputArrayPtr);

         if (_inputOutputArrayPtr != IntPtr.Zero)
            CvInvoke.cveInputOutputArrayRelease(ref _inputOutputArrayPtr);

         base.DisposeObject();

      }

      /// <summary>
      /// Pointer to the InputArray
      /// </summary>
      public IntPtr InputArrayPtr
      {
         get 
         { 
            if (_inputArrayPtr == IntPtr.Zero)
               _inputArrayPtr = MatInvoke.cveInputArrayFromMat(_ptr);
            return _inputArrayPtr; 
         }
      }

      /// <summary>
      /// Pointer to the OutputArray
      /// </summary>
      public IntPtr OutputArrayPtr
      {
         get
         {
            if (_outputArrayPtr == IntPtr.Zero)
               _outputArrayPtr = MatInvoke.cveOutputArrayFromMat(_ptr);
            return _outputArrayPtr;
         }
      }

      /// <summary>
      /// Pointer to the InputOutputArray
      /// </summary>
      public IntPtr InputOutputArrayPtr
      {
         get 
         {
            if (_inputOutputArrayPtr == IntPtr.Zero)
               _inputOutputArrayPtr = MatInvoke.cveInputOutputArrayFromMat(_ptr);
            return _inputOutputArrayPtr;
         }
      }

      public int Width
      {
         get
         {
            return Size.Width;
         }
      }

      public int Height
      {
         get
         {
            return Size.Height;
         }
      }

      /// <summary>
      /// Get the minimum and maximum value across all channels of the mat
      /// </summary>
      /// <returns>The range that contains the minimum and maximum values</returns>
      public RangeF GetValueRange()
      {
         double minVal = 0, maxVal =0;
         Point minLoc = new Point(), maxLoc = new Point();
         if (NumberOfChannels == 1)
         {
            CvInvoke.MinMaxLoc(this, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
         }
         else
         {
            using (Mat tmp = Reshape(1))
            {
               CvInvoke.MinMaxLoc(this, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
            }
         }
         return new RangeF((float)minVal, (float) maxVal);
      }

      /// <summary>
      /// Convert this Mat to Image
      /// </summary>
      /// <typeparam name="TColor">The type of Color</typeparam>
      /// <typeparam name="TDepth">The type of Depth</typeparam>
      /// <returns>The image</returns>
      public Image<TColor, TDepth> ToImage<TColor, TDepth>()
         where TColor : struct, IColor
         where TDepth : new()
      {
         TColor c = new TColor();

         int numberOfChannels = NumberOfChannels;
         if (typeof(TDepth) == CvInvoke.GetDepthType(this.Depth) && c.Dimension == numberOfChannels)
         {
            //same color, same depth
            return new Image<TColor, TDepth>(MatInvoke.cveMatToIplImage(Ptr));
         }
         else if (typeof(TDepth) != CvInvoke.GetDepthType(this.Depth) && c.Dimension == numberOfChannels)
         {
            //different depth, same color
            Image<TColor, TDepth> result = new Image<TColor, TDepth>(Size);
            if (numberOfChannels == 1)
            {
               using (Image<Gray, TDepth> tmp = this.ToImage<Gray, TDepth>())
                  result.ConvertFrom(tmp);
            }
            else if (numberOfChannels == 3)
            {
               using (Image<Bgr, TDepth> tmp = this.ToImage<Bgr, TDepth>())
                  result.ConvertFrom(tmp);
            }
            else if (numberOfChannels == 4)
            {
               using (Image<Bgra, TDepth> tmp = this.ToImage<Bgra, TDepth>())
                  result.ConvertFrom(tmp);
            }
            else
            {
               throw new Exception("Unsupported conversion");
            }
            return result;
         }
         else if (typeof(TDepth) == CvInvoke.GetDepthType(this.Depth) && c.Dimension != numberOfChannels)
         {
            //same depth, different color
            Image<TColor, TDepth> result = new Image<TColor, TDepth>(Size);

            CvEnum.DepthType depth = Depth;
            if (depth == CvEnum.DepthType.Cv8U)
            {
               using (Image<TColor, Byte> tmp = this.ToImage<TColor, Byte>())
                  result.ConvertFrom(tmp);
            }
            else if (depth == CvEnum.DepthType.Cv8S)
            {
               using (Image<TColor, SByte> tmp = this.ToImage<TColor, SByte>())
                  result.ConvertFrom(tmp);
            }
            else if (depth == CvEnum.DepthType.Cv16U)
            {
               using (Image<TColor, UInt16> tmp = this.ToImage<TColor, UInt16>())
                  result.ConvertFrom(tmp);
            }
            else if (depth == CvEnum.DepthType.Cv16S)
            {
               using (Image<TColor, Int16> tmp = this.ToImage<TColor, Int16>())
                  result.ConvertFrom(tmp);
            }
            else if (depth == CvEnum.DepthType.Cv32S)
            {
               using (Image<TColor, Int32> tmp = this.ToImage<TColor, Int32>())
                  result.ConvertFrom(tmp);
            }
            else if (depth == CvEnum.DepthType.Cv32F)
            {
               using (Image<TColor, float> tmp = this.ToImage<TColor, float>())
                  result.ConvertFrom(tmp);
            }
            else if (depth == CvEnum.DepthType.Cv64F)
            {
               using (Image<TColor, double> tmp = this.ToImage<TColor, double>())
                  result.ConvertFrom(tmp);
            }
            else
            {
               throw new Exception("Unsupported conversion");
            }
            return result;
         }
         else
         {
            //different color, different depth
            using (Mat tmp = new Mat())
            {
               ConvertTo(tmp, CvInvoke.GetDepthType(typeof(TDepth)));
               return tmp.ToImage<TColor, TDepth>();
            }
            
         }
      }
#if ANDROID
      public Bitmap Bitmap
      {
         get { return ToBitmap(Android.Graphics.Bitmap.Config.Argb8888); }
      }

      public Bitmap ToBitmap(Bitmap.Config config)
      {
         System.Drawing.Size size = Size;

         if (config == Bitmap.Config.Argb8888)
         {
            Bitmap result = Bitmap.CreateBitmap(size.Width, size.Height, Bitmap.Config.Argb8888);

            using (BitmapArgb8888Image bi = new BitmapArgb8888Image(result))
            using (Image<Rgba, Byte> tmp = ToImage<Rgba, Byte>())
            {
               tmp.Copy(bi, null);
            }
            return result;
         }
         else if (config == Bitmap.Config.Rgb565)
         {
            Bitmap result = Bitmap.CreateBitmap(size.Width, size.Height, Bitmap.Config.Rgb565);

            using (BitmapRgb565Image bi = new BitmapRgb565Image(result))
            using (Image<Bgr, Byte> tmp = ToImage<Bgr, Byte>())
               bi.ConvertFrom(tmp);
            return result;
         }
         else
         {
            throw new NotImplementedException("Only Bitmap config of Argb888 or Rgb565 is supported.");
         }
      }
#elif IOS
      public UIImage ToUIImage()
      {
         //if 4 channels, assume the color space is RGBA
         if (NumberOfChannels == 4 && this.Depth == Emgu.CV.CvEnum.DepthType.Cv8U)
         {
            Size s = this.Size;
            using (CGColorSpace cspace = CGColorSpace.CreateDeviceRGB())
            using (CGBitmapContext context = new CGBitmapContext(
               DataPointer,
               s.Width, s.Height,
               8,
               s.Width * 4,
               cspace,
               CGImageAlphaInfo.PremultipliedLast))
            using (CGImage cgImage =  context.ToImage())
            {
               return UIImage.FromImage(cgImage);
            }
         } else
         {
            using (Image<Rgba, Byte> tmp = ToImage<Rgba, Byte>())
            {
               return tmp.ToUIImage();
            }
         }
      }
#else
      /// <summary>
      /// The Get property provide a more efficient way to convert Image&lt;Gray, Byte&gt;, Image&lt;Bgr, Byte&gt; and Image&lt;Bgra, Byte&gt; into Bitmap
      /// such that the image data is <b>shared</b> with Bitmap. 
      /// If you change the pixel value on the Bitmap, you change the pixel values on the Image object as well!
      /// For other types of image this property has the same effect as ToBitmap()
      /// <b>Take extra caution not to use the Bitmap after the Mat object is disposed</b>
      /// The Set property convert the bitmap to this Image type.
      /// </summary>
      public Bitmap Bitmap
      {
         get 
         {
            int channels = NumberOfChannels;
            Type colorType;
            switch(channels)
            {
               case 1:
                  colorType = typeof(Gray);
                  break;
               case 3:
                  colorType = typeof(Bgr) ;
                  break;
               case 4:
                  colorType =typeof(Bgra);
                  break;
               default:
                  throw new Exception("Unknown color type");
            }
            return CvInvoke.RawDataToBitmap(DataPointer, this.Step, this.Size, colorType , NumberOfChannels, CvInvoke.GetDepthType(Depth), true);
         }
      }
#endif

      /// <summary>
      /// Set the mat to the specific value
      /// </summary>
      /// <param name="value">The value to set to</param>
      /// <param name="mask">Optional mask</param>
      public void SetTo(MCvScalar value, IInputArray mask = null)
      {
         using (ScalarArray ia = new ScalarArray(value))
         {
            SetTo(ia, mask);
         }
      }

      /// <summary>
      /// Set the mat to the specific value
      /// </summary>
      /// <param name="value">The value to set to</param>
      /// <param name="mask">Optional mask</param>
      public void SetTo(IInputArray value, IInputArray mask = null)
      {
         MatInvoke.cvMatSetTo(Ptr, value.InputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr);
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
         CvInvoke.MinMax(this, out minValues, out maxValues, out minLocations, out maxLocations);
      }

      /// <summary>
      /// Create a Mat object with data pointed towards the specific row of the original matrix
      /// </summary>
      /// <param name="i">The row number</param>
      /// <returns>A Mat object with data pointed towards the specific row of the original matrix</returns>
      public Mat GetRow(int i)
      {
         return new Mat(this, new Rectangle(new Point(i, 0), new Size(this.Size.Width, 1)));
      }

      /// <summary>
      /// Save this image to the specific file. 
      /// </summary>
      /// <param name="fileName">The name of the file to be saved to</param>
      /// <remarks>The image format is chosen depending on the filename extension, see cvLoadImage. Only 8-bit single-channel or 3-channel (with 'BGR' channel order) images can be saved using this function. If the format, depth or channel order is different, use cvCvtScale and cvCvtColor to convert it before saving, or use universal cvSave to save the image to XML or YAML format.</remarks>
      public void Save(string fileName)
      {
         //bool opencvSaveSuccess = true;
         Exception e = null;
         try
         {
            //save the image using OpenCV
            if (!CvInvoke.Imwrite(fileName, this))
            {
               e = new Exception("Unable to save image");
            }
         }
         catch (Exception excpt)
         {
            e = excpt;
         }

         if (e != null)
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
      /// Make a clone of the current Mat
      /// </summary>
      /// <returns>A clone fo the current Mat</returns>
      public Mat Clone()
      {
         Mat c = new Mat();
         CopyTo(c);
         return c;
      }

      ///<summary> 
      ///Split current Image into an array of gray scale images where each element 
      ///in the array represent a single color channel of the original image
      ///</summary>
      ///<returns> 
      ///An array of gray scale images where each element  
      ///in the array represent a single color channel of the original image 
      ///</returns>
      public Mat[] Split()
      {
         Mat[] mats = new Mat[NumberOfChannels];
         for (int i = 0; i < mats.Length; i++)
         {
            mats[i] = new Mat(Rows, Cols, Depth, 1);
         }
         using (VectorOfMat vm = new VectorOfMat(mats))
         {
            CvInvoke.Split(this, vm);
         }
         return mats;
      }

      IImage[] IImage.Split()
      {
         Mat[] tmp = this.Split();
         IImage[] result = new IImage[tmp.Length];
         for (int i = 0; i < result.Length; i++)
            result[i] = tmp[i];
         return result;
      }

      object ICloneable.Clone()
      {
         return this.Clone();
      }
   }

   internal static class MatInvoke
   {
      static MatInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveInputArrayFromMat(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveOutputArrayFromMat(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveInputOutputArrayFromMat(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatCreate();
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvMatRelease(ref IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static Size cvMatGetSize(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvMatCopyTo(IntPtr mat, IntPtr m, IntPtr mask);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvMatSetTo(IntPtr mat, IntPtr value, IntPtr mask);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cvMatGetElementSize(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cvMatGetChannels(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cvMatGetDepth(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatGetDataPointer(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatGetStep(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool cvMatIsEmpty(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvMatCreateData(IntPtr mat, int row, int cols, int type);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatCreateWithData(int rows, int cols, int type, IntPtr data, IntPtr step);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatCreateFromRect(IntPtr mat, ref Rectangle roi);
      /*
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatCreateFromFile(
         [MarshalAs(CvInvoke.StringMarshalType)]
         String fileName,
         CvEnum.LoadImageType flag
         );
      */
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatGetUMat(IntPtr mat, CvEnum.AccessType access);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatUseCustomAllocator(IntPtr mat, MatDataAllocatorInvoke.MatAllocateCallback allocator, MatDataAllocatorInvoke.MatDeallocateCallback deallocator, IntPtr allocateDataActionPtr, IntPtr freeDataActionPtr);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvMatConvertTo( IntPtr mat, IntPtr outArray, CvEnum.DepthType rtype, double alpha, double beta );

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatReshape(IntPtr mat, int cn, int rows);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveMatToIplImage(IntPtr mat);
   }
}

