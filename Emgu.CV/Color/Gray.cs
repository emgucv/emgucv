using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    ///<summary> Defines a Gray color </summary>
    [ColorInfo(ConversionCodeName = "GRAY")]
    public class Gray : ColorType, IComparable
    {
        ///<summary> Create a Gray color with the given intensity</summary>
        ///<param name="intensity"> The intensity for this color </param>
        public Gray(double intensity) : base(1) { _coordinate[0] = intensity; }

        ///<summary> Create a Gray color with o intensity </summary>
        public Gray() : base(1) { }

        ///<summary> The intensity of the gray color </summary>
        ///<value> The intensity of the gray color</value>
        public double Intensity { get { return _coordinate[0]; } set { _coordinate[0] = value; } }

        /// <summary>
        /// Compare method that implement IComparable interface
        /// </summary>
        /// <param name="obj">The other object to compare to</param>
        /// <returns>a positive value if greater, 0 if equal, negative value if smaller</returns>
        public virtual int CompareTo(System.Object obj)
        {
            Gray gray = obj as Gray;
            if (gray != null)
            {
                return Intensity.CompareTo( gray.Intensity);
            }
            throw new ArgumentException("object is not Gray");
        }
    }
}
