using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// Background statistics model
   /// </summary>
   public class BackgroundStatisticsModel : UnmanagedObject
   {
      /// <summary>
      /// Create a BGStatModel
      /// </summary>
      /// <param name="image">The image used for initiating the statistic model</param>
      /// <param name="type">The type of the statistics model</param>
      public BackgroundStatisticsModel(Image<Bgr, Byte> image, Emgu.CV.CvEnum.BG_STAT_TYPE type)
      {
         _ptr = (type == Emgu.CV.CvEnum.BG_STAT_TYPE.FGD_STAT_MODEL) ?
            CvInvoke.cvCreateFGDStatModel(image, IntPtr.Zero)
            : CvInvoke.cvCreateGaussianBGModel(image, IntPtr.Zero);
      }

      /// <summary>
      /// Create a forground statistic model using the given parameters
      /// </summary>
      /// <param name="image">The image used for initiating the statistic model</param>
      /// <param name="parameters">FGDStatModel</param>
      public BackgroundStatisticsModel(Image<Bgr, Byte> image, ref MCvFGDStatModelParams parameters)
      {
         _ptr = CvInvoke.cvCreateFGDStatModel(image, ref parameters);
      }

      /// <summary>
      /// Define the Release function
      /// </summary>
      /// <param name="ptr">The background mode to be released</param>
      private delegate void ReleaseFunction(ref IntPtr ptr);

      private delegate int UpdateFunctionDelagate(IntPtr img, IntPtr statModel);

      /// <summary>
      /// A cache of the update function
      /// </summary>
      private UpdateFunctionDelagate updateFunction;

      /// <summary>
      /// Update the statistic model
      /// </summary>
      /// <param name="image"></param>
      public void Update(Image<Bgr, Byte> image)
      {
         if (updateFunction == null)
         {
            updateFunction = (UpdateFunctionDelagate)Marshal.GetDelegateForFunctionPointer(MCvBGStatModel.CvUpdateBGStatModel, typeof(UpdateFunctionDelagate));
         }
         updateFunction(image.Ptr, _ptr);
      }

      /// <summary>
      /// Get the MCvBGStatModel structure
      /// </summary>
      public MCvBGStatModel MCvBGStatModel
      {
         get
         {
            return (MCvBGStatModel)Marshal.PtrToStructure(_ptr, typeof(MCvBGStatModel));
         }
      }

      /// <summary>
      /// Get a copy of the current background
      /// </summary>
      public Image<Bgr, Byte> Background
      {
         get
         {
            IntPtr statModelbackground = MCvBGStatModel.background;
            Image<Bgr, Byte> res = new Image<Bgr, byte>(CvInvoke.cvGetSize(statModelbackground));
            CvInvoke.cvCopy(statModelbackground, res.Ptr, IntPtr.Zero);
            return res;
         }
      }

      /// <summary>
      /// Get a copy of the mask for the current forground
      /// </summary>
      public Image<Gray, Byte> Foreground
      {
         get
         {
            IntPtr statModelforeground = MCvBGStatModel.foreground;
            Image<Gray, Byte> res = new Image<Gray, byte>(CvInvoke.cvGetSize(statModelforeground));
            CvInvoke.cvCopy(statModelforeground, res.Ptr, IntPtr.Zero);
            return res;
         }
      }

      /// <summary>
      /// Release the BGStatModel and all the unmanaged memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         ReleaseFunction releaseFunction = (ReleaseFunction)Marshal.GetDelegateForFunctionPointer(MCvBGStatModel.CvReleaseBGStatModel, typeof(ReleaseFunction));
         releaseFunction(ref _ptr);
      }
   }
}
