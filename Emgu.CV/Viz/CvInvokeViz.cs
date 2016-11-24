//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
#if ! (NETFX_CORE || __ANDROID__ || __IOS__ || UNITY_IOS || UNITY_ANDROID )

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

    /// <summary>
    /// Interface for all widgets
    /// </summary>
    public interface IWidget
    {
        /// <summary>
        /// Get the pointer to the widget object
        /// </summary>
        IntPtr GetWidget { get; }
    }

    /// <summary>
    /// Interface for all widget3D
    /// </summary>
    public interface IWidget3D : IWidget
    {
        /// <summary>
        /// Get the pointer to the widget3D object
        /// </summary>
        IntPtr GetWidget3D { get; }
    }

    /// <summary>
    /// Interface for all widget2D
    /// </summary>
    public interface IWidget2D : IWidget
    {
        /// <summary>
        /// Get the pointer to the widget2D object
        /// </summary>
        IntPtr GetWidget2D { get; }
    }

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

    /// <summary>
    /// This 2D Widget represents text overlay.
    /// </summary>
    public class WText : UnmanagedObject, IWidget2D
    {
        private IntPtr _widgetPtr;
        private IntPtr _widget2dPtr;

        /// <summary>
        /// Constructs a WText.
        /// </summary>
        /// <param name="text">Text content of the widget.</param>
        /// <param name="pos">Position of the text.</param>
        /// <param name="fontSize">Font size.</param>
        /// <param name="color">Color of the text.</param>
        public WText(String text, Point pos, int fontSize, MCvScalar color)
        {
            using (CvString cvs = new CvString(text))
                _ptr = CvInvoke.cveWTextCreate(cvs, ref pos, fontSize, ref color, ref _widget2dPtr, ref _widgetPtr);

        }

        /// <summary>
        /// Get the pointer to the widget2D object
        /// </summary>
        public IntPtr GetWidget2D
        {
            get { return _widget2dPtr; }
        }

        /// <summary>
        /// Get the pointer to the widget object.
        /// </summary>
        public IntPtr GetWidget
        {
            get { return _widgetPtr; }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Viz3d object
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveViz3dRelease(ref _ptr);
            _widgetPtr = IntPtr.Zero;
            _widget2dPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// This 3D Widget represents a coordinate system.
    /// </summary>
    public class WCoordinateSystem : UnmanagedObject, IWidget3D
    {
        private IntPtr _widgetPtr;
        private IntPtr _widget3dPtr;

        /// <summary>
        /// Constructs a WCoordinateSystem.
        /// </summary>
        /// <param name="scale">Determines the size of the axes.</param>
        public WCoordinateSystem(double scale = 1.0)
        {
            _ptr = CvInvoke.cveWCoordinateSystemCreate(scale, ref _widget3dPtr, ref _widgetPtr);
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
        /// Release the unmanaged memory associated with this WCoordinateSysyem object
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveWCoordinateSystemRelease(ref _ptr);
            _widgetPtr = IntPtr.Zero;
            _widget3dPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// This 3D Widget defines a cube.
    /// </summary>
    public class WCube : UnmanagedObject, IWidget3D
    {
        private IntPtr _widgetPtr;
        private IntPtr _widget3dPtr;

        /// <summary>
        /// Constructs a WCube.
        /// </summary>
        /// <param name="minPoint">Specifies minimum point of the bounding box.</param>
        /// <param name="maxPoint">Specifies maximum point of the bounding box.</param>
        /// <param name="wireFrame">If true, cube is represented as wireframe.</param>
        /// <param name="color">Color of the cube.</param>
        public WCube(MCvPoint3D64f minPoint, MCvPoint3D64f maxPoint, bool wireFrame, MCvScalar color)
        {
            _ptr = CvInvoke.cveWCubeCreate(ref minPoint, ref maxPoint, wireFrame, ref color, ref _widget3dPtr, ref _widgetPtr);
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
        /// Release the unmanaged memory associated with this WCube object
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveWCubeRelease(ref _ptr);
            _widgetPtr = IntPtr.Zero;
            _widget3dPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// This 3D Widget defines a cylinder.
    /// </summary>
    public class WCylinder : UnmanagedObject, IWidget3D
    {
        private IntPtr _widgetPtr;
        private IntPtr _widget3dPtr;

        /// <summary>
        /// Constructs a WCylinder.
        /// </summary>
        /// <param name="axisPoint1">A point1 on the axis of the cylinder.</param>
        /// <param name="axisPoint2">A point2 on the axis of the cylinder.</param>
        /// <param name="radius">Radius of the cylinder.</param>
        /// <param name="numsides">Resolution of the cylinder.</param>
        /// <param name="color">Color of the cylinder.</param>
        public WCylinder(ref MCvPoint3D64f axisPoint1, MCvPoint3D64f axisPoint2, double radius, int numsides, MCvScalar color)
        {
            _ptr = CvInvoke.cveWCylinderCreate(ref axisPoint1, ref axisPoint2, radius, numsides, ref color, ref _widget3dPtr, ref _widgetPtr);
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
        /// Release the unmanaged memory associated with this WCylinder object
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveWCylinderRelease(ref _ptr);
            _widgetPtr = IntPtr.Zero;
            _widget3dPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// This 3D Widget defines a circle.
    /// </summary>
    public class WCircle : UnmanagedObject, IWidget3D
    {
        private IntPtr _widgetPtr;
        private IntPtr _widget3dPtr;

        /// <summary>
        /// Constructs default planar circle centred at origin with plane normal along z-axis.
        /// </summary>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="thickness">Thickness of the circle.</param>
        /// <param name="color">Color of the circle.</param>
        public WCircle(double radius, double thickness, MCvScalar color)
        {
            _ptr = CvInvoke.cveWCircleCreateAtOrigin(radius, thickness, ref color, ref _widget3dPtr, ref _widgetPtr);
        }

        /// <summary>
        /// Constructs repositioned planar circle.
        /// </summary>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="center">Center of the circle.</param>
        /// <param name="normal">Normal of the plane in which the circle lies.</param>
        /// <param name="thickness">Thickness of the circle.</param>
        /// <param name="color">Color of the circle.</param>
        public WCircle(double radius, MCvPoint3D64f center, MCvPoint3D64f normal, double thickness, MCvScalar color)
        {
            _ptr = CvInvoke.cveWCircleCreate(radius, ref center, ref normal, thickness, ref color, ref _widget3dPtr, ref _widgetPtr);
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
        /// Release the unmanaged memory associated with this WCircle object
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveWCircleRelease(ref _ptr);
            _widgetPtr = IntPtr.Zero;
            _widget3dPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// This 3D Widget defines a cone.
    /// </summary>
    public class WCone : UnmanagedObject, IWidget3D
    {
        private IntPtr _widgetPtr;
        private IntPtr _widget3dPtr;

        /// <summary>
        /// Constructs default cone oriented along x-axis with center of its base located at origin.
        /// </summary>
        /// <param name="length">Length of the cone.</param>
        /// <param name="radius">Radius of the cone.</param>
        /// <param name="resolution">Resolution of the cone.</param>
        /// <param name="color">Color of the cone.</param>
        public WCone(double length, double radius, int resolution, MCvScalar color)
        {
            _ptr = CvInvoke.cveWConeCreateAtOrigin(length, radius, resolution, ref color, ref _widget3dPtr, ref _widgetPtr);
        }

        /// <summary>
        /// Constructs repositioned planar cone.
        /// </summary>
        /// <param name="radius">Radius of the cone.</param>
        /// <param name="center">Center of the cone base.</param>
        /// <param name="tip">Tip of the cone.</param>
        /// <param name="resolution">Resolution of the cone.</param>
        /// <param name="color">Color of the cone.</param>
        public WCone(double radius, MCvPoint3D64f center, MCvPoint3D64f tip, int resolution, MCvScalar color)
        {
            _ptr = CvInvoke.cveWConeCreate(radius, ref center, ref tip, resolution, ref color, ref _widget3dPtr, ref _widgetPtr);
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
        /// Release the unmanaged memory associated with this WCone object
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveWConeRelease(ref _ptr);
            _widgetPtr = IntPtr.Zero;
            _widget3dPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// This 3D Widget defines an arrow.
    /// </summary>
    public class WArrow : UnmanagedObject, IWidget3D
    {
        private IntPtr _widgetPtr;
        private IntPtr _widget3dPtr;

        /// <summary>
        /// Constructs an WArrow.
        /// </summary>
        /// <param name="pt1">Start point of the arrow.</param>
        /// <param name="pt2">End point of the arrow.</param>
        /// <param name="thickness">Thickness of the arrow. Thickness of arrow head is also adjusted accordingly.</param>
        /// <param name="color">Color of the arrow.</param>
        public WArrow(MCvPoint3D64f pt1, MCvPoint3D64f pt2, double thickness, MCvScalar color)
        {
            _ptr = CvInvoke.cveWArrowCreate(ref pt1, ref pt2, thickness, ref color, ref _widget3dPtr, ref _widgetPtr);
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
        /// Release the unmanaged memory associated with this WArrow object
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveWArrowRelease(ref _ptr);
            _widgetPtr = IntPtr.Zero;
            _widget3dPtr = IntPtr.Zero;
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


        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWTextCreate(IntPtr text, ref Point pos, int fontSize, ref MCvScalar color, ref IntPtr widget2D, ref IntPtr widget);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWTextRelease(ref IntPtr text);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWCoordinateSystemCreate(double scale, ref IntPtr widget3d, ref IntPtr widget);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWCoordinateSystemRelease(ref IntPtr system);

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


        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWCubeCreate(
           ref MCvPoint3D64f minPoint, ref MCvPoint3D64f maxPoint,
           [MarshalAs(CvInvoke.BoolMarshalType)]
         bool wireFrame, ref MCvScalar color,
           ref IntPtr widget3d, ref IntPtr widget);
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWCubeRelease(ref IntPtr cube);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWCylinderCreate(ref MCvPoint3D64f axisPoint1, ref MCvPoint3D64f axisPoint2, double radius, int numsides, ref MCvScalar color, ref IntPtr widget3d, ref IntPtr widget);
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWCylinderRelease(ref IntPtr cylinder);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWCircleCreateAtOrigin(double radius, double thickness, ref MCvScalar color, ref IntPtr widget3d, ref IntPtr widget);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWCircleCreate(double radius, ref MCvPoint3D64f center, ref MCvPoint3D64f normal, double thickness, ref MCvScalar color, ref IntPtr widget3d, ref IntPtr widget);
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWCircleRelease(ref IntPtr circle);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWConeCreateAtOrigin(double length, double radius, int resolution, ref MCvScalar color, ref IntPtr widget3d, ref IntPtr widget);
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWConeCreate(double radius, ref MCvPoint3D64f center, ref MCvPoint3D64f tip, int resolution, ref MCvScalar color, ref IntPtr widget3d, ref IntPtr widget);
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWConeRelease(ref IntPtr cone);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWArrowCreate(ref MCvPoint3D64f pt1, ref MCvPoint3D64f pt2, double thickness, ref MCvScalar color, ref IntPtr widget3d, ref IntPtr widget);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWArrowRelease(ref IntPtr arrow);
    }
}
#endif