//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Diagnostics;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;

namespace Emgu.CV.Shape
{
    /// <summary>
    /// Abstract base class for shape transformation algorithms.
    /// </summary>
    public interface IShapeTransformer
    {
        /// <summary>
        /// Get the pointer to the unmanaged shape transformer
        /// </summary>
        IntPtr ShapeTransformerPtr { get; }
    }

    /// <summary>
    /// Definition of the transformation occupied in the paper “Principal Warps: Thin-Plate Splines and Decomposition of Deformations”, by F.L. Bookstein (PAMI 1989).
    /// </summary>
    public class ThinPlateSplineShapeTransformer : SharedPtrObject, IShapeTransformer
    {
        private IntPtr _shapeTransformerPtr;

        /// <summary>
        /// Create a thin plate spline shape transformer
        /// </summary>
        /// <param name="regularizationParameter">The regularization parameter for relaxing the exact interpolation requirements of the TPS algorithm.</param>
        public ThinPlateSplineShapeTransformer(double regularizationParameter = 0)
        {
            _ptr = ShapeInvoke.cveThinPlateSplineShapeTransformerCreate(regularizationParameter, ref _shapeTransformerPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Get the pointer the the native ShapeTransformer
        /// </summary>
        public IntPtr ShapeTransformerPtr
        {
            get
            {
                return _shapeTransformerPtr;
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this ShapeTransformer object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                ShapeInvoke.cveThinPlateSplineShapeTransformerRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _shapeTransformerPtr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// Wrapper class for the OpenCV Affine Transformation algorithm.
    /// </summary>
    public class AffineTransformer : SharedPtrObject, IShapeTransformer
    {
        private IntPtr _shapeTransformerPtr;

        /// <summary>
        /// Create an affine transformer
        /// </summary>
        /// <param name="fullAffine">Full affine</param>
        public AffineTransformer(bool fullAffine)
        {
            _ptr = ShapeInvoke.cveAffineTransformerCreate(fullAffine, ref _shapeTransformerPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this ShapeTransformer object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                ShapeInvoke.cveAffineTransformerRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _shapeTransformerPtr = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Get the pointer to the native ShapeTransformer
        /// </summary>
        public IntPtr ShapeTransformerPtr { get { return _shapeTransformerPtr; } }
    }

    public static partial class ShapeInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveAffineTransformerCreate(
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool fullAffine, 
            ref IntPtr transformer, 
            ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveAffineTransformerRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveThinPlateSplineShapeTransformerCreate(double regularizationParameter, ref IntPtr transformer, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveThinPlateSplineShapeTransformerRelease(ref IntPtr sharedPtr);

        /// <summary>
        /// Estimate the transformation parameters of the current transformer algorithm, based on point matches.
        /// </summary>
        /// <param name="transformer">The shape transformer</param>
        /// <param name="transformingShape">Contour defining first shape.</param>
        /// <param name="targetShape">Contour defining second shape (Target).</param>
        /// <param name="matches">Standard vector of Matches between points.</param>
        public static void EstimateTransformation(
            this IShapeTransformer transformer, 
            IInputArray transformingShape,
            IInputArray targetShape, 
            VectorOfDMatch matches)
        {
            using(InputArray iaTransformingShape = transformingShape.GetInputArray())
            using (InputArray iaTargetShape = targetShape.GetInputArray())
            {
                cveShapeTransformerEstimateTransformation(
                    transformer.ShapeTransformerPtr,
                    iaTransformingShape,
                    iaTargetShape,
                    matches);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveShapeTransformerEstimateTransformation(
            IntPtr transformer,
            IntPtr transformingShape,
            IntPtr targetShape,
            IntPtr matches);

        /// <summary>
        /// Apply a transformation, given a pre-estimated transformation parameters.
        /// </summary>
        /// <param name="transformer">The shape transformer</param>
        /// <param name="input">Contour (set of points) to apply the transformation.</param>
        /// <param name="output">Output contour.</param>
        /// <returns></returns>
        public static float ApplyTransformation(
            this IShapeTransformer transformer,
            IInputArray input,
            IOutputArray output = null)
        {
            using (InputArray iaInput = input.GetInputArray())
            using (OutputArray oaOutput = output == null ? OutputArray.GetEmpty() : output.GetOutputArray())
            {
                return cveShapeTransformerApplyTransformation(
                    transformer.ShapeTransformerPtr,
                    iaInput,
                    oaOutput);
            }
        }
            
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static float cveShapeTransformerApplyTransformation(
            IntPtr transformer,
            IntPtr input,
            IntPtr output);

        /// <summary>
        /// Apply a transformation, given a pre-estimated transformation parameters, to an Image.
        /// </summary>
        /// <param name="transformer">The shape transformer</param>
        /// <param name="transformingImage">Input image.</param>
        /// <param name="output">Output image.</param>
        /// <param name="flags">Image interpolation method.</param>
        /// <param name="boarderMode">border style.</param>
        /// <param name="borderValue">border value.</param>
        public static void WarpImage(
            this IShapeTransformer transformer,
            IInputArray transformingImage,
            IOutputArray output,
            CvEnum.Inter flags = Inter.Linear,
            CvEnum.BorderType boarderMode = BorderType.Constant,
            MCvScalar borderValue = new MCvScalar())
        {
            using (InputArray iaTransformingImage = transformingImage.GetInputArray())
            using (OutputArray oaOutput = output.GetOutputArray())
            {
                cveShapeTransformerWarpImage(
                    transformer.ShapeTransformerPtr,
                    iaTransformingImage, 
                    oaOutput,
                    flags,
                    boarderMode, 
                    ref borderValue);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveShapeTransformerWarpImage(
            IntPtr transformer,
            IntPtr transformingImage,
            IntPtr output,
            CvEnum.Inter flags,
            CvEnum.BorderType borderMode,
            ref MCvScalar borderValue);

    }
}
