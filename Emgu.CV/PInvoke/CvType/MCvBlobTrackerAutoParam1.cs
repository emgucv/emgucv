/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Parameters of blobtracker auto ver1
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvBlobTrackerAutoParam1
   {
      /// <summary>
      /// Number of frames needed for FG (foreground) detector to train.
      /// </summary>
      public int FGTrainFrames; 

      /// <summary>
      /// FGDetector module. If this field is NULL the Process FG mask is used.
      /// </summary>
      public IntPtr FG;          

      /// <summary>
      /// Selected blob detector module. If this field is NULL default blobdetector module will be created.
      /// </summary>
      public IntPtr BD;          

      /// <summary>
      /// Selected blob tracking module. If this field is NULL default blobtracker module will be created.
      /// </summary>
      public IntPtr BT;           

      /// <summary>
      /// Selected blob trajectory generator. If this field is NULL no generator is used.
      /// </summary>
      public IntPtr BTGen;       

      /// <summary>
      /// Selected blob trajectory postprocessing module. If this field is NULL no postprocessing is done. 
      /// </summary>
      public IntPtr BTPP;        

      /// <summary>
      /// 
      /// </summary>
      public int usePPData;

      /// <summary>
      /// Selected blob trajectory analysis module. If this field is NULL no track analysis is done.   
      /// </summary>
      public IntPtr BTA;
   }
}
*/