//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

/*
using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      /// <summary>
      /// Creates video writer structure.
      /// </summary>
      /// <param name="filename">Name of the output video file.</param>
      /// <param name="fourcc">4-character code of codec used to compress the frames. For example, CV_FOURCC('P','I','M','1') is MPEG-1 codec, CV_FOURCC('M','J','P','G') is motion-jpeg codec etc.</param>
      /// <param name="fps">Framerate of the created video stream. </param>
      /// <param name="frameSize">Size of video frames.</param>
      /// <param name="isColor">If != 0, the encoder will expect and encode color frames, otherwise it will work with grayscale frames </param>
      /// <returns>The video writer</returns>
      [DllImport(OpencvFfmpegLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateVideoWriter_FFMPEG(
          [MarshalAs(StringMarshalType)] String filename,
          int fourcc,
          double fps,
          Size frameSize,
          [MarshalAs(CvInvoke.BoolToIntMarshalType)]
          bool isColor);

      /// <summary>
      /// Finishes writing to video file and releases the structure.
      /// </summary>
      /// <param name="writer">pointer to video file writer structure</param>
      [DllImport(OpencvFfmpegLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseVideoWriter_FFMPEG(ref IntPtr writer);

      /// <summary>
      /// Writes/appends one frame to video file.
      /// </summary>
      /// <param name="writer">video writer structure.</param>
      /// <param name="image">the written frame</param>
      /// <returns>True on success, false otherwise</returns>
      [DllImport(OpencvFfmpegLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolToIntMarshalType)]
      public static extern bool cvWriteFrame_FFMPEG(IntPtr writer, IntPtr image);

      /// <summary>
      /// Create capture from file
      /// </summary>
      /// <param name="filename">The name of the file</param>
      /// <returns>Pointer to the cvCapture structure</returns>
      [DllImport(OpencvFfmpegLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateFileCapture_FFMPEG([MarshalAs(StringMarshalType)] String filename);

      /// <summary>
      /// Release the capture
      /// </summary>
      /// <param name="capture">The capture to be released</param>
      [DllImport(OpencvFfmpegLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseCapture_FFMPEG(ref IntPtr capture);

      /// <summary>
      /// Grab a frame
      /// </summary>
      /// <param name="capture">Video capturing structure</param>
      /// <returns>True on success</returns>
      [DllImport(OpencvFfmpegLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolToIntMarshalType)]
      public static extern bool cvGrabFrame_FFMPEG(IntPtr capture);

      /// <summary>
      /// Get the frame grabbed with cvGrabFrame(..)
      /// This function may apply some frame processing like frame decompression, flipping etc.
      /// </summary>
      /// <param name="capture">Video capturing structure</param>
      /// <param name="streamIdx">Stream index</param>
      /// <returns>Pointer to the queryed frame</returns>
      /// <remarks>The returned image should not be released or modified by user. </remarks>
      [DllImport(OpencvFfmpegLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvRetrieveFrame_FFMPEG(IntPtr capture, int streamIdx);

      /// <summary>
      /// Retrieves the specified property of camera or video file
      /// </summary>
      /// <param name="capture">Video capturing structure</param>
      /// <param name="prop">Property identifier</param>
      /// <returns>The specified property of camera or video file</returns>
      [DllImport(OpencvFfmpegLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvGetCaptureProperty_FFMPEG(IntPtr capture, CvEnum.CAP_PROP prop);

      /// <summary>
      /// Sets the specified property of video capturing
      /// </summary>
      /// <param name="capture">Video capturing structure</param>
      /// <param name="propertyId">Property identifier</param>
      /// <param name="value">Value of the property</param>
      [DllImport(OpencvFfmpegLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSetCaptureProperty_FFMPEG(IntPtr capture, CvEnum.CAP_PROP propertyId, double value);

   }
}
*/