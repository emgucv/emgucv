//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.Util;
using System.Drawing;

namespace Emgu.CV
{
    public static partial class CvInvoke
    {
        /// <summary>
        /// Decompose an essential matrix to possible rotations and translation.
        /// </summary>
        /// <param name="e">The input essential matrix.</param>
        /// <param name="r1">One possible rotation matrix.</param>
        /// <param name="r2">Another possible rotation matrix.</param>
        /// <param name="t">One possible translation.</param>
        /// <remarks>This function decomposes the essential matrix E using svd decomposition. In general, four possible poses exist for the decomposition of E. They are [R1,t], [R1,−t], [R2,t], [R2,−t]</remarks>
        public static void DecomposeEssentialMat(IInputArray e, IOutputArray r1, IOutputArray r2, IOutputArray t)
        {
            using (InputArray iaE = e.GetInputArray())
            using (OutputArray oaR1 = r1.GetOutputArray())
            using (OutputArray oaR2 = r2.GetOutputArray())
            using (OutputArray oaT = t.GetOutputArray())
            {
                cveDecomposeEssentialMat(iaE, oaR1, oaR2, oaT);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDecomposeEssentialMat(
            IntPtr e, 
            IntPtr r1, 
            IntPtr r2, 
            IntPtr t);

        /// <summary>
        /// Decompose a homography matrix to rotation(s), translation(s) and plane normal(s).
        /// </summary>
        /// <param name="h">The input homography matrix between two images.</param>
        /// <param name="k">The input camera intrinsic matrix.</param>
        /// <param name="rotations">Array of rotation matrices.</param>
        /// <param name="translations">Array of translation matrices.</param>
        /// <param name="normals">Array of plane normal matrices.</param>
        /// <returns>Number of solutions</returns>
        public static int DecomposeHomographyMat(
            IInputArray h,
            IInputArray k,
            IOutputArrayOfArrays rotations,
            IOutputArrayOfArrays translations,
            IOutputArrayOfArrays normals)
        {
            using (InputArray iaH = h.GetInputArray())
            using (InputArray iaK = k.GetInputArray())
            using (OutputArray oaRotations = rotations.GetOutputArray())
            using (OutputArray oaTranslations = translations.GetOutputArray())
            using (OutputArray oaNormals = normals.GetOutputArray())
            {
                return cveDecomposeHomographyMat(iaH, iaK, oaRotations, oaTranslations, oaNormals);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveDecomposeHomographyMat(
            IntPtr h,
            IntPtr k,
            IntPtr rotations,
            IntPtr translations,
            IntPtr normals);
    }
}