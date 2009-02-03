using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapper to the CvBlob structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvBlob
   {
      /// <summary>
      /// blob position
      /// </summary>
      public float x;
      /// <summary>
      /// blob position
      /// </summary>
      public float y;
      /// <summary>
      /// blob sizes 
      /// </summary>
      public float w;
      /// <summary>
      /// blob sizes 
      /// </summary>
      public float h;

      /// <summary>
      /// blob ID  
      /// </summary>
      public int ID;
   }
}
