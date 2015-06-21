//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.Util;
#if ANDROID
using Bitmap = Android.Graphics.Bitmap;
#elif IOS
using UIKit;
using CoreGraphics;
#endif

namespace Emgu.CV
{
   /// <summary>
   /// The equivalent of cv::Mat, should only be used if you know what you are doing.
   /// In most case you should use the Matrix class instead
   /// </summary>
#if !NETFX_CORE
   [Serializable]
#endif
   public partial class UMat : MatDataAllocator, IImage, IEquatable<UMat>
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
      public UMat(SerializationInfo info, StreamingContext context)
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
         int depthType = (int) info.GetValue("DepthType", typeof (int));
         int numberOfChannels = (int)info.GetValue("NumberOfChannels", typeof(int));
         Create(rows, cols, (DepthType) depthType, numberOfChannels );
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

      private byte[] Bytes
      {
         get
         {
            if (IsEmpty)
               return null;
            byte[] data = new byte[Rows* Cols * ElementSize];
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            using (Mat m = new Mat(Rows, Cols, Depth, NumberOfChannels, handle.AddrOfPinnedObject(), Cols*ElementSize))
            {
               CopyTo(m);
            }
            handle.Free();
            return data;
         }
         set
         {
            Debug.Assert(value.Length == Rows * Cols* ElementSize, String.Format("Invalid byte length, expecting {0} but was {1}", Rows * Cols * ElementSize, value.Length));
            GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            using (Mat m = new Mat(Rows, Cols, Depth, NumberOfChannels, handle.AddrOfPinnedObject(), Cols * ElementSize))
            {
               m.CopyTo(this);
            }
            handle.Free();
         }
      }

      private IntPtr _oclMatAllocator;

      private bool _needDispose;

      internal UMat(IntPtr ptr, bool needDispose)
      {
         _ptr = ptr;
         _needDispose = needDispose;
         //InitActionPtr();
         //UMatInvoke.cvUMatUseCustomAllocator(_ptr, AllocateCallback, DeallocateCallback, _allocateDataActionPtr, _freeDataActionPtr, ref _memoryAllocator, ref _oclMatAllocator);
      }

      /// <summary>
      /// Create an empty cv::UMat
      /// </summary>
      public UMat()
         : this(UMatInvoke.cveUMatCreate(), true)
      {
      }

      /// <summary>
      /// Create a umat of the specific type.
      /// </summary>
      /// <param name="rows">Number of rows in a 2D array.</param>
      /// <param name="cols">Number of columns in a 2D array.</param>
      /// <param name="type">Mat element type</param>
      /// <param name="channels">Number of channels</param>
      public UMat(int rows, int cols, CvEnum.DepthType type, int channels)
         : this()
      {
         Create(rows, cols, type, channels);
      }

      /// <summary>
      /// Create a umat of the specific type.
      /// </summary>
      /// <param name="size">Size of the UMat</param>
      /// <param name="type">Mat element type</param>
      /// <param name="channels">Number of channels</param>
      public UMat(Size size, CvEnum.DepthType type, int channels)
         : this(size.Height, size.Width, type, channels)
      {

      }

      /// <summary>
      /// Get the Umat header for the specific roi of the parent
      /// </summary>
      /// <param name="parent">The parent Umat</param>
      /// <param name="roi">The region of interest</param>
      public UMat(UMat parent, Rectangle roi)
        : this(UMatInvoke.cveUMatCreateFromROI(parent.Ptr, ref roi), true)
      {
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
         UMatInvoke.cveUMatCreateData(_ptr, rows, cols, CvInvoke.MakeType(type, channels));
      }

      /// <summary>
      /// The size of this matrix
      /// </summary>
      public Size Size
      {
         get
         {
            return UMatInvoke.cveUMatGetSize(_ptr);
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
      /// The size of the elements in this matrix
      /// </summary>
      public int ElementSize
      {
         get
         {
            return UMatInvoke.cveUMatGetElementSize(_ptr);
         }
      }

      /// <summary>
      /// Copy the data in this umat to the other mat
      /// </summary>
      /// <param name="mask">Operation mask. Its non-zero elements indicate which matrix elements need to be copied.</param>
      /// <param name="m">The input array to copy to</param>
      public void CopyTo(IOutputArray m, IInputArray mask = null)
      {
         using (OutputArray oaM = m.GetOutputArray())
         using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            UMatInvoke.cveUMatCopyTo(this, oaM, iaMask);
      }

      /// <summary>
      /// Copies the values of the UMat to <paramref name="data"/>.
      /// </summary>
      /// <param name="data">The data storage, must match the size of the UMat</param>
      public void CopyTo(Array data)
      {
         if (IsEmpty)
         {
            throw new Exception("The UMat is empty");
         }

         using (Mat.MatWithHandle m = Mat.PrepareArrayForCopy(Depth, Size, NumberOfChannels, data))
            CopyTo(m);
      }

      /// <summary>
      /// Sets all or some of the array elements to the specified value.
      /// </summary>
      /// <param name="value">Assigned scalar converted to the actual array type.</param>
      /// <param name="mask">Operation mask of the same size as the umat.</param>
      public void SetTo(IInputArray value, IInputArray mask = null)
      {
         using (InputArray iaValue = value.GetInputArray())
         using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            UMatInvoke.cveUMatSetTo(Ptr, iaValue, iaMask);
      }

      /// <summary>
      /// Sets all or some of the array elements to the specified value.
      /// </summary>
      /// <param name="value">Assigned scalar value.</param>
      /// <param name="mask">Operation mask of the same size as the umat.</param>
      public void SetTo(MCvScalar value, IInputArray mask = null)
      {
         using (ScalarArray ia = new ScalarArray(value))
            SetTo(ia, mask);
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

            DepthType dt = Mat.GetDepthTypeFromArray(data);
            if (dt == DepthType.Default)
               throw new Exception("The specific data type is not supported.");

            if (dimension == 1)
            {
               this.Create(data.GetLength(0), 1, dt, 1);
            }
            else if (dimension == 2)
            {
               this.Create(data.GetLength(0), data.GetLength(1), dt, 1);
            }
            else if (dimension == 3)
            {
               this.Create(data.GetLength(0), data.GetLength(1), dt, 1);
            }
            else
            {
               throw new Exception("The Mat has to be pre-allocated");
            }
         }

         using (Mat.MatWithHandle m = Mat.PrepareArrayForCopy(Depth, Size, NumberOfChannels, data))
            m.CopyTo(this);
      }

      /// <summary>
      /// Return the Mat representation of the UMat
      /// </summary>
      public Mat ToMat(CvEnum.AccessType access)
      {
         return new Mat(UMatInvoke.cveUMatGetMat(_ptr, access), true, false);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this object.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose && _ptr != IntPtr.Zero)
            UMatInvoke.cveUMatRelease(ref _ptr);

         if (_oclMatAllocator != IntPtr.Zero)
            MatDataAllocatorInvoke.cveMatAllocatorRelease(ref _oclMatAllocator);

         base.DisposeObject();

      }

      /// <summary>
      /// Pointer to the InputArray
      /// </summary>
      public InputArray GetInputArray()
      {
         return new InputArray(UMatInvoke.cveInputArrayFromUMat(_ptr));
      }

      /// <summary>
      /// Pointer to the OutputArray
      /// </summary>
      public OutputArray GetOutputArray()
      {
         return new OutputArray( UMatInvoke.cveOutputArrayFromUMat(_ptr) );
      }

      /// <summary>
      /// Pointer to the InputOutputArray
      /// </summary>
      public InputOutputArray GetInputOutputArray()
      {
         return new InputOutputArray( UMatInvoke.cveInputOutputArrayFromUMat(_ptr) );
      }

      /// <summary>
      /// Changes the shape and/or the number of channels of a 2D matrix without copying the data.
      /// </summary>
      /// <param name="cn">New number of channels. If the parameter is 0, the number of channels remains the same.</param>
      /// <param name="rows">New number of rows. If the parameter is 0, the number of rows remains the same.</param>
      /// <returns>A new mat header that has different shape</returns>
      public UMat Reshape(int cn, int rows = 0)
      {
         return new UMat(UMatInvoke.cveUMatReshape(Ptr, cn, rows), true);
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
            Image<TColor, TDepth> img = new Image<TColor, TDepth>(Size);
            CopyTo(img);
            return img;
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
            using (UMat tmp = new UMat())
            {
               ConvertTo(tmp, CvInvoke.GetDepthType(typeof(TDepth)));
               return tmp.ToImage<TColor, TDepth>();
            }

         }
      }

      #if IOS
      public UIImage ToUIImage()
      {
         using (Image<Rgba, Byte> tmp = ToImage<Rgba, Byte>())
            {
               return tmp.ToUIImage();
            }
      }
#elif ! ( NETFX_CORE || UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO )
      /// <summary>
      /// The Get property provide a more efficient way to convert Image&lt;Gray, Byte&gt;, Image&lt;Bgr, Byte&gt; and Image&lt;Bgra, Byte&gt; into Bitmap
      /// such that the image data is <b>shared</b> with Bitmap. 
      /// If you change the pixel value on the Bitmap, you change the pixel values on the Image object as well!
      /// For other types of image this property has the same effect as ToBitmap()
      /// <b>Take extra caution not to use the Bitmap after the Image object is disposed</b>
      /// The Set property convert the bitmap to this Image type.
      /// </summary>
      public Bitmap Bitmap
      {
         get 
         {
            using (Mat tmp = ToMat(CvEnum.AccessType.Read))
            {
               return tmp.Bitmap;
            }
         }
      }
      #endif

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
      /// Converts an array to another data type with optional scaling.
      /// </summary>
      /// <param name="m">Output matrix; if it does not have a proper size or type before the operation, it is reallocated.</param>
      /// <param name="rtype">Desired output matrix type or, rather, the depth since the number of channels are the same as the input has; if rtype is negative, the output matrix will have the same type as the input.</param>
      /// <param name="alpha">Optional scale factor.</param>
      /// <param name="beta">Optional delta added to the scaled values.</param>
      public void ConvertTo(IOutputArray m, CvEnum.DepthType rtype, double alpha = 1.0, double beta = 0.0)
      {
         using (OutputArray oaM = m.GetOutputArray())
         UMatInvoke.cveUMatConvertTo(Ptr, oaM, rtype, alpha, beta);
      }

      /*
      /// <summary>
      /// Convert this Mat to UMat
      /// </summary>
      /// <param name="access">Access type</param>
      /// <returns>The UMat</returns>
      public Mat ToMat(CvEnum.AccessType access)
      {
         return new Mat(UMatInvoke.cvUMatGetMat(Ptr, access), true);
      }*/

      ///<summary> 
      ///Split current Image into an array of gray scale images where each element 
      ///in the array represent a single color channel of the original image
      ///</summary>
      ///<returns> 
      ///An array of gray scale images where each element  
      ///in the array represent a single color channel of the original image 
      ///</returns>
      public UMat[] Split()
      {
         UMat[] mats = new UMat[NumberOfChannels];
         for (int i = 0; i < mats.Length; i++)
         {
            mats[i] = new UMat(Rows, Cols, Depth, NumberOfChannels);
         }
         using (VectorOfUMat vm = new VectorOfUMat(mats))
         {
            CvInvoke.Split(this, vm);
         }
         return mats;
      }

      IImage[] IImage.Split()
      {
         UMat[] tmp = this.Split();
         IImage[] result = new IImage[tmp.Length];
         for (int i = 0; i < result.Length; i++)
            result[i] = tmp[i];
         return result;
      }

      /// <summary>
      /// Save this image to the specific file. 
      /// </summary>
      /// <param name="fileName">The name of the file to be saved to</param>
      /// <remarks>The image format is chosen depending on the filename extension, see cvLoadImage. Only 8-bit single-channel or 3-channel (with 'BGR' channel order) images can be saved using this function. If the format, depth or channel order is different, use cvCvtScale and cvCvtColor to convert it before saving, or use universal cvSave to save the image to XML or YAML format.</remarks>
      public void Save(string fileName)
      {
         using (Mat tmp = ToMat(CvEnum.AccessType.Read))
         {
            tmp.Save(fileName);
         }
      }

      /// <summary>
      /// Make a clone of the current UMat.
      /// </summary>
      /// <returns>A clone of the current UMat.</returns>
      public UMat Clone()
      {
         UMat m = new UMat();
         CopyTo(m);
         return m;
      }

      object ICloneable.Clone()
      {
         return Clone();
      }

      /// <summary>
      /// Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <param name="other">An object to compare with this object.</param>
      /// <returns>
      /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
      /// </returns>
      public bool Equals(UMat other)
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
   }

   internal static class UMatInvoke
   {
      static UMatInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveInputArrayFromUMat(IntPtr mat);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveOutputArrayFromUMat(IntPtr mat);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveInputOutputArrayFromUMat(IntPtr mat);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveUMatCreate();
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveUMatRelease(ref IntPtr mat);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static Size cveUMatGetSize(IntPtr mat);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveUMatCopyTo(IntPtr mat, IntPtr m, IntPtr mask);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cveUMatGetElementSize(IntPtr mat);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveUMatCreateData(IntPtr mat, int row, int cols, int type);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveUMatCreateFromROI(IntPtr mat, ref Rectangle roi);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveUMatSetTo(IntPtr mat, IntPtr value, IntPtr mask);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveUMatUseCustomAllocator(IntPtr mat, MatDataAllocatorInvoke.MatAllocateCallback allocator, MatDataAllocatorInvoke.MatDeallocateCallback deallocator, IntPtr allocateDataActionPtr, IntPtr freeDataActionPtr, ref IntPtr matAllocator, ref IntPtr oclAllocator);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveUMatGetMat(IntPtr umat, CvEnum.AccessType access);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveUMatConvertTo(IntPtr umat, IntPtr outArray, CvEnum.DepthType rtype, double alpha, double beta);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveUMatReshape(IntPtr mat, int cn, int rows);
   }
}

