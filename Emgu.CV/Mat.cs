//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.IO;

#if ANDROID
using Bitmap = Android.Graphics.Bitmap;
#elif IOS
using UIKit;
using CoreGraphics;
#elif NETFX_CORE || ( UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO)
#else
using System.Drawing.Imaging;
#endif

namespace Emgu.CV
{
   /// <summary>
   /// The equivalent of cv::Mat
   /// </summary>
#if !NETFX_CORE
   [Serializable]
#endif
   public partial class Mat : MatDataAllocator, IImage, IEquatable<Mat>
#if !NETFX_CORE
, ISerializable
#endif
   {

#if !NETFX_CORE
      #region Implement ISerializable interface
      /// <summary>
      /// Constructor used to deserialize runtime serialized object
      /// </summary>
      /// <param name="info">The serialization info</param>
      /// <param name="context">The streaming context</param>
      public Mat(SerializationInfo info, StreamingContext context)
         : this()
      {
         DeserializeObjectData(info, context);
      }

      /// <summary>
      /// A function used for runtime deserailization of the object
      /// </summary>
      /// <param name="info">Serialization info</param>
      /// <param name="context">Streaming context</param>
      protected virtual void DeserializeObjectData(SerializationInfo info, StreamingContext context)
      {
         int rows = (int)info.GetValue("Rows", typeof(int));
         int cols = (int)info.GetValue("Cols", typeof(int));
         int depthType = (int)info.GetValue("DepthType", typeof(int));
         int numberOfChannels = (int)info.GetValue("NumberOfChannels", typeof(int));
         Create(rows, cols, (DepthType)depthType, numberOfChannels);
         Bytes = (Byte[])info.GetValue("Bytes", typeof(Byte[]));
      }

      /// <summary>
      /// A function used for runtime serialization of the object
      /// </summary>
      /// <param name="info">Serialization info</param>
      /// <param name="context">streaming context</param>
      public void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.AddValue("Rows", Rows);
         info.AddValue("Cols", Cols);
         info.AddValue("DepthType", (int)Depth);
         info.AddValue("NumberOfChannels", NumberOfChannels);
         info.AddValue("Bytes", Bytes);
      }

      #endregion
#endif

      /// <summary>
      /// Gets or sets the data as byte array.
      /// </summary>
      /// <value>
      /// The bytes.
      /// </value>
      private byte[] Bytes
      {
         get
         {
            if (IsEmpty)
               return null;
            byte[] data = new byte[Rows * Cols * ElementSize];
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            using (Mat m = new Mat(Rows, Cols, Depth, NumberOfChannels, handle.AddrOfPinnedObject(), Cols * ElementSize))
            {
               CopyTo(m);
            }
            handle.Free();
            return data;
         }
         set
         {
            Debug.Assert(value.Length == Rows * Cols * ElementSize, String.Format("Invalid byte length, expecting {0} but was {1}", Rows * Cols * ElementSize, value.Length));
            GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            using (Mat m = new Mat(Rows, Cols, Depth, NumberOfChannels, handle.AddrOfPinnedObject(), Cols * ElementSize))
            {
               m.CopyTo(this);
            }
            handle.Free();
         }
      }

      /*
      public void SetData(Array data)
      {
         
      }*/

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
         : this(MatInvoke.cveMatCreate(), true, true)
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
      /// Create a mat of the specific type.
      /// </summary>
      /// <param name="size">Size of the Mat</param>
      /// <param name="type">Mat element type</param>
      /// <param name="channels">Number of channels</param>
      public Mat(Size size, CvEnum.DepthType type, int channels)
         : this(size.Height, size.Width, type, channels)
      {
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
         : this(MatInvoke.cveMatCreateWithData(rows, cols, CvInvoke.MakeType(type, channels), data, new IntPtr(step)), true, true)
      {
      }

      /// <summary>
      /// Create a Mat header from existing data
      /// </summary>
      /// <param name="size">Size of the Mat</param>
      /// <param name="type">Mat element type</param>
      /// <param name="channels">Number of channels</param>
      /// <param name="data">Pointer to the user data. Matrix constructors that take data and step parameters do not allocate matrix data. Instead, they just initialize the matrix header that points to the specified data, which means that no data is copied. This operation is very efficient and can be used to process external data using OpenCV functions. The external data is not automatically deallocated, so you should take care of it.</param>
      /// <param name="step">Number of bytes each matrix row occupies. The value should include the padding bytes at the end of each row, if any.</param>
      public Mat(Size size, CvEnum.DepthType type, int channels, IntPtr data, int step)
         : this(size.Height, size.Width, type, channels, data, step)
      {
      }

      /// <summary>
      /// Load the Mat from file
      /// </summary>
      /// <param name="fileName">The name of the file</param>
      /// <param name="loadType">File loading method</param>
      public Mat(String fileName, CvEnum.LoadImageType loadType)
         : this(MatInvoke.cveMatCreate(), true, false)
      {
         using (CvString s = new CvString(fileName))
            CvInvoke.cveImread(s, loadType, this);
      }

      /// <summary>
      /// Create a mat header for the specific ROI
      /// </summary>
      /// <param name="mat">The mat where the new Mat header will share data from</param>
      /// <param name="roi">The region of interest</param>
      public Mat(Mat mat, Rectangle roi)
         : this(MatInvoke.cveMatCreateFromRect(mat.Ptr, ref roi), true, true)
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
         MatInvoke.cveMatCreateData(_ptr, rows, cols, CvInvoke.MakeType(type, channels));
      }

      /// <summary>
      /// The size of this matrix
      /// </summary>
      public Size Size
      {
         get
         {
            return MatInvoke.cveMatGetSize(_ptr);
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
      /// Pointer to the beginning of the raw data
      /// </summary>
      public IntPtr DataPointer
      {
         get
         {
            return MatInvoke.cveMatGetDataPointer(_ptr);
         }
      }

      /// <summary>
      /// Gets the binary data from the specific indices.
      /// </summary>
      /// <param name="indices">The indices.</param>
      /// <returns></returns>
      /// <exception cref="System.NotImplementedException">Indices of length more than 2 is not implemented</exception>
      public byte[] GetData(params int[] indices)
      {
         switch (indices.Length)
         {
            case 0:
               return Bytes;
            case 1:
               if (IsEmpty)
                  return null;
               int row = indices[0];

               byte[] data = new byte[Cols * ElementSize];
               GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
               using (Mat matRow = GetRow(row))
               using (Mat m = new Mat(1, Cols, Depth, NumberOfChannels, handle.AddrOfPinnedObject(), Cols * ElementSize))
               {
                  matRow.CopyTo(m);
               }
               handle.Free();
               return data;
            case 2:
               if (IsEmpty)
                  return null;
               int rowIdx = indices[0];
               int colIdx = indices[1];

               byte[] rawData = new byte[ElementSize];
               Marshal.Copy(new IntPtr(DataPointer.ToInt64() + rowIdx * Step + colIdx * rawData.Length), rawData, 0, rawData.Length);
               return rawData;

            default:
               throw new NotImplementedException(String.Format("GetData with indices size {0} is not implemented", indices.Length));
         }

      }

      /// <summary>
      /// Step
      /// </summary>
      public int Step
      {
         get
         {
            return (int)MatInvoke.cveMatGetStep(_ptr);
         }
      }


      /// <summary>
      /// The size of the elements in this matrix
      /// </summary>
      public int ElementSize
      {
         get
         {
            return MatInvoke.cveMatGetElementSize(_ptr);
         }
      }

      /// <summary>
      /// Copy the data in this cv::Mat to a CvArray
      /// </summary>
      /// <param name="m">The input array to copy to</param>
      /// <param name="mask">Operation mask. Its non-zero elements indicate which matrix elements need to be copied.</param>
      public void CopyTo(IOutputArray m, IInputArray mask = null)
      {
         using (OutputArray oaM = m.GetOutputArray())
         using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            MatInvoke.cveMatCopyTo(Ptr, oaM, iaMask);
      }

      internal class MatWithHandle : Mat
      {
         private GCHandle _handle;

         public MatWithHandle(GCHandle handle, Size s, DepthType dt, int channels, int step)
            : base(s, dt, channels, handle.AddrOfPinnedObject(), step)
         {
            _handle = handle;
         }

         protected override void ReleaseManagedResources()
         {
            base.ReleaseManagedResources();
            _handle.Free();
         }
      }

      internal static MatWithHandle PrepareArrayForCopy(DepthType dt, Size s, int channels, Array data)
      {
         int dimension = data.Rank;
         int length0 = data.GetLength(0);
         if (dimension == 1)
         {
            if (data is byte[])
            {
               int elementSize =
               (dt == DepthType.Cv64F)
                  ? 8
                  : (dt == DepthType.Cv32S || dt == DepthType.Cv32F)
                     ? 4
                     : (dt == DepthType.Cv16S || dt == DepthType.Cv16U)
                        ? 2
                        : 1;
               if (length0 != s.Width * s.Height * channels * elementSize)
                  throw new Exception(String.Format(
                     "The total length of data ({0} bytes) does not match that of the Mat ({1} bytes)",
                     length0, s.Width * s.Height * channels * elementSize));
            }
            else if (length0 != s.Width * s.Height * channels)
               throw new Exception(String.Format(
                  "The total length of data ({0}) does not match that of the Mat ({1})",
                  length0, s.Width * s.Height * channels));
         }
         else //dimension > 1
         {
            if (length0 != s.Height)
               throw new Exception(String.Format(
                  "The number of rows in data ({0}) does not match that of the Mat ({1})",
                  length0, s.Height));

            if (data.GetLength(1) != s.Width)
               throw new Exception(String.Format(
                  "The number of cols in data ({0}) does not match that of the Mat ({1})",
                  data.GetLength(1), s.Width));

            if (dimension == 2 && (channels != 1))
            {
               throw new Exception(String.Format(
                  "A 3 dimension data array is required for Mat that contains {0} channels", channels));
            }

            if (dimension == 3)
            {
               if (data.GetLength(2) != channels)
                  throw new Exception(
                     String.Format(
                        "The size of the 3rd dimension in data ({0}) does not match that of the number of channels of the Mat ({1})",
                        data.GetLength(2), channels));
            }
            else if (dimension > 3)
            {
               throw new Exception(String.Format("The dimension of the data ({0}) is too high.", dimension));
            }
         }

         int step;
         if ((data is double[] || data is double[,] || data is double[, ,]) && dt == DepthType.Cv64F)
         {
            step = sizeof(double) * s.Width * channels;
         }
         else if ((data is byte[] || data is byte[,] || data is byte[, ,]) && dt == DepthType.Cv8U)
         {
            step = sizeof(byte) * s.Width * channels;
         }
         else if ((data is float[] || data is float[,] || data is float[, ,]) && dt == DepthType.Cv32F)
         {
            step = sizeof(float) * s.Width * channels;
         }
         else if ((data is int[] || data is int[,] || data is int[, ,]) && dt == DepthType.Cv32S)
         {
            step = sizeof(int) * s.Width * channels;
         }
         else if ((data is UInt16[] || data is UInt16[,] || data is UInt16[, ,]) && dt == DepthType.Cv16U)
         {
            step = sizeof(UInt16) * s.Width * channels;
         }
         else if ((data is Int16[] || data is Int16[,] || data is Int16[, ,]) && dt == DepthType.Cv16S)
         {
            step = sizeof(Int16) * s.Width * channels;
         }
         else
         {
            throw new Exception(String.Format("The type of data doesn't match the type of the Mat ({1}).", dt));
         }

         GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
         return new MatWithHandle(handle, s, dt, channels, step);
      }

      /// <summary>
      /// Copies the values of the Mat to <paramref name="data"/>.
      /// </summary>
      /// <param name="data">The data storage, must match the size of the Mat</param>
      public void CopyTo(Array data)
      {
         if (IsEmpty)
         {
            throw new Exception("The Mat is empty");
         }

         using (MatWithHandle m = PrepareArrayForCopy(Depth, Size, NumberOfChannels, data))
            CopyTo(m);
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
         using (OutputArray oaM = m.GetOutputArray())
            MatInvoke.cveMatConvertTo(Ptr, oaM, rtype, alpha, beta);
      }

      /// <summary>
      /// Changes the shape and/or the number of channels of a 2D matrix without copying the data.
      /// </summary>
      /// <param name="cn">New number of channels. If the parameter is 0, the number of channels remains the same.</param>
      /// <param name="rows">New number of rows. If the parameter is 0, the number of rows remains the same.</param>
      /// <returns>A new mat header that has different shape</returns>
      public Mat Reshape(int cn, int rows = 0)
      {
         return new Mat(MatInvoke.cveMatReshape(Ptr, cn, rows), true, false);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this object.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose && _ptr != IntPtr.Zero)
            MatInvoke.cveMatRelease(ref _ptr);

         base.DisposeObject();

      }

      /// <summary>
      /// Pointer to the InputArray
      /// </summary>
      public InputArray GetInputArray()
      {
         return new InputArray(MatInvoke.cveInputArrayFromMat(_ptr));
      }

      /// <summary>
      /// Pointer to the OutputArray
      /// </summary>
      public OutputArray GetOutputArray()
      {
         return new OutputArray(MatInvoke.cveOutputArrayFromMat(_ptr));
      }

      /// <summary>
      /// Pointer to the InputOutputArray
      /// </summary>
      public InputOutputArray GetInputOutputArray()
      {
         return new InputOutputArray(MatInvoke.cveInputOutputArrayFromMat(_ptr));
      }


      /// <summary>
      /// Get the width of the mat
      /// </summary>
      public int Width
      {
         get
         {
            return Size.Width;
         }
      }

      /// <summary>
      /// Get the height of the mat.
      /// </summary>
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
         double minVal = 0, maxVal = 0;
         Point minLoc = new Point(), maxLoc = new Point();
         if (NumberOfChannels == 1)
         {
            CvInvoke.MinMaxLoc(this, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
         }
         else
         {
            using (Mat tmp = Reshape(1))
            {
               CvInvoke.MinMaxLoc(tmp, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
            }
         }
         return new RangeF((float)minVal, (float)maxVal);
      }

      /// <summary>
      /// Convert this Mat to Image
      /// </summary>
      /// <typeparam name="TColor">The type of Color</typeparam>
      /// <typeparam name="TDepth">The type of Depth</typeparam>
      /// <returns>The image</returns>
      public Image<TColor, TDepth> ToImage<TColor, TDepth>(bool tryShareData = false)
         where TColor : struct, IColor
         where TDepth : new()
      {
         TColor c = new TColor();

         int numberOfChannels = NumberOfChannels;
         if (typeof(TDepth) == CvInvoke.GetDepthType(this.Depth) && c.Dimension == numberOfChannels)
         {
            //same color, same depth
            if (tryShareData)
               return new Image<TColor, TDepth>(MatInvoke.cveMatToIplImage(Ptr));
            else
            {
               Image<TColor, TDepth> img = new Image<TColor, TDepth>(Size);
               CopyTo(img);
               return img;
            }
         }
         else if (typeof(TDepth) != CvInvoke.GetDepthType(this.Depth) && c.Dimension == numberOfChannels)
         {
            //different depth, same color
            Image<TColor, TDepth> result = new Image<TColor, TDepth>(Size);
            ConvertTo(result, CvInvoke.GetDepthType(typeof(TDepth)));
            /*
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
            }*/
            return result;
         }
         else if (typeof(TDepth) == CvInvoke.GetDepthType(this.Depth) && c.Dimension != numberOfChannels)
         {
            //same depth, different color
            Image<TColor, TDepth> result = new Image<TColor, TDepth>(Size);
            CvInvoke.CvtColor(
               this, result,
               numberOfChannels == 1 ? typeof(Gray) :
               numberOfChannels == 3 ? typeof(Bgr) :
               typeof(Bgra),
               typeof(TColor));
            /*
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
            }*/
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
#elif !( NETFX_CORE || UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO )
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
            Size s = this.Size;
            Type colorType;
            switch (channels)
            {
               case 1:
                  colorType = typeof(Gray);
                  
                  if (s.Equals(Size.Empty))
                     return null;
                  if ((s.Width | 3) != 0) //handle the special case where width is not a multiple of 4
                  {
                     Bitmap bmp = new Bitmap(s.Width, s.Height, PixelFormat.Format8bppIndexed);
                     bmp.Palette = CvToolbox.GrayscalePalette;
                     BitmapData bitmapData = bmp.LockBits(new Rectangle(Point.Empty, s), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                     using (Mat m = new Mat(s.Height, s.Width, DepthType.Cv8U, 1, bitmapData.Scan0, bitmapData.Stride))
                     {
                        CopyTo(m);
                     }
                     bmp.UnlockBits(bitmapData);
                     return bmp;
                  }
                  break;
               case 3:
                  colorType = typeof(Bgr);
                  break;
               case 4:
                  colorType = typeof(Bgra);
                  break;
               default:
                  throw new Exception("Unknown color type");
            }
            return CvInvoke.RawDataToBitmap(DataPointer, this.Step, s, colorType, NumberOfChannels, CvInvoke.GetDepthType(Depth), true);
         }
      }
#endif

      internal static DepthType GetDepthTypeFromArray(Array data)
      {
         int dimension = data.Rank;

            if (dimension == 1)
            {
               return
                  data is byte[]
                     ? DepthType.Cv8U
                     : data is UInt16[]
                        ? DepthType.Cv16U
                        : data is Int16[]
                           ? DepthType.Cv16S
                           : data is float[]
                              ? DepthType.Cv32F
                              : data is int[]
                                 ? DepthType.Cv32S
                                 : data is double[]
                                    ? DepthType.Cv64F
                                    : DepthType.Default;                      
            } else if (dimension == 2)
            {
               return
                  data is byte[,]
                     ? DepthType.Cv8U
                     : data is UInt16[,]
                        ? DepthType.Cv16U
                        : data is Int16[,]
                           ? DepthType.Cv16S
                           : data is float[,]
                              ? DepthType.Cv32F
                              : data is int[,]
                                 ? DepthType.Cv32S
                                 : data is double[,]
                                    ? DepthType.Cv64F
                                    : DepthType.Default;               
            } else if (dimension == 3)
            {
               return
                  data is byte[,,]
                     ? DepthType.Cv8U
                     : data is UInt16[,,]
                        ? DepthType.Cv16U
                        : data is Int16[,,]
                           ? DepthType.Cv16S
                           : data is float[,,]
                              ? DepthType.Cv32F
                              : data is int[,,]
                                 ? DepthType.Cv32S
                                 : data is double[,,]
                                    ? DepthType.Cv64F
                                    : DepthType.Default;               
            }
            else
            {
               return DepthType.Default;
            }
         
      }

      /// <summary>
      /// Copies the values of the <paramref name="data"/> to Mat.
      /// </summary>
      /// <param name="data">The data storage, must match the size of the Mat</param>
      public void SetTo(Array data)
      {
         if (IsEmpty)
         {
            int dimension = data.Rank;

            DepthType dt = GetDepthTypeFromArray(data);
            if (dt == DepthType.Default)
               throw new Exception("The specific data type is not supported.");

            if (dimension == 1)
            {
               this.Create(data.GetLength(0), 1, dt, 1);
            } else if (dimension == 2)
            {
               this.Create(data.GetLength(0), data.GetLength(1), dt, 1);
            } else if (dimension == 3)
            {
               this.Create(data.GetLength(0), data.GetLength(1), dt, 1);
            }
            else
            {
               throw new Exception("The Mat has to be pre-allocated");
            } 
         }

         using (MatWithHandle m = PrepareArrayForCopy(Depth, Size, NumberOfChannels, data))
            m.CopyTo(this);
      }

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
         using (InputArray iaValue = value.GetInputArray())
         using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            MatInvoke.cvMatSetTo(Ptr, iaValue, iaMask);
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
#if IOS || NETFX_CORE || ( UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO )
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

      /// <summary>
      /// Compares two Mats and check if they are equal
      /// </summary>
      /// <param name="other">The other mat to compare with</param>
      /// <returns>True if the two Mats are equal</returns>
      public bool Equals(Mat other)
      {
         if (!(Size.Equals(other.Size) && NumberOfChannels == other.NumberOfChannels && Depth == other.Depth))
            return false;

         using (Mat cmpResult = new Mat())
         {
            CvInvoke.Compare(this, other, cmpResult, CmpType.NotEqual);
            using (Mat reshaped = cmpResult.Reshape(1))
               return CvInvoke.CountNonZero(reshaped) == 0;
         }
      }

      /// <summary>
      /// Computes a dot-product of two vectors.
      /// </summary>
      /// <param name="m">Another dot-product operand</param>
      /// <returns>The dot-product of two vectors.</returns>
      public double Dot(IInputArray m)
      {
         using (InputArray iaM = m.GetInputArray())
            return MatInvoke.cveMatDot(Ptr, iaM);
      }

      /// <summary>
      /// Computes a cross-product of two 3-element vectors.
      /// </summary>
      /// <param name="m">Another cross-product operand.</param>
      /// <returns>Cross-product of two 3-element vectors.</returns>
      public Mat Cross(IInputArray m)
      {
         Mat result = new Mat();
         using (InputArray iaM = m.GetInputArray())
            MatInvoke.cveMatCross(Ptr, iaM, result);
         return result;
      }
   }

   internal static class MatInvoke
   {
      static MatInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveInputArrayFromMat(IntPtr mat);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveOutputArrayFromMat(IntPtr mat);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveInputOutputArrayFromMat(IntPtr mat);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveMatCreate();
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveMatRelease(ref IntPtr mat);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static Size cveMatGetSize(IntPtr mat);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveMatCopyTo(IntPtr mat, IntPtr m, IntPtr mask);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cveMatGetElementSize(IntPtr mat);


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveMatGetDataPointer(IntPtr mat);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveMatGetStep(IntPtr mat);

      /*
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool cvMatIsEmpty(IntPtr mat);*/

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveMatCreateData(IntPtr mat, int row, int cols, int type);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveMatCreateWithData(int rows, int cols, int type, IntPtr data, IntPtr step);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveMatCreateFromRect(IntPtr mat, ref Rectangle roi);
      /*
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatCreateFromFile(
         [MarshalAs(CvInvoke.StringMarshalType)]
         String fileName,
         CvEnum.LoadImageType flag
         );
      */
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatGetUMat(IntPtr mat, CvEnum.AccessType access);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvMatSetTo(IntPtr mat, IntPtr value, IntPtr mask);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveMatUseCustomAllocator(IntPtr mat, MatDataAllocatorInvoke.MatAllocateCallback allocator, MatDataAllocatorInvoke.MatDeallocateCallback deallocator, IntPtr allocateDataActionPtr, IntPtr freeDataActionPtr);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveMatConvertTo(IntPtr mat, IntPtr outArray, CvEnum.DepthType rtype, double alpha, double beta);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveMatReshape(IntPtr mat, int cn, int rows);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveMatToIplImage(IntPtr mat);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static double cveMatDot(IntPtr mat, IntPtr m);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveMatCross(IntPtr mat, IntPtr m, IntPtr result);

   }
}

