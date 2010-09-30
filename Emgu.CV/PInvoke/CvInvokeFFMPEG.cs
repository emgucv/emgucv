using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;

namespace Emgu.CV
{
   /// <summary>
   /// Library to invoke OpenCV functions
   /// </summary>
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
      [DllImport(OPENCV_FFMPEG_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateVideoWriter_FFMPEG(
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
      public static IntPtr cvCreateVideoWriter_FFMPEG(
         String filename,
         int fourcc,
         double fps,
         System.Drawing.Size frameSize,
         bool isColor)
      {
         return cvCreateVideoWriter_FFMPEG(filename, fourcc, fps, frameSize, isColor ? 1 : 0);
      }

      /// <summary>
      /// Finishes writing to video file and releases the structure.
      /// </summary>
      /// <param name="writer">pointer to video file writer structure</param>
      [DllImport(OPENCV_FFMPEG_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseVideoWriter_FFMPEG(ref IntPtr writer);

      /// <summary>
      /// Writes/appends one frame to video file.
      /// </summary>
      /// <param name="writer">video writer structure.</param>
      /// <param name="image">the written frame</param>
      /// <returns></returns>
      [DllImport(OPENCV_FFMPEG_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvWriteFrame_FFMPEG(IntPtr writer, IntPtr image);

   }
}
