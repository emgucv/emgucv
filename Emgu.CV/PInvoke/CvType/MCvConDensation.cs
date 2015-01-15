//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvConDensation
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvConDensation
   {
      /// <summary>
      /// Dimension of measurement vector
      /// </summary>
      public int MP;     
      /// <summary>
      /// Dimension of state vector
      /// </summary>
      public int DP;    
      /// <summary>
      /// Matrix of the linear Dynamics system
      /// </summary>
      public IntPtr DynamMatr;      
      /// <summary>
      /// Vector of State
      /// </summary>
      public IntPtr State;          
      /// <summary>
      /// Number of the Samples
      /// </summary>
      public int SamplesNum;         
      /// <summary>
      /// Array of the Sample Vectors
      /// </summary>
      public IntPtr flSamples;      
      /// <summary>
      /// Temporary array of the Sample Vectors
      /// </summary>
      public IntPtr flNewSamples;  
      /// <summary>
      /// Confidence for each Sample
      /// </summary>
      public IntPtr flConfidence;    
      /// <summary>
      /// Cumulative confidence
      /// </summary>
      public IntPtr flCumulative;   
      /// <summary>
      /// Temporary vector
      /// </summary>
      public IntPtr Temp;          
      /// <summary>
      /// RandomVector to update sample set
      /// </summary>
      public IntPtr RandomSample;  
      /// <summary>
      /// Array of structures to generate random vectors
      /// </summary>
      public IntPtr RandS;
   }
}
