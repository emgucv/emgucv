//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Linq;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.Dpm
{
    /// <summary>
    /// A deformable parts model detector
    /// </summary>
    public class DpmDetector : UnmanagedObject
    {
        Util.VectorOfRect _rects = new Util.VectorOfRect();
        Util.VectorOfFloat _scores = new Util.VectorOfFloat();
        Util.VectorOfInt _classIds = new Util.VectorOfInt();

        /// <summary>
        /// create a new dpm detector with the specified files and classes
        /// </summary>
        /// <param name="files"></param>
        /// <param name="classes"></param>
        /// <returns></returns>
        public static DpmDetector Create(string[] files, string[] classes)
        {
            var cfiles = files.Select(s => new CvString(s)).ToArray();
            var cclasses = classes.Select(s => new CvString(s)).ToArray();

            IntPtr dpm;
            using(var vfiles = new Util.VectorOfCvString(cfiles))
            using(var vclasses = new Util.VectorOfCvString(cclasses))
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

                    var nsize = names.Size;
                    var @out = new string[nsize];
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
            _rects.Clear();
            _scores.Clear();
            _classIds.Clear();

            DpmInvoke.cveDPMDetectorDetect(_ptr, mat, _rects, _scores, _classIds);

            var detections = new ObjectDetection[_rects.Size];
            for (var i = 0; i < detections.Length; i++)
                detections[i] = new ObjectDetection(_rects[i], _scores[i], _classIds[i]);

            return detections;
        }


        /// <summary>
        /// Dispose
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DpmInvoke.cveDPMDetectorRelease(ref _ptr);
                _rects.Dispose();
                _scores.Dispose();
                _classIds.Dispose();
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
