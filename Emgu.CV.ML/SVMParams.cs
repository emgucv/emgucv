//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.Structure;
using Emgu.CV.ML.Structure;

namespace Emgu.CV.ML
{
   /// <summary>
   /// The parameters for the SVM model
   /// </summary>
   public class SVMParams
   {
      /// <summary>
      /// The type of SVM
      /// </summary>
      private MlEnum.SVM_TYPE _svmType;

      /// <summary>
      /// The type of SVM kernel
      /// </summary>
      private MlEnum.SVM_KERNEL_TYPE _kernelType;

      /// <summary>
      /// For poly
      /// </summary>
      private double _degree;
      /// <summary>
      /// For poly/rbf/sigmoid
      /// </summary>
      private double _gamma;
      /// <summary>
      /// For poly/sigmoid
      /// </summary>
      private double _coef0;

      /// <summary>
      /// For CV_SVM_C_SVC, CV_SVM_EPS_SVR and CV_SVM_NU_SVR
      /// </summary>
      private double _c;
      /// <summary>
      /// For CV_SVM_NU_SVC, CV_SVM_ONE_CLASS, and CV_SVM_NU_SVR
      /// </summary>
      private double _nu;
      /// <summary>
      /// For CV_SVM_EPS_SVR
      /// </summary>
      private double _p;
      /// <summary>
      /// For CV_SVM_C_SVC
      /// </summary>
      private Matrix<float> _classWeights;
      /// <summary>
      /// Termination criteria
      /// </summary>
      private MCvTermCriteria _termCrit;

      /// <summary>
      /// The type of SVM
      /// </summary>
      public MlEnum.SVM_TYPE SVMType
      {
         get
         {
            return _svmType;
         }
         set
         {
            _svmType = value;
         }
      }

      /// <summary>
      /// The type of SVM kernel
      /// </summary>
      public MlEnum.SVM_KERNEL_TYPE KernelType
      {
         get
         {
            return _kernelType;
         }
         set
         {
            _kernelType = value;
         }
      }
      /// <summary>
      /// For poly
      /// </summary>
      public double Degree
      {
         get
         {
            return _degree;
         }
         set
         {
            _degree = value;
         }
      }
      /// <summary>
      /// For poly/rbf/sigmoid
      /// </summary>
      public double Gamma
      {
         get
         {
            return _gamma;
         }
         set
         {
            _gamma = value;
         }
      }
      /// <summary>
      /// For poly/sigmoid
      /// </summary>
      public double Coef0
      {
         get
         {
            return _coef0;
         }
         set
         {
            _coef0 = value;
         }
      }
      /// <summary>
      /// For CV_SVM_C_SVC, CV_SVM_EPS_SVR and CV_SVM_NU_SVR
      /// </summary>
      public double C
      {
         get
         {
            return _c;
         }
         set
         {
            _c = value;
         }
      }
      /// <summary>
      /// For CV_SVM_NU_SVC, CV_SVM_ONE_CLASS, and CV_SVM_NU_SVR
      /// </summary>
      public double Nu
      {
         get
         {
            return _nu;
         }
         set
         {
            _nu = value;
         }
      }
      /// <summary>
      /// For CV_SVM_EPS_SVR
      /// </summary>
      public double P
      {
         get
         {
            return _p;
         }
         set
         {
            _p = value;
         }
      }
      /// <summary>
      /// For CV_SVM_C_SVC
      /// </summary>
      public Matrix<float> ClassWeights
      {
         get
         {
            return _classWeights;
         }
         set
         {
            _classWeights = value;
         }
      }
      /// <summary>
      /// Get or Set the termination criteria
      /// </summary>
      public MCvTermCriteria TermCrit
      {
         get
         {
            return _termCrit;
         }
         set
         {
            _termCrit = value;
         }
      }

      /// <summary>
      /// Get the equivalent representation of MCvSVMParams
      /// </summary>
      public MCvSVMParams MCvSVMParams
      {
         get
         {
            MCvSVMParams p = new MCvSVMParams();
            p.svm_type = SVMType;
            p.kernel_type = KernelType;
            p.degree = Degree;
            p.gamma = Gamma;
            p.coef0 = Coef0;
            p.C = C;
            p.nu = Nu;
            p.p = P;
            p.class_weights = ClassWeights == null ? IntPtr.Zero : ClassWeights.Ptr;
            p.term_crit = TermCrit;
            return p;
         }
      }
   }
}
