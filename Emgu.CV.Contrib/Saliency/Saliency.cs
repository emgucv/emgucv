//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using Emgu.CV.Util;
using Emgu.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Emgu.CV.Saliency
{
    /// <summary>
    /// Base interface for Saliency algorithms
    /// </summary>
    public interface ISaliency : IAlgorithm
    {
        /// <summary>
        /// Pointer to the unmanaged Saliency object
        /// </summary>
        IntPtr SaliencyPtr { get; }
    }

    /// <summary>
    /// Base interface for StaticSaliency algorithms
    /// </summary>
    public interface IStaticSaliency : ISaliency
    {
        /// <summary>
        /// Pointer to the unmanaged StaticSaliency object
        /// </summary>
        IntPtr StaticSaliencyPtr { get; }
    }

    /// <summary>
    /// Base interface for MotionSaliency algorithms
    /// </summary>
    public interface IMotionSaliency : ISaliency
    {
        /// <summary>
        /// Pointer to the unmanaged MotionSaliency object
        /// </summary>
        IntPtr MotionSaliencyPtr { get; }
    }

    /// <summary>
    /// Base interface for Objectness algorithms
    /// </summary>
    public interface IObjectness : ISaliency
    {
        /// <summary>
        /// Pointer to the unmanaged Objectness object
        /// </summary>
        IntPtr ObjectnessPtr { get; }
    }

    /// <summary>
    /// simulate the behavior of pre-attentive visual search
    /// </summary>
    public class StaticSaliencySpectralResidual : UnmanagedObject, IStaticSaliency
    {
        private IntPtr _staticSaliencyPtr;
        private IntPtr _saliencyPtr;
        private IntPtr _algorithmPtr;
        private IntPtr _sharedPtr;

        /// <summary>
        /// constructor
        /// </summary>
        public StaticSaliencySpectralResidual()
        {
            _ptr = SaliencyInvoke.cveStaticSaliencySpectralResidualCreate(ref _staticSaliencyPtr, ref _saliencyPtr,
                ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Pointer to the unmanaged StaticSaliency object
        /// </summary>
        public IntPtr StaticSaliencyPtr
        {
            get { return _staticSaliencyPtr; }
        }

        /// <summary>
        /// Pointer to the unmanaged Saliency object
        /// </summary>
        public IntPtr SaliencyPtr
        {
            get { return _saliencyPtr; }
        }

        /// <summary>
        /// Pointer to the unmanaged Algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                SaliencyInvoke.cveStaticSaliencySpectralResidualRelease(ref _ptr, ref _sharedPtr);
            }
            _staticSaliencyPtr = IntPtr.Zero;
            _saliencyPtr = IntPtr.Zero;
            _algorithmPtr = IntPtr.Zero;

        }
    }

    /// <summary>
    /// The Fine Grained Saliency approach from 
    /// Sebastian Montabone and Alvaro Soto. Human detection using a mobile platform and novel features derived from a visual saliency mechanism. In Image and Vision Computing, Vol. 28 Issue 3, pages 391–402. Elsevier, 2010.
    /// </summary>
    /// <remarks>This method calculates saliency based on center-surround differences. High resolution saliency maps are generated in real time by using integral images.</remarks>
    public class StaticSaliencyFineGrained : UnmanagedObject, IStaticSaliency
    {
        private IntPtr _staticSaliencyPtr;
        private IntPtr _saliencyPtr;
        private IntPtr _algorithmPtr;
        private IntPtr _sharedPtr;

        /// <summary>
        /// constructor
        /// </summary>
        public StaticSaliencyFineGrained()
        {
            _ptr = SaliencyInvoke.cveStaticSaliencyFineGrainedCreate(ref _staticSaliencyPtr, ref _saliencyPtr,
                ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Pointer to the unmanaged StaticSaliency object
        /// </summary>
        public IntPtr StaticSaliencyPtr
        {
            get { return _staticSaliencyPtr; }
        }

        /// <summary>
        /// Pointer to the unmanaged Saliency object
        /// </summary>
        public IntPtr SaliencyPtr
        {
            get { return _saliencyPtr; }
        }

        /// <summary>
        /// Pointer to the unmanaged Algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                SaliencyInvoke.cveStaticSaliencyFineGrainedRelease(ref _ptr, ref _sharedPtr);
            }
            _staticSaliencyPtr = IntPtr.Zero;
            _saliencyPtr = IntPtr.Zero;
            _algorithmPtr = IntPtr.Zero;

        }
    }

    /// <summary>
    /// A Fast Self-tuning Background Subtraction Algorithm.
    /// </summary>
    /// <remarks>This background subtraction algorithm is inspired to the work of B. Wang and P. Dudek [2] [2] B. Wang and P. Dudek "A Fast Self-tuning Background Subtraction Algorithm", in proc of IEEE Workshop on Change Detection, 2014</remarks>
    public partial class MotionSaliencyBinWangApr2014 : UnmanagedObject, IMotionSaliency
    {
        private IntPtr _motionSaliencyPtr;
        private IntPtr _saliencyPtr;
        private IntPtr _algorithmPtr;
        private IntPtr _sharedPtr;

        /// <summary>
        /// constructor
        /// </summary>
        public MotionSaliencyBinWangApr2014()
        {
            _ptr = SaliencyInvoke.cveMotionSaliencyBinWangApr2014Create(ref _motionSaliencyPtr, ref _saliencyPtr,
                ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Pointer to the unmanaged MotionSaliency object
        /// </summary>
        public IntPtr MotionSaliencyPtr
        {
            get { return _motionSaliencyPtr; }
        }

        /// <summary>
        /// Pointer to the unmanaged Saliency object
        /// </summary>
        public IntPtr SaliencyPtr
        {
            get { return _saliencyPtr; }
        }

        /// <summary>
        /// Pointer to the unmanaged Algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                SaliencyInvoke.cveMotionSaliencyBinWangApr2014Release(ref _ptr, ref _sharedPtr);
            }
            _motionSaliencyPtr = IntPtr.Zero;
            _saliencyPtr = IntPtr.Zero;
            _algorithmPtr = IntPtr.Zero;

        }
    }

    /// <summary>
    /// Objectness algorithms based on [3] [3] Cheng, Ming-Ming, et al. "BING: Binarized normed gradients for objectness estimation at 300fps." IEEE CVPR. 2014
    /// </summary>
    public partial class ObjectnessBING : UnmanagedObject, IObjectness
    {
        private IntPtr _objectnessPtr;
        private IntPtr _saliencyPtr;
        private IntPtr _algorithmPtr;
        private IntPtr _sharedPtr;

        /// <summary>
        /// constructor
        /// </summary>
        public ObjectnessBING()
        {
            _ptr = SaliencyInvoke.cveObjectnessBINGCreate(
                ref _objectnessPtr, 
                ref _saliencyPtr,
                ref _algorithmPtr, 
                ref _sharedPtr);
        }

        /// <summary>
        /// Pointer to the unmanaged Objectness object
        /// </summary>
        public IntPtr ObjectnessPtr
        {
            get { return _objectnessPtr; }
        }

        /// <summary>
        /// Pointer to the unmanaged Saliency object
        /// </summary>
        public IntPtr SaliencyPtr
        {
            get { return _saliencyPtr; }
        }

        /// <summary>
        /// Pointer to the unmanaged Algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                SaliencyInvoke.cveObjectnessBINGRelease(ref _ptr, ref _sharedPtr);
            }
            _objectnessPtr = IntPtr.Zero;
            _saliencyPtr = IntPtr.Zero;
            _algorithmPtr = IntPtr.Zero;
        }

        /// <summary>
        /// Return the list of the rectangles' objectness value. 
        /// </summary>
        /// <returns>The list of the rectangles' objectness value.</returns>
        public float[] GetObjectnessValues()
        {
            using (VectorOfFloat vector = new VectorOfFloat())
            {
                SaliencyInvoke.cveObjectnessBINGGetObjectnessValues(_ptr, vector);
                return vector.ToArray();
            }
        }

        /// <summary>
        /// set the correct path from which the algorithm will load the trained model. 
        /// </summary>
        /// <param name="trainingPath">The training path</param>
        public void SetTrainingPath(string trainingPath)
        {
            using (CvString trainingPathStr = new CvString(trainingPath))
            {
                SaliencyInvoke.cveObjectnessBINGSetTrainingPath(_ptr, trainingPathStr);
            }
        }
    }

    /// <summary>
    /// Provide interfaces to the Open CV Saliency functions
    /// </summary>
    public static partial class SaliencyInvoke
    {
        static SaliencyInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveStaticSaliencySpectralResidualCreate(ref IntPtr staticSaliency, ref IntPtr saliency, ref IntPtr algorithm, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStaticSaliencySpectralResidualRelease(ref IntPtr saliency, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveStaticSaliencyFineGrainedCreate(ref IntPtr staticSaliency, ref IntPtr saliency, ref IntPtr algorithm, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStaticSaliencyFineGrainedRelease(ref IntPtr saliency, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMotionSaliencyBinWangApr2014Create(ref IntPtr motionSaliency, ref IntPtr saliency, ref IntPtr algorithm, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMotionSaliencyBinWangApr2014Release(ref IntPtr saliency, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveObjectnessBINGCreate(ref IntPtr objectnessSaliency, ref IntPtr saliency, ref IntPtr algorithm, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveObjectnessBINGRelease(ref IntPtr saliency, ref IntPtr sharedPtr);

        /// <summary>
        /// Compute the saliency.
        /// </summary>
        /// <param name="saliency">The Saliency object</param>
        /// <param name="image">The image.</param>
        /// <param name="saliencyMap">The computed saliency map.</param>
        /// <returns>true if the saliency map is computed, false otherwise</returns>
        public static bool Compute(this ISaliency saliency, IInputArray image, IOutputArray saliencyMap)
        {
            using (var ia = image.GetInputArray())
            using (var oa = saliencyMap.GetOutputArray())
            {
                return cveSaliencyComputeSaliency(saliency.SaliencyPtr, ia, oa);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveSaliencyComputeSaliency(IntPtr saliency, IntPtr image, IntPtr saliencyMap);

        /// <summary>
        /// Perform a binary map of given saliency map
        /// </summary>
        /// <param name="saliencyMap">the saliency map obtained through one of the specialized algorithms</param>
        /// <param name="binaryMap">the binary map</param>
        /// <param name="saliency">The StatucSaliency object</param>
        /// <returns>True if the binary map is sucessfully computed</returns>
        public static bool ComputeBinaryMap(this IStaticSaliency saliency, IInputArray saliencyMap, IOutputArray binaryMap)
        {
            using (InputArray iaSaliencyMap = saliencyMap.GetInputArray())
            using (OutputArray oaBinaryMap = binaryMap.GetOutputArray())
                return cveStaticSaliencyComputeBinaryMap(saliency.StaticSaliencyPtr, iaSaliencyMap, oaBinaryMap);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveStaticSaliencyComputeBinaryMap(IntPtr staticSaliency, IntPtr saliencyMap, IntPtr binaryMap);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveObjectnessBINGGetObjectnessValues(IntPtr bing, IntPtr vectorOfFloat);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveObjectnessBINGSetTrainingPath(IntPtr bing, IntPtr trainingPath);

    }
}