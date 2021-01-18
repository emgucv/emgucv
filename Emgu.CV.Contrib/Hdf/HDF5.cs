//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using Emgu.CV.CvEnum;

namespace Emgu.CV.Hdf
{
    /// <summary>
    /// Hierarchical Data Format version 5 interface.
    /// </summary>
    public partial class HDF5 : SharedPtrObject
    {
        /// <summary>
        /// Open or create hdf5 file.
        /// </summary>
        /// <param name="fileName">Specify the HDF5 filename.</param>
        public HDF5(String fileName)
        {
            using (CvString csFileName = new CvString(fileName))
                _ptr = HdfInvoke.cveHDF5Create(csFileName, ref _sharedPtr);
        }

        /// <summary>
        /// Close and release hdf5 object.
        /// </summary>
        public void Close()
        {
            HdfInvoke.cveHDF5Close(_ptr);
        }

        /// <summary>
        /// Create a hdf5 group with default properties. The group is closed automatically after creation.
        /// </summary>
        /// <param name="grLabel">Specify the hdf5 group label.</param>
        public void GrCreate(String grLabel)
        {
            using (CvString csGrLabel = new CvString(grLabel))
            {
                HdfInvoke.cveHDF5GrCreate(_ptr, csGrLabel);
            }
        }

        /// <summary>
        /// Check if label exists or not.
        /// </summary>
        /// <param name="label">Specify the hdf5 dataset label.</param>
        /// <returns>Returns true if dataset exists, and false otherwise.</returns>
        public bool HlExist(String label)
        {
            using (CvString csLabel = new CvString(label))
            {
                return HdfInvoke.cveHDF5HlExists(_ptr, csLabel);
            }
        }

        /// <summary>
        /// Read specific dataset from hdf5 file into Mat object.
        /// </summary>
        /// <param name="array">Mat container where data reads will be returned.</param>
        /// <param name="dsLabel">Specify the source hdf5 dataset label.</param>
        public void DsRead(IOutputArray array, String dsLabel)
        {
            using (CvString csDsLabel = new CvString(dsLabel))
            using (OutputArray oaArray = array.GetOutputArray())
            {
                HdfInvoke.cveHDF5DsRead(_ptr, oaArray, csDsLabel);
            }
        }

        /// <summary>
        /// Write or overwrite a Mat object into specified dataset of hdf5 file.
        /// </summary>
        /// <param name="array">Specify Mat data array to be written.</param>
        /// <param name="dsLabel">Specify the target hdf5 dataset label.</param>
        public void DsWrite(IInputArray array, String dsLabel)
        {
            using (CvString csDsLabel = new CvString(dsLabel))
            using (InputArray iaArray = array.GetInputArray())
            {
                HdfInvoke.cveHDF5DsWrite(_ptr, iaArray, csDsLabel);
            }
        }

        /// <summary>
        /// Create and allocate storage for two dimensional single or multi channel dataset.
        /// </summary>
        /// <param name="rows">Declare amount of rows</param>
        /// <param name="cols">Declare amount of columns</param>
        /// <param name="depthType">The pixel depth type</param>
        /// <param name="channels">The number of channels</param>
        /// <param name="dsLabel">Specify the hdf5 dataset label. Existing dataset label will cause an error.</param>
        /// <param name="compressLevel">Specify the compression level 0-9 to be used, -1 is the default value and means no compression. The value 0 also means no compression. A value 9 indicating the best compression ration. Note that a higher compression level indicates a higher computational cost. It relies on GNU gzip for compression.</param>
        /// <param name="dimsChunks">Each array member specifies the chunking size to be used for block I/O, by default null means none at all.</param>
        public void DsCreate(int rows, int cols, DepthType depthType, int channels, String dsLabel, int compressLevel = -1, VectorOfInt dimsChunks = null)
        {
            using (CvString csDsLabel = new CvString(dsLabel))
            {
                HdfInvoke.cveHDF5DsCreate(_ptr, rows, cols, CvInvoke.MakeType(depthType, channels), csDsLabel, compressLevel, dimsChunks);
            }
        }

        /// <summary>
        /// Check whether a given attribute exits or not in the root group.
        /// </summary>
        /// <param name="atLabel">The attribute name to be checked.</param>
        /// <returns>True if the attribute exists, False otherwise.</returns>
        public bool AtExists(String atLabel)
        {
            using (CvString csAtLabel = new CvString(atLabel))
                return HdfInvoke.cveHDF5AtExists(_ptr, csAtLabel);
        }

        /// <summary>
        /// Delete an attribute from the root group.
        /// </summary>
        /// <param name="atLabel">The attribute to be deleted.</param>
        public void AtDelete(String atLabel)
        {
            using (CvString csAtLabel = new CvString(atLabel))
                HdfInvoke.cveHDF5AtDelete(_ptr, csAtLabel);
        }

        /// <summary>
        /// Read an attribute from the root group.
        /// </summary>
        /// <param name="atLabel">Attribute name</param>
        /// <returns>The int value</returns>
        public int AtReadInt(String atLabel)
        {
            int v = 0;
            using (CvString csAtLabel = new CvString(atLabel))
                HdfInvoke.cveHDF5AtReadInt(_ptr, ref v, csAtLabel);
            return v;
        }

        /// <summary>
        /// Write an attribute inside the root group.
        /// </summary>
        /// <param name="value">Attribute value.</param>
        /// <param name="atLabel">Attribute name.</param>
        public void AtWrite(int value, String atLabel)
        {
            using (CvString csAtLabel = new CvString(atLabel))
                HdfInvoke.cveHDF5AtWriteInt(_ptr, value, csAtLabel);
        }

        /// <summary>
        /// Read an attribute from the root group.
        /// </summary>
        /// <param name="atLabel">Attribute name</param>
        /// <returns>The double value</returns>
        public double AtReadDouble(String atLabel)
        {
            double v = 0;
            using (CvString csAtLabel = new CvString(atLabel))
                HdfInvoke.cveHDF5AtReadDouble(_ptr, ref v, csAtLabel);
            return v;
        }

        /// <summary>
        /// Write an attribute inside the root group.
        /// </summary>
        /// <param name="value">Attribute value.</param>
        /// <param name="atLabel">Attribute name.</param>
        public void AtWrite(double value, String atLabel)
        {
            using (CvString csAtLabel = new CvString(atLabel))
                HdfInvoke.cveHDF5AtWriteDouble(_ptr, value, csAtLabel);
        }

        /// <summary>
        /// Read an attribute from the root group.
        /// </summary>
        /// <param name="atLabel">Attribute name</param>
        /// <returns>The String value</returns>
        public String AtReadString(String atLabel)
        {
            using (CvString cvOut = new CvString())
            using (CvString csAtLabel = new CvString(atLabel))
            {
                HdfInvoke.cveHDF5AtReadString(_ptr, cvOut, csAtLabel);
                return cvOut.ToString();
            }
        }

        /// <summary>
        /// Write an attribute inside the root group.
        /// </summary>
        /// <param name="value">Attribute value.</param>
        /// <param name="atLabel">Attribute name.</param>
        public void AtWrite(String value, String atLabel)
        {
            using (CvString csValue = new CvString(value))
            using (CvString csAtLabel = new CvString(atLabel))
                HdfInvoke.cveHDF5AtWriteString(_ptr, csValue, csAtLabel);
        }


        /// <summary>
        /// Read an attribute from the root group.
        /// </summary>
        /// <param name="atLabel">Attribute name</param>
        /// <param name="array">The output value</param>
        public void AtReadArray(IOutputArray array, String atLabel)
        {
            using (OutputArray oaArray = array.GetOutputArray())
            using (CvString csAtLabel = new CvString(atLabel))
                HdfInvoke.cveHDF5AtReadArray(_ptr, oaArray, csAtLabel);
        }

        /// <summary>
        /// Write an attribute inside the root group.
        /// </summary>
        /// <param name="value">Attribute value.</param>
        /// <param name="atLabel">Attribute name.</param>
        public void AtWrite(IInputArray value, String atLabel)
        {
            using (InputArray iaValue = value.GetInputArray())
            using (CvString csAtLabel = new CvString(atLabel))
                HdfInvoke.cveHDF5AtWriteArray(_ptr, iaValue, csAtLabel);
        }

        /// <summary>
        /// Read specific keypoint dataset from hdf5 file into VectorOfKeyPoint object.
        /// </summary>
        /// <param name="keypoints">Container where data reads will be returned.</param>
        /// <param name="kpLabel">Specify the source hdf5 dataset label.</param>
        /// <param name="offset">Specify the offset location over dataset from where read starts.</param>
        /// <param name="counts">Specify the amount of keypoints from dataset to read.</param>
        public void KpRead(
            VectorOfKeyPoint keypoints, 
            String kpLabel,
            int offset = -1,
            int counts = -1)
        {
            using (CvString csKpLabel = new CvString(kpLabel))
                HdfInvoke.cveHDF5KpRead(_ptr, keypoints, csKpLabel, offset, counts);
        }

        /// <summary>
        /// Write or overwrite list of KeyPoint into specified dataset of hdf5 file.
        /// </summary>
        /// <param name="keypoints">Specify keypoints data list to be written.</param>
        /// <param name="atLabel">Specify the target hdf5 dataset label.</param>
        /// <param name="offset">Specify the offset location on dataset from where keypoints will be (over)written into dataset.</param>
        /// <param name="counts">Specify the amount of keypoints that will be written into dataset.</param>
        public void KpWrite(
            VectorOfKeyPoint keypoints, 
            String atLabel,
            int offset = -1,
            int counts = -1)
        {
            using (CvString csAtLabel = new CvString(atLabel))
                HdfInvoke.cveHDF5KpWrite(_ptr, keypoints, csAtLabel, offset, counts);
        }


        /// <summary>
        /// Release all the unmanaged memory associate with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                HdfInvoke.cveHDF5Release(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

    }

    /// <summary>
    /// Entry points to the Open CV HDF module
    /// </summary>
    public static partial class HdfInvoke
    {
        static HdfInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveHDF5Create(IntPtr fileName, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5Release(ref IntPtr hdfPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5GrCreate(IntPtr hdf, IntPtr grlabel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveHDF5HlExists(IntPtr hdf, IntPtr label);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5DsCreate(IntPtr hdf, int rows, int cols, int type, IntPtr dslabel, int compresslevel, IntPtr dimsChunks);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5DsWrite(IntPtr hdf, IntPtr array, IntPtr dslabel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5DsRead(IntPtr hdf, IntPtr array, IntPtr dslabel);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveHDF5AtExists(IntPtr hdf, IntPtr atlabel);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5AtDelete(IntPtr hdf, IntPtr atlabel);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5AtWriteInt(IntPtr hdf, int value, IntPtr atlabel);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5AtReadInt(IntPtr hdf, ref int value, IntPtr atlabel);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5AtWriteDouble(IntPtr hdf, double value, IntPtr atlabel);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5AtReadDouble(IntPtr hdf, ref double value, IntPtr atlabel);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5AtWriteString(IntPtr hdf, IntPtr value, IntPtr atlabel);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5AtReadString(IntPtr hdf, IntPtr value, IntPtr atlabel);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5AtReadArray(IntPtr hdf, IntPtr value, IntPtr atlabel);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5AtWriteArray(IntPtr hdf, IntPtr value, IntPtr atlabel);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5KpRead(
    IntPtr hdf,
    IntPtr keypoints,
    IntPtr kplabel,
    int offset,
    int counts);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5KpWrite(
            IntPtr hdf,
            IntPtr keypoints,
            IntPtr kplabel,
            int offset,
            int counts);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHDF5Close(IntPtr hdf);

    }

}