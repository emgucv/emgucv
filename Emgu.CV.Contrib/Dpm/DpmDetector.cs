//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.Dpm
{
    /// <summary>
    /// A deformable parts model detector
    /// </summary>
    public class DpmDetector : UnmanagedObject
    {
        /// <summary>
        /// create a new dpm detector with the specified files and classes
        /// </summary>
        /// <param name="files"></param>
        /// <param name="classes"></param>
        /// <returns></returns>
        public static DpmDetector Create(string[] files, string[] classes)
        {
            CvString[] cfiles = new CvString[files.Length];
            for (int i = 0; i < files.Length; i++)
                cfiles[i] = new CvString(files[i]);

            CvString[] cclasses = new CvString[classes.Length];
            for (int i = 0; i < classes.Length; i++)
                cclasses[i] = new CvString(classes[i]);

            IntPtr dpm;
            using (var vfiles = new Util.VectorOfCvString(cfiles))
            using (var vclasses = new Util.VectorOfCvString(cclasses))
                dpm = DpmInvoke.cveDPMDetectorCreate(vfiles, vclasses);

            foreach (var c in cfiles)
                c.Dispose();
            foreach (var c in cclasses)
                c.Dispose();

            return new DpmDetector(dpm);
        }

        private DpmDetector(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>
        /// Is the detector empty?
        /// </summary>
        public bool IsEmpty { get { return DpmInvoke.cveDPMDetectorIsEmpty(_ptr); } }

        /// <summary>
        /// get the class names
        /// </summary>
        public string[] ClassNames
        {
            get
            {
                using (var names = new Util.VectorOfCvString())
                {
                    DpmInvoke.cveDPMDetectorGetClassNames(_ptr, names);

                    int nsize = names.Size;
                    string[] @out = new string[nsize];
                    for (var i = 0; i < nsize; i++)
                        @out[i] = names[i].ToString();
                    return @out;
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
        /// <returns></returns>
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
        /// Dispose
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DpmInvoke.cveDPMDetectorRelease(ref _ptr);
            }
        }
    }

    public static partial class DpmInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDPMDetectorCreate(IntPtr files, IntPtr classes);

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
        internal static extern void cveDPMDetectorRelease(ref IntPtr dpm);
    }
}
