using Emgu.CV.Util;
using Emgu.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Emgu.CV.Saliency
{
    /// <summary>
    /// Base abstract class for Saliency algorithms:
    /// </summary>
    public class Saliency : UnmanagedObject, IAlgorithm
    {
        /// <summary>
        /// pointer to the saliency object
        /// </summary>
        public IntPtr AlgorithmPtr { get; private set; }

        /// <summary>
        /// Creates a specialized saliency algorithm by its name.
        /// </summary>
        /// <param name="saliencyType"></param>
        protected Saliency(string saliencyType)
        {
            using (CvString saliencyTypeStr = new CvString(saliencyType))
            {
                _ptr = SaliencyInvoke.cveSaliencyCreate(saliencyTypeStr);
            }

            AlgorithmPtr = SaliencyInvoke.cveSaliencyGetAlgorithm(_ptr);
        }

        /// <summary>
        /// Performs all the operations, according to the specific algorithm created, to obtain the saliency map.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="saliencyMap"></param>
        /// <returns></returns>
        public bool Compute(Mat image, IOutputArray saliencyMap)
        {
            using (var ia = image.GetInputArray())
            using (var oa = saliencyMap.GetOutputArray())
            {
                return SaliencyInvoke.cveSaliencyComputeSaliency(_ptr, ia, oa);
            }
        }

        /// <summary>
        /// dispose
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                SaliencyInvoke.cveSaliencyRelease(ref _ptr);
        }
    }

    /// <summary>
    /// reflects how likely an image window covers an object of any category
    /// </summary>
    public abstract class Objectness : Saliency
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="saliencyType"></param>
        protected Objectness(string saliencyType) : base(saliencyType) { }
    }

    /// <summary>
    /// detect salient objects over time (hence also over frame)
    /// </summary>
    public abstract class MotionSaliency : Saliency
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="saliencyType"></param>
        protected MotionSaliency(string saliencyType) : base(saliencyType) { }
    }

    /// <summary>
    /// detect salient objects in a non dynamic scenarios.
    /// </summary>
    public abstract class StaticSaliency : Saliency
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="saliencyType"></param>
        protected StaticSaliency(string saliencyType) : base(saliencyType) { }

        /// <summary>
        /// perform a binary map of given saliency map
        /// </summary>
        /// <param name="saliencyMap">the saliency map obtained through one of the specialized algorithms</param>
        /// <param name="binaryMap">the binary map</param>
        /// <returns></returns>
        public bool ComputeBinaryMap(Mat saliencyMap, Mat binaryMap)
        {
            return SaliencyInvoke.cveSaliencyStaticComputeBinaryMap(_ptr, saliencyMap, binaryMap);
        }
    }

    /// <summary>
    /// simulate the behavior of pre-attentive visual search
    /// </summary>
    public class SpectralResidualSaliency : StaticSaliency
    {
        /// <summary>
        /// constructor
        /// </summary>
        public SpectralResidualSaliency() : base("SPECTRAL_RESIDUAL")
        {
        }
    }

    /// <summary>
    /// This method calculates saliency based on center-surround differences
    /// </summary>
    public class FineGrainedSaliency : StaticSaliency
    {
        /// <summary>
        /// constructor
        /// </summary>
        public FineGrainedSaliency() : base("FINE_GRAINED")
        {
        }
    }

    /// <summary>
    /// the Fast Self-tuning Background Subtraction Algorithm
    /// </summary>
    public class BinWangApr2014Saliency : MotionSaliency
    {
        /// <summary>
        /// constructor
        /// </summary>
        public BinWangApr2014Saliency() : base("BinWangApr2014")
        {

        }

        /// <summary>
        /// This function allows the correct initialization of all data structures that will be used by the algorithm. 
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            return SaliencyInvoke.cveSaliencyMotionInit(_ptr);
        }

        /// <summary>
        /// set the correct size (taken from the input image) in the corresponding variables that will be used to size the data structures of the algorithm. 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetImageSize(int width, int height)
        {
            SaliencyInvoke.cveSaliencyMotionSetImageSize(_ptr, width, height);
        }
    }

    /// <summary>
    /// Objectness algorithms based on [3] [3] Cheng, Ming-Ming, et al. "BING: Binarized normed gradients for objectness estimation at 300fps." IEEE CVPR. 2014
    /// </summary>
    public class ObjectnessBing : Objectness
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ObjectnessBing() : base("BING")
        {
        }

        /// <summary>
        /// Return the list of the rectangles' objectness value,. 
        /// </summary>
        /// <returns></returns>
        public VectorOfFloat GetObjectnessValues()
        {
            //pretty sure that the vector<float> is owned by the saliency object, so we shouldn't dispose it.
            VectorOfFloat vector = new VectorOfFloat();
            SaliencyInvoke.cveSaliencyGetObjectnessValues(_ptr, vector);
            return vector;
        }

        /// <summary>
        /// Performs all the operations and calls all internal functions necessary for the accomplishment of the Binarized normed gradients algorithm. 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="boxes"></param>
        /// <returns></returns>
        public bool Compute(Mat image, VectorOfRect boxes)
        {
            using (var ia = image.GetInputArray())
            using (var oa = boxes.GetOutputArray())
            {
                return SaliencyInvoke.cveSaliencyComputeSaliency(_ptr, ia, oa);
            }
        }

        /// <summary>
        /// set the correct path from which the algorithm will load the trained model. 
        /// </summary>
        /// <param name="trainingPath"></param>
        public void SetTrainingPath(string trainingPath)
        {
            using (CvString trainingPathStr = new CvString(trainingPath))
            {
                SaliencyInvoke.cveSaliencySetTrainingPath(_ptr, trainingPathStr);
            }
        }
    }

    /// <summary>
    /// Provide interfaces to the Open CV Saliency functions
    /// </summary>
    public static class SaliencyInvoke
    {
        static SaliencyInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSaliencyCreate(IntPtr saliencyType);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSaliencyRelease(ref IntPtr saliency);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveSaliencyComputeSaliency(IntPtr saliency, IntPtr image, IntPtr saliencyMap);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveSaliencyStaticComputeBinaryMap(IntPtr staticSaliency, IntPtr saliencyMap, IntPtr binaryMap);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveSaliencyMotionInit(IntPtr binWang2014);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSaliencyMotionSetImageSize(IntPtr binWang2014, int width, int height);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSaliencyGetObjectnessValues(IntPtr bing, IntPtr vectorOfFloat);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSaliencySetTrainingPath(IntPtr bing, IntPtr trainingPath);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSaliencyGetAlgorithm(IntPtr saliency);
    }
}