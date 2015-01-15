//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed CvKalman structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvKalman
   {
      ///<summary>
      ///  number of measurement vector dimensions 
      ///</summary>
      public int MP;

      ///<summary>
      ///  number of state vector dimensions 
      ///</summary>
      public int DP;

      ///<summary>
      ///  number of control vector dimensions 
      ///</summary>
      public int CP;

      #region backward compatibility fields

      ///<summary>
      ///  =state_pre->data.fl 
      ///</summary>
      public IntPtr PosterState;
      ///<summary>
      ///  =state_post->data.fl 
      ///</summary>
      public IntPtr PriorState;
      ///<summary>
      ///  =transition_matrix->data.fl 
      ///</summary>
      public IntPtr DynamMatr;
      ///<summary>
      ///  =measurement_matrix->data.fl 
      ///</summary>
      public IntPtr MeasurementMatr;
      ///<summary>
      ///  =measurement_noise_cov->data.fl 
      ///</summary>
      public IntPtr MNCovariance;
      ///<summary>
      ///  =process_noise_cov->data.fl 
      ///</summary>
      public IntPtr PNCovariance;
      ///<summary>
      ///  =gain->data.fl 
      ///</summary>
      public IntPtr KalmGainMatr;
      ///<summary>
      ///  =error_cov_pre->data.fl 
      ///</summary>
      public IntPtr PriorErrorCovariance;
      ///<summary>
      ///  =error_cov_post->data.fl 
      ///</summary>
      public IntPtr PosterErrorCovariance;
      ///<summary>
      ///  temp1->data.fl 
      ///</summary>
      public IntPtr Temp1Data;
      ///<summary>
      ///  temp2->data.fl 
      ///</summary>
      public IntPtr Temp2Data;
      #endregion
      
      ///<summary>
      ///  predicted state (x'(k)):
      ///                                    x(k)=A*x(k-1)+B*u(k) 
      ///</summary>
      public IntPtr state_pre;
      ///<summary>
      ///  corrected state (x(k)):
      ///                                    x(k)=x'(k)+K(k)*(z(k)-H*x'(k)) 
      ///</summary>
      public IntPtr state_post;
      ///<summary>
      ///  state transition matrix (A) 
      ///</summary>
      public IntPtr transition_matrix;
      ///<summary>
      ///  control matrix (B)
      ///                                   (it is not used if there is no control)
      ///</summary>
      public IntPtr control_matrix;
      ///<summary>
      ///  measurement matrix (H) 
      ///</summary>
      public IntPtr measurement_matrix;
      ///<summary>
      ///  process noise covariance matrix (Q) 
      ///</summary>
      public IntPtr process_noise_cov;
      ///<summary>
      ///  measurement noise covariance matrix (R) 
      ///</summary>
      public IntPtr measurement_noise_cov;
      ///<summary>
      ///  priori error estimate covariance matrix P'(k)=A*P(k-1)*At + Q)
      ///</summary>
      public IntPtr error_cov_pre;
      ///<summary>
      ///  Kalman gain matrix (K(k)):
      ///                                    K(k)=P'(k)*Ht*inv(H*P'(k)*Ht+R)
      ///</summary>
      public IntPtr gain;
      ///<summary>
      ///  posteriori error estimate covariance matrix P(k)=(I-K(k)*H)*P'(k) 
      ///</summary>
      public IntPtr error_cov_post;
      ///<summary>
      ///  temporary matrices 
      ///</summary>
      public IntPtr temp1;
      ///<summary>
      ///  temporary matrices 
      ///</summary>
      public IntPtr temp2;
      ///<summary>
      ///  temporary matrices 
      ///</summary>
      public IntPtr temp3;
      ///<summary>
      ///  temporary matrices 
      ///</summary>
      public IntPtr temp4;
      ///<summary>
      ///  temporary matrices 
      ///</summary>
      public IntPtr temp5;
   }
}
