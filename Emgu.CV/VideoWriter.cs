using System;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// Create a video writer that write images to video format
   /// </summary>
   public class VideoWriter : UnmanagedObject
   {
      /// <summary>
      /// Create a video writer using the specific information
      /// </summary>
      /// <param name="fileName">The name of the video file to be written to </param>
      /// <param name="fps">frame rate per second</param>
      /// <param name="width">the width of the frame</param>
      /// <param name="height">the height of the frame</param>
      /// <param name="isColor">true if this is a color video, false otherwise</param>
      public VideoWriter(String fileName, int fps, int width, int height, bool isColor)
         : this(fileName, CvInvoke.CV_FOURCC('M', 'J', 'P', 'G'), fps, width, height, isColor)
      {
      }

      /// <summary>
      /// Create a video writer using the specific information
      /// </summary>
      /// <param name="fileName">The name of the video file to be written to </param>
      /// <param name="compressionCode">compression code</param>
      /// <param name="fps">frame rate per second</param>
      /// <param name="width">the width of the frame</param>
      /// <param name="height">the height of the frame</param>
      /// <param name="isColor">true if this is a color video, false otherwise</param>
      public VideoWriter(String fileName, int compressionCode, int fps, int width, int height, bool isColor)
      {
         _ptr = CvInvoke.cvCreateVideoWriter(fileName, compressionCode, fps, new System.Drawing.Size(width, height), isColor);
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
         CvInvoke.cvWriteFrame(_ptr, frame.Ptr);
      }

      /// <summary>
      /// Release the video writer and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.cvReleaseVideoWriter(ref _ptr);
      }
   }
}
