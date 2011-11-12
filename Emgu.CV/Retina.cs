using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Drawing;

namespace Emgu.CV
{
   /// <summary>
   /// A wrapper class which allows the Gipsa/Listic Labs model to be used.
   /// This retina model allows spatio-temporal image processing (applied on still images, video sequences).
   /// As a summary, these are the retina model properties:
   /// 1. It applies a spectral whithening (mid-frequency details enhancement);
   /// 2. high frequency spatio-temporal noise reduction;
   /// 3. low frequency luminance to be reduced (luminance range compression);
   /// 4. local logarithmic luminance compression allows details to be enhanced in low light conditions.
   /// USE : this model can be used basically for spatio-temporal video effects but also for :
   ///     _using the getParvo method output matrix : texture analysiswith enhanced signal to noise ratio and enhanced details robust against input images luminance ranges
   ///      _using the getMagno method output matrix : motion analysis also with the previously cited properties
   ///      
   /// For more information, reer to the following papers :
   /// Benoit A., Caplier A., Durette B., Herault, J., "USING HUMAN VISUAL SYSTEM MODELING FOR BIO-INSPIRED LOW LEVEL IMAGE PROCESSING", Elsevier, Computer Vision and Image Understanding 114 (2010), pp. 758-773, DOI: http://dx.doi.org/10.1016/j.cviu.2010.01.011
   /// Vision: Images, Signals and Neural Networks: Models of Neural Processing in Visual Perception (Progress in Neural Processing),By: Jeanny Herault, ISBN: 9814273686. WAPI (Tower ID): 113266891.
   ///
   /// The retina filter includes the research contributions of phd/research collegues from which code has been redrawn by the author :
   /// _take a look at the retinacolor.hpp module to discover Brice Chaix de Lavarene color mosaicing/demosaicing and the reference paper:
   /// B. Chaix de Lavarene, D. Alleysson, B. Durette, J. Herault (2007). "Efficient demosaicing through recursive filtering", IEEE International Conference on Image Processing ICIP 2007
   /// _take a look at imagelogpolprojection.hpp to discover retina spatial log sampling which originates from Barthelemy Durette phd with Jeanny Herault. A Retina / V1 cortex projection is also proposed and originates from Jeanny's discussions.
   /// more informations in the above cited Jeanny Heraults's book.
   /// </summary>
   public class Retina : UnmanagedObject
   {
      #region pinvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr CvRetinaCreate(
         Size inputSize,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool colorMode,
         ColorSamplingMethod colorSamplingMethod,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useRetinaLogSampling,
         double reductionFactor,
         double samplingStrength);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void CvRetinaRun(IntPtr retina, IntPtr image);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void CvRetinaGetParvo(IntPtr retina, IntPtr parvo);


      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void CvRetinaGetMagno(IntPtr retina, IntPtr magno);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void CvRetinaRelease(ref IntPtr retina);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void CvRetinaClearBuffers(IntPtr retina);
      #endregion

      private Size _inputSize;

      public Retina(Size inputSize, bool colorMode, ColorSamplingMethod colorSamplingMethod, bool useRetinaLogSampling, double reductionFactor, double samplingStrength)
      {
         _inputSize = inputSize;
         _ptr = CvRetinaCreate(inputSize, colorMode, colorSamplingMethod, useRetinaLogSampling, reductionFactor, samplingStrength);
      }

      /// <summary>
      /// Method which allows retina to be applied on an input image, after run, encapsulated retina module is ready to deliver its outputs using dedicated acccessors. <seealso cref="GetParvo()"/> and <seealso cref="GetMagno()"/>
      /// </summary>
      /// <param name="image">The input image to be processed</param>
      public void Run(Image<Bgr, byte> image)
      {
         CvRetinaRun(_ptr, image);
      }

      /// <summary>
      /// Accessor of the details channel of the retina (models foveal vision)
      /// </summary>
      /// <returns>The details channel of the retina.</returns>
      public Image<Bgr, Byte> GetParvo()
      {
         if (_ptr == IntPtr.Zero)
            return null;

         Image<Bgr, byte> parvo = new Image<Bgr, byte>(_inputSize);
         CvRetinaGetParvo(_ptr, parvo);
         return parvo;
      }

      /// <summary>
      /// Accessor of the motion channel of the retina (models peripheral vision)
      /// </summary>
      /// <returns>The motion channel of the retina.</returns>
      public Image<Gray, byte> GetMagno()
      {
         if (_ptr == IntPtr.Zero)
            return null;

         Image<Gray, Byte> magno = new Image<Gray, byte>(_inputSize);
         CvRetinaGetMagno(_ptr, magno);
         return magno;
      }

      /// <summary>
      /// Clear all retina buffers (equivalent to opening the eyes after a long period of eye close.
      /// </summary>
      public void ClearBuffers()
      {
         CvRetinaClearBuffers(_ptr);
      }

      /// <summary>
      /// Release all unmanaged memory associated with the retina model.
      /// </summary>
      protected override void DisposeObject()
      {
         CvRetinaRelease(ref _ptr);
      }

      /// <summary>
      /// The retina color sampling method.
      /// </summary>
      public enum ColorSamplingMethod
      {
         /// <summary>
         /// Each pixel position is either R, G or B in a random choice
         /// </summary>
         ColorRandom,
         /// <summary>
         /// Color sampling is RGBRGBRGB..., line 2 BRGBRGBRG..., line 3, GBRGBRGBR...
         /// </summary>
         ColorDiagonal,
         /// <summary>
         /// Standard bayer sampling
         /// </summary>
         ColorBayer
      }
   }
}
