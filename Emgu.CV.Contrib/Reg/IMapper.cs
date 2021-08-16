//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;


namespace Emgu.CV.Reg
{
    /// <summary>
    /// The interface for cv::reg::Mapper
    /// </summary>
    public interface IMapper 
    {
        /// <summary>
        /// Pointer to the native cv::reg::Mapper object
        /// </summary>
        IntPtr MapperPtr { get; }
    }


    public static partial class RegInvoke
    {
        /// <summary>
        /// Calculate the map between the images
        /// </summary>
        /// <param name="mapper">The mapper</param>
        /// <param name="img1">The first image</param>
        /// <param name="img2">The second image</param>
        /// <param name="init">The initial guess for the map</param>
        /// <returns>The map between the images</returns>
        public static Map Calculate(
            this IMapper mapper, 
            IInputArray img1, 
            IInputArray img2, 
            Map init = null)
        {
            IntPtr ptr;
            IntPtr sharedPtr = IntPtr.Zero;
            using (InputArray iaImg1 = img1.GetInputArray())
            using (InputArray iaImg2 = img2.GetInputArray())
            {
                ptr = cveMapperCalculate(
                    mapper.MapperPtr, 
                    iaImg1,
                    iaImg2,
                    init,
                    ref sharedPtr);
            }

            return new Map(ptr, sharedPtr);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMapperCalculate(
            IntPtr mapper,
            IntPtr img1,
            IntPtr img2,
            IntPtr init,
            ref IntPtr sharedPtr);

    }
}
