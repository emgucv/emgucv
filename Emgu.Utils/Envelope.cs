using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Emgu.Data
{
    /// <summary>
    /// An envelope that contains the maximum and minimum value
    /// </summary>
    [XmlRoot("Envelop")]
    public class Envelope<T> where T: IComparable, new()
    {
        private T _max;
        private T _min;

        /// <summary>
        /// Create an envelope with default value
        /// </summary>
        public Envelope() 
            : this(new T(), new T()) 
        { 
        }

        /// <summary>
        /// Create an envelope with specific data
        /// </summary>
        /// <param name="min">The lower bound</param>
        /// <param name="max">The higher bound</param>
        public Envelope(T min, T max)
        {
            _min = min; 
            _max = max;
            Debug.Assert(min.CompareTo(max) <= 0);
        }

        /// <summary>
        /// The maximum value of the envelop
        /// </summary>
        [XmlAttribute("Max")]
        public T Max { get { return _max; } set { _max = value; } }

        /// <summary>
        /// The minimum value of the envelope
        /// </summary>
        [XmlAttribute("Min")]
        public T Min { get { return _min; } set { _min = value; } }

        /// <summary>
        /// Returns true if the value is in the envelope
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>true if the value is in the envelope</returns>
        public bool InRange(T value)
        {
            return (_max.CompareTo(value) >= 0) && (_min.CompareTo(value) <= 0);
        }
    }

    /// <summary>
    /// An Envelope that contains the maximum, minimun and mean
    /// </summary>
    [XmlRoot("Envelop3M")]
    public class Envelope3M<T> : Envelope<T> where T: IComparable, new()
    {
        private T _mean;

        /// <summary>
        /// The mean value of the envelop
        /// </summary>
        [XmlAttribute("Mean")]
        public T Mean { get { return _mean; } set { _mean = value; } }

        /// <summary>
        /// Create an envelope with default value
        /// </summary>
        public Envelope3M() : this(new T(), new T(), new T()) { }

        /// <summary>
        /// Create an envelope with the specific data
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="mean">The mean value</param>
        /// <param name="max">The maximun value</param>
        public Envelope3M(T min, T mean, T max)
            : base(min, max)
        {
            _mean = mean;
        }

        /// <summary>
        /// Add two envelops together and return the sum
        /// </summary>
        /// <param name="e1">The first envelop</param>
        /// <param name="e2">The second envelop</param>
        /// <returns>The sum of the two envelop</returns>
        public static Envelope3M<T> operator +(Envelope3M<T> e1, Envelope3M<T> e2)
        {
            Utils.Func<T, T, T> conv = delegate(T v1, T v2) { return (T) System.Convert.ChangeType(System.Convert.ToDouble(v1) + System.Convert.ToDouble(v2), typeof(T)); };

            return new Envelope3M<T>( conv(e1.Min, e2.Min), conv(e1.Mean, e2.Mean), conv(e1.Max, e2.Max));
        }
    }
}
