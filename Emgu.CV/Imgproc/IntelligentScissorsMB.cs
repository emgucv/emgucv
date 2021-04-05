//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
	/// <summary>
	/// This class is used to find the path (contour) between two points which can be used for image segmentation.
	/// </summary>
    public partial class IntelligentScissorsMB : UnmanagedObject
    {
	    /// <summary>
	    /// Create a new Intelligent Scissors for image segmentation.
	    /// </summary>
		public IntelligentScissorsMB()
        {
			_ptr = CvInvoke.cveIntelligentScissorsMBCreate();
		}

	    /// <summary>
	    /// Specify weights of feature functions.
	    /// Consider keeping weights normalized (sum of weights equals to 1.0) Discrete dynamic programming (DP) goal is minimization of costs between pixels.
	    /// </summary>
	    /// <param name="weightNonEdge">Specify cost of non-edge pixels (default: 0.43f)</param>
	    /// <param name="weightGradientDirection">Specify cost of gradient direction function (default: 0.43f)</param>
	    /// <param name="weightGradientMagnitude">Specify cost of gradient magnitude function (default: 0.14f)</param>
		public void SetWeights(
			float weightNonEdge,
			float weightGradientDirection,
			float weightGradientMagnitude)
        {
			CvInvoke.cveIntelligentScissorsMBSetWeights(
				_ptr,
				weightNonEdge,
				weightGradientDirection,
				weightGradientMagnitude);
		}

		/// <summary>
		/// Switch edge feature extractor to use Canny edge detector.
		/// </summary>
		/// <param name="threshold1">First threshold for the hysteresis procedure.</param>
		/// <param name="threshold2">Second threshold for the hysteresis procedure.</param>
		/// <param name="apertureSize">Aperture size for the Sobel operator.</param>
		/// <param name="L2gradient">a flag, indicating whether a more accurate L2 norm should be used to calculate the image gradient magnitude ( L2gradient=true ), or whether the default L1 norm is enough ( L2gradient=false ).</param>
		/// <remarks>"Laplacian Zero-Crossing" feature extractor is used by default (following to original article)</remarks>
		public void SetEdgeFeatureCannyParameters(
			double threshold1,
			double threshold2,
			int apertureSize = 3,
			bool L2gradient = false)
        {
			CvInvoke.cveIntelligentScissorsMBSetEdgeFeatureCannyParameters(
				_ptr,
				threshold1,
				threshold2,
				apertureSize,
				L2gradient);
		}

		/// <summary>
		/// Specify input image and extract image features.
		/// </summary>
		/// <param name="image">The image used for segmentation</param>
		public void ApplyImage(IInputArray image)
        {
			using (InputArray iaImage = image.GetInputArray())
            {
				CvInvoke.cveIntelligentScissorsMBApplyImage(_ptr, iaImage);
			}
        }

		/// <summary>
		/// Prepares a map of optimal paths for the given source point on the image.
		/// </summary>
		/// <param name="sourcePt">The source point used to find the paths</param>
		public void BuildMap(Point sourcePt)
		{
			CvInvoke.cveIntelligentScissorsMBBuildMap(_ptr, ref sourcePt);
		}

		/// <summary>
		/// Specify custom features of input image.
		/// </summary>
		/// <param name="nonEdge">Specify cost of non-edge pixels. Type is CV_8UC1. Expected values are {0, 1}.</param>
		/// <param name="gradientDirection">Specify gradient direction feature. Type is CV_32FC2. Values are expected to be normalized: x^2 + y^2 == 1</param>
		/// <param name="gradientMagnitude">Specify cost of gradient magnitude function: Type is CV_32FC1. Values should be in range [0, 1].</param>
		/// <param name="image">Optional parameter. Must be specified if subset of features is specified (non-specified features are calculated internally)</param>
		public void ApplyImageFeatures(
			IInputArray nonEdge,
			IInputArray gradientDirection,
			IInputArray gradientMagnitude,
			IInputArray image = null)
        {
			using (InputArray iaNonEdge = nonEdge.GetInputArray())
			using (InputArray iaGradientDirection = gradientDirection.GetInputArray())
			using (InputArray iaGradientMagnitude = gradientMagnitude.GetInputArray())
			using (InputArray iaImage = image == null ? InputArray.GetEmpty() : image.GetInputArray())
			{
				CvInvoke.cveIntelligentScissorsMBApplyImageFeatures(
					_ptr,
					iaNonEdge,
					iaGradientDirection,
					iaGradientMagnitude, 
					iaImage);
			}
        }

		/// <summary>
		/// Extracts optimal contour for the given target point on the image
		/// </summary>
		/// <param name="targetPt">The target point</param>
		/// <param name="contour">The list of pixels which contains optimal path between the source and the target points of the image. Type is CV_32SC2 (compatible with VectorOfPoint)</param>
		/// <param name="backward">Flag to indicate reverse order of retrieved pixels (use "true" value to fetch points from the target to the source point)</param>
		/// <remarks>BuildMap() must be called before this call</remarks>
		public void GetContour(
			Point targetPt,
			IOutputArray contour,
			bool backward = false)
		{
			using (OutputArray oaContour = contour.GetOutputArray())
			{
				CvInvoke.cveIntelligentScissorsMBGetContour(
					_ptr, 
					ref targetPt,
					oaContour,
					backward);
			}
		}

		/// <summary>
		/// Release unmanaged resources
		/// </summary>
		protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                CvInvoke.cveIntelligentScissorsMBRelease(ref _ptr);
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveIntelligentScissorsMBCreate();

		[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
		internal static extern void cveIntelligentScissorsMBRelease(ref IntPtr ptr);

		[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
		internal static extern void cveIntelligentScissorsMBSetWeights(
			IntPtr ptr,
			float weightNonEdge,
			float weightGradientDirection,
			float weightGradientMagnitude);
		
		[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
		internal static extern void cveIntelligentScissorsMBSetEdgeFeatureCannyParameters(
			IntPtr ptr,
			double threshold1,
			double threshold2,
			int apertureSize,
			[MarshalAs(CvInvoke.BoolMarshalType)]
			bool L2gradient);
		
		[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
		internal static extern void cveIntelligentScissorsMBApplyImage(IntPtr ptr, IntPtr image);
		
		[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
		internal static extern void cveIntelligentScissorsMBApplyImageFeatures(
			IntPtr ptr,
			IntPtr nonEdge,
			IntPtr gradientDirection,
			IntPtr gradientMagnitude,
			IntPtr image);
			
		[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
		internal static extern void  cveIntelligentScissorsMBBuildMap(
			IntPtr ptr, 
			ref Point sourcePt);
		
		[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
		internal static extern void  cveIntelligentScissorsMBGetContour(
			IntPtr ptr,
			ref Point targetPt,
			IntPtr contour,
			[MarshalAs(CvInvoke.BoolMarshalType)]
			bool backward);
	}
}
