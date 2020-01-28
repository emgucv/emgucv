//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// Random Number Generator.
    /// </summary>
    public class RNG : UnmanagedObject
    {
        /// <summary>
        /// Distribution type
        /// </summary>
        public enum DistType
        {
            /// <summary>
            /// Uniform distribution
            /// </summary>
            Uniform = 0,
            /// <summary>
            /// Normal distribution
            /// </summary>
            Normal = 1
        };

        /// <summary>
        /// Create a Random Number Generator.
        /// </summary>
        public RNG()
        {
            _ptr = CvInvoke.cveRngCreate();
        }

        /// <summary>
        /// Create a Random Number Generator using a seed.
        /// </summary>
        /// <param name="state">64-bit value used to initialize the RNG</param>
        public RNG(UInt64 state)
        {
            _ptr = CvInvoke.cveRngCreateWithSeed(state);
        }

        /// <summary>
        /// Release the unmanaged resources
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                CvInvoke.cveRngRelease(ref _ptr);
        }

        /// <summary>
        /// Fills arrays with random numbers.
        /// </summary>
        /// <param name="mat">2D or N-dimensional matrix; currently matrices with more than 4 channels are not supported by the methods, use Mat::reshape as a possible workaround.</param>
        /// <param name="distType">distribution type</param>
        /// <param name="a">First distribution parameter; in case of the uniform distribution, this is an inclusive lower boundary, in case of the normal distribution, this is a mean value.</param>
        /// <param name="b">Second distribution parameter; in case of the uniform distribution, this is a non-inclusive upper boundary, in case of the normal distribution, this is a standard deviation (diagonal of the standard deviation matrix or the full standard deviation matrix).</param>
        /// <param name="saturateRange">Pre-saturation flag; for uniform distribution only; if true, the method will first convert a and b to the acceptable value range (according to the mat datatype) and then will generate uniformly distributed random numbers within the range [saturate(a), saturate(b)), if saturateRange=false, the method will generate uniformly distributed random numbers in the original range [a, b) and then will saturate them</param>
        public void Fill(
            IInputOutputArray mat,
            DistType distType,
            IInputArray a,
            IInputArray b,
            bool saturateRange = false)
        {
            using (InputArray iaA = a.GetInputArray())
            using (InputArray iaB = b.GetInputArray())
            using (InputOutputArray ioaMat = mat.GetInputOutputArray())
            {
                CvInvoke.cveRngFill(_ptr, ioaMat, distType, iaA, iaB, saturateRange);
            }
        }

        /// <summary>
        /// Fills arrays with random numbers.
        /// </summary>
        /// <param name="mat">2D or N-dimensional matrix; currently matrices with more than 4 channels are not supported by the methods, use Mat::reshape as a possible workaround.</param>
        /// <param name="distType">distribution type</param>
        /// <param name="a">First distribution parameter; in case of the uniform distribution, this is an inclusive lower boundary, in case of the normal distribution, this is a mean value.</param>
        /// <param name="b">Second distribution parameter; in case of the uniform distribution, this is a non-inclusive upper boundary, in case of the normal distribution, this is a standard deviation (diagonal of the standard deviation matrix or the full standard deviation matrix).</param>
        /// <param name="saturateRange">Pre-saturation flag; for uniform distribution only; if true, the method will first convert a and b to the acceptable value range (according to the mat datatype) and then will generate uniformly distributed random numbers within the range [saturate(a), saturate(b)), if saturateRange=false, the method will generate uniformly distributed random numbers in the original range [a, b) and then will saturate them</param>
        public void Fill(
            IInputOutputArray mat,
            DistType distType,
            MCvScalar a,
            MCvScalar b,
            bool saturateRange = false)
        {
            using (ScalarArray saA = new ScalarArray(a))
            using (ScalarArray saB = new ScalarArray(b) )
            {
                Fill(mat, distType, saA, saB, saturateRange);
            }
        }

        /// <summary>
        /// Returns the next random number sampled from the Gaussian distribution.
        /// </summary>
        /// <param name="sigma">standard deviation of the distribution.</param>
        /// <returns>Returns the next random number from the Gaussian distribution N(0,sigma) . That is, the mean value of the returned random numbers is zero and the standard deviation is the specified sigma .</returns>
        public double Gaussian(double sigma)
        {
            return CvInvoke.cveRngGaussian(_ptr, sigma);
        }

        /// <summary>
        /// The method updates the state using the MWC algorithm and returns the next 32-bit random number.
        /// </summary>
        /// <returns>The next 32-bit random number</returns>
        public UInt32 Next()
        {
            return CvInvoke.cveRngNext(_ptr);
        }

        /// <summary>
        /// Returns uniformly distributed integer random number from [a,b) range
        /// </summary>
        /// <param name="a">Lower inclusive boundary of the returned random number.</param>
        /// <param name="b">Upper non-inclusive boundary of the returned random number.</param>
        /// <returns>Uniformly distributed integer random number from [a,b) range</returns>
        public int Uniform(int a, int b)
        {
            return CvInvoke.cveRngUniformInt(_ptr, a, b);
        }

        /// <summary>
        /// Returns uniformly distributed random float number from [a,b) range
        /// </summary>
        /// <param name="a">Lower inclusive boundary of the returned random number.</param>
        /// <param name="b">Upper non-inclusive boundary of the returned random number.</param>
        /// <returns>Uniformly distributed random float number from [a,b) range</returns>
        public float Uniform(float a, float b)
        {
            return CvInvoke.cveRngUniformFloat(_ptr, a, b);
        }

        /// <summary>
        /// Returns uniformly distributed random double number from [a,b) range
        /// </summary>
        /// <param name="a">Lower inclusive boundary of the returned random number.</param>
        /// <param name="b">Upper non-inclusive boundary of the returned random number.</param>
        /// <returns>Uniformly distributed random double number from [a,b) range</returns>
        public double Uniform(double a, double b)
        {
            return CvInvoke.cveRngUniformDouble(_ptr, a, b);
        }
    }

    public static partial class CvInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        static internal extern IntPtr cveRngCreate();
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        static internal extern IntPtr cveRngCreateWithSeed(UInt64 state);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        static internal extern void cveRngFill(IntPtr rng, IntPtr mat, RNG.DistType distType, IntPtr a, IntPtr b, bool saturateRange);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        static internal extern double cveRngGaussian(IntPtr rng, double sigma);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        static internal extern UInt32 cveRngNext(IntPtr rng);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        static internal extern int cveRngUniformInt(IntPtr rng, int a, int b);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        static internal extern float cveRngUniformFloat(IntPtr rng, float a, float b);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        static internal extern double cveRngUniformDouble(IntPtr rng, double a, double b);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        static internal extern void cveRngRelease(ref IntPtr rng);
    }

}
