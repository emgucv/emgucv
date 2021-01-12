//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// A GpuMat, use the generic version if possible. The non generic version is good for use as buffer in stream calls.
    /// </summary>
    [DebuggerTypeProxy(typeof(GpuMat.DebuggerProxy))]
    public partial class GpuMat : UnmanagedObject, IEquatable<GpuMat>, IInputOutputArray
    {

        internal bool _needDispose;

        /// <summary>
        /// Create an empty GpuMat
        /// </summary>
        public GpuMat()
           : this(CudaInvoke.gpuMatCreateDefault(), true)
        {
        }

        /// <summary>
        /// Create a GpuMat of the specified size
        /// </summary>
        /// <param name="rows">The number of rows (height)</param>
        /// <param name="cols">The number of columns (width)</param>
        /// <param name="channels">The number of channels</param>
        /// <param name="depthType">The type of depth</param>
        /// <param name="continuous">Indicates if the data should be continuous</param>
        public GpuMat(int rows, int cols, DepthType depthType, int channels, bool continuous = false)
           : this(
              continuous ?
              CudaInvoke.gpuMatCreateContinuous(rows, cols, CvInvoke.MakeType(depthType, channels))
              : CudaInvoke.gpuMatCreateDefault(),
           true)
        {
            if (!continuous)
                Create(rows, cols, depthType, channels);
        }

        /// <summary>
        /// allocates new GpuMat data unless the GpuMat already has specified size and type
        /// </summary>
        /// <param name="rows">The number of rows</param>
        /// <param name="cols">The number of cols</param>
        /// <param name="depthType">The depth type</param>
        /// <param name="channels">The number of channels.</param>
        public void Create(int rows, int cols, DepthType depthType, int channels)
        {
            CudaInvoke.gpuMatCreate(Ptr, rows, cols, CvInvoke.MakeType(depthType, channels));
        }

        /// <summary>
        /// Create a GpuMat from the specific pointer
        /// </summary>
        /// <param name="ptr">Pointer to the unmanaged gpuMat</param>
        /// <param name="needDispose">True if we need to call Release function to <paramref name="ptr"/> during object disposal</param>
        internal GpuMat(IntPtr ptr, bool needDispose)
        {
            _ptr = ptr;
            _needDispose = needDispose;
        }

        /// <summary>
        /// Create a GpuMat from an CvArray of the same depth type
        /// </summary>
        /// <param name="arr">The CvArray to be converted to GpuMat</param>
        public GpuMat(IInputArray arr)
           : this()
        {
            Upload(arr);
        }

        /// <summary>
        /// Create a GpuMat from the specific region of <paramref name="mat"/>. The data is shared between the two GpuMat
        /// </summary>
        /// <param name="mat">The matrix where the region is extracted from</param>
        /// <param name="colRange">The column range.</param>
        /// <param name="rowRange">The row range.</param>
        public GpuMat(GpuMat mat, Emgu.CV.Structure.Range rowRange, Emgu.CV.Structure.Range colRange)
           : this(CudaInvoke.GetRegion(mat, ref rowRange, ref colRange), true)
        {
        }

        /// <summary>
        /// Release the unmanaged memory associated with this GpuMat
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                CudaInvoke.gpuMatRelease(ref _ptr);
        }

        /// <summary>
        /// Get the GpuMat size:
        /// width == number of columns, height == number of rows
        /// </summary>
        public Size Size
        {
            get
            {
                Size s = new Size();
                CudaInvoke.gpuMatGetSize(_ptr, ref s);
                return s;
            }
        }

        /// <summary>
        /// Get the type of the GpuMat
        /// </summary>
        public int Type
        {
            get { return CudaInvoke.gpuMatGetType(_ptr); }
        }

        /// <summary>
        /// Pointer to the InputArray
        /// </summary>
        /// <returns>The input array</returns>
        public InputArray GetInputArray()
        {
            return new InputArray(CudaInvoke.cveInputArrayFromGpuMat(_ptr), this);
        }

        /// <summary>
        /// Pointer to the OutputArray
        /// </summary>
        /// <returns>The output array</returns>
        public OutputArray GetOutputArray()
        {
            return new OutputArray(CudaInvoke.cveOutputArrayFromGpuMat(_ptr), this);
        }

        /// <summary>
        /// Pointer to the InputOutputArray
        /// </summary>
        /// <returns>The input output array</returns>
        public InputOutputArray GetInputOutputArray()
        {
            return new InputOutputArray(CudaInvoke.cveInputOutputArrayFromGpuMat(_ptr), this);
        }

        /// <summary>
        /// Upload data to GpuMat
        /// </summary>
        /// <param name="arr">The CvArray to be uploaded to GpuMat</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>     
        public void Upload(IInputArray arr, Stream stream = null)
        {
            using (InputArray iaArr = arr.GetInputArray())
                CudaInvoke.gpuMatUpload(_ptr, iaArr, stream);
        }

        /// <summary>
        /// Downloads data from device to host memory. 
        /// </summary>
        /// <param name="arr">The destination CvArray where the GpuMat data will be downloaded to.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>     
        public void Download(IOutputArray arr, Stream stream = null)
        {
            //Debug.Assert(arr.Size.Equals(Size), "Destination CvArray size does not match source GpuMat size");
            using (OutputArray oaArr = arr.GetOutputArray())
                CudaInvoke.gpuMatDownload(_ptr, oaArr, stream);
        }

        /// <summary>
        /// Convert the GpuMat to Mat
        /// </summary>
        /// <returns>The Mat that contains the same data as this GpuMat</returns>
        public Mat ToMat()
        {
            Mat m = new Mat();
            Download(m);
            return m;
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
            using (Mat m = new Mat())
            {
                Download(m);
                return m.GetData(jagged);
            }
        }

        /// <summary>
        /// Copies scalar value to every selected element of the destination GpuMat:
        /// arr(I)=value if mask(I)!=0
        /// </summary>
        /// <param name="value">Fill value</param>
        /// <param name="mask">Operation mask, 8-bit single channel GpuMat; specifies elements of destination GpuMat to be changed. Can be IntPtr.Zero if not used</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>     
        public void SetTo(MCvScalar value, IInputArray mask = null, Stream stream = null)
        {
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                CudaInvoke.gpuMatSetTo(Ptr, ref value, iaMask, stream);
        }

        /// <summary>
        /// Copy the source GpuMat to destination GpuMat, using an optional mask.
        /// </summary>
        /// <param name="dst">The output array to be copied to</param>
        /// <param name="mask">The optional mask, use IntPtr.Zero if not needed.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public void CopyTo(IOutputArray dst, IInputArray mask = null, Stream stream = null)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                CudaInvoke.gpuMatCopyTo(Ptr, oaDst, iaMask, stream);
        }

        /// <summary>
        /// This function has several different purposes and thus has several synonyms. It copies one GpuMat to another with optional scaling, which is performed first, and/or optional type conversion, performed after:
        /// dst(I)=src(I)*scale + (shift,shift,...)
        /// All the channels of multi-channel GpuMats are processed independently.
        /// The type conversion is done with rounding and saturation, that is if a result of scaling + conversion can not be represented exactly by a value of destination GpuMat element type, it is set to the nearest representable value on the real axis.
        /// In case of scale=1, shift=0 no prescaling is done. This is a specially optimized case and it has the appropriate convertTo synonym.
        /// </summary>
        /// <param name="dst">Destination GpuMat</param>
        /// <param name="rtype">Result type</param>
        /// <param name="scale">Scale factor</param>
        /// <param name="shift">Value added to the scaled source GpuMat elements</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>      
        public void ConvertTo(IOutputArray dst, CvEnum.DepthType rtype, double scale = 1.0, double shift = 0, Stream stream = null)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
                CudaInvoke.gpuMatConvertTo(Ptr, oaDst, rtype, scale, shift, stream);
        }

        /// <summary>
        /// Changes shape of GpuMat without copying data.
        /// </summary>
        /// <param name="newCn">New number of channels. newCn = 0 means that the number of channels remains unchanged.</param>
        /// <param name="newRows">New number of rows. newRows = 0 means that the number of rows remains unchanged unless it needs to be changed according to newCn value.</param>
        /// <returns>A GpuMat of different shape</returns>
        public GpuMat Reshape(int newCn, int newRows = 0)
        {
            GpuMat result = new GpuMat();
            CudaInvoke.gpuMatReshape(Ptr, result, newCn, newRows);
            return result;
        }

        /// <summary>
        /// Returns a GpuMat corresponding to the ith row of the GpuMat. The data is shared with the current GpuMat. 
        /// </summary>
        /// <param name="i">The row to be extracted</param>
        /// <returns>The ith row of the GpuMat</returns>
        /// <remarks>The parent GpuMat should never be released before the returned GpuMat that represent the subregion</remarks>
        public GpuMat Row(int i)
        {
            return RowRange(i, i + 1);
        }

        /// <summary>
        /// Returns a GpuMat corresponding to the [<paramref name="start"/> <paramref name="end"/>) rows of the GpuMat. The data is shared with the current GpuMat. 
        /// </summary>
        /// <param name="start">The inclusive stating row to be extracted</param>
        /// <param name="end">The exclusive ending row to be extracted</param>
        /// <returns>The [<paramref name="start"/> <paramref name="end"/>) rows of the GpuMat</returns>
        /// <remarks>The parent GpuMat should never be released before the returned GpuMat that represent the subregion</remarks>
        public GpuMat RowRange(int start, int end)
        {
            return new GpuMat(this, new Emgu.CV.Structure.Range(start, end), Emgu.CV.Structure.Range.All);
        }

        /// <summary>
        /// Returns a GpuMat corresponding to the ith column of the GpuMat. The data is shared with the current GpuMat. 
        /// </summary>
        /// <param name="i">The column to be extracted</param>
        /// <returns>The ith column of the GpuMat</returns>
        /// <remarks>The parent GpuMat should never be released before the returned GpuMat that represent the subregion</remarks>
        public GpuMat Col(int i)
        {
            return ColRange(i, i + 1);
        }

        /// <summary>
        /// Returns a GpuMat corresponding to the [<paramref name="start"/> <paramref name="end"/>) columns of the GpuMat. The data is shared with the current GpuMat. 
        /// </summary>
        /// <param name="start">The inclusive stating column to be extracted</param>
        /// <param name="end">The exclusive ending column to be extracted</param>
        /// <returns>The [<paramref name="start"/> <paramref name="end"/>) columns of the GpuMat</returns>
        /// <remarks>The parent GpuMat should never be released before the returned GpuMat that represent the subregion</remarks>
        public GpuMat ColRange(int start, int end)
        {
            return new GpuMat(this, Emgu.CV.Structure.Range.All, new Emgu.CV.Structure.Range(start, end));
        }

        /// <summary>
        /// Returns true if the two GpuMat equals
        /// </summary>
        /// <param name="other">The other GpuMat to be compares with</param>
        /// <returns>True if the two GpuMat equals</returns>
        public bool Equals(GpuMat other)
        {
            if (this.IsEmpty)
            {
                if (!other.IsEmpty)
                    return false;
            }
            else if (other.IsEmpty)
            {
                return false;
            }

            if (NumberOfChannels != other.NumberOfChannels || Size != other.Size || Type != other.Type) return false;

            Size s = Size;
            using (GpuMat xor = new GpuMat())
            {
                CudaInvoke.BitwiseXor(this, other, xor, null, null);

                if (xor.NumberOfChannels == 1)
                    return CudaInvoke.CountNonZero(xor) == 0;
                else
                {
                    using (GpuMat singleChannel = xor.Reshape(1, 0))
                    {
                        return CudaInvoke.CountNonZero(singleChannel) == 0;
                    }
                }
            }
        }

        /// <summary>
        /// Makes multi-channel array out of several single-channel arrays
        /// </summary>
        ///<param name="gpuMats"> 
        ///An array of single channel GpuMat where each item
        ///in the array represent a single channel of the GpuMat 
        ///</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public void MergeFrom(GpuMat[] gpuMats, Stream stream = null)
        {

            //If single channel, perform a copy
            if (gpuMats.Length == 1)
            {
                gpuMats[0].CopyTo(this, null, stream);
            }

            //handle multiple channels
            using (VectorOfGpuMat vm = new VectorOfGpuMat(gpuMats))
            {
                CudaInvoke.Merge(vm, this, stream);
            }
        }

        ///<summary> 
        ///Split current Image into an array of gray scale images where each element 
        ///in the array represent a single color channel of the original image
        ///</summary>
        ///<param name="gpuMats"> 
        ///An array of single channel GpuMat where each item
        ///in the array represent a single channel of the original GpuMat 
        ///</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        private void SplitInto(GpuMat[] gpuMats, Stream stream)
        {
            Debug.Assert(NumberOfChannels == gpuMats.Length, "Number of channels does not agrees with the length of gpuMats");

            if (NumberOfChannels == 1)
            {
                //If single channel, return a copy
                CopyTo(gpuMats[0], null, stream);
            }
            else
            {
                //handle multiple channels

                Size size = Size;

                for (int i = 0; i < gpuMats.Length; i++)
                {
                    gpuMats[i].Create(size.Height, size.Width, Depth, 1);
                }

                using (VectorOfGpuMat vm = new VectorOfGpuMat(gpuMats))
                    CudaInvoke.Split(this, vm, stream);

            }
        }

        ///<summary> 
        ///Split current GpuMat into an array of single channel GpuMat where each element 
        ///in the array represent a single channel of the original GpuMat
        ///</summary>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        ///<returns> 
        ///An array of single channel GpuMat where each element  
        ///in the array represent a single channel of the original GpuMat 
        ///</returns>
        public GpuMat[] Split(Stream stream = null)
        {
            GpuMat[] result = new GpuMat[NumberOfChannels];
            Size size = Size;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new GpuMat();
            }

            SplitInto(result, stream);
            return result;
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
            if (NumberOfChannels == 1)
            {
                CudaInvoke.MinMaxLoc(this, ref minVal, ref maxVal, ref minLoc, ref maxLoc, null);
                minValues[0] = minVal; maxValues[0] = maxVal;
                minLocations[0] = minLoc; maxLocations[0] = maxLoc;
            }
            else
            {
                GpuMat[] channels = Split(null);
                try
                {
                    for (int i = 0; i < NumberOfChannels; i++)
                    {
                        CudaInvoke.MinMaxLoc(channels[i], ref minVal, ref maxVal, ref minLoc, ref maxLoc, null);
                        minValues[i] = minVal; maxValues[i] = maxVal;
                        minLocations[i] = minLoc; maxLocations[i] = maxLoc;
                    }
                }
                finally
                {
                    foreach (GpuMat mat in channels) mat.Dispose();
                }
            }
        }

        /// <summary>
        /// Save the GpuMat to a file
        /// </summary>
        /// <param name="fileName">The file name</param>
        public void Save(string fileName)
        {
            CvInvoke.Imwrite(fileName, this);
        }

        /// <summary>
        /// Make a clone of the GpuMat
        /// </summary>
        /// <returns>A clone of the GPU Mat</returns>
        public object Clone()
        {
            GpuMat clone = new GpuMat();
            CopyTo(clone);
            return clone;
        }

        internal class DebuggerProxy
        {
            private GpuMat _v;

            public DebuggerProxy(GpuMat v)
            {
                _v = v;
            }

            public Mat Mat
            {
                get
                {
                    Mat m = new Mat();
                    _v.Download(m);
                    return m;
                }
            }

            /*
            public Array Data
            {
                get
                {
                    return _v.GetData(true);
                }
            }*/
        }
    }



    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveInputArrayFromGpuMat(IntPtr mat);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveOutputArrayFromGpuMat(IntPtr mat);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveInputOutputArrayFromGpuMat(IntPtr mat);

        /// <summary>
        /// Release the GpuMat
        /// </summary>
        /// <param name="mat">Pointer to the GpuMat</param>
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void gpuMatRelease(ref IntPtr mat);

        /// <summary>
        /// Create an empty GpuMat 
        /// </summary>
        /// <returns>Pointer to an empty GpuMat</returns>
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr gpuMatCreateDefault();


        /// <summary>
        /// Convert a CvArr to a GpuMat
        /// </summary>
        /// <param name="arr">Pointer to a CvArr</param>
        /// <returns>Pointer to the GpuMat</returns>
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr gpuMatCreateFromInputArray(IntPtr arr);

        /// <summary>
        /// Get the GpuMat size:
        /// width == number of columns, height == number of rows
        /// </summary>
        /// <param name="gpuMat">The GpuMat</param>
        /// <param name="size">The size of the matrix</param>
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void gpuMatGetSize(IntPtr gpuMat, ref Size size);

        /// <summary>
        /// Get the GpuMat type
        /// </summary>
        /// <param name="gpuMat">The GpuMat</param>
        /// <returns>The GpuMat type</returns>
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int gpuMatGetType(IntPtr gpuMat);

        /// <summary>
        /// Create a GpuMat of the specified size
        /// </summary>
        /// <param name="mat">Pointer to the native cv::Mat</param>
        /// <param name="rows">The number of rows (height)</param>
        /// <param name="cols">The number of columns (width)</param>
        /// <param name="type">The type of GpuMat</param>
        /// <returns>Pointer to the GpuMat</returns>
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void gpuMatCreate(IntPtr mat, int rows, int cols, int type);

        /// <summary>
        /// Create a GpuMat of the specified size. The allocated data is continuous within this GpuMat.
        /// </summary>
        /// <param name="rows">The number of rows (height)</param>
        /// <param name="cols">The number of columns (width)</param>
        /// <param name="type">The type of GpuMat</param>
        /// <returns>Pointer to the GpuMat</returns>
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr gpuMatCreateContinuous(int rows, int cols, int type);


        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void gpuMatSetTo(IntPtr mat, ref MCvScalar value, IntPtr mask, IntPtr stream);

        /// <summary>
        /// Performs blocking upload data to GpuMat.
        /// </summary>
        /// <param name="gpuMat">The destination gpuMat</param>
        /// <param name="arr">The CvArray to be uploaded to GPU</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void gpuMatUpload(IntPtr gpuMat, IntPtr arr, IntPtr stream);

        /// <summary>
        /// Downloads data from device to host memory. Blocking calls.
        /// </summary>
        /// <param name="gpuMat">The source GpuMat</param>
        /// <param name="arr">The CvArray where data will be downloaded to</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void gpuMatDownload(IntPtr gpuMat, IntPtr arr, IntPtr stream);

        /// <summary>
        /// Copy the source GpuMat to destination GpuMat, using an optional mask.
        /// </summary>
        /// <param name="src">The GpuMat to be copied from</param>
        /// <param name="dst">The GpuMat to be copied to</param>
        /// <param name="mask">The optional mask, use IntPtr.Zero if not needed.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void gpuMatCopyTo(IntPtr src, IntPtr dst, IntPtr mask, IntPtr stream);

        /// <summary>
        /// This function has several different purposes and thus has several synonyms. It copies one GpuMat to another with optional scaling, which is performed first, and/or optional type conversion, performed after:
        /// dst(I)=src(I)*scale + (shift,shift,...)
        /// All the channels of multi-channel GpuMats are processed independently.
        /// The type conversion is done with rounding and saturation, that is if a result of scaling + conversion can not be represented exactly by a value of destination GpuMat element type, it is set to the nearest representable value on the real axis.
        /// In case of scale=1, shift=0 no prescaling is done. This is a specially optimized case and it has the appropriate convertTo synonym.
        /// </summary>
        /// <param name="src">Source GpuMat</param>
        /// <param name="dst">Destination GpuMat</param>
        /// <param name="rtype">The depth type of the destination GpuMat</param>
        /// <param name="scale">Scale factor</param>
        /// <param name="shift">Value added to the scaled source GpuMat elements</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>      
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void gpuMatConvertTo(IntPtr src, IntPtr dst, CvEnum.DepthType rtype, double scale, double shift, IntPtr stream);

        /// <summary>
        /// Changes shape of GpuMat without copying data.
        /// </summary>
        /// <param name="src">The GpuMat to be reshaped.</param>
        /// <param name="dst">The result GpuMat.</param>
        /// <param name="newCn">New number of channels. newCn = 0 means that the number of channels remains unchanged.</param>
        /// <param name="newRows">New number of rows. newRows = 0 means that the number of rows remains unchanged unless it needs to be changed according to newCn value.</param>
        /// <returns>A GpuMat of different shape</returns>
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void gpuMatReshape(IntPtr src, IntPtr dst, int newCn, int newRows);
    }

    /// <summary>
    /// Similar to CvArray but use GPU for processing
    /// </summary>
    /// <typeparam name="TDepth">The type of element in the matrix</typeparam>
    public class GpuMat<TDepth> : GpuMat
       where TDepth : new()
    {
        #region constructors
        /// <summary>
        /// Create a GpuMat from the unmanaged pointer
        /// </summary>
        /// <param name="ptr">The unmanaged pointer to the GpuMat</param>
        /// <param name="needDispose">If true, will call the release function on the <paramref name="ptr"/></param>
        internal GpuMat(IntPtr ptr, bool needDispose)
           : base(ptr, needDispose)
        {
        }

        /// <summary>
        /// Create an empty GpuMat
        /// </summary>
        public GpuMat()
           : base()
        {
        }

        /// <summary>
        /// Create a GpuMat from an CvArray of the same depth type
        /// </summary>
        /// <param name="arr">The CvArry to be converted to GpuMat</param>
        public GpuMat(IInputArray arr)
           : base(arr)
        {
        }

        /// <summary>
        /// Create a GpuMat of the specified size
        /// </summary>
        /// <param name="rows">The number of rows (height)</param>
        /// <param name="cols">The number of columns (width)</param>
        /// <param name="channels">The number of channels</param>
        /// <param name="continuous">Indicates if the data should be continuous</param>
        public GpuMat(int rows, int cols, int channels, bool continuous = false)
           : base(rows, cols, CvInvoke.GetDepthType(typeof(TDepth)), channels, continuous)
        {
        }

        /// <summary>
        /// Create a GpuMat of the specified size
        /// </summary>
        /// <param name="size">The size of the GpuMat</param>
        /// <param name="channels">The number of channels</param>
        public GpuMat(Size size, int channels)
           : this(size.Height, size.Width, channels)
        {
        }

        #endregion

        /// <summary>
        /// Convert this GpuMat to a Matrix
        /// </summary>
        /// <returns>The matrix that contains the same values as this GpuMat</returns>
        public Matrix<TDepth> ToMatrix()
        {
            Size size = Size;
            Matrix<TDepth> result = new Matrix<TDepth>(size.Height, size.Width, NumberOfChannels);
            Download(result);
            return result;
        }

        /// <summary>
        /// Returns a GpuMat corresponding to a specified rectangle of the current GpuMat. The data is shared with the current matrix. In other words, it allows the user to treat a rectangular part of input array as a stand-alone array.
        /// </summary>
        /// <param name="region">Zero-based coordinates of the rectangle of interest.</param>
        /// <returns>A GpuMat that represent the region of the current matrix.</returns>
        /// <remarks>The parent GpuMat should never be released before the returned GpuMat the represent the subregion</remarks>
        public GpuMat<TDepth> GetSubRect(Rectangle region)
        {
            return new GpuMat<TDepth>(CudaInvoke.GetSubRect(this, ref region), true);
        }
    }
}
