//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV
{
    /// <summary>
    /// The equivalent of cv::Mat
    /// </summary>
    [Serializable]
    [DebuggerTypeProxy(typeof(Mat.DebuggerProxy))]
    public partial class Mat : UnmanagedObject, IEquatable<Mat>, IInputOutputArray, ISerializable
    {
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
                byte[] data = new byte[Total.ToInt32() * ElementSize];
                /*
                GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                MatInvoke.cveMatCopyDataTo(this, handle.AddrOfPinnedObject());
                handle.Free();*/
                CopyTo(data);
                return data;
            }
            set
            {
                /*
                Debug.Assert(value.Length == Total.ToInt32() * ElementSize, String.Format("Invalid byte length, expecting {0} but was {1}", Total.ToInt32() * ElementSize, value.Length));
                GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
                MatInvoke.cveMatCopyDataFrom(this, handle.AddrOfPinnedObject());
                handle.Free();*/
                SetTo(value);
            }
        }

        /// <summary>
        /// Copy data from this Mat to the managed array
        /// </summary>
        /// <typeparam name="T">The type of managed data array</typeparam>
        /// <param name="data">The managed array where data will be copied to.</param>
        public void CopyTo<T>(T[] data)
        {
            Debug.Assert(Toolbox.SizeOf<T>() * data.Length >= Total.ToInt32() * ElementSize,
               String.Format("Insufficient data size, required at least {0}, but was {1} ", Total.ToInt32() * ElementSize / Toolbox.SizeOf<T>(), data.Length));
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            MatInvoke.cveMatCopyDataTo(this, handle.AddrOfPinnedObject());
            handle.Free();
        }

        /// <summary>
        /// Copy data from managed array to this Mat
        /// </summary>
        /// <typeparam name="T">The type of managed data array</typeparam>
        /// <param name="data">The managed array where data will be copied from</param>
        public void SetTo<T>(T[] data)
        {
            Debug.Assert(data.Length == Total.ToInt32() * ElementSize / Toolbox.SizeOf<T>(), String.Format("Invalid data length, expecting array of size {0} but got array of size {1}", Total.ToInt32() * ElementSize / Toolbox.SizeOf<T>(), data.Length));
            //Debug.Assert(CvInvoke.GetDepthType(typeof(T)) == this.Depth, String.Format("Array type {0} do not match Mat data type {1}.", typeof(T), this.Depth));
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            MatInvoke.cveMatCopyDataFrom(this, handle.AddrOfPinnedObject());
            handle.Free();
        }

        internal bool _needDispose;

        /// <summary>
        /// An option parent object to keep reference to
        /// </summary>
        internal object _parent;

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
        /// Create multi-dimension mat using existing data.
        /// </summary>
        /// <param name="sizes">The sizes of each dimension</param>
        /// <param name="type">The type of data</param>
        /// <param name="data">The pointer to the unmanaged data</param>
        /// <param name="steps">The steps</param>
        public Mat(int[] sizes, CvEnum.DepthType type, IntPtr data, IntPtr[] steps = null)
           : this(MatInvoke.cveMatCreateMultiDimWithData(sizes, type, data, steps), true, true)
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
        public Mat(String fileName, CvEnum.ImreadModes loadType = ImreadModes.Color)
           : this(MatInvoke.cveMatCreate(), true, false)
        {

            FileInfo fi = new FileInfo(fileName);
            if (!fi.Exists)
            {
                throw new ArgumentException(String.Format("File {0} do not exist", fileName));
            }

            String extension = fi.Extension.ToLower();

            //Open CV's libpng doesn't seem to be able to handle png in iOS
            //Use CGImage to load png
            if (extension.Equals(".png") &&
                (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.IOS ||
                 Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.MacOS))
            {
                bool success = Emgu.CV.NativeMatFileIO.ReadFileToMat(fileName, this, loadType);
                if (success)
                    return;
            }

            using (CvString s = new CvString(fileName))
            {
                CvInvoke.cveImread(s, loadType, this);

                if (this.IsEmpty) //failed to load in the first attempt
                {
                    //try again to see if this is a Unicode issue in the file name. 
                    //Work around for Open CV ticket:
                    //https://github.com/Itseez/opencv/issues/4292
                    //https://github.com/Itseez/opencv/issues/4866

                    byte[] raw = File.ReadAllBytes(fileName);
                    CvInvoke.Imdecode(raw, loadType, this);

                    if (IsEmpty)
                    {
                        //try again to load with Native implementation
                        bool success = Emgu.CV.NativeMatFileIO.ReadFileToMat(fileName, this, loadType);
                        if (!success)
                            throw new ArgumentException(String.Format("Unable to decode file: {0}", fileName));
                    }
                }
            }
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
        /// Create a mat header for the specific ROI
        /// </summary>
        /// <param name="mat">The mat where the new Mat header will share data from</param>
        /// <param name="rowRange">The region of interest</param>
        /// <param name="colRange">The region of interest</param>
        public Mat(Mat mat, Emgu.CV.Structure.Range rowRange, Emgu.CV.Structure.Range colRange)
           : this(MatInvoke.cveMatCreateFromRange(mat.Ptr, ref rowRange, ref colRange), true, true)
        {
        }

        /// <summary>
        /// Convert this Mat to UMat
        /// </summary>
        /// <param name="access">Access type</param>
        /// <param name="usageFlags">Usage flags</param>
        /// <returns>The UMat</returns>
        public UMat GetUMat(CvEnum.AccessType access, UMat.Usage usageFlags = UMat.Usage.Default)
        {
            return new UMat(MatInvoke.cvMatGetUMat(Ptr, access, usageFlags), true);
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
                Size s = new Size();
                MatInvoke.cveMatGetSize(_ptr, ref s);
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
        /// Get a pointer to the raw data given the specific index
        /// </summary>
        /// <param name="index">The index to the Mat data</param>
        /// <returns>A pointer to the raw data given the specific index</returns>
        public IntPtr GetDataPointer(params int[] index)
        {
            if (index.Length == 0)
                return DataPointer;
            int[] indices = new int[Dims];
            Array.Copy(index, indices, index.Length);
            GCHandle handle = GCHandle.Alloc(indices, GCHandleType.Pinned);
            IntPtr result = MatInvoke.cveMatGetDataPointer2(Ptr, handle.AddrOfPinnedObject());
            handle.Free();
            return result;
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

            Type t = CvInvoke.GetDepthType(this.Depth);
            if (t == null)
                return null;

            Array array;
            int byteSize = this.Total.ToInt32() * this.ElementSize;

            if (jagged)
            {
                int[] dim = this.SizeOfDimension;
                int numberOfChannels = this.NumberOfChannels;
                if (numberOfChannels == 1)
                    array = Array.CreateInstance(t, dim);
                else
                {
                    int[] newDim = new int[dim.Length + 1];
                    Array.Copy(dim, newDim, dim.Length);
                    newDim[newDim.Length - 1] = numberOfChannels;
                    array = Array.CreateInstance(t, newDim);
                }
            }
            else
            {
                int len = byteSize / Marshal.SizeOf(t);
                array = Array.CreateInstance(t, len);
            }

            GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
            CvInvoke.cveMemcpy(handle.AddrOfPinnedObject(), DataPointer, byteSize);
            handle.Free();
            return array;
        }

        /// <summary>
        /// Gets the binary data from the specific indices.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <returns>The raw data in byte</returns>
        /// <exception cref="System.NotImplementedException">Indices of length more than 2 is not implemented</exception>
        public byte[] GetRawData(params int[] indices)
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
                    using (Mat matRow = Row(row))
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
                    throw new NotImplementedException(String.Format("GetRawData with indices size {0} is not implemented", indices.Length));
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
        /// Copy the data in this cv::Mat to an output array
        /// </summary>
        /// <param name="m">The output array to copy to</param>
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

        /*
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
              throw new Exception(String.Format("The type of data doesn't match the type of the Mat ({0}).", dt));
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
        }*/

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

            //base.DisposeObject();

        }

        /// <summary>
        /// Pointer to the InputArray
        /// </summary>
        /// <returns>The input array</returns>
        public InputArray GetInputArray()
        {
            return new InputArray(MatInvoke.cveInputArrayFromMat(_ptr), this);
        }

        /// <summary>
        /// Pointer to the OutputArray
        /// </summary>
        /// <returns>The output array</returns>
        public OutputArray GetOutputArray()
        {
            return new OutputArray(MatInvoke.cveOutputArrayFromMat(_ptr), this);
        }

        /// <summary>
        /// Pointer to the InputOutputArray
        /// </summary>
        /// <returns>The input output array</returns>
        public InputOutputArray GetInputOutputArray()
        {
            return new InputOutputArray(MatInvoke.cveInputOutputArrayFromMat(_ptr), this);
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
        /// <param name="tryShareData">If true, we will try to see if we can create an Image object that shared the pixel memory with this Mat.</param>
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
            }
            else if (dimension == 2)
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
            }
            else if (dimension == 3)
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
        }*/

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
        /// Returns an identity matrix of the specified size and type.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="cols">Number of columns.</param>
        /// <param name="type">Mat element type</param>
        /// <param name="channels">Number of channels</param>
        /// <returns>An identity matrix of the specified size and type.</returns>
        public static Mat Eye(int rows, int cols, CvEnum.DepthType type, int channels)
        {
            Mat m = new Mat();
            MatInvoke.cveMatEye(rows, cols, CvInvoke.MakeType(type, channels), m.Ptr);
            return m;
        }

        /// <summary>
        /// Extracts a diagonal from a matrix. The method makes a new header for the specified matrix diagonal. The new matrix is represented as a single-column matrix. Similarly to Mat::row and Mat::col, this is an O(1) operation.
        /// </summary>
        /// <param name="d">Index of the diagonal, with the following values: d=0 is the main diagonal; d &lt; 0 is a diagonal from the lower half. For example, d=-1 means the diagonal is set immediately below the main one; d &gt; 0 is a diagonal from the upper half. For example, d=1 means the diagonal is set immediately above the main one.</param>
        /// <returns>A diagonal from a matrix</returns>
        public Mat Diag(int d = 0)
        {
            Mat m = new Mat();
            MatInvoke.cveMatDiag(Ptr, d, m);
            return m;
        }

        /// <summary>
        /// Transposes a matrix.
        /// </summary>
        /// <returns>The transposes of the matrix.</returns>
        public Mat T()
        {
            Mat m = new Mat();
            MatInvoke.cveMatT(Ptr, m);
            return m;
        }

        /// <summary>
        /// Returns a zero array of the specified size and type.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="cols">Number of columns.</param>
        /// <param name="type">Mat element type</param>
        /// <param name="channels">Number of channels</param>
        /// <returns>A zero array of the specified size and type.</returns>
        public static Mat Zeros(int rows, int cols, CvEnum.DepthType type, int channels)
        {
            Mat m = new Mat();
            MatInvoke.cveMatZeros(rows, cols, CvInvoke.MakeType(type, channels), m.Ptr);
            return m;
        }

        /// <summary>
        /// Returns an array of all 1's of the specified size and type.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="cols">Number of columns.</param>
        /// <param name="type">Mat element type</param>
        /// <param name="channels">Number of channels</param>
        /// <returns>An array of all 1's of the specified size and type.</returns>
        public static Mat Ones(int rows, int cols, CvEnum.DepthType type, int channels)
        {
            Mat m = new Mat();
            MatInvoke.cveMatOnes(rows, cols, CvInvoke.MakeType(type, channels), m.Ptr);
            return m;
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
        /// Creates a matrix header for the specified matrix row.
        /// </summary>
        /// <param name="y">A 0-based row index.</param>
        /// <returns>A matrix header for the specified matrix row.</returns>
        public Mat Row(int y)
        {
            return new Mat(this, new Emgu.CV.Structure.Range(y, y + 1), Emgu.CV.Structure.Range.All);
        }

        /// <summary>
        /// Creates a matrix header for the specified matrix column.
        /// </summary>
        /// <param name="x">A 0-based column index.</param>
        /// <returns>A matrix header for the specified matrix column.</returns>
        public Mat Col(int x)
        {
            return new Mat(this, Emgu.CV.Structure.Range.All, new Emgu.CV.Structure.Range(x, x + 1));
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
                //try to load with Native implementation
                if (Emgu.CV.NativeMatFileIO.WriteMatToFile(this, fileName))
                    return;

                throw e;
            }
        }

        /// <summary>
        /// Make a clone of the current Mat
        /// </summary>
        /// <returns>A clone of the current Mat</returns>
        public Mat Clone()
        {
            Mat c = new Mat();
            CopyTo(c);
            return c;
        }

        /// <summary> 
        /// Split current Image into an array of gray scale images where each element 
        /// in the array represent a single color channel of the original image
        /// </summary>
        /// <returns> 
        /// An array of gray scale images where each element in the array represent a single color channel of the original image 
        /// </returns>
        public Mat[] Split()
        {
            Mat[] mats = new Mat[NumberOfChannels];
            Size s = this.Size;
            DepthType d = this.Depth;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = new Mat(s, d, 1);
            }
            using (VectorOfMat vm = new VectorOfMat(mats))
            {
                CvInvoke.Split(this, vm);
            }
            return mats;
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

        /// <summary>
        /// Get an array of the size of the dimensions. e.g. if the mat is 9x10x11, the array of {9, 10, 11} will be returned.
        /// </summary>
        public int[] SizeOfDimension
        {
            get
            {
                int[] sizes = new int[Dims];
                GCHandle handle = GCHandle.Alloc(sizes, GCHandleType.Pinned);
                try
                {
                    MatInvoke.cveMatGetSizeOfDimension(_ptr, handle.AddrOfPinnedObject());
                }
                finally
                {
                    handle.Free();
                }
                return sizes;
            }
        }

#region Operator overload

        /// <summary>
        /// Perform an element wise AND operation on the two mats
        /// </summary>
        /// <param name="mat1">The first mat to AND</param>
        /// <param name="mat2">The second mat to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static Mat operator &(Mat mat1, Mat mat2)
        {
            Mat m = new Mat();
            CvInvoke.BitwiseAnd(mat1, mat2, m);
            return m;
        }

        /// <summary>
        /// Perform an element wise AND operation using a mat and a scalar
        /// </summary>
        /// <param name="mat1">The first image to AND</param>
        /// <param name="val">The value to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static Mat operator &(Mat mat1, double val)
        {
            using (ScalarArray saVal = new ScalarArray(val))
            {
                Mat m = new Mat();
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
        public static Mat operator &(double val, Mat mat1)
        {
            return mat1 & val;
        }

        /// <summary>
        /// Perform an element wise AND operation using a mat and a scalar
        /// </summary>
        /// <param name="mat1">The first mat to AND</param>
        /// <param name="val">The value to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static Mat operator &(Mat mat1, MCvScalar val)
        {
            using (ScalarArray saVal = new ScalarArray(val))
            {
                Mat m = new Mat();
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
        public static Mat operator &(MCvScalar val, Mat mat1)
        {
            return mat1 & val;
        }

        /// <summary> Perform an element wise OR operation with another mat and return the result</summary>
        /// <param name="mat1">The first mat to apply bitwise OR operation</param>
        /// <param name="mat2">The second mat to apply bitwise OR operation</param>
        /// <returns> The result of the OR operation</returns>
        public static Mat operator |(Mat mat1, Mat mat2)
        {
            Mat m = new Mat();
            CvInvoke.BitwiseOr(mat1, mat2, m);
            return m;
        }

        /// <summary> 
        /// Perform an binary OR operation with some value
        /// </summary>
        /// <param name="mat1">The mat to OR</param>
        /// <param name="val"> The color to OR</param>
        /// <returns> The result of the OR operation</returns>
        public static Mat operator |(Mat mat1, double val)
        {
            using (ScalarArray saVal = new ScalarArray(val))
            {
                Mat m = new Mat();
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
        public static Mat operator |(double val, Mat mat1)
        {
            return mat1 | val;
        }

        /// <summary> 
        /// Perform an binary OR operation with some scalar
        /// </summary>
        /// <param name="mat1">The mat to OR</param>
        /// <param name="val"> The value to OR</param>
        /// <returns> The result of the OR operation</returns>
        public static Mat operator |(Mat mat1, MCvScalar val)
        {
            using (ScalarArray saVal = new ScalarArray(val))
            {
                Mat m = new Mat();
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
        public static Mat operator |(MCvScalar val, Mat mat1)
        {
            return mat1 | val;
        }

        /// <summary>Compute the complement Mat</summary>
        /// <param name="mat">The mat to be inverted</param>
        /// <returns>The complement image</returns>
        public static Mat operator ~(Mat mat)
        {
            Mat invert = new Mat();
            CvInvoke.BitwiseNot(mat, invert);
            return invert;
        }

        /// <summary>
        /// Element wise add <paramref name="mat1"/> with <paramref name="mat2"/>
        /// </summary>
        /// <param name="mat1">The first image to be added</param>
        /// <param name="mat2">The second image to be added</param>
        /// <returns>The sum of the two images</returns>
        public static Mat operator +(Mat mat1, Mat mat2)
        {
            Mat m = new Mat();
            CvInvoke.Add(mat1, mat2, m);
            return m;
        }

        /// <summary>
        /// Element wise add <paramref name="mat1"/> with <paramref name="value"/>
        /// </summary>
        /// <param name="mat1">The mat to be added</param>
        /// <param name="value">The value to be added</param>
        /// <returns>The mat plus the value</returns>
        public static Mat operator +(double value, Mat mat1)
        {
            return mat1 + value;
        }

        /// <summary>
        /// Element wise add <paramref name="mat"/> with <paramref name="value"/>
        /// </summary>
        /// <param name="mat">The mat to be added</param>
        /// <param name="value">The value to be added</param>
        /// <returns>The images plus the value</returns>
        public static Mat operator +(Mat mat, double value)
        {
            using (ScalarArray saVal = new ScalarArray(value))
            {
                Mat m = new Mat();
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
        public static Mat operator +(Mat mat, MCvScalar value)
        {
            using (ScalarArray saVal = new ScalarArray(value))
            {
                Mat m = new Mat();
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
        public static Mat operator +(MCvScalar value, Mat mat)
        {
            return mat + value;
        }

        /// <summary>
        /// Element wise subtract another mat from the current mat
        /// </summary>
        /// <param name="mat1">The mat to be subtracted from.</param>
        /// <param name="mat2">The second image to be subtracted from <paramref name="mat1"/></param>
        /// <returns> The result of element wise subtracting img2 from <paramref name="mat1"/> </returns>
        public static Mat operator -(Mat mat1, Mat mat2)
        {
            Mat m = new Mat();
            CvInvoke.Subtract(mat1, mat2, m);
            return m;
        }

        /// <summary>
        /// Element wise subtract another mat from the current mat
        /// </summary>
        /// <param name="mat">The mat to be subtracted</param>
        /// <param name="value">The value to be subtracted</param>
        /// <returns> The result of element wise subtracting <paramref name="value"/> from <paramref name="mat"/> </returns>
        public static Mat operator -(Mat mat, MCvScalar value)
        {
            using (ScalarArray saValue = new ScalarArray(value))
            {
                Mat m = new Mat();
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
        public static Mat operator -(MCvScalar value, Mat mat)
        {
            using (ScalarArray saValue = new ScalarArray(value))
            {
                Mat m = new Mat();
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
        public static Mat operator -(double value, Mat mat)
        {
            using (ScalarArray saValue = new ScalarArray(value))
            {
                Mat m = new Mat();
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
        public static Mat operator -(Mat mat, double value)
        {
            using (ScalarArray saValue = new ScalarArray(value))
            {
                Mat m = new Mat();
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
        public static Mat operator *(Mat mat, double scale)
        {
            using (ScalarArray saValue = new ScalarArray(scale))
            {
                Mat m = new Mat();
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
        public static Mat operator *(double scale, Mat mat)
        {
            return mat * scale;
        }

        /// <summary>
        ///  <paramref name="mat"/> / <paramref name="scale"/>
        /// </summary>
        /// <param name="mat">The mat</param>
        /// <param name="scale">The division scale</param>
        /// <returns><paramref name="mat"/> / <paramref name="scale"/></returns>
        public static Mat operator /(Mat mat, double scale)
        {
            return mat * (1.0 / scale);
        }

        /// <summary>
        ///   <paramref name="scale"/> / <paramref name="mat"/>
        /// </summary>
        /// <param name="mat">The mat</param>
        /// <param name="scale">The scale</param>
        /// <returns><paramref name="scale"/> / <paramref name="mat"/></returns>
        public static Mat operator /(double scale, Mat mat)
        {
            using (ScalarArray saScale = new ScalarArray(scale))
            {
                Mat m = new Mat();
                CvInvoke.Divide(saScale, mat, m);
                return m;
            }
        }

#endregion

        internal class DebuggerProxy
        {
            private Mat _v;

            public DebuggerProxy(Mat v)
            {
                _v = v;
            }

            public Array Data
            {
                get
                {
                    return _v.GetData(true);
                }
            }
        }
    }

    internal static class MatInvoke
    {
        static MatInvoke()
        {
            CvInvoke.Init();
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
        internal extern static void cveMatGetSize(IntPtr mat, ref Size s);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMatCopyTo(IntPtr mat, IntPtr m, IntPtr mask);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static int cveMatGetElementSize(IntPtr mat);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveMatGetDataPointer(IntPtr mat);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveMatGetDataPointer2(IntPtr mat, IntPtr indices);

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

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveMatCreateFromRange(IntPtr mat, ref Emgu.CV.Structure.Range rowRange, ref Emgu.CV.Structure.Range colRange);

        /*
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cvMatCreateFromFile(
           [MarshalAs(CvInvoke.StringMarshalType)]
           String fileName,
           CvEnum.ImreadModes flag
           );
        */
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cvMatGetUMat(IntPtr mat, CvEnum.AccessType access, UMat.Usage usageFlags);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cvMatSetTo(IntPtr mat, IntPtr value, IntPtr mask);

        //[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        //internal extern static IntPtr cveMatUseCustomAllocator(IntPtr mat, MatDataAllocatorInvoke.MatAllocateCallback allocator, MatDataAllocatorInvoke.MatDeallocateCallback deallocator, IntPtr allocateDataActionPtr, IntPtr freeDataActionPtr);

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

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMatCopyDataTo(IntPtr mat, IntPtr dest);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMatCopyDataFrom(IntPtr mat, IntPtr source);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMatGetSizeOfDimension(IntPtr mat, IntPtr sizes);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveMatCreateMultiDimWithData(int ndims, IntPtr sizes, CvEnum.DepthType type, IntPtr data,
           IntPtr steps);

        internal static IntPtr cveMatCreateMultiDimWithData(int[] sizes, CvEnum.DepthType type, IntPtr data,
           IntPtr[] steps)
        {
            GCHandle sizesHandle = GCHandle.Alloc(sizes, GCHandleType.Pinned);

            try
            {
                if (steps == null)
                {
                    return cveMatCreateMultiDimWithData(sizes.Length, sizesHandle.AddrOfPinnedObject(), type, data,
                       IntPtr.Zero);
                }
                else
                {
                    GCHandle stepsHandle = GCHandle.Alloc(steps, GCHandleType.Pinned);
                    try
                    {
                        return cveMatCreateMultiDimWithData(sizes.Length, sizesHandle.AddrOfPinnedObject(), type, data, stepsHandle.AddrOfPinnedObject());
                    }
                    finally
                    {
                        stepsHandle.Free();
                    }

                }
            }
            finally
            {
                sizesHandle.Free();
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMatEye(int rows, int cols, int type, IntPtr m);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMatDiag(IntPtr src, int d, IntPtr dst);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMatT(IntPtr src, IntPtr dst);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMatZeros(int rows, int cols, int type, IntPtr dst);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMatOnes(int rows, int cols, int type, IntPtr dst);

    }
}

