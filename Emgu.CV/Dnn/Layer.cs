//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.Dnn
{
    /// <summary>
    /// This interface class allows to build new Layers - are building blocks of networks.
    /// </summary>
    public partial class Layer : SharedPtrObject
    {
        internal Layer(IntPtr sharedPtr, IntPtr ptr)
        {
            _sharedPtr = sharedPtr;
            _ptr = ptr;
        }

        /// <summary>
        /// List of learned parameters must be stored here to allow read them by using Net::getParam().
        /// </summary>
        public VectorOfMat Blobs
        {
            get
            {
                return new VectorOfMat(DnnInvoke.cveDnnLayerGetBlobs(_ptr), false);
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Layer.
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_sharedPtr))
            {
                DnnInvoke.cveDnnLayerRelease(ref _sharedPtr);
            }
        }
    }

    public static partial class DnnInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void  cveDnnLayerRelease(ref IntPtr layerPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnLayerGetBlobs(IntPtr layer);

    }
}
