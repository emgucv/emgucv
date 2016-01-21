//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using Emgu.Util;

namespace Emgu.CV.ML
{
   /// <summary>
   /// The KNearest classifier
   /// </summary>
   public partial class KNearest : UnmanagedObject, IStatModel
   {
      private IntPtr _statModelPtr;
      private IntPtr _algorithmPtr;

      /// <summary>
      /// Create a default KNearest classifier
      /// </summary>
      public KNearest()
      {
         _ptr = MlInvoke.CvKNearestCreate(ref _statModelPtr, ref _algorithmPtr);
      }

      /// <summary>
      /// Release the classifier and all the memory associated with it
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvKNearestRelease(ref _ptr);
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
