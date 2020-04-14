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
    }
}
