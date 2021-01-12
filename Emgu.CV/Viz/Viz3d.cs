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
    /// Represents a 3D visualizer window. 
    /// </summary>
    public partial class Viz3d : UnmanagedObject
    {
        /// <summary>
        /// Create a new 3D visualizer windows
        /// </summary>
        /// <param name="windowName">The name of the windows</param>
        public Viz3d(String windowName)
        {
            using (CvString cvs = new CvString(windowName))
            {
                _ptr = CvInvoke.cveViz3dCreate(cvs);
            }
        }

        /// <summary>
        /// Show a widget in the window
        /// </summary>
        /// <param name="id">A unique id for the widget.</param>
        /// <param name="widget">The widget to be displayed in the window.</param>
        /// <param name="pose">Pose of the widget.</param>
        public void ShowWidget(String id, IWidget widget, Affine3d pose = null)
        {
            using (CvString cvsId = new CvString(id))
                CvInvoke.cveViz3dShowWidget(_ptr, cvsId, widget.GetWidget, pose);
        }

        /// <summary>
        /// Removes a widget from the window.
        /// </summary>
        /// <param name="id">The id of the widget that will be removed.</param>
        public void RemoveWidget(String id)
        {
            using (CvString cvsId = new CvString(id))
            {
                CvInvoke.cveViz3dRemoveWidget(_ptr, cvsId);
            }
        }

        /// <summary>
        /// Sets pose of a widget in the window.
        /// </summary>
        /// <param name="id">The id of the widget whose pose will be set.</param>
        /// <param name="pose">The new pose of the widget.</param>
        public void SetWidgetPose(String id, Affine3d pose)
        {
            using (CvString cvsId = new CvString(id))
                CvInvoke.cveViz3dSetWidgetPose(_ptr, cvsId, pose);
        }

        /// <summary>
        /// The window renders and starts the event loop.
        /// </summary>
        public void Spin()
        {
            CvInvoke.cveViz3dSpin(_ptr);
        }

        /// <summary>
        /// Starts the event loop for a given time.
        /// </summary>
        /// <param name="time">	Amount of time in milliseconds for the event loop to keep running.</param>
        /// <param name="forceRedraw">If true, window renders.</param>
        public void SpinOnce(int time = 1, bool forceRedraw = false)
        {
            CvInvoke.cveViz3dSpinOnce(_ptr, time, forceRedraw);
        }

        /// <summary>
        /// Returns whether the event loop has been stopped.
        /// </summary>
        public bool WasStopped
        {
            get { return CvInvoke.cveViz3dWasStopped(_ptr); }

        }

        /// <summary>
        /// Set the background color
        /// </summary>
        public void SetBackgroundMeshLab()
        {
            CvInvoke.cveViz3dSetBackgroundMeshLab(_ptr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Viz3d object
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveViz3dRelease(ref _ptr);
        }
    }


    public static partial class CvInvoke
    {
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveViz3dCreate(IntPtr s);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveViz3dShowWidget(IntPtr viz, IntPtr id, IntPtr widget, IntPtr pose);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveViz3dSetWidgetPose(IntPtr viz, IntPtr id, IntPtr pose);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveViz3dRemoveWidget(IntPtr viz, IntPtr id);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveViz3dSpin(IntPtr viz);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveViz3dSpinOnce(
            IntPtr viz,
            int time,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool forceRedraw);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveViz3dWasStopped(IntPtr viz);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveViz3dSetBackgroundMeshLab(IntPtr viz);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveViz3dRelease(ref IntPtr viz);
    }
}
#endif