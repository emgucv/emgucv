//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
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

    public static partial class QualityBaseInvoke
    {
        public static MCvScalar Compute(
            this IQualityBase qualityBase,
            IInputArray cmpImgs)
        {
            MCvScalar score = new MCvScalar();
            using (InputArray iaCmpImgs = cmpImgs.GetInputArray())
                cveQualityBaseCompute(qualityBase.QualityBasePtr, iaCmpImgs, ref score);
            return score;
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveQualityBaseCompute(IntPtr qualityBase, IntPtr cmpImgs, ref MCvScalar score);

        public static void GetQualityMaps(
            this IQualityBase qualityBase,
            IOutputArray dst)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
                cveQualityBaseGetQualityMaps(qualityBase.QualityBasePtr, oaDst);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveQualityBaseGetQualityMaps(IntPtr qualityBase, IntPtr dst);

    }
}
