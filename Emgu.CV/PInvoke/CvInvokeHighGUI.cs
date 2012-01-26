//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      /// <summary>
      /// Allocates and initialized the CvCapture structure for reading a video stream from the camera. Currently two camera interfaces can be used on Windows: Video for Windows (VFW) and Matrox Imaging Library (MIL); and two on Linux: V4L and FireWire (IEEE1394). 
      /// </summary>
      /// <param name="index">Index of the camera to be used. If there is only one camera or it does not matter what camera to use -1 may be passed</param>
      /// <returns>Pointer to the capture structure</returns>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateCameraCapture(int index);

      /// <summary>
      /// Allocates and initialized the CvCapture structure for reading the video stream from the specified file. 
      ///After the allocated structure is not used any more it should be released by cvReleaseCapture function. 
      /// </summary>
      /// <param name="filename">Name of the video file.</param>
      /// <returns>Pointer to the capture structure.</returns>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateFileCapture([MarshalAs(StringMarshalType)] String filename);

      /// <summary>
      /// The function cvReleaseCapture releases the CvCapture structure allocated by cvCreateFileCapture or cvCreateCameraCapture
      /// </summary>
      /// <param name="capture">pointer to video capturing structure.</param>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseCapture(ref IntPtr capture);

      /// <summary>
      /// Grabs a frame from camera or video file, decompresses and returns it. This function is just a combination of cvGrabFrame and cvRetrieveFrame in one call. 
      /// </summary>
      /// <param name="capture">Video capturing structure</param>
      /// <returns>Pointer to the queryed frame</returns>
      /// <remarks>The returned image should not be released or modified by user. </remarks>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvQueryFrame(IntPtr capture);

      /// <summary>
      /// Grab a frame
      /// </summary>
      /// <param name="capture">Video capturing structure</param>
      /// <returns>True on success</returns>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolToIntMarshalType)]
      public static extern bool cvGrabFrame(IntPtr capture);

      /// <summary>
      /// Get the frame grabbed with cvGrabFrame(..)
      /// This function may apply some frame processing like frame decompression, flipping etc.
      /// </summary>
      /// <param name="capture">Video capturing structure</param>
      /// <param name="streamIdx">Stream index</param>
      /// <returns>Pointer to the queryed frame</returns>
      /// <remarks>The returned image should not be released or modified by user. </remarks>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvRetrieveFrame(IntPtr capture, int streamIdx);

      /// <summary>
      /// Retrieves the specified property of camera or video file
      /// </summary>
      /// <param name="capture">Video capturing structure</param>
      /// <param name="prop">Property identifier</param>
      /// <returns>The specified property of camera or video file</returns>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvGetCaptureProperty(IntPtr capture, CvEnum.CAP_PROP prop);

      /// <summary>
      /// Sets the specified property of video capturing
      /// </summary>
      /// <param name="capture">Video capturing structure</param>
      /// <param name="propertyId">Property identifier</param>
      /// <param name="value">Value of the property</param>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSetCaptureProperty(IntPtr capture, CvEnum.CAP_PROP propertyId, double value);

      /// <summary>
      /// Return the type of the capture (eg, CV_CAP_V4W, CV_CAP_UNICAP), which is unknown if created with CV_CAP_ANY
      /// </summary>
      /// <param name="capture">The pointer to CvCapture</param>
      /// <returns>The type of the capture</returns>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvGetCaptureDomain(IntPtr capture);  

      /// <summary>
      /// Loads an image from the specified file and returns the pointer to the loaded image. Currently the following file formats are supported: 
      /// Windows bitmaps - BMP, DIB; 
      /// JPEG files - JPEG, JPG, JPE; 
      /// Portable Network Graphics - PNG; 
      /// Portable image format - PBM, PGM, PPM; 
      /// Sun rasters - SR, RAS; 
      /// TIFF files - TIFF, TIF; 
      /// OpenEXR HDR images - EXR; 
      /// JPEG 2000 images - jp2. 
      /// </summary>
      /// <param name="filename">The name of the file to be loaded</param>
      /// <param name="loadType">The load image type</param>
      /// <returns>The loaded image</returns>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvLoadImage(
         [MarshalAs(StringMarshalType)] String filename,
         CvEnum.LOAD_IMAGE_TYPE loadType);

      /// <summary>
      /// Saves the image to the specified file. The image format is chosen depending on the filename extension, see cvLoadImage. Only 8-bit single-channel or 3-channel (with 'BGR' channel order) images can be saved using this function. If the format, depth or channel order is different, use cvCvtScale and cvCvtColor to convert it before saving, or use universal cvSave to save the image to XML or YAML format
      /// </summary>
      /// <param name="filename">The name of the file to be saved to</param>
      /// <param name="image">The image to be saved</param>
      /// <param name="parameters">The parameters</param>
      /// <returns></returns>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern bool cvSaveImage([MarshalAs(StringMarshalType)] String filename, IntPtr image, IntPtr parameters);

      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cvNamedWindow")]
      private static extern int _cvNamedWindow([MarshalAs(StringMarshalType)] String name, int flags);

      /// <summary>
      /// Creates a window which can be used as a placeholder for images and trackbars. Created windows are reffered by their names. 
      /// If the window with such a name already exists, the function does nothing.
      /// </summary>
      /// <param name="name">Name of the window which is used as window identifier and appears in the window caption</param>
      public static int cvNamedWindow(String name)
      {
         return _cvNamedWindow(name, 1);
      }

      /// <summary>
      /// Waits for key event infinitely (delay &lt;= 0) or for "delay" milliseconds. 
      /// </summary>
      /// <param name="delay">Delay in milliseconds.</param>
      /// <returns>The code of the pressed key or -1 if no key were pressed until the specified timeout has elapsed</returns>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvWaitKey(int delay);

      /// <summary>
      /// Shows the image in the specified window
      /// </summary>
      /// <param name="name">Name of the window</param>
      /// <param name="image">Image to be shown</param>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvShowImage([MarshalAs(StringMarshalType)] String name, IntPtr image);

      /// <summary>
      /// Destroys the window with a given name
      /// </summary>
      /// <param name="name">Name of the window to be destroyed</param>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvDestroyWindow([MarshalAs(StringMarshalType)] String name);

      /// <summary>
      /// Creates video writer structure.
      /// </summary>
      /// <param name="filename">Name of the output video file.</param>
      /// <param name="fourcc">4-character code of codec used to compress the frames. For example, CV_FOURCC('P','I','M','1') is MPEG-1 codec, CV_FOURCC('M','J','P','G') is motion-jpeg codec etc.</param>
      /// <param name="fps">Framerate of the created video stream. </param>
      /// <param name="frameSize">Size of video frames.</param>
      /// <param name="isColor">If != 0, the encoder will expect and encode color frames, otherwise it will work with grayscale frames </param>
      /// <returns>The video writer</returns>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateVideoWriter(
          [MarshalAs(StringMarshalType)] String filename,
          int fourcc,
          double fps,
          System.Drawing.Size frameSize,
          int isColor);

      /// <summary>
      /// Creates video writer structure.
      /// </summary>
      /// <param name="filename">Name of the output video file.</param>
      /// <param name="fourcc">4-character code of codec used to compress the frames. For example, CV_FOURCC('P','I','M','1') is MPEG-1 codec, CV_FOURCC('M','J','P','G') is motion-jpeg codec etc.</param>
      /// <param name="fps">Framerate of the created video stream. </param>
      /// <param name="frameSize">Size of video frames.</param>
      /// <param name="isColor">If it is true, the encoder will expect and encode color frames, otherwise it will work with grayscale frames </param>
      /// <returns>The video writer</returns>
      public static IntPtr cvCreateVideoWriter(String filename,
          int fourcc,
          double fps,
          System.Drawing.Size frameSize,
          bool isColor)
      {
         return cvCreateVideoWriter(filename, fourcc, fps, frameSize, isColor ? 1 : 0);
      }

      /// <summary>
      /// Finishes writing to video file and releases the structure.
      /// </summary>
      /// <param name="writer">pointer to video file writer structure</param>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseVideoWriter(ref IntPtr writer);

      /// <summary>
      /// Writes/appends one frame to video file.
      /// </summary>
      /// <param name="writer">video writer structure.</param>
      /// <param name="image">the written frame</param>
      /// <returns>True on success, false otherwise</returns>
      [DllImport(OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolToIntMarshalType)]
      public static extern bool cvWriteFrame(IntPtr writer, IntPtr image);

   }
}
