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

namespace Emgu.CV.DnnSuperres
{
    internal static partial class DnnSuperresInvoke
    {
        public class DnnSuperResImpl : UnmanagedObject
        {
            public DnnSuperResImpl()
            {
                _ptr = DnnSuperresInvoke.cveDnnSuperResImplCreate();
            }

            public void ReadModel(String path)
            {
                using (CvString csPath = new CvString(path))
                    DnnSuperresInvoke.cveDnnSuperResImplReadModel1(_ptr, csPath);
            }

            public void ReadModel(String weight, String definition)
            {
                using (CvString csWeight = new CvString(weight))
                using (CvString csDefinition = new CvString(definition))
                {
                    DnnSuperresInvoke.cveDnnSuperResImplReadModel2(_ptr, csWeight, csDefinition);
                }
            }

            public void SetModel(String algorithm, int scale)
            {
                using (CvString csAlgorithm = new CvString(algorithm))
                    DnnSuperresInvoke.cveDnnSuperResImplSetModel(_ptr, csAlgorithm, scale);
            }

            public void Upsample(IInputArray img, IOutputArray result)
            {
                using (InputArray iaImg = img.GetInputArray())
                using (OutputArray oaResult = result.GetOutputArray())
                {
                    DnnSuperresInvoke.cveDnnSuperResImplUpsample(_ptr, iaImg, oaResult);
                }
            }

            public int Scale
            {
                get { return DnnSuperresInvoke.cveDnnSuperResImplGetScale(_ptr); }
            }

            public String Algorithm
            {
                get
                {
                    using (CvString csAlgorithm = new CvString())
                    {
                        DnnSuperresInvoke.cveDnnSuperResImplGetAlgorithm(_ptr, csAlgorithm);
                        return csAlgorithm.ToString();
                    }
                }
            }

            protected override void DisposeObject()
            {
                if (_ptr == IntPtr.Zero)
                {
                    DnnSuperresInvoke.cveDnnSuperResImplRelease(ref _ptr);
                }
            }
        }

        static DnnSuperresInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnSuperResImplCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnSuperResImplSetModel(IntPtr dnnSuperRes, IntPtr algorithm, int scale);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnSuperResImplReadModel1(IntPtr dnnSuperRes, IntPtr path);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnSuperResImplReadModel2(IntPtr dnnSuperRes, IntPtr weights, IntPtr definition);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnSuperResImplUpsample(IntPtr dnnSuperRes, IntPtr img, IntPtr result);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnSuperResImplRelease(ref IntPtr dnnSuperRes);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveDnnSuperResImplGetScale(IntPtr dnnSuperRes);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnSuperResImplGetAlgorithm(IntPtr dnnSuperRes, IntPtr algorithm);
    }
}
