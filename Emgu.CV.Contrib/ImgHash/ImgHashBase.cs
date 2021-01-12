//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.ImgHash
{
    /// <summary>
    /// The Image Hash base class
    /// </summary>
    public abstract class ImgHashBase : UnmanagedObject
    {
        /// <summary>
        /// The pointer to the ImgHashBase object
        /// </summary>
        protected IntPtr _imgHashBase;

        /// <summary>
        /// Get the pointer to the ImgHashBase object
        /// </summary>
        /// <returns>The pointer to the ImgHashBase object</returns>
        public IntPtr ImgHashBasePtr
        {
            get { return _imgHashBase; }
        }

        /// <summary>
        /// Reset the pointers
        /// </summary>
        protected override void DisposeObject()
        {
            _imgHashBase = IntPtr.Zero;
        }

        /// <summary>
        /// Computes hash of the input image
        /// </summary>
        /// <param name="inputArr">input image to compute hash value</param>
        /// <param name="outputArr">hash of the image</param>
        public void Compute(IInputArray inputArr, IOutputArray outputArr)
        {
            using (InputArray iaInputArr = inputArr.GetInputArray())
            using (OutputArray oaOutputArr = outputArr.GetOutputArray())
            {
                ImgHashInvoke.cveImgHashBaseCompute(_imgHashBase, iaInputArr, oaOutputArr);
            }
        }

        /// <summary>
        /// Compare the hash value between inOne and inTwo
        /// </summary>
        /// <param name="hashOne">Hash value one</param>
        /// <param name="hashTwo">Hash value two</param>
        /// <returns>indicate similarity between inOne and inTwo, the meaning of the value vary from algorithms to algorithms</returns>
        public double Compare(IInputArray hashOne, IInputArray hashTwo)
        {
            using (InputArray iaHashOne = hashOne.GetInputArray())
            using (InputArray iaHashTwo = hashTwo.GetInputArray())
            {
                return ImgHashInvoke.cveImgHashBaseCompare(_imgHashBase, iaHashOne, iaHashTwo);
            }
        }
    }
}
