//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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
        /// The type of termination criteria
        /// </summary>
        public CvEnum.TermCritType Type;

        /// <summary>
        /// Maximum iteration
        /// </summary>
        public int MaxCount;

        /// <summary>
        /// The desired accuracy of change in parameters at which the iterative algorithm stops.
        /// </summary>
        public double Epsilon;

        /// <summary>
        /// Create the termination criteria using the constrain of maximum iteration
        /// </summary>
        /// <param name="maxIteration">The maximum number of iteration allowed</param>
        public MCvTermCriteria(int maxIteration)
        {
            MaxCount = maxIteration;
            Epsilon = 0.0;
            Type = CvEnum.TermCritType.MaxIter;
        }

        /// <summary>
        /// Create the termination Criteria using only the constrain of epsilon
        /// </summary>
        /// <param name="eps"> The epsilon value</param>
        public MCvTermCriteria(double eps)
        {
            MaxCount = 0;
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
            MaxCount = maxIteration;
            Epsilon = eps;
            Type = CvEnum.TermCritType.Eps | CvEnum.TermCritType.MaxIter;
        }
    }
}
