using Emgu.CV.Util;
using Emgu.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Emgu.CV.Saliency
{
    /// <summary>
    /// Base interface for Saliency algorithms:
    /// </summary>
    public interface ISaliency : IAlgorithm
    {
        IntPtr SaliencyPtr { get; }
    }

    public interface IStaticSaliency : ISaliency
    {
        IntPtr StaticSaliencyPtr { get; }
    }

    public interface IMotionSaliency : ISaliency
    {
        IntPtr MotionSaliencyPtr { get; }
    }

    public interface IObjectness : ISaliency
    {
        IntPtr ObjectnessPtr { get;  }
    }

    /// <summary>
    /// simulate the behavior of pre-attentive visual search
    /// </summary>
    public class StaticSaliencySpectralResidual :  UnmanagedObject, IStaticSaliency
    {
        private  IntPtr _staticSaliencyPtr;
        private IntPtr _saliencyPtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// constructor
        /// </summary>
        public StaticSaliencySpectralResidual()
        {
            _ptr = SaliencyInvoke.cveStaticSaliencySpectralResidualCreate(ref _staticSaliencyPtr, ref _saliencyPtr,
                ref _algorithmPtr);
        }


        public IntPtr StaticSaliencyPtr => _staticSaliencyPtr;

        public IntPtr SaliencyPtr => _saliencyPtr;

        public IntPtr AlgorithmPtr => _algorithmPtr;

        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                SaliencyInvoke.cveStaticSaliencySpectralResidualRelease(ref _ptr);
            }
            _staticSaliencyPtr = IntPtr.Zero;
            _saliencyPtr = IntPtr.Zero;
            _algorithmPtr=IntPtr.Zero;
            
        }
    }

    public class StaticSaliencyFineGrained : UnmanagedObject, IStaticSaliency
    {
        private IntPtr _staticSaliencyPtr;
        private IntPtr _saliencyPtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// constructor
        /// </summary>
        public StaticSaliencyFineGrained()
        {
            _ptr = SaliencyInvoke.cveStaticSaliencyFineGrainedCreate(ref _staticSaliencyPtr, ref _saliencyPtr,
                ref _algorithmPtr);
        }


        public IntPtr StaticSaliencyPtr => _staticSaliencyPtr;

        public IntPtr SaliencyPtr => _saliencyPtr;

        public IntPtr AlgorithmPtr => _algorithmPtr;

        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                SaliencyInvoke.cveStaticSaliencyFineGrainedRelease(ref _ptr);
            }
            _staticSaliencyPtr = IntPtr.Zero;
            _saliencyPtr = IntPtr.Zero;
            _algorithmPtr = IntPtr.Zero;

        }
    }

    public partial class MotionSaliencyBinWangApr2014 : UnmanagedObject, IMotionSaliency
    {
        private IntPtr _motionSaliencyPtr;
        private IntPtr _saliencyPtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// constructor
        /// </summary>
        public MotionSaliencyBinWangApr2014()
        {
            _ptr = SaliencyInvoke.cveMotionSaliencyBinWangApr2014Create(ref _motionSaliencyPtr, ref _saliencyPtr,
                ref _algorithmPtr);
        }


        public IntPtr MotionSaliencyPtr => _motionSaliencyPtr;

        public IntPtr SaliencyPtr => _saliencyPtr;

        public IntPtr AlgorithmPtr => _algorithmPtr;

        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                SaliencyInvoke.cveMotionSaliencyBinWangApr2014Release(ref _ptr);
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

        /// <summary>
        /// constructor
        /// </summary>
        public ObjectnessBING()
        {
            _ptr = SaliencyInvoke.cveMotionSaliencyBinWangApr2014Create(ref _objectnessPtr, ref _saliencyPtr,
                ref _algorithmPtr);
        }

        public IntPtr ObjectnessPtr => _objectnessPtr;

        public IntPtr SaliencyPtr => _saliencyPtr;

        public IntPtr AlgorithmPtr => _algorithmPtr;

        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                SaliencyInvoke.cveMotionSaliencyBinWangApr2014Release(ref _ptr);
            }
            _objectnessPtr = IntPtr.Zero;
            _saliencyPtr = IntPtr.Zero;
            _algorithmPtr = IntPtr.Zero;
        }

        /// <summary>
        /// Return the list of the rectangles' objectness value,. 
        /// </summary>
        /// <returns></returns>
        public VectorOfFloat GetObjectnessValues()
        {
            //pretty sure that the vector<float> is owned by the saliency object, so we shouldn't dispose it.
            VectorOfFloat vector = new VectorOfFloat();
            SaliencyInvoke.cveObjectnessBINGGetObjectnessValues(_ptr, vector);
            return vector;
        }

        /// <summary>
        /// set the correct path from which the algorithm will load the trained model. 
        /// </summary>
        /// <param name="trainingPath"></param>
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
        internal static extern IntPtr cveStaticSaliencySpectralResidualCreate(ref IntPtr staticSaliency, ref IntPtr saliency, ref IntPtr algorithm);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStaticSaliencySpectralResidualRelease(ref IntPtr saliency);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveStaticSaliencyFineGrainedCreate(ref IntPtr staticSaliency, ref IntPtr saliency, ref IntPtr algorithm);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStaticSaliencyFineGrainedRelease(ref IntPtr saliency);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMotionSaliencyBinWangApr2014Create(ref IntPtr motionSaliency, ref IntPtr saliency, ref IntPtr algorithm);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMotionSaliencyBinWangApr2014Release(ref IntPtr saliency);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveObjectnessBINGCreate(ref IntPtr objectnessSaliency, ref IntPtr saliency, ref IntPtr algorithm);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveObjectnessBINGRelease(ref IntPtr saliency);

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
        /// <returns></returns>
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