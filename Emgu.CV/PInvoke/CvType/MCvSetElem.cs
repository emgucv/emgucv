//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;

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
