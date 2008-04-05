using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    ///<summary>
    ///An ellipse
    ///</summary>
    public class Ellipse<T> : Box2D<T> where T : IComparable, new()
    {

        ///<summary> 
        ///Create an ellipse with default parameters
        ///</summary>
        public Ellipse()
            : base()
        {
        }
        ///<summary>
        ///Create an ellipse with specific parameters
        ///</summary>
        ///<param name="center"> The center of the ellipse</param>
        ///<param name="size"> The width and height of the ellipse</param>
        ///<param name="angle"> The rotation angle in radian for the ellipse</param>
        public Ellipse(Point2D<T> center, Point2D<T> size, double angle)
            : base(center, size, angle)
        {
        }
    };
}
