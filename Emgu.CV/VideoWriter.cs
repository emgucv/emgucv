//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using Emgu.CV.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// Create a video writer that write images to video format
   /// </summary>
   public class VideoWriter : UnmanagedObject
   {
      
      /// <summary>
      /// Create a video writer using the specific information.
      /// On windows, it will open a codec selection dialog.
      /// On linux, it will use the default codec for the specified filename
      /// </summary>
      /// <param name="fileName">The name of the video file to be written to </param>
      /// <param name="fps">frame rate per second</param>
      /// <param name="size">the size of the frame</param>
      /// <param name="isColor">true if this is a color video, false otherwise</param>
      public VideoWriter(String fileName, int fps, System.Drawing.Size size, bool isColor)
         : this(fileName, 
         /*Emgu.Util.Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Windows ? -1 :*/ Fourcc('I', 'Y', 'U', 'V'), 
         fps, size, isColor)
      {
      }

      /// <summary>
      /// Create a video writer using the specific information
      /// </summary>
      /// <param name="fileName">The name of the video file to be written to </param>
      /// <param name="compressionCode">Compression code. Usually computed using CvInvoke.CV_FOURCC. 
      /// On windows use -1 to open a codec selection dialog.
      /// On Linux, use CvInvoke.CV_FOURCC('I', 'Y', 'U', 'V') for default codec for the specific file name.
      /// </param>
      /// <param name="fps">frame rate per second</param>
      /// <param name="size">the size of the frame</param>
      /// <param name="isColor">true if this is a color video, false otherwise</param>
      public VideoWriter(String fileName, int compressionCode, int fps, System.Drawing.Size size, bool isColor)
      {
         using (CvString s = new CvString(fileName))
         _ptr = /*CvToolbox.HasFFMPEG?
            CvInvoke.cvCreateVideoWriter_FFMPEG(fileName, compressionCode, fps, new System.Drawing.Size(width, height), isColor):*/
            CvInvoke.cveVideoWriterCreate(s, compressionCode, fps, ref size, isColor);

         if (_ptr == IntPtr.Zero)
           throw new NullReferenceException("Unable to create VideoWriter. Make sure you have the specific codec installed");
      }

      /// <summary>
      /// Write a single frame to the video writer
      /// </summary>
      /// <param name="frame">The frame to be written to the video writer</param>
      public void Write(Mat frame)
      {
         CvInvoke.cveVideoWriterWrite(_ptr, frame);
      }

      /// <summary>
      /// Generate 4-character code of codec used to compress the frames. For example, CV_FOURCC('P','I','M','1') is MPEG-1 codec, CV_FOURCC('M','J','P','G') is motion-jpeg codec etc.
      /// </summary>
      /// <param name="c1"></param>
      /// <param name="c2"></param>
      /// <param name="c3"></param>
      /// <param name="c4"></param>
      /// <returns></returns>
      public static int Fourcc(char c1, char c2, char c3, char c4)
      {
         return CvInvoke.cveVideoWriterFourcc(c1, c2, c3, c4);
      }

      /// <summary>
      /// Release the video writer and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         /*
         if (CvToolbox.HasFFMPEG)
            CvInvoke.cvReleaseVideoWriter_FFMPEG(ref _ptr);
         else*/ 
         CvInvoke.cveVideoWriterRelease(ref _ptr);
      }
   }

   public partial class CvInvoke
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
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveVideoWriterCreate(
         IntPtr filename,
         int fourcc,
         double fps,
         ref System.Drawing.Size frameSize,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool isColor);

      /// <summary>
      /// Finishes writing to video file and releases the structure.
      /// </summary>
      /// <param name="writer">pointer to video file writer structure</param>
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveVideoWriterRelease(ref IntPtr writer);

      /// <summary>
      /// Writes/appends one frame to video file.
      /// </summary>
      /// <param name="writer">video writer structure.</param>
      /// <param name="image">the written frame</param>
      /// <returns>True on success, false otherwise</returns>
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveVideoWriterWrite(IntPtr writer, IntPtr image);

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cveVideoWriterFourcc(char c1, char c2, char c3, char c4);
   }
}
