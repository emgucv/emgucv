using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Emgu.CV
{
    ///<summary> 
    /// Class Histogram 
    ///</summary>
    ///<remarks><B>This is a Beta version</B></remarks>
    //TODO: Improve the histogram class
    public class Histogram : UnmanagedObject
    {
        private int _dimension;
        private int[] _binSize;

        public Histogram(int[] binSizes, float[] min, float[] max)
        {
            _binSize = binSizes;
            _dimension = binSizes.Length;
            if (min.Length != _dimension || max.Length != _dimension)
                throw new Emgu.PrioritizedException(Emgu.ExceptionLevel.Critical, "incompatible dimension");

            IntPtr[] r = new IntPtr[Dimension];
            for (int i = 0; i < _dimension; i++)
            {
                float[] es = new float[2] { min[i], max[i] };
                IntPtr e = Marshal.AllocHGlobal(2 * sizeof(float));
                Marshal.Copy(es, 0, e, 2);
                r[i] = e;
            }
            GCHandle handle = GCHandle.Alloc(binSizes, GCHandleType.Pinned);
            _ptr = CvInvoke.cvCreateHist(_dimension, handle.AddrOfPinnedObject(), 0, r, 1);
            handle.Free();
            foreach (IntPtr e in r)
                Marshal.FreeHGlobal(e);
        }

        ///<summary> 
        /// Clear this histogram
        ///</summary>
        public void Clear()
        {
            CvInvoke.cvClearHist(_ptr);
        }

        ///<summary> 
        /// Project the image to the histogram bins 
        ///</summary>
        public void Accumulate<D>(Image<Gray, D>[] imgs)
        {
            Debug.Assert(imgs.Length == Dimension, "incompatible dimension");

            IntPtr[] imgPtrs = 
                System.Array.ConvertAll<Image<Gray, D>, IntPtr>(
                    imgs,
                    delegate(Image<Gray, D> img) { return img.Ptr; });

            CvInvoke.cvCalcHist(imgPtrs, _ptr, 1, IntPtr.Zero);
        }

        ///<summary> Back project the histogram into an gray scale image</summary>
        public Image<Gray, D> BackProject<D>(Image<Gray, D>[] srcs)
        {
            
            Debug.Assert(srcs.Length == _dimension, "incompatible dimension");

            IntPtr[] imgPtrs = 
                System.Array.ConvertAll<Image<Gray,D>, IntPtr>(
                    srcs, 
                    delegate(Image<Gray, D> img) { return img.Ptr; });

            Image<Gray, D> res = srcs[0].BlankClone();
            CvInvoke.cvCalcBackProject(imgPtrs, res.Ptr, _ptr);
            return res;
        }

        ///<summary>
        ///Clears histogram bins that are below the specified threshold.
        ///</summary>
        ///<param name="thresh">The threshold used to clear the bins</param>
        public void Threshold(double thresh)
        {
            CvInvoke.cvThreshHist(_ptr, thresh);
        }

        ///<summary> Retrieve item counts for the specific bin </summary>
        public double Query(int[] binIndex)
        {
            if (binIndex.Length != _dimension)
                throw new Emgu.PrioritizedException(Emgu.ExceptionLevel.Critical, "incompatible dimension");

            switch (binIndex.Length)
            {
                case 1:
                    return CvInvoke.cvQueryHistValue_1D(_ptr, binIndex[0]);
                case 2:
                    return CvInvoke.cvQueryHistValue_2D(_ptr, binIndex[0], binIndex[1]);
                case 3:
                    return CvInvoke.cvQueryHistValue_3D(_ptr, binIndex[0], binIndex[1], binIndex[2]);
                default:
                    throw new NotImplementedException(String.Format("Retrive from {0} dimensional histogram is not implemented", binIndex.Length));
            }
        }

        public int Dimension { get { return _dimension; } }

        public int[] BinSize { get { return _binSize; } }

        /// <summary>
        /// Release the histogram and all memory associate with it
        /// </summary>
        protected override void FreeUnmanagedObjects()
        {
            CvInvoke.cvReleaseHist(ref _ptr);
        }
    }
}
