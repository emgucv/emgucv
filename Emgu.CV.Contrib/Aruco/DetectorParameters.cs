//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV.Aruco
{
   public struct DetectorParameters
   {

      public int AdaptiveThreshWinSizeMin;
      public int AdaptiveThreshWinSizeMax;
      public int AdaptiveThreshWinSizeStep;
      public double AdaptiveThreshConstant;
      public double MinMarkerPerimeterRate;
      public double MaxMarkerPerimeterRate;
      public double PolygonalApproxAccuracyRate;
      public double MinCornerDistanceRate;
      public int MinDistanceToBorder;
      public double MinMarkerDistanceRate;
      public bool DoCornerRefinement;
      public int CornerRefinementWinSize;
      public int CornerRefinementMaxIterations;
      public double CornerRefinementMinAccuracy;
      public int MarkerBorderBits;
      public int PerspectiveRemovePixelPerCell;
      public double PerspectiveRemoveIgnoredMarginPerCell;
      public double MaxErroneousBitsInBorderRate;
      public double MinOtsuStdDev;
      public double ErrorCorrectionRate;
   }
}