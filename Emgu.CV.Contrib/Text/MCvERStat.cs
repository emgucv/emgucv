//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using System.Diagnostics;

namespace Emgu.CV.Text
{
    /// <summary>
    /// The ERStat structure represents a class-specific Extremal Region (ER).
    /// An ER is a 4-connected set of pixels with all its grey-level values smaller than the values in its outer boundary. 
    /// A class-specific ER is selected (using a classifier) from all the ER’s in the component tree of the image.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvERStat
    {
        /// <summary>
        /// Seed point
        /// </summary>
        public int Pixel;

        /// <summary>
        /// Threshold (max grey-level value)
        /// </summary>
        public int Level;

        /// <summary>
        /// Area
        /// </summary>
        public int Area;
        /// <summary>
        /// Perimeter
        /// </summary>
        public int Perimeter;
        /// <summary>
        /// Euler number
        /// </summary>
        public int Euler;

        /// <summary>
        /// Bounding box
        /// </summary>
        public System.Drawing.Rectangle Rect;

        /// <summary>
        /// Order 1 raw moments to derive the centroid
        /// </summary>
        public double RawMoments0;
        /// <summary>
        /// Order 1 raw moments to derive the centroid
        /// </summary>
        public double RawMoments1;
        /// <summary>
        /// Order 2 central moments to construct the covariance matrix
        /// </summary>
        public double CentralMoments0;
        /// <summary>
        /// Order 2 central moments to construct the covariance matrix
        /// </summary>
        public double CentralMoments1;
        /// <summary>
        /// Order 2 central moments to construct the covariance matrix
        /// </summary>
        public double CentralMoments2;

        /// <summary>
        /// Pointer owner to horizontal crossings
        /// </summary>
        public IntPtr CrossingsOwner;
        /// <summary>
        /// Pointer to horizontal crossings
        /// </summary>
        public IntPtr CrossingsStored;

        /// <summary>
        /// Median of the crossings at three different height levels
        /// </summary>
        public float MedCrossings;

        /// <summary>
        /// Hole area ratio
        /// </summary>
        public float HoleAreaRatio;
        /// <summary>
        /// Convex hull ratio
        /// </summary>
        public float ConvexHullRatio;
        /// <summary>
        /// Number of inflexion points
        /// </summary>
        public float NumInflexionPoints;

        
        private IntPtr _pixels;

        /// <summary>
        /// Get the pixels list. 
        /// </summary>
        public VectorOfInt Pixels
        {
            get
            {
                if (IntPtr.Zero == Pixels)
                {
                    return null;
                }
                else
                {
                    return new VectorOfInt(_pixels, false);
                }
            }
        }

        /// <summary>
        /// Probability that the ER belongs to the class we are looking for
        /// </summary>
        public double probability;

        /// <summary>
        /// Pointer to the parent ERStat
        /// </summary>
        public IntPtr ParentPtr;
        /// <summary>
        /// Pointer to the child ERStat
        /// </summary>
        public IntPtr ChildPtr;
        /// <summary>
        /// Pointer to the next ERStat
        /// </summary>
        public IntPtr NextPtr;
        /// <summary>
        /// Pointer to the previous ERStat
        /// </summary>
        public IntPtr PrevPtr;

        private byte _localMaxima;

        /// <summary>
        /// If or not the regions is a local maxima of the probability
        /// </summary>
        public bool LocalMaxima
        {
            get { return _localMaxima != 0; }
            set { _localMaxima = value ? (byte) 1 : (byte) 0; }
        }

        /// <summary>
        /// Pointer to the ERStat that is the max probability ancestor
        /// </summary>
        public IntPtr MaxProbabilityAncestor;
        /// <summary>
        /// Pointer to the ERStat that is the min probability ancestor
        /// </summary>
        public IntPtr MinProbabilityAncestor;

        /// <summary>
        /// Get the center of the region
        /// </summary>
        /// <param name="imageWidth">The source image width</param>
        /// <returns>The center of the region</returns>
        public System.Drawing.Point GetCenter(int imageWidth)
        {
            return new System.Drawing.Point(Pixel % imageWidth, Pixel / imageWidth);
        }
    }
}
