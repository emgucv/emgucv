using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapper CvSetElem structure
   /// </summary>
   public struct MCvSetElem
   {
      /// <summary>
      /// flags
      /// </summary>
      public int flags;
      /// <summary>
      /// next_free
      /// </summary>
      public IntPtr next_free;
   }
}
