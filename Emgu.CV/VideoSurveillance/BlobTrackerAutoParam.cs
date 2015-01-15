/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.Structure;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// Parameters of blobtracker auto ver1
   /// </summary>
   /// <typeparam name="TColor">The type of color for the image to be tracked. Due to a bug in OpenCV, only Gray is supported at the moment</typeparam>
   public class BlobTrackerAutoParam<TColor>
      where TColor : struct, IColor
   {
      /// <summary>
      /// Number of frames needed for FG (foreground) detector to train.
      /// </summary>
      private int _FGTrainFrames;

      /// <summary>
      /// FGDetector module. If this field is NULL the Process FG mask is used.
      /// </summary>
      private FGDetector<TColor> _foregroundDetector;

      /// <summary>
      /// Selected blob detector module. If this field is NULL default blobdetector module will be created.
      /// </summary>
      private BlobDetector _blobDetector;

      /// <summary>
      /// Selected blob tracking module. If this field is NULL default blobtracker module will be created.
      /// </summary>
      private BlobTracker _blobTracker;

      /// <summary>
      /// Selected blob trajectory generator. If this field is IntPtr.Zero no generator is used.
      /// </summary>
      public IntPtr BTGen;

      /// <summary>
      /// Selected blob trajectory postprocessing module. If this field is NULL no postprocessing is done. 
      /// </summary>
      private BlobTrackPostProc _postProcessModule;

      /// <summary>
      /// Selected blob trajectory postprocessing module. If this field is NULL no postprocessing is done. 
      /// </summary>
      public BlobTrackPostProc BTPP
      {
         get { return _postProcessModule; }
         set { _postProcessModule = value; }
      }

      /// <summary>
      /// Indicates if postprocess data should be used
      /// </summary>
      private int _usePPData;

      /// <summary>
      /// Selected blob trajectory analysis module. If this field is NULL no track analysis is done.   
      /// </summary>
      public IntPtr BTA;

      /// <summary>
      /// Selected blob detector module. If this field is NULL default blobdetector module will be created.
      /// </summary>
      public BlobDetector BlobDetector
      {
         get
         {
            return _blobDetector;
         }
         set
         {
            _blobDetector = value;
         }
      }

      /// <summary>
      /// Selected blob tracking module. If this field is NULL default blobtracker module will be created.
      /// </summary>
      public BlobTracker BlobTracker
      {
         get
         {
            return _blobTracker;
         }
         set
         {
            _blobTracker = value;
         }
      }

      /// <summary>
      /// Number of frames needed for FG (foreground) detector to train.
      /// </summary>
      public int FGTrainFrames
      {
         get
         {
            return _FGTrainFrames;
         }
         set
         {
            _FGTrainFrames = value;
         }
      }
      
      /// <summary>
      /// FGDetector module. If this field is NULL the Process FG mask is used.
      /// </summary>
      public FGDetector<TColor> FGDetector
      {
         get
         {
            return _foregroundDetector;
         }
         set
         {
            _foregroundDetector = value;
         }
      }

      /// <summary>
      /// Indicates if postprocess data should be used
      /// </summary>
      public int UsePPData
      {
         get
         {
            return _usePPData;
         }
         set
         {
            _usePPData = value;
         }
      }

      /// <summary>
      /// Get the equivalent MCvBlobTrackerAutoParam1
      /// </summary>
      public MCvBlobTrackerAutoParam1 MCvBlobTrackerAutoParam1
      {
         get
         {
            MCvBlobTrackerAutoParam1 param = new MCvBlobTrackerAutoParam1();
            param.BD = BlobDetector;
            param.BT = BlobTracker;
            param.BTA = BTA;
            param.BTGen = BTGen;
            param.BTPP = _postProcessModule;
            param.FG = FGDetector;
            param.FGTrainFrames = FGTrainFrames;
            param.usePPData = UsePPData;
            return param;
         }
      }
   }
}
*/