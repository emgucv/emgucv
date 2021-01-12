//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;

namespace Emgu.CV
{
    public partial class CvInvoke
    {
        /// <summary>
        /// Creates a window which can be used as a placeholder for images and trackbars. Created windows are reffered by their names. 
        /// If the window with such a name already exists, the function does nothing.
        /// </summary>
        /// <param name="name">Name of the window which is used as window identifier and appears in the window caption</param>
        /// <param name="flags">Flags of the window.</param>
        public static void NamedWindow(String name, CvEnum.WindowFlags flags = WindowFlags.AutoSize)
        {
            using (CvString s = new CvString(name))
                cveNamedWindow(s, flags);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveNamedWindow(IntPtr name, CvEnum.WindowFlags flags);

        /// <summary>
        /// Changes parameters of a window dynamically.
        /// </summary>
        /// <param name="name">Name of the window.</param>
        /// <param name="propId">Window property to edit.</param>
        /// <param name="propValue">New value of the window property.</param>
        public static void SetWindowProperty(String name, CvEnum.WindowPropertyFlags propId, double propValue)
        {
            using (CvString s = new CvString(name))
                cveSetWindowProperty(s, propId, propValue);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSetWindowProperty(IntPtr name, CvEnum.WindowPropertyFlags propId, double propValue);

        /// <summary>
        /// Provides parameters of a window.
        /// </summary>
        /// <param name="name">Name of the window.</param>
        /// <param name="propId">Window property to retrieve.</param>
        /// <returns>Value of the window property</returns>
        public static double GetWindowProperty(String name, CvEnum.WindowPropertyFlags propId)
        {
            using (CvString s = new CvString(name))
                return cveGetWindowProperty(s, propId);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cveGetWindowProperty(IntPtr winname, CvEnum.WindowPropertyFlags propId);

        /// <summary>
        /// Updates window title
        /// </summary>
        /// <param name="winname">Name of the window.</param>
        /// <param name="title">New title.</param>
        public static void SetWindowTitle(String winname, String title)
        {
            using (CvString s = new CvString(winname))
            using (CvString sTitle = new CvString(title))
                cveSetWindowTitle(s, sTitle);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSetWindowTitle(IntPtr winname, IntPtr title);

        /// <summary>
        /// Waits for key event infinitely (delay &lt;= 0) or for "delay" milliseconds. 
        /// </summary>
        /// <param name="delay">Delay in milliseconds.</param>
        /// <returns>The code of the pressed key or -1 if no key were pressed until the specified timeout has elapsed</returns>
        public static int WaitKey(int delay = 0)
        {
            return cveWaitKey(delay);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern int cveWaitKey(int delay);

        /// <summary>
        /// Shows the image in the specified window
        /// </summary>
        /// <param name="name">Name of the window</param>
        /// <param name="image">Image to be shown</param>
        public static void Imshow(String name, IInputArray image)
        {
            using (CvString s = new CvString(name))
            using (InputArray iaImage = image.GetInputArray())
            {
                cveImshow(s, iaImage);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveImshow(IntPtr name, IntPtr image);

        /// <summary>
        /// Destroys the window with a given name
        /// </summary>
        /// <param name="name">Name of the window to be destroyed</param>
        public static void DestroyWindow(String name)
        {
            using (CvString s = new CvString(name))
                cveDestroyWindow(s);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDestroyWindow(IntPtr name);

        /// <summary>
        /// Destroys all of the HighGUI windows.
        /// </summary>
        public static void DestroyAllWindows()
        {
            cveDestroyAllWindows();
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDestroyAllWindows();

        /// <summary>
        /// Selects ROI on the given image. Function creates a window and allows user to select a ROI using mouse. Controls: use space or enter to finish selection, use key c to cancel selection (function will return the zero cv::Rect).
        /// </summary>
        /// <param name="windowName"> Name of the window where selection process will be shown.</param>
        /// <param name="img"> Image to select a ROI.</param>
        /// <param name="showCrosshair"> If true crosshair of selection rectangle will be shown.</param>
        /// <param name="fromCenter"> If true center of selection will match initial mouse position. In opposite case a corner of selection rectangle will correspont to the initial mouse position.</param>
        /// <returns> Selected ROI or empty rect if selection canceled.</returns>
        public static Rectangle SelectROI(String windowName, IInputArray img, bool showCrosshair = true, bool fromCenter = false)
        {
            Rectangle roi = new Rectangle();
            using (CvString csWindowName = new CvString(windowName))
            using (InputArray iaImg = img.GetInputArray())
            {
                cveSelectROI(csWindowName, iaImg, showCrosshair, fromCenter, ref roi);
            }

            return roi;
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSelectROI(
            IntPtr windowName,
            IntPtr img,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool showCrosshair,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool fromCenter,
            ref Rectangle roi);

        /// <summary>
        /// Selects ROIs on the given image. Function creates a window and allows user to select a ROIs using mouse. Controls: use space or enter to finish current selection and start a new one, use esc to terminate multiple ROI selection process.
        /// </summary>
        /// <param name="windowName"> Name of the window where selection process will be shown.</param>
        /// <param name="img"> Image to select a ROI.</param>
        /// <param name="showCrosshair"> If true crosshair of selection rectangle will be shown.</param>
        /// <param name="fromCenter"> If true center of selection will match initial mouse position. In opposite case a corner of selection rectangle will correspont to the initial mouse position.</param>
        /// <returns> Selected ROIs.</returns>
        public static Rectangle[] SelectROIs(
            String windowName, 
            IInputArray img, 
            bool showCrosshair = true,
            bool fromCenter = false)
        {
            using (VectorOfRect vr = new VectorOfRect())
            using (CvString csWindowName = new CvString(windowName))
            using (InputArray iaImg = img.GetInputArray())
            {
                cveSelectROIs(csWindowName, iaImg, vr, showCrosshair, fromCenter);
                return vr.ToArray();
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSelectROIs(
            IntPtr windowName,
            IntPtr img,
            IntPtr boundingBoxs,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool showCrosshair,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool fromCenter);

    }
}
