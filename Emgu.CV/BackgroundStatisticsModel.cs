using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// Create a background statistics model
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

      private delegate int UpdateFunctionDelagate(IntPtr img, IntPtr statModel);
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
            MCvBGStatModel statModel = MCvBGStatModel;
            System.Drawing.Size size = CvInvoke.cvGetSize(statModel.background);
            Image<Bgr, Byte> res = new Image<Bgr, byte>(size.Width, size.Height);
            CvInvoke.cvCopy(statModel.background, res.Ptr, IntPtr.Zero);
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
            MCvBGStatModel statModel = MCvBGStatModel;
            System.Drawing.Size size = CvInvoke.cvGetSize(statModel.background);
            Image<Gray, Byte> res = new Image<Gray, byte>(size.Width, size.Height);
            CvInvoke.cvCopy(statModel.foreground, res.Ptr, IntPtr.Zero);
            return res;
         }
      }

      private delegate void ReleaseFunction(ref IntPtr ptr);

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

namespace Emgu.CV.CvEnum
{
   /// <summary>
   /// The type of BGStatModel
   /// </summary>
   public enum BG_STAT_TYPE
   {
      /// <summary>
      /// 
      /// </summary>
      FGD_STAT_MODEL,
      /// <summary>
      /// Gaussian background model
      /// </summary>
      GAUSSIAN_BG_MODEL
   }
}
