//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// An interface for a descriptor generator
   /// </summary>
   public interface IDescriptorExtractor
   {
      /// <summary>
      /// Get the pointer to the descriptor extractor. 
      /// </summary>
      /// <returns>The descriptor extractor</returns>
      IntPtr DescriptorExtratorPtr { get; }
   }
}
