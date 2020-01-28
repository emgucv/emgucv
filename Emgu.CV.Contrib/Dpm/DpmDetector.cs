//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Dpm
{
    /// <summary>
    /// A deformable parts model detector
    /// </summary>
    public class DpmDetector : SharedPtrObject
    {
        /// <summary>
        /// Create a new dpm detector with the specified files and classes
        /// </summary>
        /// <param name="files">A set of file names storing the trained detectors (models). Each file contains one model.</param>
        /// <param name="classes">A set of trained models names. If it's empty then the name of each model will be constructed from the name of file containing the model. E.g. the model stored in "/home/user/cat.xml" will get the name "cat".</param>
        public DpmDetector(string[] files, string[] classes = null)
        {
            CvString[] cfiles = new CvString[files.Length];
            for (int i = 0; i < files.Length; i++)
                cfiles[i] = new CvString(files[i]);

            CvString[] cclasses;
            if (classes == null)
            {
                cclasses = new CvString[0];
            }
            else
            {
                cclasses = new CvString[classes.Length];
                for (int i = 0; i < classes.Length; i++)
                    cclasses[i] = new CvString(classes[i]);
            }
                
            try
            {
                using (var vfiles = new Util.VectorOfCvString(cfiles))
                using (var vclasses = new Util.VectorOfCvString(cclasses))
                    _ptr = DpmInvoke.cveDPMDetectorCreate(vfiles, vclasses, ref _sharedPtr);
            }
            finally
            {
                foreach (var c in cfiles)
                    c.Dispose();
                foreach (var c in cclasses)
                    c.Dispose();
            }
        }

        /// <summary>
        /// Return true if the detector is empty
        /// </summary>
        public bool IsEmpty { get { return DpmInvoke.cveDPMDetectorIsEmpty(_ptr); } }

        /// <summary>
        /// Get the class names
        /// </summary>
        public string[] ClassNames
        {
            get
            {
                using (var names = new Util.VectorOfCvString())
                {
                    DpmInvoke.cveDPMDetectorGetClassNames(_ptr, names);
                    return names.ToArray();
                }
            }
        }

        /// <summary>
        /// get the number of classes
        /// </summary>
        public int ClassCount { get { return (int)DpmInvoke.cveDPMDetectorGetClassCount(_ptr).ToUInt32(); } }

        /// <summary>
        /// Perform detection on the image
        /// </summary>
        /// <param name="mat">The image for detection.</param>
        /// <returns>The detection result</returns>
        public ObjectDetection[] Detect(Mat mat)
        {
            using (Util.VectorOfRect rects = new Util.VectorOfRect())
            using (Util.VectorOfFloat scores = new Util.VectorOfFloat())
            using (Util.VectorOfInt classIds = new Util.VectorOfInt())
            {
                DpmInvoke.cveDPMDetectorDetect(_ptr, mat, rects, scores, classIds);
                ObjectDetection[] detections = new ObjectDetection[rects.Size];
                for (var i = 0; i < detections.Length; i++)
                    detections[i] = new ObjectDetection(rects[i], scores[i], classIds[i]);
                return detections;
            }
        }

        /// <summary>
        /// Dispose the unmanaged memory associated with this DPM
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                DpmInvoke.cveDPMDetectorRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class DpmInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDPMDetectorCreate(IntPtr files, IntPtr classes, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDPMDetectorDetect(IntPtr dpm, IntPtr mat, IntPtr rects, IntPtr scores, IntPtr classIds);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern UIntPtr cveDPMDetectorGetClassCount(IntPtr dpm);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDPMDetectorGetClassNames(IntPtr dpm, IntPtr vector);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveDPMDetectorIsEmpty(IntPtr dpm);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDPMDetectorRelease(ref IntPtr sharedPtr);
    }
}
