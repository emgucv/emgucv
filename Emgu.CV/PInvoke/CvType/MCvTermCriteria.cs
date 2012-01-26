//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvTermCriteria
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvTermCriteria
   {
      /// <summary>
      /// CV_TERMCRIT value
      /// </summary>
      public CvEnum.TERMCRIT type;

      /// <summary>
      /// Maximum iteration
      /// </summary>
      public int max_iter;

      /// <summary>
      /// Epsilon
      /// </summary>
      public double epsilon;

      ///<summary>
      ///Create the termination criteria using the constrain of maximum iteration
      ///</summary>
      ///<param name="maxIteration">The maximum number of iteration allowed</param>
      public MCvTermCriteria(int maxIteration)
      {
         max_iter = maxIteration;
         epsilon = 0.0;
         type = CvEnum.TERMCRIT.CV_TERMCRIT_ITER;
      }

      ///<summary>
      ///Create the termination Criteria using only the constrain of epsilon
      ///</summary>
      ///<param name="eps"> The epsilon value</param>
      public MCvTermCriteria(double eps)
      {
         max_iter = 0;
         epsilon = eps;
         type = CvEnum.TERMCRIT.CV_TERMCRIT_EPS;
      }

      /// <summary>
      /// Create the termination criteria using the constrain of maximum iteration as well as epsilon
      /// </summary>
      /// <param name="maxIteration">The maximum number of iteration allowed</param>
      /// <param name="eps">The epsilon value</param>
      public MCvTermCriteria(int maxIteration, double eps)
      {
         max_iter = maxIteration;
         epsilon = eps;
         type = CvEnum.TERMCRIT.CV_TERMCRIT_EPS | CvEnum.TERMCRIT.CV_TERMCRIT_ITER;
      }
   }
}
