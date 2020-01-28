//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.Util
{
    /// <summary>
    /// An object that can be interpolated
    /// </summary>
    /// <typeparam name="T">The type of the object</typeparam>
    public interface IInterpolatable<T> where T: new()
   {
      /// <summary>
      /// The index that will be used for interpolation
      /// </summary>
      double InterpolationIndex { get; }

        /*
        /// <summary>
        /// Multiplication with a scale
        /// </summary>
        /// <param name="scale">The multiplication scale</param>
        void Mul (double scale);

        /// <summary>
        /// Computes the sum of the two elements
        /// </summary>
        /// <param name="i">The other element to be added</param>
        /// <returns>The sum of the two elements</returns>
        void Add(T i);

        /// <summary>
        /// Subtract the other element from the current element
        /// </summary>
        /// <param name="i">The element to be subtracted</param>
        /// <returns>The result of subtracting the other element</returns>
        void Sub(T i);*/

        /// <summary>
        /// Interpolate base on this point and the other point with the given index
        /// </summary>
        /// <param name="other">The other point</param>
        /// <param name="index">The interpolation index</param>
        /// <returns>The interpolated point</returns>
        T LinearInterpolate(T other, double index);
   }
}
