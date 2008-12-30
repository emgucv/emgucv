using System;
using System.Collections.Generic;
using System.Text;
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
      /// for poly
      /// </summary>
      private double _degree;
      /// <summary>
      /// for poly/rbf/sigmoid
      /// </summary>
      private double _gamma;
      /// <summary>
      /// for poly/sigmoid
      /// </summary>
      private double _coef0;

      /// <summary>
      /// for CV_SVM_C_SVC, CV_SVM_EPS_SVR and CV_SVM_NU_SVR
      /// </summary>
      private double _c;
      /// <summary>
      /// for CV_SVM_NU_SVC, CV_SVM_ONE_CLASS, and CV_SVM_NU_SVR
      /// </summary>
      private double _nu;
      /// <summary>
      /// for CV_SVM_EPS_SVR
      /// </summary>
      private double _p;
      /// <summary>
      /// for CV_SVM_C_SVC
      /// </summary>
      private Matrix<float> _classWeights;
      /// <summary>
      /// termination criteria
      /// </summary>
      private MCvTermCriteria _termCrit;

      /// <summary>
      /// The type of SVM
      /// </summary>
      public MlEnum.SVM_TYPE SvmType
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
      /// for poly
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
      /// for poly/rbf/sigmoid
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
      /// for poly/sigmoid
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
      /// for CV_SVM_C_SVC, CV_SVM_EPS_SVR and CV_SVM_NU_SVR
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
      /// for CV_SVM_NU_SVC, CV_SVM_ONE_CLASS, and CV_SVM_NU_SVR
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
      /// for CV_SVM_EPS_SVR
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
      /// for CV_SVM_C_SVC
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
      /// termination criteria
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
            p.svm_type = SvmType;
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
