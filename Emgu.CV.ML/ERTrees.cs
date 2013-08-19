//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Extreme Random tree
   /// </summary>
   public class ERTrees : RTrees
   {
      /// <summary>
      /// Create an extreme Random tree
      /// </summary>
      public ERTrees()
      {
         //This will call the default base constructor, which calls the Init() function
      }

      /// <summary>
      /// Initialize the extreme Randome tree
      /// </summary>
      protected override void Init()
      {
         _ptr = MlInvoke.CvERTreesCreate();
      }

      /// <summary>
      /// Release the extreme random tree and all memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvERTreesRelease(ref _ptr);
      }
   }
}
