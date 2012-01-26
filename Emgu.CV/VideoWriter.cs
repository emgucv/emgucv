//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using Emgu.CV.Util;

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
      /// <param name="width">the width of the frame</param>
      /// <param name="height">the height of the frame</param>
      /// <param name="isColor">true if this is a color video, false otherwise</param>
      public VideoWriter(String fileName, int fps, int width, int height, bool isColor)
         : this(fileName, 
         /*Emgu.Util.Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Windows ? -1 :*/ CvInvoke.CV_FOURCC('I', 'Y', 'U', 'V'), 
         fps, width, height, isColor)
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
      /// <param name="width">the width of the frame</param>
      /// <param name="height">the height of the frame</param>
      /// <param name="isColor">true if this is a color video, false otherwise</param>
      public VideoWriter(String fileName, int compressionCode, int fps, int width, int height, bool isColor)
      {
         _ptr = /*CvToolbox.HasFFMPEG?
            CvInvoke.cvCreateVideoWriter_FFMPEG(fileName, compressionCode, fps, new System.Drawing.Size(width, height), isColor):*/
            CvInvoke.cvCreateVideoWriter(fileName, compressionCode, fps, new System.Drawing.Size(width, height), isColor);

         if (_ptr == IntPtr.Zero)
           throw new NullReferenceException("Unable to create VideoWriter. Make sure you have the specific codec installed");
      }

      /// <summary>
      /// Write a single frame to the video writer
      /// </summary>
      /// <typeparam name="TColor">The color type of the frame</typeparam>
      /// <typeparam name="TDepth">The depth of the frame</typeparam>
      /// <param name="frame">The frame to be written to the video writer</param>
      public void WriteFrame<TColor, TDepth>(Image<TColor, TDepth> frame)
         where TColor : struct, IColor
         where TDepth : new()
      {
         bool success = /*CvToolbox.HasFFMPEG ?
            CvInvoke.cvWriteFrame_FFMPEG(_ptr, frame.Ptr) :*/
            CvInvoke.cvWriteFrame(_ptr, frame.Ptr);
         if (!success) throw new InvalidOperationException("Unable to write frame to the video writer");
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
         CvInvoke.cvReleaseVideoWriter(ref _ptr);
      }
   }
}
