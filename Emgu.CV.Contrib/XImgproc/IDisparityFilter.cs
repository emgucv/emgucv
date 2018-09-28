//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV.XImgproc
{
    
    public interface IDisparityFilter
    {
        IntPtr DisparityFilterPtr { get; }
    }


    public static partial class XImgprocInvoke
    {
        public static void Filter(
            this IDisparityFilter filter,
            IInputArray disparityMapLeft,
            IOutputArray leftView,
            IOutputArray filteredDisparityMap,
            IInputArray disparityMapRight = null,
            Rectangle roi = new Rectangle(),
            IInputArray rightView = null)
        {
            
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDisparityFilterFilter(
            IntPtr disparityFilter,
            IntPtr disparityMapLeft, IntPtr leftView, IntPtr filteredDisparityMap,
            IntPtr disparityMapRight, ref Rectangle ROI, IntPtr rightView);

        
    }
}
