using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    /// <summary> The Image which contains time stamp which specified what time this image is created </summary>
    public class TimedImage<C, D> : Image<C, D> where C : ColorType, new()
    {
        private System.DateTime _timestamp;

        ///<summary>
        ///Create a blank Image of size one by one
        ///</summary>
        public TimedImage()
            : base()
        {
            _timestamp = DateTime.Now;
        }

        ///<summary>
        ///Create a blank Image of the specified width, height, depth and color.
        ///</summary>
        ///<param name="width">The width of the image</param>
        ///<param name="height">The height of the image</param>
        ///<param name="value">The initial color of the image</param>
        public TimedImage(int width, int height, C value)
            : base(width, height, value)
        {
            _timestamp = DateTime.Now;
        }

        ///<summary>
        ///Create a blank Image of the specified width, height, depth. 
        ///<b>Warning</b>: The color is not initialized and could be random value 
        ///</summary>
        ///<param name="width">The width of the image</param>
        ///<param name="height">The height of the image</param>
        public TimedImage(int width, int height)
            : base(width, height)
        {
            _timestamp = DateTime.Now;
        }

        ///<summary> 
        ///The time this image is captured
        ///</summary>
        public System.DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }
    };
}
