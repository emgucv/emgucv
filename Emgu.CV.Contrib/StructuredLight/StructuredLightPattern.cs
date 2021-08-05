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
using Emgu.CV.Quality;

namespace Emgu.CV.StructuredLight
{
    /// <summary>
    /// Structured Light Pattern interface
    /// </summary>
    public interface IStructuredLightPattern : IAlgorithm
    {
        /// <summary>
        /// Pointer to the cv::structured_light::StructuredLightPattern object
        /// </summary>
        IntPtr StructuredLightPatternPtr { get; }
    }

    /// <summary>
    /// The structured light pattern decode flag
    /// </summary>
    public enum DecodeFlag
    {
        /// <summary>
        /// Kyriakos Herakleous, Charalambos Poullis. "3DUNDERWORLD-SLS: An Open-Source Structured-Light Scanning System for Rapid Geometry Acquisition", arXiv preprint arXiv:1406.6595 (2014)
        /// </summary>
        Decode3dUnderworld = 0
    }

    /// <summary>
    /// Provide interfaces to the Open CV StructuredLight functions
    /// </summary>
    public static partial class StructuredLightInvoke
    {
        static StructuredLightInvoke()
        {
            CvInvoke.Init();
        }

        /// <summary>
        /// Generates the structured light pattern to project.
        /// </summary>
        /// <param name="structuredLightPattern">The strucutred light pattern</param>
        /// <param name="patternImages">
        /// The generated pattern: a VectorOfMat, in which each image is a CV_8U Mat at projector's resolution.
        /// </param>
        /// <returns>True if successful.</returns>
        public static bool Generate(
            this IStructuredLightPattern structuredLightPattern,
            IOutputArrayOfArrays patternImages)
        {
            using (OutputArray oaPatternImages = patternImages.GetOutputArray())
                return cveStructuredLightPatternGenerate(structuredLightPattern.StructuredLightPatternPtr, oaPatternImages);
        }

        /// <summary>
        /// Decodes the structured light pattern, generating a disparity map.
        /// </summary>
        /// <param name="structuredLightPattern">The strucutred light pattern</param>
        /// <param name="patternImages">The acquired pattern images to decode VectorOfVectorOfMat), loaded as grayscale and previously rectified.</param>
        /// <param name="disparityMap">The decoding result: a CV_64F Mat at image resolution, storing the computed disparity map.</param>
        /// <param name="blackImages">The all-black images needed for shadowMasks computation.</param>
        /// <param name="whiteImages">The all-white images needed for shadowMasks computation.</param>
        /// <param name="flags">Flags setting decoding algorithms.</param>
        /// <returns>True if successful.</returns>
        public static bool Decode(
            this IStructuredLightPattern structuredLightPattern,
            VectorOfVectorOfMat patternImages,
            IOutputArray disparityMap,
            IInputArrayOfArrays blackImages = null,
            IInputArrayOfArrays whiteImages = null,
            DecodeFlag flags = DecodeFlag.Decode3dUnderworld)
        {
            using (OutputArray oaDisparityMap = disparityMap.GetOutputArray())
            using (InputArray iaBlackImages = blackImages == null? InputArray.GetEmpty() : blackImages.GetInputArray())
            using (InputArray iaWhiteImages = whiteImages == null? InputArray.GetEmpty() : whiteImages.GetInputArray())
            {
                return cveStructuredLightPatternDecode(
                    structuredLightPattern.StructuredLightPatternPtr,
                    patternImages,
                    oaDisparityMap,
                    iaBlackImages,
                    iaWhiteImages,
                    flags
                    );
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveStructuredLightPatternGenerate(
            IntPtr structuredLight,
            IntPtr patternImages);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveStructuredLightPatternDecode(
            IntPtr structuredLight,
            IntPtr patternImages,
            IntPtr disparityMap,
            IntPtr blackImages,
            IntPtr whiteImages,
            DecodeFlag flags);

    }
}