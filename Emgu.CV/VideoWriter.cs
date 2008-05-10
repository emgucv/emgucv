using System;
using System.Collections.Generic;
using System.Text;

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
        /// <param name="filename">The name of the video file to be written to </param>
        /// <param name="fps">frame rate per second</param>
        /// <param name="frameSize">the size of the frame</param>
        /// <param name="isColor">true if this is a color video, false otherwise</param>
        public VideoWriter(String filename, int fps, Point2D<int> frameSize, bool isColor)
        {
            _ptr = CvInvoke.cvCreateVideoWriter(filename, CvInvoke.CV_FOURCC('P', 'I', 'M', '1'), fps, new MCvSize(frameSize.X, frameSize.Y), isColor);  
        }

        /// <summary>
        /// Write a single frame to the video writer
        /// </summary>
        /// <typeparam name="C">The color type of the frame</typeparam>
        /// <typeparam name="D">The depth of the frame</typeparam>
        /// <param name="frame">The frame to be written to the video writer</param>
        public void WriteFrame<C,D>(Image<C, D> frame) 
            where C: ColorType, new()
            where D: new()
        {
            CvInvoke.cvWriteFrame(_ptr, frame.Ptr);
        }

        /// <summary>
        /// Release the video writer and all the memory associate with it
        /// </summary>
        protected override void FreeUnmanagedObjects()
        {
            CvInvoke.cvReleaseVideoWriter(ref _ptr);
        }
    }
}
