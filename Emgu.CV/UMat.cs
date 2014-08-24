//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
#if ANDROID
using Bitmap = Android.Graphics.Bitmap;
#elif IOS
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
#endif

namespace Emgu.CV
{
   /// <summary>
   /// The equivalent of cv::Mat, should only be used if you know what you are doing.
   /// In most case you should use the Matrix class instead
   /// </summary>
   public class UMat : MatDataAllocator, IImage
   {
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
         : this(UMatInvoke.cvUMatCreate(), true)
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
      /// Get the Umat header for the specific roi of the parent
      /// </summary>
      /// <param name="parent">The parent Umat</param>
      /// <param name="roi">The region of interest</param>
      public UMat(UMat parent, Rectangle roi)
        : this(UMatInvoke.cvUMatCreateFromROI(parent.Ptr, ref roi), true)
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
         UMatInvoke.cvUMatCreateData(_ptr, rows, cols, CvInvoke.MakeType(type, channels));
      }

      /// <summary>
      /// The size of this matrix
      /// </summary>
      public Size Size
      {
         get
         {
            return UMatInvoke.cvUMatGetSize(_ptr);
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
      /// Get the number of channels
      /// </summary>
      public int NumberOfChannels
      {
         get
         {
            return (int)UMatInvoke.cvUMatGetChannels(_ptr);
         }
      }

      /// <summary>
      /// Depth type
      /// </summary>
      public CvEnum.DepthType Depth
      {
         get
         {
            return (CvEnum.DepthType)UMatInvoke.cvUMatGetDepth(_ptr);
         }
      }

      /// <summary>
      /// The size of the elements in this matrix
      /// </summary>
      public int ElementSize
      {
         get
         {
            return UMatInvoke.cvUMatGetElementSize(_ptr);
         }
      }

      /// <summary>
      /// Copy the data in this mat to the other mat
      /// </summary>
      /// <param name="mask">Operation mask. Its non-zero elements indicate which matrix elements need to be copied.</param>
      /// <param name="m">The input array to copy to</param>
      public void CopyTo(IOutputArray m, IInputArray mask = null)
      {
         using (OutputArray oaM = m.GetOutputArray())
         using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
         UMatInvoke.cvUMatCopyTo(this, oaM, iaMask);
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
         UMatInvoke.cvUMatSetTo(Ptr, iaValue, iaMask);
      }

      /// <summary>
      /// Sets all or some of the array elements to the specified value.
      /// </summary>
      /// <param name="value">Assigned scalar value.</param>
      /// <param name="mask">Operation mask of the same size as the umat.</param>
      public void SetTo(MCvScalar value, IInputArray mask = null)
      {
         using (ScalarArray ia = new ScalarArray(value))
         {
            SetTo(ia, mask);
         }
      }

      /// <summary>
      /// Indicates if this cv::UMat is empty
      /// </summary>
      public bool IsEmpty
      {
         get
         {
            return UMatInvoke.cvUMatIsEmpty(_ptr);
         }
      }

      /// <summary>
      /// Return the Mat representation of the UMat
      /// </summary>
      public Mat ToMat(CvEnum.AccessType access)
      {
         return new Mat(UMatInvoke.cvUMatGetMat(_ptr, access), true, false);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this object.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose && _ptr != IntPtr.Zero)
            UMatInvoke.cvUMatRelease(ref _ptr);

         if (_oclMatAllocator != IntPtr.Zero)
            MatDataAllocatorInvoke.cvMatAllocatorRelease(ref _oclMatAllocator);

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
         return new UMat(UMatInvoke.cvUMatReshape(Ptr, cn, rows), true);
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
      #elif ! ( NETFX_CORE || ( UNITY_ANDROID || UNITY_IPHONE ) )
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
         UMatInvoke.cvUMatConvertTo(Ptr, oaM, rtype, alpha, beta);
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
      internal extern static IntPtr cvUMatCreate();
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvUMatRelease(ref IntPtr mat);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static Size cvUMatGetSize(IntPtr mat);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvUMatCopyTo(IntPtr mat, IntPtr m, IntPtr mask);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cvUMatGetElementSize(IntPtr mat);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cvUMatGetChannels(IntPtr mat);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cvUMatGetDepth(IntPtr mat);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool cvUMatIsEmpty(IntPtr mat);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvUMatCreateData(IntPtr mat, int row, int cols, int type);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvUMatCreateFromROI(IntPtr mat, ref Rectangle roi);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvUMatSetTo(IntPtr mat, IntPtr value, IntPtr mask);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvUMatUseCustomAllocator(IntPtr mat, MatDataAllocatorInvoke.MatAllocateCallback allocator, MatDataAllocatorInvoke.MatDeallocateCallback deallocator, IntPtr allocateDataActionPtr, IntPtr freeDataActionPtr, ref IntPtr matAllocator, ref IntPtr oclAllocator);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvUMatGetMat(IntPtr umat, CvEnum.AccessType access);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvUMatConvertTo(IntPtr umat, IntPtr outArray, CvEnum.DepthType rtype, double alpha, double beta);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvUMatReshape(IntPtr mat, int cn, int rows);
   }
}

