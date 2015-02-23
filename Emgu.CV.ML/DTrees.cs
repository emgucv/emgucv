//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.CV.ML.Structure;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Decision Trees 
   /// </summary>
   public partial class DTrees : UnmanagedObject , IStatModel
   {
      private IntPtr _statModelPtr;
      private IntPtr _algorithmPtr;


      /// <summary>
      /// Create a default decision tree
      /// </summary>
      public DTrees()
      {
         _ptr = MlInvoke.cveDTreesCreate(ref _statModelPtr, ref _algorithmPtr);
      }

      /// <summary>
      /// Release the decision tree and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.cveDTreesRelease(ref _ptr);
         _statModelPtr = IntPtr.Zero;
         _algorithmPtr = IntPtr.Zero;
      }

      IntPtr IStatModel.StatModelPtr
      {
         get { return _statModelPtr; }
      }

      IntPtr IAlgorithm.AlgorithmPtr
      {
         get { return _algorithmPtr; }
      }
   }
}
