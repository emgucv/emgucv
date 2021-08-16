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
    /// The interface for cv::reg::Map
    /// </summary>
    public interface IMap 
    {
        /// <summary>
        /// Pointer to the native cv::reg::Map object
        /// </summary>
        IntPtr MapPtr { get; }
    }

    /// <summary>
    /// Entry points for the cv::reg functions
    /// </summary>
    public static partial class RegInvoke
    {
        static RegInvoke()
        {
            CvInvoke.Init();
        }

        /// <summary>
        /// Warps image to a new coordinate frame. 
        /// </summary>
        /// <param name="map">The map</param>
        /// <param name="img1">Original image</param>
        /// <param name="img2">Warped image</param>
        public static void Warp(this IMap map, IInputArray img1, IOutputArray img2)
        {
            using (InputArray iaImg1 = img1.GetInputArray())
            using (OutputArray oaImg2 = img2.GetOutputArray())
            {
                cveMapWarp(map.MapPtr, iaImg1, oaImg2);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMapWarp(
            IntPtr map,
            IntPtr img1,
            IntPtr img2);

        /// <summary>
        /// Warps image to a new coordinate frame.
        /// </summary>
        /// <param name="map">The map</param>
        /// <param name="img1">Original image</param>
        /// <param name="img2">Warped image</param>
        public static void InverseWarp(this IMap map, IInputArray img1, IOutputArray img2)
        {
            using (InputArray iaImg1 = img1.GetInputArray())
            using (OutputArray oaImg2 = img2.GetOutputArray())
            {
                cveMapInverseWarp(map.MapPtr, iaImg1, oaImg2);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMapInverseWarp(
            IntPtr map,
            IntPtr img1,
            IntPtr img2);

        /// <summary>
        /// Scales the map by a given factor as if the coordinates system is expanded/compressed by that factor.
        /// </summary>
        /// <param name="map">The map</param>
        /// <param name="factor">Expansion if bigger than one, compression if smaller than one</param>
        public static void Scale(this IMap map, double factor)
        {
            cveMapScale(map.MapPtr, factor);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMapScale(
            IntPtr map,
            double factor);

    }
}
