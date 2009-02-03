using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Some declarations for specific Likelihood tracker
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvBlobTrackerParamLH
   {
      /// <summary>
      /// see Prob.h 
      /// </summary>
      int HistType; 
      /// <summary>
      /// 
      /// </summary>
      int ScaleAfter;
   }
}
