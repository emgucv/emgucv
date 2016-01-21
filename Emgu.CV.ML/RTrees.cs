//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.ML.Structure;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Random trees
   /// </summary>
   public partial class RTrees : UnmanagedObject, IStatModel
   {
      private IntPtr _statModelPtr;
      private IntPtr _algorithmPtr;

      /// <summary>
      /// Create a random tree
      /// </summary>
      public RTrees()
      {
         _ptr = MlInvoke.cveRTreesCreate(ref _statModelPtr, ref _algorithmPtr);
      }


      /// <summary>
      /// Release the random tree and all memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            MlInvoke.cveRTreesRelease(ref _ptr);
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
