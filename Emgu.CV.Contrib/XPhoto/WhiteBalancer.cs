//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.CvEnum;
using Emgu.Util;

namespace Emgu.CV.XPhoto
{ 
    /// <summary>
    /// The base class for auto white balance algorithms.
    /// </summary>
    public abstract class WhiteBalancer : UnmanagedObject
    {
        /// <summary>
        /// Pointer to the native white balancer object
        /// </summary>
        protected IntPtr _whiteBalancerPtr;

        /// <summary>
        /// Applies white balancing to the input image.
        /// </summary>
        /// <param name="src">Input image</param>
        /// <param name="dst">White balancing result</param>
        public void BalanceWhite(IInputArray src, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                XPhotoInvoke.cveWhiteBalancerBalanceWhite(_whiteBalancerPtr, iaSrc, oaDst);
        }

        /// <summary>
        /// Reset the pointer to the native white balancer object
        /// </summary>
        protected override void DisposeObject()
        {
            _whiteBalancerPtr = IntPtr.Zero;
        }
    }
}
