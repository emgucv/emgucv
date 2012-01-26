//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.Structure;

namespace Emgu.CV.ML.Structure
{
   /// <summary>
   /// Wrapped CvSVMParams
   /// </summary>
   public struct MCvSVMParams
   {
      /// <summary>
      /// The type of SVM
      /// </summary>
      public MlEnum.SVM_TYPE svm_type;

      /// <summary>
      /// The type of SVM kernel
      /// </summary>
      public MlEnum.SVM_KERNEL_TYPE kernel_type;
      /// <summary>
      /// for poly
      /// </summary>
      public double degree; 
      /// <summary>
      /// for poly/rbf/sigmoid
      /// </summary>
      public double gamma;  
      /// <summary>
      /// for poly/sigmoid
      /// </summary>
      public double coef0;  

      /// <summary>
      /// for CV_SVM_C_SVC, CV_SVM_EPS_SVR and CV_SVM_NU_SVR
      /// </summary>
      public double C;  
      /// <summary>
      /// for CV_SVM_NU_SVC, CV_SVM_ONE_CLASS, and CV_SVM_NU_SVR
      /// </summary>
      public double nu; 
      /// <summary>
      /// for CV_SVM_EPS_SVR
      /// </summary>
      public double p; 
      /// <summary>
      /// for CV_SVM_C_SVC
      /// </summary>
      public IntPtr class_weights; 
      /// <summary>
      /// termination criteria
      /// </summary>
      public MCvTermCriteria term_crit; 
   }
}
