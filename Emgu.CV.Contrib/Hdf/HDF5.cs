//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV.Hdf
{

    public partial class HDF5 : SharedPtrObject
    {

        public HDF5(String fileName)
        {
            using (CvString csFileName = new CvString(fileName))
                _ptr = HdfInvoke.cveHDF5Create(csFileName, ref _sharedPtr);
        }

        public void Close()
        {
            HdfInvoke.cveHDF5Close(_ptr);
        }

        public void GrCreate(String grLabel)
        {
            using (CvString csGrLabel = new CvString(grLabel))
            {
                HdfInvoke.cveHDF5GrCreate(_ptr, csGrLabel);
            }
        }

        public bool HlExist(String label)
        {
            using (CvString csLabel = new CvString(label))
            {
                return HdfInvoke.cveHDF5HlExists(_ptr, csLabel);
            }
        }

        public void DsRead(IOutputArray array, String dslabel)
        {
            using (CvString csDsLabel = new CvString(dslabel))
            using (OutputArray oaArray = array.GetOutputArray())
            {
                HdfInvoke.cveHDF5DsRead(_ptr, oaArray, csDsLabel);
            }
        }

        public void DsWrite(IInputArray array, String dslabel)
        {
            using (CvString csDsLabel = new CvString(dslabel))
            using (InputArray iaArray = array.GetInputArray())
            {
                HdfInvoke.cveHDF5DsWrite(_ptr, iaArray, csDsLabel);
            }
        }

        public void cveHDF5DsCreate(int rows, int cols, int type, String dslabel, int compresslevel = -1, VectorOfInt dimsChunks = null)
        {
            using (CvString csDsLabel = new CvString(dslabel))
            {
                HdfInvoke.cveHDF5DsCreate(_ptr, rows, cols, type, csDsLabel, compresslevel, dimsChunks);
            }
        }

        public bool AtExists(String atlabel)
        {
            using (CvString csAtLabel = new CvString(atlabel))
                return HdfInvoke.cveHDF5AtExists(_ptr, csAtLabel);
        }

        public void AtDelete(String atlabel)
        {
            using (CvString csAtLabel = new CvString(atlabel))
                HdfInvoke.cveHDF5AtDelete(_ptr, csAtLabel);
        }

        public int AtReadInt(String atlabel)
        {
            int v = 0;
            using (CvString csAtLabel = new CvString(atlabel))
                HdfInvoke.cveHDF5AtReadInt(_ptr, ref v, csAtLabel);
            return v;
        }

        public void AtWrite(int value, String atlabel)
        {
            using (CvString csAtLabel = new CvString(atlabel))
                HdfInvoke.cveHDF5AtWriteInt(_ptr, value, csAtLabel);
        }

        public double AtReadDouble(String atlabel)
        {
            double v = 0;
            using (CvString csAtLabel = new CvString(atlabel))
                HdfInvoke.cveHDF5AtReadDouble(_ptr, ref v, csAtLabel);
            return v;
        }

        public void AtWrite(double value, String atlabel)
        {
            using (CvString csAtLabel = new CvString(atlabel))
                HdfInvoke.cveHDF5AtWriteDouble(_ptr, value, csAtLabel);
        }

        public String AtReadString(String atlabel)
        {
            using (CvString cvOut = new CvString())
            using (CvString csAtLabel = new CvString(atlabel))
            {
                HdfInvoke.cveHDF5AtReadString(_ptr, cvOut, csAtLabel);
                return cvOut.ToString();
            }
        }

        public void AtWrite(String value, String atlabel)
        {
            using (CvString csValue = new CvString(value))
            using (CvString csAtLabel = new CvString(atlabel))
                HdfInvoke.cveHDF5AtWriteString(_ptr, csValue, csAtLabel);
        }


        public void AtReadArray(IOutputArray array, String atlabel)
        {
            using (OutputArray oaArray = array.GetOutputArray())
            using (CvString csAtLabel = new CvString(atlabel))
                HdfInvoke.cveHDF5AtReadArray(_ptr, oaArray, csAtLabel);
        }

        public void AtWrite(IInputArray value, String atlabel)
        {
            using (InputArray iaValue = value.GetInputArray())
            using (CvString csAtLabel = new CvString(atlabel))
                HdfInvoke.cveHDF5AtWriteArray(_ptr, iaValue, csAtLabel);
        }

        public void KpRead(
            VectorOfKeyPoint keypoints, 
            String kplabel,
            int offset = -1,
            int counts = -1)
        {
            using (CvString csKpLabel = new CvString(kplabel))
                HdfInvoke.cveHDF5KpRead(_ptr, keypoints, csKpLabel, offset, counts);
        }

        public void KpWrite(
            VectorOfKeyPoint keypoints, 
            String atlabel,
            int offset = -1,
            int counts = -1)
        {
            using (CvString csAtLabel = new CvString(atlabel))
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
            CvInvoke.CheckLibraryLoaded();
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