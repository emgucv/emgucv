//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Util;

namespace Emgu.CV.Bioinspired
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
    public class Retina : SharedPtrObject
    {
        /// <summary>
        /// Create a retina model
        /// </summary>
        /// <param name="inputSize">The input frame size</param>
        public Retina(Size inputSize)
           : this(inputSize, true, ColorSamplingMethod.ColorBayer, false, 1.0, 10.0)
        {
        }

        /// <summary>
        /// Create a retina model
        /// </summary>
        /// <param name="inputSize">The input frame size</param>
        /// <param name="colorMode">Specifies if (true) color is processed of not (false) to then processing gray level image</param>
        /// <param name="colorSamplingMethod">Specifies which kind of color sampling will be used</param>
        /// <param name="useRetinaLogSampling">Activate retina log sampling, if true, the 2 following parameters can be used</param>
        /// <param name="reductionFactor">Only useful if param useRetinaLogSampling=true, specifies the reduction factor of the output frame (as the center (fovea) is high resolution and corners can be underscaled, then a reduction of the output is allowed without precision leak</param>
        /// <param name="samplingStrength">Only useful if param useRetinaLogSampling=true, specifies the strength of the log scale that is applied</param>
        public Retina(Size inputSize, bool colorMode, ColorSamplingMethod colorSamplingMethod, bool useRetinaLogSampling, double reductionFactor, double samplingStrength)
        {
            _ptr = BioinspiredInvoke.cveRetinaCreate(ref inputSize, colorMode, colorSamplingMethod, useRetinaLogSampling, reductionFactor, samplingStrength, ref _sharedPtr);
        }

        /// <summary>
        /// Get or Set the Retina parameters.
        /// </summary>
        public RetinaParameters Parameters
        {
            get
            {
                RetinaParameters p = new RetinaParameters();
                BioinspiredInvoke.cveRetinaGetParameters(_ptr, ref p);
                return p;
            }
            set
            {
                BioinspiredInvoke.cveRetinaSetParameters(_ptr, ref value);
            }
        }

        /// <summary>
        /// Method which allows retina to be applied on an input image, after run, encapsulated retina module is ready to deliver its outputs using dedicated acccessors. <seealso cref="GetParvo"/> and <seealso cref="GetMagno"/>
        /// </summary>
        /// <param name="image">The input image to be processed</param>
        public void Run(IInputArray image)
        {
            using (InputArray iaImage = image.GetInputArray())
                BioinspiredInvoke.cveRetinaRun(_ptr, iaImage);
        }

        /// <summary>
        /// Accessors of the details channel of the retina (models foveal vision)
        /// </summary>
        /// <param name="parvo">The details channel of the retina.</param>
        public void GetParvo(IOutputArray parvo)
        {
            if (_ptr != IntPtr.Zero)
            {
                using (OutputArray oaParvo = parvo.GetOutputArray())
                    BioinspiredInvoke.cveRetinaGetParvo(_ptr, oaParvo);
            }
        }

        /// <summary>
        /// Accessors of the motion channel of the retina (models peripheral vision)
        /// </summary>
        /// <param name="magno">The motion channel of the retina.</param>
        public void GetMagno(IOutputArray magno)
        {
            if (_ptr != IntPtr.Zero)
            {
                using (OutputArray oaMagno = magno.GetOutputArray())
                    BioinspiredInvoke.cveRetinaGetMagno(_ptr, oaMagno);
            }
        }

        /// <summary>
        /// Clear all retina buffers (equivalent to opening the eyes after a long period of eye close.
        /// </summary>
        public void ClearBuffers()
        {
            BioinspiredInvoke.cveRetinaClearBuffers(_ptr);
        }

        /// <summary>
        /// Release all unmanaged memory associated with the retina model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                BioinspiredInvoke.cveRetinaRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
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

        /// <summary>
        /// Outer Plexiform Layer (OPL) and Inner Plexiform Layer Parvocellular (IplParvo) parameters 
        /// </summary>
        public struct OPLandIplParvoParameters
        {
            /// <summary>
            /// Specifies if (true) color is processed of not (false) to then processing gray level image
            /// </summary>
            [MarshalAs(CvInvoke.BoolMarshalType)]
            public bool ColorMode;

            /// <summary>
            /// Normalise output. Use true for default
            /// </summary>
            [MarshalAs(CvInvoke.BoolMarshalType)]
            public bool NormaliseOutput;

            /// <summary>
            /// Photoreceptors local adaptation sensitivity. Use 0.7 for default
            /// </summary>
            public float PhotoreceptorsLocalAdaptationSensitivity;

            /// <summary>
            /// Photoreceptors temporal constant. Use 0.5 for default
            /// </summary>
            public float PhotoreceptorsTemporalConstant;

            /// <summary>
            /// Photoreceptors spatial constant. Use 0.53 for default
            /// </summary>
            public float PhotoreceptorsSpatialConstant;

            /// <summary>
            /// Horizontal cells gain. Use 0.0 for default
            /// </summary>
            public float HorizontalCellsGain;

            /// <summary>
            /// Hcells temporal constant. Use 1.0 for default
            /// </summary>
            public float HcellsTemporalConstant;

            /// <summary>
            /// Hcells spatial constant. Use 7.0 for default
            /// </summary>
            public float HcellsSpatialConstant;

            /// <summary>
            /// Ganglion cells sensitivity. Use 0.7 for default
            /// </summary>
            public float GanglionCellsSensitivity;
        }

        /// <summary>
        /// Inner Plexiform Layer Magnocellular channel (IplMagno)
        /// </summary>
        public struct IplMagnoParameters
        {
            /// <summary>
            /// Normalise output
            /// </summary>
            [MarshalAs(CvInvoke.BoolMarshalType)]
            public bool NormaliseOutput;

            /// <summary>
            /// ParasolCells_beta. Use 0.0 for default
            /// </summary>
            public float ParasolCellsBeta;
            /// <summary>
            /// ParasolCells_tau. Use 0.0 for default
            /// </summary>
            public float ParasolCellsTau;
            /// <summary>
            /// ParasolCells_k. Use 7.0 for default
            /// </summary>
            public float ParasolCellsK;
            /// <summary>
            /// Amacrin cells temporal cut frequency. Use 1.2 for default
            /// </summary>
            public float AmacrinCellsTemporalCutFrequency;
            /// <summary>
            /// V0 compression parameter. Use 0.95 for default
            /// </summary>
            public float V0CompressionParameter;
            /// <summary>
            /// LocalAdaptintegration_tau. Use 0.0 for default
            /// </summary>
            public float LocalAdaptintegrationTau;
            /// <summary>
            /// LocalAdaptintegration_k. Use 7.0 for default
            /// </summary>
            public float LocalAdaptintegrationK;
        }

        /// <summary>
        /// Retina parameters
        /// </summary>
        public struct RetinaParameters
        {
            /// <summary>
            /// Outer Plexiform Layer (OPL) and Inner Plexiform Layer Parvocellular (IplParvo) parameters 
            /// </summary>
            public OPLandIplParvoParameters OPLandIplParvo;
            /// <summary>
            /// Inner Plexiform Layer Magnocellular channel (IplMagno)
            /// </summary>
            public IplMagnoParameters IplMagno;
        }
    }


}
