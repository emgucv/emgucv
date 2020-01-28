//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV
{
    /// <summary> 
    /// The Image which contains time stamp which specified what time this image is created 
    /// </summary>
    /// <typeparam name="TColor">The color of this map</typeparam>
    /// <typeparam name="TDepth">The depth of this map</typeparam>
    [Serializable]
    public class TimedImage<TColor, TDepth>
      : Image<TColor, TDepth>
      where TColor : struct, IColor
      where TDepth : new()
    {
        private DateTime _timestamp;

        /// <summary>
        /// Create a empty Image 
        /// </summary>
        protected TimedImage()
           : base()
        {
            _timestamp = DateTime.Now;
        }

        /// <summary>
        /// Create a blank Image of the specified width, height, depth and color.
        /// </summary>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        /// <param name="value">The initial color of the image</param>
        public TimedImage(int width, int height, TColor value)
           : base(width, height, value)
        {
            _timestamp = DateTime.Now;
        }

        /// <summary>
        /// Create an empty Image of the specified width and height
        /// </summary>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        public TimedImage(int width, int height)
           : base(width, height)
        {
            _timestamp = DateTime.Now;
        }

        /// <summary> 
        /// The time this image is captured
        /// </summary>
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }
    }
}
