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

    }
}