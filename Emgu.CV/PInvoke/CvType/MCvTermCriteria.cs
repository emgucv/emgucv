//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
    /// <summary>
    /// Managed structure equivalent to CvTermCriteria
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvTermCriteria
    {
        /// <summary>
        /// CV_TERMCRIT value
        /// </summary>
        public CvEnum.TermCritType Type;

        /// <summary>
        /// Maximum iteration
        /// </summary>
        public int MaxIter;

        /// <summary>
        /// Epsilon
        /// </summary>
        public double Epsilon;

        /// <summary>
        /// Create the termination criteria using the constrain of maximum iteration
        /// </summary>
        /// <param name="maxIteration">The maximum number of iteration allowed</param>
        public MCvTermCriteria(int maxIteration)
        {
            MaxIter = maxIteration;
            Epsilon = 0.0;
            Type = CvEnum.TermCritType.Iter;
        }

        /// <summary>
        /// Create the termination Criteria using only the constrain of epsilon
        /// </summary>
        /// <param name="eps"> The epsilon value</param>
        public MCvTermCriteria(double eps)
        {
            MaxIter = 0;
            Epsilon = eps;
            Type = CvEnum.TermCritType.Eps;
        }

        /// <summary>
        /// Create the termination criteria using the constrain of maximum iteration as well as epsilon
        /// </summary>
        /// <param name="maxIteration">The maximum number of iteration allowed</param>
        /// <param name="eps">The epsilon value</param>
        public MCvTermCriteria(int maxIteration, double eps)
        {
            MaxIter = maxIteration;
            Epsilon = eps;
            Type = CvEnum.TermCritType.Eps | CvEnum.TermCritType.Iter;
        }
    }
}
