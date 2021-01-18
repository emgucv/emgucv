//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV
{
    /// <summary>
    /// The equivalent of cv::Mat, should only be used if you know what you are doing.
    /// In most case you should use the Matrix class instead
    /// </summary>
    [Serializable]
    [DebuggerTypeProxy(typeof(UMat.DebuggerProxy))]
    public partial class UMat : UnmanagedObject, IEquatable<UMat>, IInputOutputArray, ISerializable
    {
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

        /// <summary>
        /// Allocation usage.
        /// </summary>
        public enum Usage
        {
            /// <summary>
            /// Default
            /// </summary>
            Default = 0,

            /// <summary>
            /// Buffer allocation policy is platform and usage specific 
            /// </summary>
            AllocateHostMemory = 1 << 0,
            /// <summary>
            /// Buffer allocation policy is platform and usage specific 
            /// </summary>
            AllocateDeviceMemory = 1 << 1,
            /// <summary>
            /// Buffer allocation policy is platform and usage specific 
            /// It is not equal to: AllocateHostMemory | AllocateDeviceMemory
            /// </summary>
            AllocateSharedMemory = 1 << 2
        }

        /// <summary>
        /// Get or Set the raw image data
        /// </summary>
        public byte[] Bytes
        {
            get
            {
                if (IsEmpty)
                    return null;
                byte[] data = new byte[Total.ToInt32() * ElementSize];
                CopyTo(data);
                return data;
            }
            set
            {
                SetTo(value);
            }
        }

        //private IntPtr _oclMatAllocator;

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
        /// <param name="usage">Allocation Usage</param>
        public UMat(int rows, int cols, CvEnum.DepthType type, int channels, Usage usage = Usage.Default)
           : this()
        {
            Create(rows, cols, type, channels, usage);
        }

        /// <summary>
        /// Create a umat of the specific type.
        /// </summary>
        /// <param name="size">Size of the UMat</param>
        /// <param name="type">Mat element type</param>
        /// <param name="channels">Number of channels</param>
        /// <param name="usage">Allocation Usage</param>
        public UMat(Size size, CvEnum.DepthType type, int channels, Usage usage = Usage.Default)
           : this(size.Height, size.Width, type, channels, usage)
        {

        }

        /// <summary>
        /// Get the Umat header for the specific roi of the parent
        /// </summary>
        /// <param name="parent">The parent Umat</param>
        /// <param name="roi">The region of interest</param>
        public UMat(UMat parent, Rectangle roi)
          : this(UMatInvoke.cveUMatCreateFromRect(parent.Ptr, ref roi), true)
        {
        }

        /// <summary>
        /// Create a umat header for the specific ROI
        /// </summary>
        /// <param name="umat">The umat where the new UMat header will share data from</param>
        /// <param name="rowRange">The region of interest</param>
        /// <param name="colRange">The region of interest</param>
        public UMat(UMat umat, Emgu.CV.Structure.Range rowRange, Emgu.CV.Structure.Range colRange)
           : this(UMatInvoke.cveUMatCreateFromRange(umat.Ptr, ref rowRange, ref colRange), true)
        {
        }

        /// <summary>
        /// Allocates new array data if needed.
        /// </summary>
        /// <param name="rows">New number of rows.</param>
        /// <param name="cols">New number of columns.</param>
        /// <param name="type">New matrix element depth type.</param>
        /// <param name="channels">New matrix number of channels</param>
        /// <param name="usage">Allocation Usage</param>
        public void Create(int rows, int cols, CvEnum.DepthType type, int channels, Usage usage = Usage.Default)
        {
            UMatInvoke.cveUMatCreateData(_ptr, rows, cols, CvInvoke.MakeType(type, channels), usage);
        }

        /// <summary>
        /// Read a UMat from file.
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="loadType">The read mode</param>
        public UMat(String fileName, ImreadModes loadType)
           : this()
        {
            using (Mat tmp = new Mat(fileName, loadType))
            {
                tmp.CopyTo(this);
            }
        }

        /// <summary>
        /// The size of this matrix
        /// </summary>
        public Size Size
        {
            get
            {
                Size s = new Size();
                UMatInvoke.cveUMatGetSize(_ptr, ref s);
                return s;
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

        /*
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
        }*/

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

        /*
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
        }*/

        /// <summary>
        /// Return the Mat representation of the UMat
        /// </summary>
        /// <param name="access">The access type</param>
        /// <returns>The Mat representation of the UMat</returns>
        public Mat GetMat(CvEnum.AccessType access)
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

            //if (_oclMatAllocator != IntPtr.Zero)
            //    MatDataAllocatorInvoke.cveMatAllocatorRelease(ref _oclMatAllocator);

            //base.DisposeObject();

        }

        /// <summary>
        /// Pointer to the InputArray
        /// </summary>
        /// <returns>The input array</returns>
        public InputArray GetInputArray()
        {
            return new InputArray(UMatInvoke.cveInputArrayFromUMat(_ptr), this);
        }

        /// <summary>
        /// Pointer to the OutputArray
        /// </summary>
        /// <returns>The output array</returns>
        public OutputArray GetOutputArray()
        {
            return new OutputArray(UMatInvoke.cveOutputArrayFromUMat(_ptr), this);
        }

        /// <summary>
        /// Pointer to the InputOutputArray
        /// </summary>
        /// <returns>The input output array</returns>
        public InputOutputArray GetInputOutputArray()
        {
            return new InputOutputArray(UMatInvoke.cveInputOutputArrayFromUMat(_ptr), this);
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
                CvEnum.DepthType depth = Depth;
                Mat resultMat = result.Mat;
                this.ConvertTo(resultMat, resultMat.Depth);

                /*
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
            else if (typeof(TDepth) == CvInvoke.GetDepthType(this.Depth) && c.Dimension != numberOfChannels)
            {
                //same depth, different color
                Image<TColor, TDepth> result = new Image<TColor, TDepth>(Size);
                Type t = numberOfChannels == 1
                   ? typeof(Gray)
                   : numberOfChannels == 3
                      ? typeof(Bgr)
                      : numberOfChannels == 4
                         ? typeof(Bgra)
                         : null;
                if (t == null)
                    throw new Exception("Unsupported conversion");
                CvInvoke.CvtColor(this, result, t, typeof(TColor));
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
            Size s = this.Size;
            DepthType d = this.Depth;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = new UMat(s, d, 1);
            }
            using (VectorOfUMat vm = new VectorOfUMat(mats))
            {
                CvInvoke.Split(this, vm);
            }
            return mats;
        }

        /// <summary>
        /// Save this image to the specific file. 
        /// </summary>
        /// <param name="fileName">The name of the file to be saved to</param>
        /// <remarks>The image format is chosen depending on the filename extension, see cvLoadImage. Only 8-bit single-channel or 3-channel (with 'BGR' channel order) images can be saved using this function. If the format, depth or channel order is different, use cvCvtScale and cvCvtColor to convert it before saving, or use universal cvSave to save the image to XML or YAML format.</remarks>
        public void Save(string fileName)
        {
            using (Mat tmp = GetMat(CvEnum.AccessType.Read))
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

            using (UMat cmpResult = new UMat())
            {
                CvInvoke.Compare(this, other, cmpResult, CmpType.NotEqual);
                using (UMat reshaped = cmpResult.Reshape(1))
                    return CvInvoke.CountNonZero(reshaped) == 0;
            }
        }

        /// <summary>
        /// Copy data from this Mat to the managed array
        /// </summary>
        /// <typeparam name="T">The type of managed data array</typeparam>
        /// <param name="data">The managed array where data will be copied to.</param>
        public void CopyTo<T>(T[] data)
        {
            Debug.Assert(
               Toolbox.SizeOf<T>() * data.Length >= Total.ToInt32() * ElementSize,
               String.Format("Size of data is not enough, required at least {0}, but was {1} ", Total.ToInt32() * ElementSize / Toolbox.SizeOf<T>(), data.Length));
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            UMatInvoke.cveUMatCopyDataTo(this, handle.AddrOfPinnedObject());
            handle.Free();
        }

        /// <summary>
        /// Copy data from managed array to this Mat
        /// </summary>
        /// <typeparam name="T">The type of managed data array</typeparam>
        /// <param name="data">The managed array where data will be copied from</param>
        public void SetTo<T>(T[] data)
        {

            Debug.Assert(
               data.Length == Total.ToInt32() * ElementSize / Toolbox.SizeOf<T>(), String.Format("Invalid data length, expecting {0} but was {1}", Total.ToInt32() * ElementSize / Toolbox.SizeOf<T>(), data.Length));
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            UMatInvoke.cveUMatCopyDataFrom(this, handle.AddrOfPinnedObject());
            handle.Free();
        }

        /// <summary>
        /// Computes the dot product of two mats
        /// </summary>
        /// <param name="mat">The matrix to compute dot product with</param>
        /// <returns>The dot product</returns>
        public double Dot(IInputArray mat)
        {
            using (InputArray iaMat = mat.GetInputArray())
                return UMatInvoke.cveUMatDot(Ptr, iaMat);
        }

        /// <summary>
        /// Creates a matrix header for the specified matrix row.
        /// </summary>
        /// <param name="y">A 0-based row index.</param>
        /// <returns>A matrix header for the specified matrix row.</returns>
        public UMat Row(int y)
        {
            return new UMat(this, new Emgu.CV.Structure.Range(y, y + 1), Emgu.CV.Structure.Range.All);
        }

        /// <summary>
        /// Creates a matrix header for the specified matrix column.
        /// </summary>
        /// <param name="x">A 0-based column index.</param>
        /// <returns>A matrix header for the specified matrix column.</returns>
        public UMat Col(int x)
        {
            return new UMat(this, Emgu.CV.Structure.Range.All, new Emgu.CV.Structure.Range(x, x + 1));
        }

        /// <summary>
        /// Get a copy of the data values as an array
        /// </summary>
        /// <param name="jagged">If true, a jagged array will returned. Otherwise it will return a regular array.</param>
        /// <returns>a copy of the data values as an array</returns>
        public Array GetData(bool jagged = true)
        {
            if (IsEmpty)
                return null;
            using (InputArray iaM = this.GetInputArray())
            using (Mat m = iaM.GetMat())
            {
                return m.GetData(jagged);
            }
        }

        #region Operator overload

        /// <summary>
        /// Perform an element wise AND operation on the two mats
        /// </summary>
        /// <param name="mat1">The first mat to AND</param>
        /// <param name="mat2">The second mat to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static UMat operator &(UMat mat1, UMat mat2)
        {
            UMat m = new UMat();
            CvInvoke.BitwiseAnd(mat1, mat2, m);
            return m;
        }

        /// <summary>
        /// Perform an element wise AND operation using a mat and a scalar
        /// </summary>
        /// <param name="mat1">The first image to AND</param>
        /// <param name="val">The value to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static UMat operator &(UMat mat1, double val)
        {
            using (ScalarArray saVal = new ScalarArray(val))
            {
                UMat m = new UMat();
                CvInvoke.BitwiseAnd(mat1, saVal, m);
                return m;
            }
        }


        /// <summary>
        /// Perform an element wise AND operation using a mat and a color
        /// </summary>
        /// <param name="mat1">The first mat to AND</param>
        /// <param name="val">The value to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static UMat operator &(double val, UMat mat1)
        {
            return mat1 & val;
        }

        /// <summary>
        /// Perform an element wise AND operation using a mat and a scalar
        /// </summary>
        /// <param name="mat1">The first mat to AND</param>
        /// <param name="val">The value to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static UMat operator &(UMat mat1, MCvScalar val)
        {
            using (ScalarArray saVal = new ScalarArray(val))
            {
                UMat m = new UMat();
                CvInvoke.BitwiseAnd(mat1, saVal, m);
                return m;
            }
        }

        /// <summary>
        /// Perform an element wise AND operation using a mat and a scalar
        /// </summary>
        /// <param name="mat1">The first mat to AND</param>
        /// <param name="val">The scalar to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static UMat operator &(MCvScalar val, UMat mat1)
        {
            return mat1 & val;
        }

        /// <summary> Perform an element wise OR operation with another mat and return the result</summary>
        /// <param name="mat1">The first mat to apply bitwise OR operation</param>
        /// <param name="mat2">The second mat to apply bitwise OR operation</param>
        /// <returns> The result of the OR operation</returns>
        public static UMat operator |(UMat mat1, UMat mat2)
        {
            UMat m = new UMat();
            CvInvoke.BitwiseOr(mat1, mat2, m);
            return m;
        }

        /// <summary> 
        /// Perform an binary OR operation with some value
        /// </summary>
        /// <param name="mat1">The mat to OR</param>
        /// <param name="val"> The color to OR</param>
        /// <returns> The result of the OR operation</returns>
        public static UMat operator |(UMat mat1, double val)
        {
            using (ScalarArray saVal = new ScalarArray(val))
            {
                UMat m = new UMat();
                CvInvoke.BitwiseOr(mat1, saVal, m);
                return m;
            }
        }

        /// <summary> 
        /// Perform an binary OR operation with some color
        /// </summary>
        /// <param name="mat1">The mat to OR</param>
        /// <param name="val"> The color to OR</param>
        /// <returns> The result of the OR operation</returns>
        public static UMat operator |(double val, UMat mat1)
        {
            return mat1 | val;
        }

        /// <summary> 
        /// Perform an binary OR operation with some scalar
        /// </summary>
        /// <param name="mat1">The mat to OR</param>
        /// <param name="val"> The value to OR</param>
        /// <returns> The result of the OR operation</returns>
        public static UMat operator |(UMat mat1, MCvScalar val)
        {
            using (ScalarArray saVal = new ScalarArray(val))
            {
                UMat m = new UMat();
                CvInvoke.BitwiseOr(mat1, saVal, m);
                return m;
            }
        }

        /// <summary> 
        /// Perform an binary OR operation with some scalar
        /// </summary>
        /// <param name="mat1">The mat to OR</param>
        /// <param name="val"> The color to OR</param>
        /// <returns> The result of the OR operation</returns>
        public static UMat operator |(MCvScalar val, UMat mat1)
        {
            return mat1 | val;
        }

        /// <summary>Compute the complement Mat</summary>
        /// <param name="mat">The mat to be inverted</param>
        /// <returns>The complement image</returns>
        public static UMat operator ~(UMat mat)
        {
            UMat invert = new UMat();
            CvInvoke.BitwiseNot(mat, invert);
            return invert;
        }

        /// <summary>
        /// Element wise add <paramref name="mat1"/> with <paramref name="mat2"/>
        /// </summary>
        /// <param name="mat1">The first mat to be added</param>
        /// <param name="mat2">The second mat to be added</param>
        /// <returns>The sum of the two images</returns>
        public static UMat operator +(UMat mat1, UMat mat2)
        {
            UMat m = new UMat();
            CvInvoke.Add(mat1, mat2, m);
            return m;
        }

        /// <summary>
        /// Element wise add <paramref name="mat1"/> with <paramref name="value"/>
        /// </summary>
        /// <param name="mat1">The mat to be added</param>
        /// <param name="value">The value to be added</param>
        /// <returns>The mat plus the value</returns>
        public static UMat operator +(double value, UMat mat1)
        {
            return mat1 + value;
        }

        /// <summary>
        /// Element wise add <paramref name="mat"/> with <paramref name="value"/>
        /// </summary>
        /// <param name="mat">The mat to be added</param>
        /// <param name="value">The value to be added</param>
        /// <returns>The images plus the value</returns>
        public static UMat operator +(UMat mat, double value)
        {
            using (ScalarArray saVal = new ScalarArray(value))
            {
                UMat m = new UMat();
                CvInvoke.Add(mat, saVal, m);
                return m;
            }
        }

        /// <summary>
        /// Element wise add <paramref name="mat"/> with <paramref name="value"/>
        /// </summary>
        /// <param name="mat">The mat to be added</param>
        /// <param name="value">The value to be added</param>
        /// <returns>The mat plus the value</returns>
        public static UMat operator +(UMat mat, MCvScalar value)
        {
            using (ScalarArray saVal = new ScalarArray(value))
            {
                UMat m = new UMat();
                CvInvoke.Add(mat, saVal, m);
                return m;
            }
        }

        /// <summary>
        /// Element wise add <paramref name="mat"/> with <paramref name="value"/>
        /// </summary>
        /// <param name="mat">The mat to be added</param>
        /// <param name="value">The color to be added</param>
        /// <returns>The images plus the color</returns>
        public static UMat operator +(MCvScalar value, UMat mat)
        {
            return mat + value;
        }

        /// <summary>
        /// Element wise subtract another mat from the current mat
        /// </summary>
        /// <param name="mat1">The mat to be subtracted from.</param>
        /// <param name="mat2">The second image to be subtracted from <paramref name="mat1"/></param>
        /// <returns> The result of element wise subtracting img2 from <paramref name="mat1"/> </returns>
        public static UMat operator -(UMat mat1, UMat mat2)
        {
            UMat m = new UMat();
            CvInvoke.Subtract(mat1, mat2, m);
            return m;
        }

        /// <summary>
        /// Element wise subtract another mat from the current mat
        /// </summary>
        /// <param name="mat">The mat to be subtracted</param>
        /// <param name="value">The value to be subtracted</param>
        /// <returns> The result of element wise subtracting <paramref name="value"/> from <paramref name="mat"/> </returns>
        public static UMat operator -(UMat mat, MCvScalar value)
        {
            using (ScalarArray saValue = new ScalarArray(value))
            {
                UMat m = new UMat();
                CvInvoke.Subtract(mat, saValue, m);
                return m;
            }
        }

        /// <summary>
        /// Element wise subtract value from the current mat
        /// </summary>
        /// <param name="mat">The mat to be subtracted</param>
        /// <param name="value">The color to be subtracted</param>
        /// <returns> <paramref name="value"/> - <paramref name="mat"/> </returns>
        public static UMat operator -(MCvScalar value, UMat mat)
        {
            using (ScalarArray saValue = new ScalarArray(value))
            {
                UMat m = new UMat();
                CvInvoke.Subtract(saValue, mat, m);
                return m;
            }
        }

        /// <summary>
        /// <paramref name="value"/> - <paramref name="mat"/>
        /// </summary>
        /// <param name="mat">The mat to be subtracted</param>
        /// <param name="value">The value to be subtracted</param>
        /// <returns> <paramref name="value"/> - <paramref name="mat"/> </returns>
        public static UMat operator -(double value, UMat mat)
        {
            using (ScalarArray saValue = new ScalarArray(value))
            {
                UMat m = new UMat();
                CvInvoke.Subtract(saValue, mat, m);
                return m;
            }
        }

        /// <summary>
        /// Element wise subtract value from the current mat
        /// </summary>
        /// <param name="mat">The mat to be subtracted</param>
        /// <param name="value">The value to be subtracted</param>
        /// <returns> <paramref name="mat"/> - <paramref name="value"/>   </returns>
        public static UMat operator -(UMat mat, double value)
        {
            using (ScalarArray saValue = new ScalarArray(value))
            {
                UMat m = new UMat();
                CvInvoke.Subtract(mat, saValue, m);
                return m;
            }
        }

        /// <summary>
        ///  <paramref name="mat"/> * <paramref name="scale"/>
        /// </summary>
        /// <param name="mat">The mat</param>
        /// <param name="scale">The multiplication scale</param>
        /// <returns><paramref name="mat"/> * <paramref name="scale"/></returns>
        public static UMat operator *(UMat mat, double scale)
        {
            using (ScalarArray saValue = new ScalarArray(scale))
            {
                UMat m = new UMat();
                CvInvoke.Multiply(mat, saValue, m);
                return m;
            }
        }

        /// <summary>
        ///   <paramref name="scale"/>*<paramref name="mat"/>
        /// </summary>
        /// <param name="mat">The mat</param>
        /// <param name="scale">The multiplication scale</param>
        /// <returns><paramref name="scale"/>*<paramref name="mat"/></returns>
        public static UMat operator *(double scale, UMat mat)
        {
            return mat * scale;
        }

        /// <summary>
        ///  <paramref name="mat"/> / <paramref name="scale"/>
        /// </summary>
        /// <param name="mat">The mat</param>
        /// <param name="scale">The division scale</param>
        /// <returns><paramref name="mat"/> / <paramref name="scale"/></returns>
        public static UMat operator /(UMat mat, double scale)
        {
            return mat * (1.0 / scale);
        }

        /// <summary>
        ///   <paramref name="scale"/> / <paramref name="mat"/>
        /// </summary>
        /// <param name="mat">The mat</param>
        /// <param name="scale">The scale</param>
        /// <returns><paramref name="scale"/> / <paramref name="mat"/></returns>
        public static UMat operator /(double scale, UMat mat)
        {
            using (ScalarArray saScale = new ScalarArray(scale))
            {
                UMat m = new UMat();
                CvInvoke.Divide(saScale, mat, m);
                return m;
            }
        }

        #endregion

        internal class DebuggerProxy
        {
            private UMat _v;

            public DebuggerProxy(UMat v)
            {
                _v = v;
            }

            public Mat Mat
            {
                get
                {
                    Mat m = new Mat();
                    _v.CopyTo(m);
                    return m;
                }
            }
        }
    }

    internal static class UMatInvoke
    {
        static UMatInvoke()
        {
            CvInvoke.Init();
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
        internal extern static void cveUMatGetSize(IntPtr mat, ref Size s);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveUMatCopyTo(IntPtr mat, IntPtr m, IntPtr mask);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static int cveUMatGetElementSize(IntPtr mat);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveUMatCreateData(IntPtr mat, int row, int cols, int type, UMat.Usage flags);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveUMatCreateFromRect(IntPtr mat, ref Rectangle roi);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveUMatCreateFromRange(IntPtr mat, ref Emgu.CV.Structure.Range rowRange, ref Emgu.CV.Structure.Range colRange);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveUMatSetTo(IntPtr mat, IntPtr value, IntPtr mask);

        //[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        //internal extern static void cveUMatUseCustomAllocator(IntPtr mat, MatDataAllocatorInvoke.MatAllocateCallback allocator, MatDataAllocatorInvoke.MatDeallocateCallback deallocator, IntPtr allocateDataActionPtr, IntPtr freeDataActionPtr, ref IntPtr matAllocator, ref IntPtr oclAllocator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveUMatGetMat(IntPtr umat, CvEnum.AccessType access);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveUMatConvertTo(IntPtr umat, IntPtr outArray, CvEnum.DepthType rtype, double alpha, double beta);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveUMatReshape(IntPtr mat, int cn, int rows);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveUMatCopyDataTo(IntPtr mat, IntPtr dest);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveUMatCopyDataFrom(IntPtr mat, IntPtr source);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static double cveUMatDot(IntPtr mat, IntPtr m);


    }
}

