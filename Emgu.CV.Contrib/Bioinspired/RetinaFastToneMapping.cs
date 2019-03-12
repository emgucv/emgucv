//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Util;

namespace Emgu.CV.Bioinspired
{

    public class RetinaFastToneMapping : SharedPtrObject
    {

        public RetinaFastToneMapping(Size inputSize)
        {
            _ptr = BioinspiredInvoke.cveRetinaFastToneMappingCreate(ref inputSize, ref _sharedPtr);
        }

        public void ApplyFastToneMapping(
            IInputArray inputImage,
            IOutputArray outputToneMappedImage)
        {
            using (InputArray iaInputImage = inputImage.GetInputArray())
            using (OutputArray oaOutputToneMappedImage = outputToneMappedImage.GetOutputArray())
                BioinspiredInvoke.cveRetinaFastToneMappingApplyFastToneMapping(_ptr, iaInputImage, oaOutputToneMappedImage);
        }


        /// <summary>
        /// Release all unmanaged memory associated with the RetinaFastToneMapping model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                BioinspiredInvoke.cveRetinaFastToneMappingRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }


    }


}
