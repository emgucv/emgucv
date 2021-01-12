//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
#if ! ( UNITY_IOS || UNITY_ANDROID )

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Emgu.CV
{
    /// <summary>
    /// This 3D Widget defines a point cloud.
    /// </summary>
    public class WCloud : UnmanagedObject, IWidget3D
    {
        private IntPtr _widgetPtr;
        private IntPtr _widget3dPtr;

        /// <summary>
        /// Constructs a WCloud.
        /// </summary>
        /// <param name="cloud">Set of points which can be of type: CV_32FC3, CV_32FC4, CV_64FC3, CV_64FC4.</param>
        /// <param name="color">Set of colors. It has to be of the same size with cloud.</param>
        public WCloud(IInputArray cloud, IInputArray color)
        {
            using (InputArray iaCloud = cloud.GetInputArray())
            using (InputArray iaColor = color.GetInputArray())
                CvInvoke.cveWCloudCreateWithColorArray(iaCloud, iaColor, ref _widget3dPtr, ref _widgetPtr);
        }

        /// <summary>
        /// Constructs a WCloud.
        /// </summary>
        /// <param name="cloud">Set of points which can be of type: CV_32FC3, CV_32FC4, CV_64FC3, CV_64FC4.</param>
        /// <param name="color">A single Color for the whole cloud.</param>
        public WCloud(IInputArray cloud, MCvScalar color)
        {
            using (InputArray iaCloud = cloud.GetInputArray())
                CvInvoke.cveWCloudCreateWithColor(iaCloud, ref color, ref _widget3dPtr, ref _widgetPtr);
        }

        /// <summary>
        /// Get the pointer to the Widget3D obj
        /// </summary>
        public IntPtr GetWidget3D
        {
            get { return _widget3dPtr; }
        }

        /// <summary>
        /// Get the pointer to the Widget obj
        /// </summary>
        public IntPtr GetWidget
        {
            get { return _widgetPtr; }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this WCloud
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveWCloudRelease(ref _ptr);
                _widgetPtr = IntPtr.Zero;
                _widget3dPtr = IntPtr.Zero;
            }
        }
    }

    public static partial class CvInvoke
    {

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWCloudCreateWithColorArray(IntPtr cloud, IntPtr color, ref IntPtr widget3d, ref IntPtr widget);
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWCloudCreateWithColor(IntPtr cloud, ref MCvScalar color, ref IntPtr widget3d, ref IntPtr widget);
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWCloudRelease(ref IntPtr cloud);

        /// <summary>
        /// Read point cloud from file
        /// </summary>
        /// <param name="file">The point cloud file</param>
        /// <param name="colors">The color of the points</param>
        /// <param name="normals">The normal of the points</param>
        /// <returns>The points</returns>
        public static Mat ReadCloud(String file, IOutputArray colors = null, IOutputArray normals = null)
        {
            using (CvString cs = new CvString(file))
            using (OutputArray oaColors = colors == null ? OutputArray.GetEmpty() : colors.GetOutputArray())
            using (OutputArray oaNormals = normals == null ? OutputArray.GetEmpty() : normals.GetOutputArray())
            {
                Mat cloud = new Mat();
                cveReadCloud(cs, cloud, oaColors, oaNormals);
                return cloud;
            }
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveReadCloud(IntPtr file, IntPtr cloud, IntPtr colors, IntPtr normals);

        /// <summary>
        /// Write point cloud to file
        /// </summary>
        /// <param name="file">The point cloud file name</param>
        /// <param name="cloud">The point cloud</param>
        /// <param name="colors">The color</param>
        /// <param name="normals">The normals</param>
        public static void WriteCloud(String file, IInputArray cloud, IInputArray colors = null, IInputArray normals = null)
        {
            using (CvString cs = new CvString(file))
            using (InputArray iaCloud = cloud.GetInputArray())
            using (InputArray iaColors = colors == null ? InputArray.GetEmpty() : colors.GetInputArray())
            using (InputArray iaNormals = normals == null ? InputArray.GetEmpty() : normals.GetInputArray())
            {
                cveWriteCloud(cs, iaCloud, iaColors, iaNormals);
            }
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveWriteCloud(IntPtr file, IntPtr cloud, IntPtr colors, IntPtr normals);

    }
}
#endif