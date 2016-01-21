//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if NETFX_CORE
namespace System
{
   /// <summary>
   /// Replicate of the System.ICloneable interface in .NET that do not exist in NETFX_CORE
   /// </summary>
   public interface ICloneable
   {
      /// <summary>
      /// Return a clone of the object
      /// </summary>
      /// <returns>A clone of the object</returns>
      object Clone();
   }
}
#endif
