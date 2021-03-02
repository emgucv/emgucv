//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV.CvEnum;

namespace Emgu.CV.DepthAI
{
    /// <summary>
    /// FrameMetadata
    /// </summary>
    public partial class FrameMetadata : UnmanagedObject
    {
        /// <summary>
        /// Create a FrameMetaData object
        /// </summary>
        public FrameMetadata()
        {
            _ptr = DepthAIInvoke.depthaiFrameMetadataCreate();
        }

        /// <summary>
        /// Release all unmanaged memory associated with the FrameMetadata.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DepthAIInvoke.depthaiFrameMetadataRelease(ref _ptr);
            }
        }
    }

}