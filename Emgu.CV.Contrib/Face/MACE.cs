//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Face
{
    
    public class MACE : SharedPtrObject
    {

        
        public MACE(int imgSize)
        {
            _ptr = FaceInvoke.cveMaceCreate(imgSize, ref _sharedPtr);
        }

        public void Salt(String passphrase)
        {
            using (CvString csPassphrase = new CvString(passphrase))
            {
                FaceInvoke.cveMaceSalt(_ptr, csPassphrase);
            }
        }

        public void Train(IInputArrayOfArrays images)
        {
            using (InputArray iaImages = images.GetInputArray())
            {
                FaceInvoke.cveMaceTrain(_ptr, iaImages);
            }
        }

        public bool Same(IInputArray query)
        {
            using (InputArray iaQuery = query.GetInputArray())
            {
                return FaceInvoke.cveMaceSame(_ptr, iaQuery);
            }
        }
        /// <summary>
        /// Release the unmanaged memory associated with this BIF
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                FaceInvoke.cveMaceRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class FaceInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr  cveMaceCreate(int imgSize, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMaceSalt(IntPtr mace, IntPtr passphrase);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMaceTrain(IntPtr mace, IntPtr images);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveMaceSame(IntPtr mace, IntPtr query);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMaceRelease(ref IntPtr sharedPtr);
    }
}