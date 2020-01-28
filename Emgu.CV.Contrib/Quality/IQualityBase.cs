//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Drawing;

namespace Emgu.CV.Quality
{
    /// <summary>
    /// Main interface for all quality filters.
    /// </summary>
    public interface IQualityBase : IAlgorithm
    {
        /// <summary>
        /// Pointer to the native QualityBase object
        /// </summary>
        IntPtr QualityBasePtr { get; }
    }

    public static partial class QualityInvoke
    {
        /// <summary>
        /// Compute quality score per channel with the per-channel score in each element of the result
        /// </summary>
        /// <param name="qualityBase">The quality base object</param>
        /// <param name="cmpImgs">Comparison image(s), or image(s) to evaluate for no-reference quality algorithms</param>
        /// <returns>Quality score per channel</returns>
        public static MCvScalar Compute(
            this IQualityBase qualityBase,
            IInputArrayOfArrays cmpImgs)
        {
            MCvScalar score = new MCvScalar();
            using (InputArray iaCmpImgs = cmpImgs.GetInputArray())
                cveQualityBaseCompute(qualityBase.QualityBasePtr, iaCmpImgs, ref score);
            return score;
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveQualityBaseCompute(IntPtr qualityBase, IntPtr cmpImgs, ref MCvScalar score);

        /// <summary>
        /// Returns output quality map images that were generated during computation, if supported by the algorithm.
        /// </summary>
        /// <param name="qualityBase">The quality base object</param>
        /// <param name="dst">Output quality map images that were generated during computation, if supported by the algorithm.</param>
        public static void GetQualityMap(
            this IQualityBase qualityBase,
            IOutputArrayOfArrays dst)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
                cveQualityBaseGetQualityMap(qualityBase.QualityBasePtr, oaDst);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveQualityBaseGetQualityMap(IntPtr qualityBase, IntPtr dst);

    }
}
