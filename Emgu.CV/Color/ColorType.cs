using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    ///<summary>
    /// A color type
    ///</summary>
    public abstract class ColorType : Point<double>
    {
        /// <summary>
        /// Create a color type of certain dimension
        /// </summary>
        /// <param name="dimension"></param>
        public ColorType(int dimension) : base(dimension) { }

        /// <summary>
        /// The equivalent of MCvScalar value
        /// </summary>
        public MCvScalar MCvScalar
        {
            get
            {
                double[] v = Resize(4).Coordinate;
                return new MCvScalar(v[0], v[1], v[2], v[3]);
            }
            set
            {
                int size = Math.Min(Dimension, 4);
                for (int i = 0; i < size; i++)
                    _coordinate[i] = value.v[i];
            }
        }

        
        /// <summary>
        /// To be removed in the next version
        /// </summary>
        [Obsolete("To be removed in the next version, use MCvScalar property instead")]
        public MCvScalar CvScalar
        {
            get { return MCvScalar; }
            set { MCvScalar = value; }
        }
    }
}
