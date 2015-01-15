//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   ///<summary>
   /// A color type
   ///</summary>
   public interface IColor
   {
      /// <summary>
      /// The equivalent MCvScalar value
      /// </summary>
      MCvScalar MCvScalar
      {
         get;
         set;
      }

      /// <summary>
      /// Get the dimension of the color type
      /// </summary>
      int Dimension
      {
         get;
      }
   }
}
