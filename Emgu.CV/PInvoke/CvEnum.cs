//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
   /// <summary>
   /// Type for cvNorm
   /// </summary>
   [Flags]
   public enum NORM_TYPE
   {
      /// <summary>
      /// if arr2 is NULL, norm = ||arr1||_C = max_I abs(arr1(I));
      /// if arr2 is not NULL, norm = ||arr1-arr2||_C = max_I abs(arr1(I)-arr2(I))
      /// </summary>
      CV_C = 1,
      /// <summary>
      /// if arr2 is NULL, norm = ||arr1||_L1 = sum_I abs(arr1(I));
      /// if arr2 is not NULL, norm = ||arr1-arr2||_L1 = sum_I abs(arr1(I)-arr2(I))
      /// </summary>
      CV_L1 = 2,
      /// <summary>
      /// if arr2 is NULL, norm = ||arr1||_L2 = sqrt( sum_I arr1(I)^2);
      /// if arr2 is not NULL, norm = ||arr1-arr2||_L2 = sqrt( sum_I (arr1(I)-arr2(I))^2 )
      /// </summary>
      CV_L2 = 4,
      /// <summary>
      /// 
      /// </summary>
      CV_NORM_MASK = 7,
      /// <summary>
      /// It is used in combination with either CV_C, CV_L1 or CV_L2
      /// </summary>
      CV_RELATIVE = 8,
      /// <summary>
      /// It is used in combination with either CV_C, CV_L1 or CV_L2
      /// </summary>
      CV_DIFF = 16,
      /// <summary>
      /// 
      /// </summary>
      CV_MINMAX = 32,
      /// <summary>
      /// 
      /// </summary>
      CV_DIFF_C = (CV_DIFF | CV_C),
      /// <summary>
      /// 
      /// </summary>
      CV_DIFF_L1 = (CV_DIFF | CV_L1),
      /// <summary>
      /// 
      /// </summary>
      CV_DIFF_L2 = (CV_DIFF | CV_L2),
      /// <summary>
      /// norm = ||arr1-arr2||_C/||arr2||_C
      /// </summary>
      CV_RELATIVE_C = (CV_RELATIVE | CV_C),
      /// <summary>
      /// norm = ||arr1-arr2||_L1/||arr2||_L1
      /// </summary>
      CV_RELATIVE_L1 = (CV_RELATIVE | CV_L1),
      /// <summary>
      /// norm = ||arr1-arr2||_L2/||arr2||_L2
      /// </summary>
      CV_RELATIVE_L2 = (CV_RELATIVE | CV_L2)
   }

   /// <summary>
   /// Type used for cvReduce function
   /// </summary>
   public enum REDUCE_TYPE
   {
      /// <summary>
      /// The output is the sum of all the matrix rows/columns
      /// </summary>
      CV_REDUCE_SUM = 0,
      /// <summary>
      /// The output is the mean vector of all the matrix rows/columns
      /// </summary>
      CV_REDUCE_AVG = 1,
      /// <summary>
      /// The output is the maximum (column/row-wise) of all the matrix rows/columns
      /// </summary>
      CV_REDUCE_MAX = 2,
      /// <summary>
      /// The output is the minimum (column/row-wise) of all the matrix rows/columns
      /// </summary>
      CV_REDUCE_MIN = 3
   }

   /// <summary>
   /// Type used for cvReduce function
   /// </summary>
   public enum REDUCE_DIMENSION
   {
      /// <summary>
      /// The matrix is reduced to a single row
      /// </summary>
      SINGLE_ROW = 0,
      /// <summary>
      /// The matrix is reduced to a single column
      /// </summary>
      SINGLE_COL = 1,
      /// <summary>
      /// The dimension is chosen automatically by analysing the dst size
      /// </summary>
      AUTO = -1,
   }

   /// <summary>
   /// Type used for cvCmp function
   /// </summary>
   public enum CMP_TYPE
   {
      /// <summary>
      /// src1(I) "equal to" src2(I)
      /// </summary>
      CV_CMP_EQ = 0,
      /// <summary>
      /// src1(I) "greater than" src2(I)
      /// </summary>
      CV_CMP_GT = 1,
      /// <summary>
      /// src1(I) "greater or equal" src2(I)
      /// </summary>
      CV_CMP_GE = 2,
      /// <summary>
      /// src1(I) "less than" src2(I)
      /// </summary>
      CV_CMP_LT = 3,
      /// <summary>
      /// src1(I) "less or equal" src2(I)
      /// </summary>
      CV_CMP_LE = 4,
      /// <summary>
      /// src1(I) "not equal to" src2(I)
      /// </summary>
      CV_CMP_NE = 5
   }

   /// <summary>
   /// Polygon approximation type
   /// </summary>
   public enum APPROX_POLY_TYPE
   {
      /// <summary>
      /// Douglas-Peucker algorithm
      /// </summary>
      CV_POLY_APPROX_DP = 0
   }

   /// <summary>
   /// CV Capture property identifier
   /// </summary>
   public enum CAP_PROP
   {
      /// <summary>
      /// film current position in milliseconds or video capture timestamp
      /// </summary>
      CV_CAP_PROP_POS_MSEC = 0,
      /// <summary>
      /// 0-based index of the frame to be decoded/captured next
      /// </summary>
      CV_CAP_PROP_POS_FRAMES = 1,
      /// <summary>
      /// position in relative units (0 - start of the file, 1 - end of the file)
      /// </summary>
      CV_CAP_PROP_POS_AVI_RATIO = 2,
      /// <summary>
      /// width of frames in the video stream
      /// </summary>
      CV_CAP_PROP_FRAME_WIDTH = 3,
      /// <summary>
      /// height of frames in the video stream
      /// </summary>
      CV_CAP_PROP_FRAME_HEIGHT = 4,
      /// <summary>
      /// frame rate 
      /// </summary>
      CV_CAP_PROP_FPS = 5,
      /// <summary>
      /// 4-character code of codec
      /// </summary>
      CV_CAP_PROP_FOURCC = 6,
      /// <summary>
      /// number of frames in video file
      /// </summary>
      CV_CAP_PROP_FRAME_COUNT = 7,
      /// <summary>
      /// 
      /// </summary>
      CV_CAP_PROP_FORMAT = 8,
      /// <summary>
      /// 
      /// </summary>
      CV_CAP_PROP_MODE = 9,
      /// <summary>
      /// Brightness
      /// </summary>
      CV_CAP_PROP_BRIGHTNESS = 10,
      /// <summary>
      /// Contrast
      /// </summary>
      CV_CAP_PROP_CONTRAST = 11,
      /// <summary>
      /// Saturation
      /// </summary>
      CV_CAP_PROP_SATURATION = 12,
      /// <summary>
      /// Hue
      /// </summary>
      CV_CAP_PROP_HUE = 13,
      /// <summary>
      /// Gain
      /// </summary>
      CV_CAP_PROP_GAIN = 14,
      /// <summary>
      /// Exposure
      /// </summary>
      CV_CAP_PROP_EXPOSURE = 15,
      /// <summary>
      /// Convert RGB
      /// </summary>
      CV_CAP_PROP_CONVERT_RGB = 16,
      /// <summary>
      /// White balance blue u
      /// </summary>
      CV_CAP_PROP_WHITE_BALANCE_BLUE_U = 17,
      /// <summary>
      /// Rectification
      /// </summary>
      CV_CAP_PROP_RECTIFICATION = 18,
      /// <summary>
      /// Monocrome
      /// </summary>
      CV_CAP_PROP_MONOCROME = 19,
      /// <summary>
      /// Sharpness
      /// </summary>
      CV_CAP_PROP_SHARPNESS = 20,
      /// <summary>
      /// Exposure control done by camera, user can adjust refernce level using this feature
      /// </summary>
      CV_CAP_PROP_AUTO_EXPOSURE = 21,
      /// <summary>
      /// Gamma
      /// </summary>
      CV_CAP_PROP_GAMMA = 22,
      /// <summary>
      /// Temperature
      /// </summary>
      CV_CAP_PROP_TEMPERATURE = 23,
      /// <summary>
      /// Trigger
      /// </summary>
      CV_CAP_PROP_TRIGGER = 24,
      /// <summary>
      /// Trigger delay
      /// </summary>
      CV_CAP_PROP_TRIGGER_DELAY = 25,
      /// <summary>
      /// White balance red v
      /// </summary>
      CV_CAP_PROP_WHITE_BALANCE_RED_V = 26,
      /// <summary>
      /// Max DC1394
      /// </summary>
      CV_CAP_PROP_MAX_DC1394 = 27,
      /// <summary>
      /// property for highgui class CvCapture_Android only
      /// </summary>
      CV_CAP_PROP_AUTOGRAB = 1024,
      /// <summary>
      /// tricky property, returns cpnst char* indeed
      /// </summary>
      CV_CAP_PROP_SUPPORTED_PREVIEW_SIZES_STRING = 1025,
      /// <summary>
      /// OpenNI map generators
      /// </summary>
      CV_CAP_OPENNI_DEPTH_GENERATOR = 0,
      /// <summary>
      /// OpenNI map generators
      /// </summary>
      CV_CAP_OPENNI_IMAGE_GENERATOR = 1 << 31,
      /// <summary>
      /// OpenNI map generators
      /// </summary>
      CV_CAP_OPENNI_GENERATORS_MASK = 1 << 31,

      /// <summary>
      /// Properties of cameras available through OpenNI interfaces
      /// </summary>
      CV_CAP_PROP_OPENNI_OUTPUT_MODE = 100,
      /// <summary>
      /// Properties of cameras available through OpenNI interfaces, in mm.
      /// </summary>
      CV_CAP_PROP_OPENNI_FRAME_MAX_DEPTH = 101,
      /// <summary>
      /// Properties of cameras available through OpenNI interfaces, in mm.
      /// </summary>
      CV_CAP_PROP_OPENNI_BASELINE = 102,
      /// <summary>
      /// Properties of cameras available through OpenNI interfaces, in pixels.
      /// </summary>
      CV_CAP_PROP_OPENNI_FOCAL_LENGTH = 103,
      /// <summary>
      /// Image generator output mode
      /// </summary>
      CV_CAP_OPENNI_IMAGE_GENERATOR_OUTPUT_MODE = CV_CAP_OPENNI_IMAGE_GENERATOR + CV_CAP_PROP_OPENNI_OUTPUT_MODE,
      /// <summary>
      /// Depth generator baseline, in mm.
      /// </summary>
      CV_CAP_OPENNI_DEPTH_GENERATOR_BASELINE = CV_CAP_OPENNI_DEPTH_GENERATOR + CV_CAP_PROP_OPENNI_BASELINE,
      /// <summary>
      /// Depth generator focal lenght, in pixels.
      /// </summary>
      CV_CAP_OPENNI_DEPTH_GENERATOR_FOCAL_LENGTH = CV_CAP_OPENNI_DEPTH_GENERATOR + CV_CAP_PROP_OPENNI_FOCAL_LENGTH,

      /// <summary>
      /// Properties of cameras available through GStreamer interface. Default is 1
      /// </summary>
      CV_CAP_GSTREAMER_QUEUE_LENGTH = 200
   }

   /// <summary>
   /// contour approximation method
   /// </summary>
   public enum CHAIN_APPROX_METHOD : int
   {
      /// <summary>
      /// output contours in the Freeman chain code. All other methods output polygons (sequences of vertices). 
      /// </summary>
      CV_CHAIN_CODE = 0,
      /// <summary>
      /// translate all the points from the chain code into points;
      /// </summary>
      CV_CHAIN_APPROX_NONE = 1,
      /// <summary>
      /// compress horizontal, vertical, and diagonal segments, that is, the function leaves only their ending points; 
      /// </summary>
      CV_CHAIN_APPROX_SIMPLE = 2,
      /// <summary>
      /// 
      /// </summary>
      CV_CHAIN_APPROX_TC89_L1 = 3,
      /// <summary>
      /// apply one of the flavors of Teh-Chin chain approximation algorithm
      /// </summary>
      CV_CHAIN_APPROX_TC89_KCOS = 4,
      /// <summary>
      /// use completely different contour retrieval algorithm via linking of horizontal segments of 1s. Only CV_RETR_LIST retrieval mode can be used with this method
      /// </summary>
      CV_LINK_RUNS = 5
   }

   /// <summary>
   /// Color Conversion code
   /// </summary>
   public enum COLOR_CONVERSION
   {
      ///<summary>
      ///Convert BGR color to BGRA color
      ///</summary>
      CV_BGR2BGRA = 0,
      /// <summary>
      /// Convert RGB color to RGBA color
      /// </summary>
      CV_RGB2RGBA = CV_BGR2BGRA,

      ///<summary>
      ///Convert BGRA color to BGR color
      ///</summary>
      CV_BGRA2BGR = 1,
      /// <summary>
      /// Convert RGBA color to RGB color
      /// </summary>
      CV_RGBA2RGB = CV_BGRA2BGR,
      /// <summary>
      /// Convert BGR color to RGBA color
      /// </summary>
      CV_BGR2RGBA = 2,
      /// <summary>
      /// Convert RGB color to BGRA color
      /// </summary>
      CV_RGB2BGRA = CV_BGR2RGBA,
      /// <summary>
      /// Convert RGBA color to BGR color
      /// </summary>
      CV_RGBA2BGR = 3,
      /// <summary>
      /// Convert BGRA color to RGB color
      /// </summary>
      CV_BGRA2RGB = CV_RGBA2BGR,
      /// <summary>
      /// Convert BGR color to RGB color
      /// </summary>
      CV_BGR2RGB = 4,
      /// <summary>
      /// Convert RGB color to BGR color
      /// </summary>
      CV_RGB2BGR = CV_BGR2RGB,
      /// <summary>
      /// Convert BGRA color to RGBA color
      /// </summary>
      CV_BGRA2RGBA = 5,
      /// <summary>
      /// Convert RGBA color to BGRA color
      /// </summary>
      CV_RGBA2BGRA = CV_BGRA2RGBA,
      ///<summary>
      ///Convert BGR color to GRAY color
      ///</summary>
      CV_BGR2GRAY = 6,
      /// <summary>
      /// Convert RGB color to GRAY color
      /// </summary>
      CV_RGB2GRAY = 7,
      ///<summary>
      ///Convert GRAY color to BGR color
      ///</summary>
      CV_GRAY2BGR = 8,
      /// <summary>
      /// Convert GRAY color to RGB color
      /// </summary>
      CV_GRAY2RGB = CV_GRAY2BGR,
      ///<summary>
      ///Convert GRAY color to BGRA color
      ///</summary>
      CV_GRAY2BGRA = 9,
      /// <summary>
      /// Convert GRAY color to RGBA color
      /// </summary>
      CV_GRAY2RGBA = CV_GRAY2BGRA,
      ///<summary>
      ///Convert BGRA color to GRAY color
      ///</summary>
      CV_BGRA2GRAY = 10,
      /// <summary>
      /// Convert RGBA color to GRAY color
      /// </summary>
      CV_RGBA2GRAY = 11,
      ///<summary>
      ///Convert BGR color to BGR565 color
      ///</summary>
      CV_BGR2BGR565 = 12,
      /// <summary>
      /// Convert RGB color to BGR565 color
      /// </summary>
      CV_RGB2BGR565 = 13,
      ///<summary>
      ///Convert BGR565 color to BGR color
      ///</summary>
      CV_BGR5652BGR = 14,
      /// <summary>
      /// Convert BGR565 color to RGB color
      /// </summary>
      CV_BGR5652RGB = 15,
      ///<summary>
      ///Convert BGRA color to BGR565 color
      ///</summary>
      CV_BGRA2BGR565 = 16,
      /// <summary>
      /// Convert RGBA color to BGR565 color
      /// </summary>
      CV_RGBA2BGR565 = 17,
      ///<summary>
      ///Convert BGR565 color to BGRA color
      ///</summary>
      CV_BGR5652BGRA = 18,
      /// <summary>
      /// Convert BGR565 color to RGBA color
      /// </summary>
      CV_BGR5652RGBA = 19,
      ///<summary>
      ///Convert GRAY color to BGR565 color
      ///</summary>
      CV_GRAY2BGR565 = 20,
      ///<summary>
      ///Convert BGR565 color to GRAY color
      ///</summary>
      CV_BGR5652GRAY = 21,
      ///<summary>
      ///Convert BGR color to BGR555 color
      ///</summary>
      CV_BGR2BGR555 = 22,
      /// <summary>
      /// Convert RGB color to BGR555 color
      /// </summary>
      CV_RGB2BGR555 = 23,
      ///<summary>
      ///Convert BGR555 color to BGR color
      ///</summary>
      CV_BGR5552BGR = 24,
      /// <summary>
      /// Convert BGR555 color to RGB color
      /// </summary>
      CV_BGR5552RGB = 25,
      ///<summary>
      ///Convert BGRA color to BGR555 color
      ///</summary>
      CV_BGRA2BGR555 = 26,
      /// <summary>
      /// Convert RGBA color to BGR555 color
      /// </summary>
      CV_RGBA2BGR555 = 27,
      ///<summary>
      ///Convert BGR555 color to BGRA color
      ///</summary>
      CV_BGR5552BGRA = 28,
      /// <summary>
      /// Convert BGR555 color to RGBA color
      /// </summary>
      CV_BGR5552RGBA = 29,
      ///<summary>
      ///Convert GRAY color to BGR555 color
      ///</summary>
      CV_GRAY2BGR555 = 30,
      ///<summary>
      ///Convert BGR555 color to GRAY color
      ///</summary>
      CV_BGR5552GRAY = 31,
      ///<summary>
      ///Convert BGR color to XYZ color
      ///</summary>
      CV_BGR2XYZ = 32,
      /// <summary>
      /// Convert RGB color to XYZ color
      /// </summary>
      CV_RGB2XYZ = 33,
      ///<summary>
      ///Convert XYZ color to BGR color
      ///</summary>
      CV_XYZ2BGR = 34,
      /// <summary>
      /// Convert XYZ color to RGB color
      /// </summary>
      CV_XYZ2RGB = 35,
      ///<summary>
      ///Convert BGR color to YCrCb color
      ///</summary>
      CV_BGR2YCrCb = 36,
      /// <summary>
      /// Convert RGB color to YCrCb color
      /// </summary>
      CV_RGB2YCrCb = 37,
      ///<summary>
      ///Convert YCrCb color to BGR color
      ///</summary>
      CV_YCrCb2BGR = 38,
      /// <summary>
      /// Convert YCrCb color to RGB color
      /// </summary>
      CV_YCrCb2RGB = 39,
      ///<summary>
      ///Convert BGR color to HSV color
      ///</summary>
      CV_BGR2HSV = 40,
      /// <summary>
      /// Convert RGB colot to HSV color
      /// </summary>
      CV_RGB2HSV = 41,
      ///<summary>
      ///Convert BGR color to Lab color
      ///</summary>
      CV_BGR2Lab = 44,
      /// <summary>
      /// Convert RGB color to Lab color
      /// </summary>
      CV_RGB2Lab = 45,
      ///<summary>
      ///Convert BayerBG color to BGR color
      ///</summary>
      CV_BayerBG2BGR = 46,
      ///<summary>
      ///Convert BayerGB color to BGR color
      ///</summary>
      CV_BayerGB2BGR = 47,
      ///<summary>
      ///Convert BayerRG color to BGR color
      ///</summary>
      CV_BayerRG2BGR = 48,
      ///<summary>
      ///Convert BayerGR color to BGR color
      ///</summary>
      CV_BayerGR2BGR = 49,
      /// <summary>
      /// Convert BayerBG color to BGR color
      /// </summary>
      CV_BayerBG2RGB = CV_BayerRG2BGR,
      /// <summary>
      /// Convert BayerRG color to BGR color
      /// </summary>
      CV_BayerGB2RGB = CV_BayerGR2BGR,
      /// <summary>
      /// Convert BayerRG color to RGB color
      /// </summary>
      CV_BayerRG2RGB = CV_BayerBG2BGR,
      /// <summary>
      /// Convert BayerGR color to RGB color
      /// </summary>
      CV_BayerGR2RGB = CV_BayerGB2BGR,
      ///<summary>
      ///Convert BGR color to Luv color
      ///</summary>
      CV_BGR2Luv = 50,
      /// <summary>
      /// Convert RGB color to Luv color
      /// </summary>
      CV_RGB2Luv = 51,
      ///<summary>
      ///Convert BGR color to HLS color
      ///</summary>
      CV_BGR2HLS = 52,
      /// <summary>
      /// Convert RGB color to HLS color
      /// </summary>
      CV_RGB2HLS = 53,
      ///<summary>
      ///Convert HSV color to BGR color
      ///</summary>
      CV_HSV2BGR = 54,
      /// <summary>
      /// Convert HSV color to RGB color
      /// </summary>
      CV_HSV2RGB = 55,
      ///<summary>
      ///Convert Lab color to BGR color
      ///</summary>
      CV_Lab2BGR = 56,
      /// <summary>
      /// Convert Lab color to RGB color
      /// </summary>
      CV_Lab2RGB = 57,
      ///<summary>
      ///Convert Luv color to BGR color
      ///</summary>
      CV_Luv2BGR = 58,
      /// <summary>
      /// Convert Luv color to RGB color
      /// </summary>
      CV_Luv2RGB = 59,
      ///<summary>
      ///Convert HLS color to BGR color
      ///</summary>
      CV_HLS2BGR = 60,
      /// <summary>
      /// Convert HLS color to RGB color
      /// </summary>
      CV_HLS2RGB = 61,
      /// <summary>
      /// Convert BayerBG pattern to BGR color using VNG
      /// </summary>
      CV_BayerBG2BGR_VNG = 62,
      /// <summary>
      /// Convert BayerGB pattern to BGR color using VNG
      /// </summary>
      CV_BayerGB2BGR_VNG = 63,
      /// <summary>
      /// Convert BayerRG pattern to BGR color using VNG
      /// </summary>
      CV_BayerRG2BGR_VNG = 64,
      /// <summary>
      /// Convert BayerGR pattern to BGR color using VNG
      /// </summary>
      CV_BayerGR2BGR_VNG = 65,
      /// <summary>
      /// Convert BayerBG pattern to RGB color using VNG
      /// </summary>
      CV_BayerBG2RGB_VNG = CV_BayerRG2BGR_VNG,
      /// <summary>
      /// Convert BayerGB pattern to RGB color using VNG
      /// </summary>
      CV_BayerGB2RGB_VNG = CV_BayerGR2BGR_VNG,
      /// <summary>
      /// Convert BayerRG pattern to RGB color using VNG
      /// </summary>
      CV_BayerRG2RGB_VNG = CV_BayerBG2BGR_VNG,
      /// <summary>
      /// Convert BayerGR pattern to RGB color using VNG
      /// </summary>
      CV_BayerGR2RGB_VNG = CV_BayerGB2BGR_VNG,

      /// <summary>
      /// Convert BGR to HSV
      /// </summary>
      CV_BGR2HSV_FULL = 66,
      /// <summary>
      /// Convert RGB to HSV
      /// </summary>
      CV_RGB2HSV_FULL = 67,
      /// <summary>
      /// Convert BGR to HLS
      /// </summary>
      CV_BGR2HLS_FULL = 68,
      /// <summary>
      /// Convert RGB to HLS
      /// </summary>
      CV_RGB2HLS_FULL = 69,

      /// <summary>
      /// Convert HSV color to BGR color
      /// </summary>
      CV_HSV2BGR_FULL = 70,
      /// <summary>
      /// Convert HSV color to RGB color
      /// </summary>
      CV_HSV2RGB_FULL = 71,
      /// <summary>
      /// Convert HLS color to BGR color
      /// </summary>
      CV_HLS2BGR_FULL = 72,
      /// <summary>
      /// Convert HLS color to RGB color
      /// </summary>
      CV_HLS2RGB_FULL = 73,

      /// <summary>
      /// Convert sBGR color to Lab color
      /// </summary>
      CV_LBGR2Lab = 74,
      /// <summary>
      /// Convert sRGB color to Lab color
      /// </summary>
      CV_LRGB2Lab = 75,
      /// <summary>
      /// Convert sBGR color to Luv color
      /// </summary>
      CV_LBGR2Luv = 76,
      /// <summary>
      /// Convert sRGB color to Luv color
      /// </summary>
      CV_LRGB2Luv = 77,

      /// <summary>
      /// Convert Lab color to sBGR color
      /// </summary>
      CV_Lab2LBGR = 78,
      /// <summary>
      /// Convert Lab color to sRGB color
      /// </summary>
      CV_Lab2LRGB = 79,
      /// <summary>
      /// Convert Luv color to sBGR color
      /// </summary>
      CV_Luv2LBGR = 80,
      /// <summary>
      /// Convert Luv color to sRGB color
      /// </summary>
      CV_Luv2LRGB = 81,

      /// <summary>
      /// Convert BGR color to YUV
      /// </summary>
      CV_BGR2YUV = 82,
      /// <summary>
      /// Convert RGB color to YUV
      /// </summary>
      CV_RGB2YUV = 83,
      /// <summary>
      /// Convert YUV color to BGR
      /// </summary>
      CV_YUV2BGR = 84,
      /// <summary>
      /// Convert YUV color to RGB
      /// </summary>
      CV_YUV2RGB = 85,

      /// <summary>
      /// The max number, do not use
      /// </summary>
      CV_COLORCVT_MAX = 100
   }

   /// <summary>
   /// Type for cvPyrUp(cvPryDown)
   /// </summary>
   public enum FILTER_TYPE
   {
      /// <summary>
      /// 
      /// </summary>
      CV_GAUSSIAN_5x5 = 7
   }

   /// <summary>
   /// Fonts
   /// </summary>
   public enum FONT
   {
      /// <summary>
      /// HERSHEY_SIMPLEX
      /// </summary>
      CV_FONT_HERSHEY_SIMPLEX = 0,
      /// <summary>
      /// HERSHEY_PLAIN
      /// </summary>
      CV_FONT_HERSHEY_PLAIN = 1,
      /// <summary>
      /// HERSHEY_DUPLEX
      /// </summary>
      CV_FONT_HERSHEY_DUPLEX = 2,
      /// <summary>
      /// HERSHEY_COMPLEX
      /// </summary>
      CV_FONT_HERSHEY_COMPLEX = 3,
      /// <summary>
      /// HERSHEY_TRIPLEX
      /// </summary>
      CV_FONT_HERSHEY_TRIPLEX = 4,
      /// <summary>
      /// HERSHEY_COMPLEX_SMALL
      /// </summary>
      CV_FONT_HERSHEY_COMPLEX_SMALL = 5,
      /// <summary>
      /// HERSHEY_SCRIPT_SIMPLEX
      /// </summary>
      CV_FONT_HERSHEY_SCRIPT_SIMPLEX = 6,
      /// <summary>
      /// HERSHEY_SCRIPT_COMPLEX
      /// </summary>
      CV_FONT_HERSHEY_SCRIPT_COMPLEX = 7
   }

   /// <summary>
   /// Flags used for cvGEMM function
   /// </summary>
   [Flags]
   public enum GEMM_TYPE
   {
      /// <summary>
      /// Do not apply transpose to neither matrices
      /// </summary>
      CV_GEMM_DEFAULT = 0,
      /// <summary>
      /// transpose src1
      /// </summary>
      CV_GEMM_A_T = 1,
      /// <summary>
      /// transpose src2
      /// </summary>
      CV_GEMM_B_T = 2,
      /// <summary>
      /// transpose src3
      /// </summary>
      CV_GEMM_C_T = 4
   }

   /// <summary>
   /// Hough detection type
   /// </summary>
   public enum HOUGH_TYPE
   {
      /// <summary>
      /// Classical or standard Hough transform. Every line is represented by two floating-point numbers (rho, theta), where rho is a distance between (0,0) point and the line, and theta is the angle between x-axis and the normal to the line. Thus, the matrix must be (the created sequence will be) of CV_32FC2 type
      /// </summary>
      CV_HOUGH_STANDARD = 0,
      /// <summary>
      /// Probabilistic Hough transform (more efficient in case if picture contains a few long linear segments). It returns line segments rather than the whole lines. Every segment is represented by starting and ending points, and the matrix must be (the created sequence will be) of CV_32SC4 type
      /// </summary>
      CV_HOUGH_PROBABILISTIC = 1,
      /// <summary>
      /// Multi-scale variant of classical Hough transform. The lines are encoded the same way as in CV_HOUGH_STANDARD
      /// </summary>
      CV_HOUGH_MULTI_SCALE = 2,
      /// <summary>
      /// 
      /// </summary>
      CV_HOUGH_GRADIENT = 3
   }

   /// <summary>
   /// Inpaint type
   /// </summary>
   public enum INPAINT_TYPE : int
   {
      /// <summary>
      /// Navier-Stokes based method.
      /// </summary>
      CV_INPAINT_NS = 0,
      /// <summary>
      /// The method by Alexandru Telea 
      /// </summary>
      CV_INPAINT_TELEA = 1
   }

   /// <summary>
   /// Types for CvResize
   /// </summary>
   public enum INTER
   {
      /// <summary>
      /// Nearest-neighbor interpolation
      /// </summary>
      CV_INTER_NN = 0,
      /// <summary>
      /// Bilinear interpolation
      /// </summary>
      CV_INTER_LINEAR = 1,
      /// <summary>
      /// resampling using pixel area relation. It is the preferred method for image decimation that gives moire-free results. In case of zooming it is similar to CV_INTER_NN method
      /// </summary>
      CV_INTER_CUBIC = 2,
      /// <summary>
      /// Bicubic interpolation
      /// </summary>
      CV_INTER_AREA = 3
   }

   /// <summary>
   /// Interpolation type
   /// </summary>
   public enum SMOOTH_TYPE
   {
      /// <summary>
      /// (simple blur with no scaling) - summation over a pixel param1xparam2 neighborhood. If the neighborhood size may vary, one may precompute integral image with cvIntegral function
      /// </summary>
      CV_BLUR_NO_SCALE = 0,
      /// <summary>
      /// (simple blur) - summation over a pixel param1xparam2 neighborhood with subsequent scaling by 1/(param1xparam2). 
      /// </summary>
      CV_BLUR = 1,
      /// <summary>
      /// (gaussian blur) - convolving image with param1xparam2 Gaussian kernel. 
      /// </summary>
      CV_GAUSSIAN = 2,
      /// <summary>
      /// (median blur) - finding median of param1xparam1 neighborhood (i.e. the neighborhood is square). 
      /// </summary>
      CV_MEDIAN = 3,
      /// <summary>
      /// (bilateral filter) - applying bilateral 3x3 filtering with color sigma=param1 and space sigma=param2. Information about bilateral filtering can be found 
      /// </summary>
      CV_BILATERAL = 4
   }

   /// <summary>
   /// cvLoadImage type
   /// </summary>
   [Flags]
   public enum LOAD_IMAGE_TYPE
   {
      /// <summary>
      /// 8bit, color or not 
      /// </summary>
      CV_LOAD_IMAGE_UNCHANGED = -1,

      /// <summary>
      /// 8bit, gray
      /// </summary>
      CV_LOAD_IMAGE_GRAYSCALE = 0,

      /// <summary>
      /// ?, color
      /// </summary>
      CV_LOAD_IMAGE_COLOR = 1,

      /// <summary>
      /// any depth, ?
      /// </summary>
      CV_LOAD_IMAGE_ANYDEPTH = 2,

      /// <summary>
      /// ?, any color
      /// </summary>
      CV_LOAD_IMAGE_ANYCOLOR = 4,
   }

   /// <summary>
   /// Type of matrix depth
   /// </summary>
   public enum MAT_DEPTH
   {
      /// <summary>
      /// 8bit unsigned
      /// </summary>
      CV_8U = 0,
      /// <summary>
      /// 8bit signed
      /// </summary>
      CV_8S = 1,
      /// <summary>
      /// 16bit unsigned
      /// </summary>
      CV_16U = 2,
      /// <summary>
      /// 16bit signed
      /// </summary>
      CV_16S = 3,
      /// <summary>
      /// 32bit signed 
      /// </summary>
      CV_32S = 4,
      /// <summary>
      /// 32bit float
      /// </summary>
      CV_32F = 5,
      /// <summary>
      /// 64bit
      /// </summary>
      CV_64F = 6
   }

   /// <summary>
   /// CV_RAND TYPE
   /// </summary>
   public enum RAND_TYPE
   {
      /// <summary>
      /// Uniform distribution
      /// </summary>
      CV_RAND_UNI = 0,
      /// <summary>
      /// Normal distribution
      /// </summary>
      CV_RAND_NORMAL = 1
   }

   /// <summary>
   /// contour retrieval mode
   /// </summary>
   public enum RETR_TYPE : int
   {
      /// <summary>
      /// retrive only the extreme outer contours 
      /// </summary>
      CV_RETR_EXTERNAL = 0,
      /// <summary>
      ///  retrieve all the contours and puts them in the list 
      /// </summary>
      CV_RETR_LIST = 1,
      /// <summary>
      /// retrieve all the contours and organizes them into two-level hierarchy: top level are external boundaries of the components, second level are bounda boundaries of the holes 
      /// </summary>
      CV_RETR_CCOMP = 2,
      /// <summary>
      /// retrieve all the contours and reconstructs the full hierarchy of nested contours 
      /// </summary>
      CV_RETR_TREE = 3
   }

   internal static class SeqConst
   {
      /// <summary>
      /// The bit to shift for SEQ_ELTYPE
      /// </summary>
      public const int CV_SEQ_ELTYPE_BITS = 12;

      /// <summary>
      /// The mask of CV_SEQ_ELTYPE
      /// </summary>
      public const int CV_SEQ_ELTYPE_MASK = ((1 << CV_SEQ_ELTYPE_BITS) - 1);

      /// <summary>
      /// The bits to shift for SEQ_KIND
      /// </summary>
      public const int CV_SEQ_KIND_BITS = 2;
      /// <summary>
      /// The bits to shift for SEQ_FLAG
      /// </summary>
      public const int CV_SEQ_FLAG_SHIFT = CV_SEQ_KIND_BITS + CV_SEQ_ELTYPE_BITS;
   }

   /// <summary>
   /// CV_SEQ_ELTYPE
   /// </summary>
   public enum SEQ_ELTYPE
   {
      ///<summary>
      ///  (x,y) 
      ///</summary>
      CV_SEQ_ELTYPE_POINT = (((int)MAT_DEPTH.CV_32S) + (((2) - 1) << 3)),
      ///<summary>  
      ///freeman code: 0..7 
      ///</summary>
      CV_SEQ_ELTYPE_CODE = MAT_DEPTH.CV_8U + 0 << 3,
      ///<summary>  
      ///unspecified type of sequence elements 
      ///</summary>
      CV_SEQ_ELTYPE_GENERIC = 0,
      ///<summary>  
      ///=6 
      ///</summary>
      CV_SEQ_ELTYPE_PTR = 7,
      ///<summary>  
      ///pointer to element of other sequence 
      ///</summary>
      CV_SEQ_ELTYPE_PPOINT = 7,
      ///<summary>  
      ///index of element of some other sequence 
      ///</summary>
      CV_SEQ_ELTYPE_INDEX = MAT_DEPTH.CV_32S,
      ///<summary>  
      ///next_o, next_d, vtx_o, vtx_d 
      ///</summary>
      CV_SEQ_ELTYPE_GRAPH_EDGE = 0,
      ///<summary>  
      ///first_edge, (x,y) 
      ///</summary>
      CV_SEQ_ELTYPE_GRAPH_VERTEX = 0,
      ///<summary>  
      ///vertex of the binary tree   
      ///</summary>
      CV_SEQ_ELTYPE_TRIAN_ATR = 0,
      ///<summary>  
      ///connected component  
      ///</summary>
      CV_SEQ_ELTYPE_CONNECTED_COMP = 0,
      ///<summary>  
      ///(x,y,z)  
      ///</summary>
      CV_SEQ_ELTYPE_POINT3D = MAT_DEPTH.CV_32F + 2 << 3

   }

   /// <summary>
   /// The kind of sequence available
   /// </summary>
   public enum SEQ_KIND
   {
      /// <summary>
      /// generic (unspecified) kind of sequence 
      /// </summary>
      CV_SEQ_KIND_GENERIC = (0 << SeqConst.CV_SEQ_ELTYPE_BITS),
      /// <summary>
      /// dense sequence subtypes 
      /// </summary>
      CV_SEQ_KIND_CURVE = (1 << SeqConst.CV_SEQ_ELTYPE_BITS),
      /// <summary>
      /// dense sequence subtypes 
      /// </summary>
      CV_SEQ_KIND_BIN_TREE = (2 << SeqConst.CV_SEQ_ELTYPE_BITS),
      /// <summary>
      /// sparse sequence (or set) subtypes 
      /// </summary>
      CV_SEQ_KIND_GRAPH = (1 << SeqConst.CV_SEQ_ELTYPE_BITS),
      /// <summary>
      /// sparse sequence (or set) subtypes 
      /// </summary>
      CV_SEQ_KIND_SUBDIV2D = (2 << SeqConst.CV_SEQ_ELTYPE_BITS)
   }

   /// <summary>
   /// Sequence flag
   /// </summary>
   public enum SEQ_FLAG
   {
      /// <summary>
      /// close sequence
      /// </summary>
      CV_SEQ_FLAG_CLOSED = (1 << SeqConst.CV_SEQ_FLAG_SHIFT),
      /// <summary>
      /// 
      /// </summary>
      CV_SEQ_FLAG_SIMPLE = (2 << SeqConst.CV_SEQ_FLAG_SHIFT),
      /// <summary>
      /// 
      /// </summary>
      CV_SEQ_FLAG_CONVEX = (4 << SeqConst.CV_SEQ_FLAG_SHIFT),
      /// <summary>
      /// 
      /// </summary>
      CV_SEQ_FLAG_HOLE = (8 << SeqConst.CV_SEQ_FLAG_SHIFT)
   }

   /// <summary>
   /// Sequence type for point sets
   /// </summary>
   public enum SEQ_TYPE
   {
      /// <summary>
      /// 
      /// </summary>
      CV_SEQ_POINT_SET = (SEQ_KIND.CV_SEQ_KIND_GENERIC | SEQ_ELTYPE.CV_SEQ_ELTYPE_POINT),
      /// <summary>
      /// 
      /// </summary>
      CV_SEQ_POINT3D_SET = (SEQ_KIND.CV_SEQ_KIND_GENERIC | SEQ_ELTYPE.CV_SEQ_ELTYPE_POINT3D),
      /// <summary>
      /// 
      /// </summary>
      CV_SEQ_POLYLINE = (SEQ_KIND.CV_SEQ_KIND_CURVE | SEQ_ELTYPE.CV_SEQ_ELTYPE_POINT),
      /// <summary>
      /// 
      /// </summary>
      CV_SEQ_POLYGON = (SEQ_FLAG.CV_SEQ_FLAG_CLOSED | CV_SEQ_POLYLINE),
      //CV_SEQ_CONTOUR         =CV_SEQ_POLYGON,
      /// <summary>
      /// 
      /// </summary>
      CV_SEQ_SIMPLE_POLYGON = (SEQ_FLAG.CV_SEQ_FLAG_SIMPLE | CV_SEQ_POLYGON)
   }

   /// <summary>
   /// CV_TERMCRIT
   /// </summary>
   [Flags]
   public enum TERMCRIT
   {
      /// <summary>
      /// Iteration
      /// </summary>
      CV_TERMCRIT_ITER = 1,
      /// <summary>
      /// Epsilon
      /// </summary>
      CV_TERMCRIT_EPS = 2
   }

   /// <summary>
   /// Types of thresholding 
   /// </summary>
   public enum THRESH
   {
      ///<summary>
      ///value = value > threshold ? max_value : 0
      ///</summary>
      CV_THRESH_BINARY = 0,
      ///<summary>
      /// value = value > threshold ? 0 : max_value       
      ///</summary>
      CV_THRESH_BINARY_INV = 1,
      ///<summary>
      /// value = value > threshold ? threshold : value   
      ///</summary>
      CV_THRESH_TRUNC = 2,
      ///<summary>
      /// value = value > threshold ? value : 0           
      ///</summary>
      CV_THRESH_TOZERO = 3,
      ///<summary>
      /// value = value > threshold ? 0 : value           
      ///</summary>
      CV_THRESH_TOZERO_INV = 4,
      /// <summary>
      /// 
      /// </summary>
      CV_THRESH_MASK = 7,
      ///<summary>
      /// use Otsu algorithm to choose the optimal threshold value;
      /// combine the flag with one of the above CV_THRESH_* values 
      ///</summary>
      CV_THRESH_OTSU = 8

   }

   /// <summary>
   /// Methods for comparing two array
   /// </summary>
   public enum TM_TYPE
   {
      /// <summary>
      /// R(x,y)=sumx',y'[T(x',y')-I(x+x',y+y')]2
      /// </summary>
      CV_TM_SQDIFF = 0,
      /// <summary>
      /// R(x,y)=sumx',y'[T(x',y')-I(x+x',y+y')]2/sqrt[sumx',y'T(x',y')2 sumx',y'I(x+x',y+y')2]
      /// </summary>
      CV_TM_SQDIFF_NORMED = 1,
      /// <summary>
      /// R(x,y)=sumx',y'[T(x',y') I(x+x',y+y')]
      /// </summary>
      CV_TM_CCORR = 2,
      /// <summary>
      /// R(x,y)=sumx',y'[T(x',y') I(x+x',y+y')]/sqrt[sumx',y'T(x',y')2 sumx',y'I(x+x',y+y')2]
      /// </summary>
      CV_TM_CCORR_NORMED = 3,
      /// <summary>
      /// R(x,y)=sumx',y'[T'(x',y') I'(x+x',y+y')],
      /// where T'(x',y')=T(x',y') - 1/(wxh) sumx",y"T(x",y")
      ///    I'(x+x',y+y')=I(x+x',y+y') - 1/(wxh) sumx",y"I(x+x",y+y")
      /// </summary>
      CV_TM_CCOEFF = 4,
      /// <summary>
      /// R(x,y)=sumx',y'[T'(x',y') I'(x+x',y+y')]/sqrt[sumx',y'T'(x',y')2 sumx',y'I'(x+x',y+y')2]
      /// </summary>
      CV_TM_CCOEFF_NORMED = 5
   }

   /// <summary>
   /// IPL_DEPTH
   /// </summary>
   public enum IPL_DEPTH : uint
   {
      /// <summary>
      /// indicates if the value is signed
      /// </summary>
      IPL_DEPTH_SIGN = 0x80000000,
      /// <summary>
      /// 1bit unsigned
      /// </summary>
      IPL_DEPTH_1U = 1,
      /// <summary>
      /// 8bit unsigned (Byte)
      /// </summary>
      IPL_DEPTH_8U = 8,
      /// <summary>
      /// 16bit unsigned
      /// </summary>
      IPL_DEPTH_16U = 16,
      /// <summary>
      /// 32bit float (Single)
      /// </summary>
      IPL_DEPTH_32F = 32,
      /// <summary>
      /// 8bit signed
      /// </summary>
      IPL_DEPTH_8S = (IPL_DEPTH_SIGN | 8),
      /// <summary>
      /// 16bit signed
      /// </summary>
      IPL_DEPTH_16S = (IPL_DEPTH_SIGN | 16),
      /// <summary>
      /// 32bit signed 
      /// </summary>
      IPL_DEPTH_32S = (IPL_DEPTH_SIGN | 32),
      /// <summary>
      /// double
      /// </summary>
      IPL_DEPTH_64F = 64
   }

   /// <summary>
   /// Enumeration used by cvFlip
   /// </summary>
   [Flags]
   public enum FLIP
   {
      /// <summary>
      /// No flipping
      /// </summary>
      NONE = 0,
      /// <summary>
      /// Flip horizontally
      /// </summary>
      HORIZONTAL = 1,
      /// <summary>
      /// Flip vertically
      /// </summary>
      VERTICAL = 2
   }

   /// <summary>
   /// Enumeration used by cvCheckArr
   /// </summary>
   [Flags]
   public enum CHECK_TYPE
   {
      /// <summary>
      /// Checks that every element is neigther NaN nor Infinity
      /// </summary>
      CHECK_NAN_INFINITY = 0,
      /// <summary>
      /// If set, the function checks that every value of array is within [minVal,maxVal) range, otherwise it just checks that every element is neigther NaN nor Infinity
      /// </summary>
      CHECK_RANGE = 1,
      /// <summary>
      /// If set, the function does not raises an error if an element is invalid or out of range
      /// </summary>
      CHECK_QUIET = 2
   }

   /// <summary>
   /// Type of floodfill operation
   /// </summary>
   [Flags]
   public enum FLOODFILL_FLAG
   {
      /// <summary>
      /// The default type
      /// </summary>
      DEFAULT = 0,
      /// <summary>
      /// If set the difference between the current pixel and seed pixel is considered,
      /// otherwise difference between neighbor pixels is considered (the range is floating).
      /// </summary>
      FIXED_RANGE = (1 << 16),
      /// <summary>
      /// If set, the function does not fill the image (new_val is ignored),
      /// but the fills mask (that must be non-NULL in this case).
      /// </summary>
      MASK_ONLY = (1 << 17)
   }

   /// <summary>
   /// The type for cvSampleLine
   /// </summary>
   public enum CONNECTIVITY
   {
      /// <summary>
      /// 8-connected
      /// </summary>
      EIGHT_CONNECTED = 8,
      /// <summary>
      /// 4-connected
      /// </summary>
      FOUR_CONNECTED = 4
   }

   /// <summary>
   /// The type of line for drawing
   /// </summary>
   public enum LINE_TYPE
   {
      /// <summary>
      /// 8-connected
      /// </summary>
      EIGHT_CONNECTED = 8,
      /// <summary>
      /// 4-connected
      /// </summary>
      FOUR_CONNECTED = 4,
      /// <summary>
      /// Antialias
      /// </summary>
      CV_AA = 16
   }

   /// <summary>
   /// Defines for Distance Transform
   /// </summary>
   public enum DIST_TYPE
   {
      ///<summary>
      ///  User defined distance 
      ///</summary>
      CV_DIST_USER = -1,

      ///<summary>
      ///  distance = |x1-x2| + |y1-y2| 
      ///</summary>
      CV_DIST_L1 = 1,
      ///<summary>
      ///  Simple euclidean distance 
      ///</summary>
      CV_DIST_L2 = 2,
      ///<summary>
      ///  distance = max(|x1-x2|,|y1-y2|) 
      ///</summary>
      CV_DIST_C = 3,
      ///<summary>
      ///  L1-L2 metric: distance = 2(sqrt(1+x*x/2) - 1)) 
      ///</summary>
      CV_DIST_L12 = 4,
      ///<summary>
      ///  distance = c^2(|x|/c-log(1+|x|/c)), c = 1.3998 
      ///</summary>
      CV_DIST_FAIR = 5,
      ///<summary>
      ///  distance = c^2/2(1-exp(-(x/c)^2)), c = 2.9846 
      ///</summary>
      CV_DIST_WELSCH = 6,
      ///<summary>
      ///  distance = |x|&lt;c ? x^2/2 : c(|x|-c/2), c=1.345 
      ///</summary>
      CV_DIST_HUBER = 7,
   }

   /// <summary>
   /// The types for cvMulSpectrums
   /// </summary>
   [Flags]
   public enum MUL_SPECTRUMS_TYPE
   {
      /// <summary>
      /// The default type
      /// </summary>
      DEFAULT = 0,
      /// <summary>
      /// Do forward or inverse transform of every individual row of the input matrix. This flag allows user to transform multiple vectors simultaneously and can be used to decrease the overhead (which is sometimes several times larger than the processing itself), to do 3D and higher-dimensional transforms etc
      /// </summary>
      CV_DXT_ROWS = 4,
      /// <summary>
      /// Conjugate the second argument of cvMulSpectrums
      /// </summary>
      CV_DXT_MUL_CONJ = 8
   }

   /// <summary>
   /// Flag used for cvDFT
   /// </summary>
   [Flags]
   public enum CV_DXT
   {
      /// <summary>
      /// Do forward 1D or 2D transform. The result is not scaled
      /// </summary>
      CV_DXT_FORWARD = 0,
      /// <summary>
      /// Do inverse 1D or 2D transform. The result is not scaled. CV_DXT_FORWARD and CV_DXT_INVERSE are mutually exclusive, of course
      /// </summary>
      CV_DXT_INVERSE = 1,
      /// <summary>
      /// Scale the result: divide it by the number of array elements. Usually, it is combined with CV_DXT_INVERSE, and one may use a shortcut 
      /// </summary>
      CV_DXT_SCALE = 2,
      /// <summary>
      /// Do forward or inverse transform of every individual row of the input matrix. This flag allows user to transform multiple vectors simultaneously and can be used to decrease the overhead (which is sometimes several times larger than the processing itself), to do 3D and higher-dimensional transforms etc
      /// </summary>
      CV_DXT_ROWS = 4,
      /// <summary>
      /// Inverse and scale
      /// </summary>
      CV_DXT_INV_SCALE = (CV_DXT_SCALE | CV_DXT_INVERSE)
   }

   /// <summary>
   /// Flag used for cvDCT
   /// </summary>
   public enum CV_DCT_TYPE
   {
      /// <summary>
      /// Do forward 1D or 2D transform. The result is not scaled
      /// </summary>
      CV_DXT_FORWARD = 0,
      /// <summary>
      /// Do inverse 1D or 2D transform. The result is not scaled. CV_DXT_FORWARD and CV_DXT_INVERSE are mutually exclusive, of course
      /// </summary>
      CV_DXT_INVERSE = 1,
      /// <summary>
      /// Do forward or inverse transform of every individual row of the input matrix. This flag allows user to transform multiple vectors simultaneously and can be used to decrease the overhead (which is sometimes several times larger than the processing itself), to do 3D and higher-dimensional transforms etc
      /// </summary>
      CV_DXT_ROWS = 4
   }

   /// <summary>
   /// Calculates fundamental matrix given a set of corresponding points
   /// </summary>
   [Flags]
   public enum CV_FM
   {
      /// <summary>
      /// for 7-point algorithm. N == 7
      /// </summary>
      CV_FM_7POINT = 1,
      /// <summary>
      /// for 8-point algorithm. N >= 8
      /// </summary>
      CV_FM_8POINT = 2,
      /// <summary>
      /// for LMedS algorithm. N >= 8
      /// </summary>
      CV_FM_LMEDS_ONLY = 4,
      /// <summary>
      /// for RANSAC algorithm. N >= 8
      /// </summary>
      CV_FM_RANSAC_ONLY = 8,
      /// <summary>
      /// CV_FM_LMEDS_ONLY | CV_FM_8POINT
      /// </summary>
      CV_FM_LMEDS = (CV_FM_LMEDS_ONLY | CV_FM_8POINT),
      /// <summary>
      /// CV_FM_RANSAC_ONLY | CV_FM_8POINT
      /// </summary>
      CV_FM_RANSAC = (CV_FM_RANSAC_ONLY | CV_FM_8POINT)
   }

   /// <summary>
   /// General enumeration
   /// </summary>
   public enum GENERAL
   {
      /// <summary>
      /// 
      /// </summary>
      CV_MAX_DIM = 32,
      /// <summary>
      /// 
      /// </summary>
      CV_SEQ_MAGIC_VAL = 0x42990000,
      /// <summary>
      /// 
      /// </summary>
      CV_SET_MAGIC_VAL = 0x42980000
   }

   ///<summary>
   /// Error codes
   /// </summary>
   public enum ERROR_CODES
   {
      /// <summary>
      /// 
      /// </summary>
      CV_STSOK = 0,
      /// <summary>
      /// 
      /// </summary>
      CV_STSBACKTRACE = -1,
      /// <summary>
      /// 
      /// </summary>
      CV_STSERROR = -2,
      /// <summary>
      /// 
      /// </summary>
      CV_STSINTERNAL = -3,
      /// <summary>
      /// 
      /// </summary>
      CV_STSNOMEM = -4,
      /// <summary>
      /// 
      /// </summary>
      CV_STSBADARG = -5,
      /// <summary>
      /// 
      /// </summary>
      CV_STSBADFUNC = -6,
      /// <summary>
      /// 
      /// </summary>
      CV_STSNOCONV = -7,
      /// <summary>
      /// 
      /// </summary>
      CV_STSAUTOTRACE = -8,
      /// <summary>
      /// 
      /// </summary>
      CV_HEADERISNULL = -9,
      /// <summary>
      /// 
      /// </summary>
      CV_BADIMAGESIZE = -10,
      /// <summary>
      /// 
      /// </summary>
      CV_BADOFFSET = -11,
      /// <summary>
      /// 
      /// </summary>
      CV_BADDATAPTR = -12,
      /// <summary>
      /// 
      /// </summary>
      CV_BADSTEP = -13,
      /// <summary>
      /// 
      /// </summary>
      CV_BADMODELORCHSEQ = -14,
      /// <summary>
      /// 
      /// </summary>
      CV_BADNUMCHANNELS = -15,
      /// <summary>
      /// 
      /// </summary>
      CV_BADNUMCHANNEL1U = -16,
      /// <summary>
      /// 
      /// </summary>
      CV_BADDEPTH = -17,
      /// <summary>
      /// 
      /// </summary>
      CV_BADALPHACHANNEL = -18,
      /// <summary>
      /// 
      /// </summary>
      CV_BADORDER = -19,
      /// <summary>
      /// 
      /// </summary>
      CV_BADORIGIN = -20,
      /// <summary>
      /// 
      /// </summary>
      CV_BADALIGN = -21,
      /// <summary>
      /// 
      /// </summary>
      CV_BADCALLBACK = -22,
      /// <summary>
      /// 
      /// </summary>
      CV_BADTILESIZE = -23,
      /// <summary>
      /// 
      /// </summary>
      CV_BADCOI = -24,
      /// <summary>
      /// 
      /// </summary>
      CV_BADROISIZE = -25,
      /// <summary>
      /// 
      /// </summary>
      CV_MASKISTILED = -26,
      /// <summary>
      /// 
      /// </summary>
      CV_STSNULLPTR = -27,
      /// <summary>
      /// 
      /// </summary>
      CV_STSVECLENGTHERR = -28,
      /// <summary>
      /// 
      /// </summary>
      CV_STSFILTERSTRUCTCONTENTERR = -29,
      /// <summary>
      /// 
      /// </summary>
      CV_STSKERNELSTRUCTCONTENTERR = -30,
      /// <summary>
      /// 
      /// </summary>
      CV_STSFILTEROFFSETERR = -31,
      /// <summary>
      /// 
      /// </summary>
      CV_STSBADSIZE = -201,
      /// <summary>
      /// 
      /// </summary>
      CV_STSDIVBYZERO = -202,
      /// <summary>
      /// 
      /// </summary>
      CV_STSINPLACENOTSUPPORTED = -203,
      /// <summary>
      /// 
      /// </summary>
      CV_STSOBJECTNOTFOUND = -204,
      /// <summary>
      /// 
      /// </summary>
      CV_STSUNMATCHEDFORMATS = -205,
      /// <summary>
      /// 
      /// </summary>
      CV_STSBADFLAG = -206,
      /// <summary>
      /// 
      /// </summary>
      CV_STSBADPOINT = -207,
      /// <summary>
      /// 
      /// </summary>
      CV_STSBADMASK = -208,
      /// <summary>
      /// 
      /// </summary>
      CV_STSUNMATCHEDSIZES = -209,
      /// <summary>
      /// 
      /// </summary>
      CV_STSUNSUPPORTEDFORMAT = -210,
      /// <summary>
      /// 
      /// </summary>
      CV_STSOUTOFRANGE = -211,
      /// <summary>
      /// 
      /// </summary>
      CV_STSPARSEERROR = -212,
      /// <summary>
      /// 
      /// </summary>
      CV_STSNOTIMPLEMENTED = -213,
      /// <summary>
      /// 
      /// </summary>
      CV_STSBADMEMBLOCK = -214
   }

   /// <summary>
   /// Types for CvWarpAffine
   /// </summary>
   public enum WARP
   {
      /// <summary>
      /// Neither FILL_OUTLIERS nor CV_WRAP_INVERSE_MAP
      /// </summary>
      CV_WARP_DEFAULT = 0,
      /// <summary>
      /// Fill all the destination image pixels. If some of them correspond to outliers in the source image, they are set to fillval.
      /// </summary>
      CV_WARP_FILL_OUTLIERS = 8,
      /// <summary>
      /// Indicates that matrix is inverse transform from destination image to source and, thus, can be used directly for pixel interpolation. Otherwise, the function finds the inverse transform from map_matrix.
      /// </summary>
      CV_WARP_INVERSE_MAP = 16
   }

   /// <summary>
   /// Types of Adaptive Threshold
   /// </summary>
   public enum ADAPTIVE_THRESHOLD_TYPE
   {
      /// <summary>
      /// indicates that "Mean minus C" should be used for adaptive threshold.
      /// </summary>
      CV_ADAPTIVE_THRESH_MEAN_C = 0,
      /// <summary>
      /// indicates that "Gaussian minus C" should be used for adaptive threshold.
      /// </summary>
      CV_ADAPTIVE_THRESH_GAUSSIAN_C = 1
   }

   /// <summary>
   /// Shape of the Structuring Element
   /// </summary>
   public enum CV_ELEMENT_SHAPE
   {
      /// <summary>
      /// A rectangular element.
      /// </summary>
      CV_SHAPE_RECT = 0,
      /// <summary>
      /// A cross-shaped element.
      /// </summary>
      CV_SHAPE_CROSS = 1,
      /// <summary>
      /// An elliptic element.
      /// </summary>
      CV_SHAPE_ELLIPSE = 2,
      /// <summary>
      /// A user-defined element.
      /// </summary>
      CV_SHAPE_CUSTOM = 100
   }

   /// <summary>
   /// PCA Type
   /// </summary>
   [Flags]
   public enum PCA_TYPE
   {
      /// <summary>
      /// the vectors are stored as rows (i.e. all the components of a certain vector are stored continously)
      /// </summary>
      CV_PCA_DATA_AS_ROW = 0,
      /// <summary>
      ///  the vectors are stored as columns (i.e. values of a certain vector component are stored continuously)
      /// </summary>
      CV_PCA_DATA_AS_COL = 1,
      /// <summary>
      /// use pre-computed average vector
      /// </summary>
      CV_PCA_USE_AVG = 2
   }

   /// <summary>
   /// Type of Morphological Operation
   /// </summary>
   public enum CV_MORPH_OP
   {
      /// <summary>
      /// Opening.
      /// </summary>
      CV_MOP_OPEN = 2,
      /// <summary>
      /// Closing.
      /// </summary>
      CV_MOP_CLOSE = 3,
      /// <summary>
      /// Morphological Gradient.
      /// </summary>
      CV_MOP_GRADIENT = 4,
      /// <summary>
      /// "Top Hat".
      /// </summary>
      CV_MOP_TOPHAT = 5,
      /// <summary>
      /// "Black Hat".
      /// </summary>
      CV_MOP_BLACKHAT = 6
   }

   /// <summary>
   /// The type of histogram
   /// </summary>
   public enum HIST_TYPE
   {
      /// <summary>
      /// array
      /// </summary>
      CV_HIST_ARRAY = 0,
      /// <summary>
      /// sparse matrix
      /// </summary>
      CV_HIST_SPARSE = 1
   }

   /// <summary>
   /// cvInvert method
   /// </summary>
   public enum SOLVE_METHOD
   {
      /// <summary>
      /// Gaussian elimination with optimal pivot element chose
      /// In case of LU method the function returns src1 determinant (src1 must be square). If it is 0, the matrix is not inverted and src2 is filled with zeros.
      /// </summary>
      CV_LU = 0,
      /// <summary>
      /// Singular value decomposition (SVD) method
      /// In case of SVD methods the function returns the inversed condition number of src1 (ratio of the smallest singular value to the largest singular value) and 0 if src1 is all zeros. The SVD methods calculate a pseudo-inverse matrix if src1 is singular
      /// </summary>
      CV_SVD = 1,
      /// <summary>
      /// method for a symmetric positively-defined matrix
      /// </summary>
      CV_SVD_SYM = 2
   }

   /// <summary>
   /// cvCalcCovarMatrix method types
   /// </summary>
   [Flags]
   public enum COVAR_METHOD
   {
      /// <summary>
      /// Calculates covariation matrix for a set of vectors 
      /// transpose([v1-avg, v2-avg,...]) * [v1-avg,v2-avg,...] 
      /// </summary>
      CV_COVAR_SCRAMBLED = 0,

      /// <summary>
      /// [v1-avg, v2-avg,...] * transpose([v1-avg,v2-avg,...])
      /// </summary>
      CV_COVAR_NORMAL = 1,

      /// <summary>
      /// Do not calc average (i.e. mean vector) - use the input vector instead
      /// (useful for calculating covariance matrix by parts)
      /// </summary>
      CV_COVAR_USE_AVG = 2,

      /// <summary>
      /// Scale the covariance matrix coefficients by number of the vectors
      /// </summary>
      CV_COVAR_SCALE = 4,

      /// <summary>
      /// All the input vectors are stored in a single matrix, as its rows 
      /// </summary>
      CV_COVAR_ROWS = 8,

      /// <summary>
      /// All the input vectors are stored in a single matrix, as its columns
      /// </summary>
      CV_COVAR_COLS = 16,

   }

   /// <summary>
   /// Type for cvSVD
   /// </summary>
   [Flags]
   public enum SVD_TYPE
   {
      /// <summary>
      /// The default type
      /// </summary>
      CV_SVD_DEFAULT = 0,
      /// <summary>
      /// enables modification of matrix src1 during the operation. It speeds up the processing. 
      /// </summary>
      CV_SVD_MODIFY_A = 1,
      /// <summary>
      /// means that the tranposed matrix U is returned. Specifying the flag speeds up the processing. 
      /// </summary>
      CV_SVD_U_T = 2,
      /// <summary>
      /// means that the tranposed matrix V is returned. Specifying the flag speeds up the processing. 
      /// </summary>
      CV_SVD_V_T = 4
   }

   /// <summary>
   /// Type for cvCalcOpticalFlowPyrLK
   /// </summary>
   public enum LKFLOW_TYPE
   {
      /// <summary>
      /// The default type
      /// </summary>
      DEFAULT = 0,
      /// <summary>
      /// Pyramid for the first frame is precalculated before the call
      /// </summary>
      CV_LKFLOW_PYR_A_READY = 1,
      /// <summary>
      /// Pyramid for the second frame is precalculated before the call
      /// </summary>
      CV_LKFLOW_PYR_B_READY = 2,
      /// <summary>
      /// Array B contains initial coordinates of features before the function call.
      /// </summary>
      CV_LKFLOW_INITIAL_GUESSES = 4
   }

   /// <summary>
   /// Various camera calibration flags
   /// </summary>
   [Flags]
   public enum CALIB_TYPE
   {
      /// <summary>
      /// The default value
      /// </summary>
      DEFAULT = 0,
      /// <summary>
      /// intrinsic_matrix contains valid initial values of fx, fy, cx, cy that are optimized further. Otherwise, (cx, cy) is initially set to the image center (image_size is used here), and focal distances are computed in some least-squares fashion
      /// </summary>
      CV_CALIB_USE_INTRINSIC_GUESS = 1,
      /// <summary>
      /// The optimization procedure consider only one of fx and fy as independent variable and keeps the aspect ratio fx/fy the same as it was set initially in intrinsic_matrix. In this case the actual initial values of (fx, fy) are either taken from the matrix (when CV_CALIB_USE_INTRINSIC_GUESS is set) or estimated somehow (in the latter case fx, fy may be set to arbitrary values, only their ratio is used)
      /// </summary>
      CV_CALIB_FIX_ASPECT_RATIO = 2,
      /// <summary>
      /// The principal point is not changed during the global optimization, it stays at the center and at the other location specified (when CV_CALIB_FIX_FOCAL_LENGTH - Both fx and fy are fixed.
      /// CV_CALIB_USE_INTRINSIC_GUESS is set as well)
      /// </summary>
      CV_CALIB_FIX_PRINCIPAL_POINT = 4,
      /// <summary>
      /// Tangential distortion coefficients are set to zeros and do not change during the optimization
      /// </summary>
      CV_CALIB_ZERO_TANGENT_DIST = 8,
      /// <summary>
      /// The focal length is fixed
      /// </summary>
      CV_CALIB_FIX_FOCAL_LENGTH = 16,
      /// <summary>
      /// The 1st distortion coefficient (k1) is fixed to 0 or to the initial passed value if CV_CALIB_USE_INTRINSIC_GUESS is passed
      /// </summary>
      CV_CALIB_FIX_K1 = 32,
      /// <summary>
      /// The 2nd distortion coefficient (k2) is fixed to 0 or to the initial passed value if CV_CALIB_USE_INTRINSIC_GUESS is passed
      /// </summary>
      CV_CALIB_FIX_K2 = 64,
      /// <summary>
      /// The 3rd distortion coefficient (k3) is fixed to 0 or to the initial passed value if CV_CALIB_USE_INTRINSIC_GUESS is passed
      /// </summary>
      CV_CALIB_FIX_K3 = 128,
      /// <summary>
      /// The 4th distortion coefficient (k4) is fixed (see above)
      /// </summary>
      CV_CALIB_FIX_K4 = 2048,
      /// <summary>
      /// The 5th distortion coefficient (k5) is fixed to 0 or to the initial passed value if CV_CALIB_USE_INTRINSIC_GUESS is passed
      /// </summary>
      CV_CALIB_FIX_K5 = 4096,
      /// <summary>
      /// The 6th distortion coefficient (k6) is fixed to 0 or to the initial passed value if CV_CALIB_USE_INTRINSIC_GUESS is passed
      /// </summary>
      CV_CALIB_FIX_K6 = 8192,
      /// <summary>
      /// Rational model
      /// </summary>
      CV_CALIB_RATIONAL_MODEL = 16384
   }

   /// <summary>
   /// Type of chessboard calibration
   /// </summary>
   [Flags]
   public enum CALIB_CB_TYPE
   {
      /// <summary>
      /// Default type
      /// </summary>
      DEFAULT = 0,
      /// <summary>
      /// Use adaptive thresholding to convert the image to black-n-white, rather than a fixed threshold level (computed from the average image brightness)
      /// </summary>
      ADAPTIVE_THRESH = 1,
      /// <summary>
      /// Normalize the image using cvNormalizeHist before applying fixed or adaptive thresholding.
      /// </summary>
      NORMALIZE_IMAGE = 2,
      /// <summary>
      /// Use additional criteria (like contour area, perimeter, square-like shape) to filter out false quads that are extracted at the contour retrieval stage
      /// </summary>
      FILTER_QUADS = 4,
      /// <summary>
      /// If it is on, then this check is performed before the main algorithm and if a chessboard is not found, the function returns 0 instead of wasting 0.3-1s on doing the full search.
      /// </summary>
      FAST_CHECK = 8,
   }

   /// <summary>
   /// IO type for eigen object related functions
   /// </summary>
   public enum EIGOBJ_TYPE
   {
      /// <summary>
      /// No callback
      /// </summary>
      CV_EIGOBJ_NO_CALLBACK = 0,
      /// <summary>
      /// input callback
      /// </summary>
      CV_EIGOBJ_INPUT_CALLBACK = 1,
      /// <summary>
      /// output callback
      /// </summary>
      CV_EIGOBJ_OUTPUT_CALLBACK = 2,
      /// <summary>
      /// both callback
      /// </summary>
      CV_EIGOBJ_BOTH_CALLBACK = 3
   }

   /// <summary>
   /// CvNextEdgeType
   /// </summary>
   public enum CV_NEXT_EDGE_TYPE
   {
      /// <summary>
      /// next around the edge origin (eOnext)
      /// </summary>
      CV_NEXT_AROUND_ORG = 0x00,
      /// <summary>
      /// next around the edge vertex (eDnext) 
      /// </summary>
      CV_NEXT_AROUND_DST = 0x22,
      /// <summary>
      /// previous around the edge origin (reversed eRnext)
      /// </summary>
      CV_PREV_AROUND_ORG = 0x11,
      /// <summary>
      /// previous around the edge destination (reversed eLnext) 
      /// </summary>
      CV_PREV_AROUND_DST = 0x33,
      /// <summary>
      /// next around the left facet (eLnext) 
      /// </summary>
      CV_NEXT_AROUND_LEFT = 0x13,
      /// <summary>
      /// next around the right facet (eRnext)
      /// </summary>
      CV_NEXT_AROUND_RIGHT = 0x31,
      /// <summary>
      /// previous around the left facet (reversed eOnext)
      /// </summary>
      CV_PREV_AROUND_LEFT = 0x20,
      /// <summary>
      /// previous around the right facet (reversed eDnext) 
      /// </summary>
      CV_PREV_AROUND_RIGHT = 0x02
   }

   /// <summary>
   /// orientation
   /// </summary>
   public enum ORIENTATION
   {
      /// <summary>
      /// clockwise
      /// </summary>
      CV_CLOCKWISE = 1,
      /// <summary>
      /// counter clockwise
      /// </summary>
      CV_COUNTER_CLOCKWISE = 2
   }

   /// <summary>
   /// Stereo Block Matching type
   /// </summary>
   public enum STEREO_BM_TYPE
   {
      /// <summary>
      /// Basic type
      /// </summary>
      BASIC = 0,
      /// <summary>
      /// Fish eye
      /// </summary>
      FISH_EYE = 1,
      /// <summary>
      /// Narrow
      /// </summary>
      NARROW = 2
   }

   /// <summary>
   /// Stereo Block Matching Prefilter type
   /// </summary>
   public enum STEREO_BM_PREFILTER
   {
      /// <summary>
      /// No prefilter
      /// </summary>
      NORMALIZED_RESPONSE = 0,
      /// <summary>
      /// XSobel
      /// </summary>
      XSOBEL = 1
   }

   /// <summary>
   /// Type of cvHomography method
   /// </summary>
   public enum HOMOGRAPHY_METHOD
   {
      /// <summary>
      /// regular method using all the point pairs
      /// </summary>
      DEFAULT = 0,
      /// <summary>
      /// Least-Median robust method
      /// </summary>
      LMEDS = 4,
      /// <summary>
      /// RANSAC-based robust method
      /// </summary>
      RANSAC = 8
   }

   /// <summary>
   /// Type used by cvMatchShapes
   /// </summary>
   public enum CONTOURS_MATCH_TYPE
   {
      /// <summary>
      /// I_1(A,B)=sum_{i=1..7} abs(1/m^A_i - 1/m^B_i) where m^A_i=sign(h^A_i) log(h^A_i), m^B_i=sign(h^B_i) log(h^B_i), h^A_i, h^B_i - Hu moments of A and B, respectively
      /// </summary> 
      CV_CONTOUR_MATCH_I1 = 1,
      /// <summary>
      /// I_2(A,B)=sum_{i=1..7} abs(m^A_i - m^B_i) where m^A_i=sign(h^A_i) log(h^A_i), m^B_i=sign(h^B_i) log(h^B_i), h^A_i, h^B_i - Hu moments of A and B, respectively
      /// </summary>
      CV_CONTOURS_MATCH_I2 = 2,
      /// <summary>
      /// I_3(A,B)=sum_{i=1..7} abs(m^A_i - m^B_i)/abs(m^A_i) where m^A_i=sign(h^A_i) log(h^A_i), m^B_i=sign(h^B_i) log(h^B_i), h^A_i, h^B_i - Hu moments of A and B, respectively
      /// </summary>
      CV_CONTOURS_MATCH_I3 = 3
   }

   /// <summary>
   /// The result type of cvSubdiv2DLocate.
   /// </summary>
   public enum Subdiv2DPointLocationType
   {
      /// <summary>
      /// One of input arguments is invalid.
      /// </summary>
      ERROR = -2,
      /// <summary>
      /// Point is outside the subdivision reference rectangle
      /// </summary>
      OUTSIDE_RECT = -1,
      /// <summary>
      /// Point falls into some facet
      /// </summary>
      INSIDE = 0,
      /// <summary>
      /// Point coincides with one of subdivision vertices
      /// </summary>
      VERTEX = 1,
      /// <summary>
      /// Point falls onto the edge
      /// </summary>
      ON_EDGE = 2
   }

   /// <summary>
   /// Type used in cvStereoRectify
   /// </summary>
   public enum STEREO_RECTIFY_TYPE
   {
      /// <summary>
      /// Shift one of the image in horizontal or vertical direction (depending on the orientation of epipolar lines) in order to maximise the useful image area
      /// </summary>
      DEFAULT = 0,
      /// <summary>
      /// Makes the principal points of each camera have the same pixel coordinates in the rectified views
      /// </summary>
      CALIB_ZERO_DISPARITY = 1024
   }

   /// <summary>
   /// The type for CopyMakeBorder function
   /// </summary>
   public enum BORDER_TYPE
   {
      /// <summary>
      /// Border is filled with the fixed value, passed as last parameter of the function
      /// </summary>
      CONSTANT = 0,
      /// <summary>
      /// The pixels from the top and bottom rows, the left-most and right-most columns are replicated to fill the border
      /// </summary>
      REPLICATE = 1,
      /// <summary>
      /// Reflect
      /// </summary>
      REFLECT = 2,
      /// <summary>
      /// Wrap
      /// </summary>
      WRAP = 3,
      /// <summary>
      /// Reflect 101
      /// </summary>
      REFLECT101 = 4,
      /// <summary>
      /// Transparent
      /// </summary>
      TRANSPARENT = 5,
      /// <summary>
      /// Isolated
      /// </summary>
      ISOLATED = 6
   }

   /// <summary>
   /// The types for haar detection
   /// </summary>
   [Flags]
   public enum HAAR_DETECTION_TYPE
   {
      /// <summary>
      /// The default type where no optimization is done.
      /// </summary>
      DEFAULT = 0,
      /// <summary>
      /// If it is set, the function uses Canny edge detector to reject some image regions that contain too few or too much edges and thus can not contain the searched object. The particular threshold values are tuned for face detection and in this case the pruning speeds up the processing
      /// </summary>
      DO_CANNY_PRUNING = 1,
      /// <summary>
      /// For each scale factor used the function will downscale the image rather than "zoom" the feature coordinates in the classifier cascade. Currently, the option can only be used alone, i.e. the flag can not be set together with the others
      /// </summary>
      SCALE_IMAGE = 2,
      /// <summary>
      /// If it is set, the function finds the largest object (if any) in the image. That is, the output sequence will contain one (or zero) element(s)
      /// </summary>
      FIND_BIGGEST_OBJECT = 4,
      /// <summary>
      /// It should be used only when CV_HAAR_FIND_BIGGEST_OBJECT is set and min_neighbors &gt; 0. If the flag is set, the function does not look for candidates of a smaller size as soon as it has found the object (with enough neighbor candidates) at the current scale. Typically, when min_neighbors is fixed, the mode yields less accurate (a bit larger) object rectangle than the regular single-object mode (flags=CV_HAAR_FIND_BIGGEST_OBJECT), but it is much faster, up to an order of magnitude. A greater value of min_neighbors may be specified to improve the accuracy
      /// </summary>
      DO_ROUGH_SEARCH = 8
   }

   /// <summary>
   /// Specific if it is back or front
   /// </summary>
   public enum BACK_OR_FRONT
   {
      /// <summary>
      /// Back
      /// </summary>
      BACK,
      /// <summary>
      /// Front
      /// </summary>
      FRONT
   }

   /// <summary>
   /// The method for matching contour tree
   /// </summary>
   public enum MATCH_CONTOUR_TREE_METHOD
   {
      /// <summary>
      /// 
      /// </summary>
      CONTOUR_TREES_MATCH_I1 = 1
   }

   /// <summary>
   /// The file storage operation type
   /// </summary>
   public enum STORAGE_OP
   {
      /// <summary>
      /// The storage is open for reading
      /// </summary>
      READ = 0,
      /// <summary>
      /// The storage is open for writing
      /// </summary>
      WRITE = 1,
      /// <summary>
      /// The storage is open for append
      /// </summary>
      APPEND = 2
   }

   /// <summary>
   /// The type of blob detector
   /// </summary>
   public enum BLOB_DETECTOR_TYPE
   {
      /// <summary>
      /// Simple blob detector
      /// </summary>
      Simple,
      /// <summary>
      /// Conected Component blob detector
      /// </summary>
      CC
   }

   /// <summary>
   /// MCvBlobTrackerParamMS profile
   /// </summary>
   public enum BLOBTRACKER_MS_PROFILE
   {
      /// <summary>
      /// EPANECHNIKOV
      /// </summary>
      PROFILE_EPANECHNIKOV = 0,
      /// <summary>
      /// DoG
      /// </summary>
      PROFILE_DOG = 1
   }

   /// <summary>
   /// The types of blob trakcer
   /// </summary>
   public enum BLOBTRACKER_TYPE
   {
      /// <summary>
      /// Simple blob tracker based on connected component tracking
      /// </summary>
      CC,
      /// <summary>
      /// Connected component tracking and mean-shift particle filter collion-resolver
      /// </summary>
      CCMSPF,
      /// <summary>
      /// Blob tracker that integrates meanshift and connected components
      /// </summary>
      MSFG,
      /// <summary>
      /// Blob tracker that integrates meanshift and connected components
      /// </summary>
      MSFGS,
      /// <summary>
      /// Meanshift without connected-components
      /// </summary>
      MS,
      /// <summary>
      /// Particle filtering via Bhattacharya coefficient, which is roughly the dot-product of two probability densities.
      /// </summary>
      MSPF,
   }

   /// <summary>
   /// The type of blob post process module
   /// </summary>
   public enum BLOB_POST_PROCESS_TYPE
   {
      /// <summary>
      /// Kalman 
      /// </summary>
      Kalman,
      /// <summary>
      /// TimeAverRect
      /// </summary>
      TimeAverRect,
      /// <summary>
      /// TimeAverExp
      /// </summary>
      TimeAverExp
   }

   /// <summary>
   /// Histogram comparison method
   /// </summary>
   public enum HISTOGRAM_COMP_METHOD
   {
      /// <summary>
      /// Correlation/ 
      /// </summary>
      CV_COMP_CORREL = 0,
      /// <summary>
      /// Chi-Square
      /// </summary>
      CV_COMP_CHISQR = 1,
      /// <summary>
      /// Intersection
      /// </summary>
      CV_COMP_INTERSECT = 2,
      /// <summary>
      /// Bhattacharyya distance
      /// </summary>
      CV_COMP_BHATTACHARYYA = 3
   }

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

   /// <summary>
   /// Type of foreground detector
   /// </summary>
   public enum FORGROUND_DETECTOR_TYPE
   {
      /// <summary>
      /// Latest and greatest algorithm
      /// </summary>
      FGD = 0,
      /// <summary>
      /// "Mixture of Gaussians", older algorithm
      /// </summary>
      MOG = 1,
      /// <summary>
      ///  A simplified version of FGD
      /// </summary>
      FGD_SIMPLE = 2
   }

   /// <summary>
   /// The available flags for farneback optical flow computation
   /// </summary>
   [Flags]
   public enum OPTICALFLOW_FARNEBACK_FLAG
   {
      /// <summary>
      /// Default
      /// </summary>
      DEFAULT = 0,
      /// <summary>
      /// Use the input flow as the initial flow approximation
      /// </summary>
      USE_INITIAL_FLOW = 4,
      /// <summary>
      /// Use a Gaussian winsize x winsizefilter instead of box
      /// filter of the same size for optical flow estimation. Usually, this option gives more accurate
      /// flow than with a box filter, at the cost of lower speed (and normally winsize for a
      /// Gaussian window should be set to a larger value to achieve the same level of robustness)
      /// </summary>
      FARNEBACK_GAUSSIAN = 256
   }

   /*
   /// <summary>
   /// Grabcut type
   /// </summary>
   public enum GRABCUT_TYPE
   {
      /// <summary>
      /// background
      /// </summary>
      BGD = 0, 
      /// <summary>
      /// foreground
      /// </summary>
      FGD = 1,  
      /// <summary>
      /// most probably background
      /// </summary>
      PR_BGD = 2,  
      /// <summary>
      /// most probably foreground 
      /// </summary>
      PR_FGD = 3  
   }*/

   /// <summary>
   /// Grabcut initialization type
   /// </summary>
   public enum GRABCUT_INIT_TYPE
   {
      /// <summary>
      /// Initialize with rectangle
      /// </summary>
      INIT_WITH_RECT = 0,
      /// <summary>
      /// Initialize with mask
      /// </summary>
      INIT_WITH_MASK = 1,
      /// <summary>
      /// Eval
      /// </summary>
      EVAL = 2
   }

   /// <summary>
   /// CvCapture type. This is the equivalent to CV_CAP_ macros.
   /// </summary>
   public enum CaptureType
   {
      /// <summary>
      /// Autodetect
      /// </summary>
      ANY = 0,

      /// <summary>
      /// MIL proprietary drivers
      /// </summary>
      MIL = 100,

      /// <summary>
      /// Platform native
      /// </summary>
      VFW = 200,
      /// <summary>
      /// Platform native
      /// </summary>
      V4L = 200,
      /// <summary>
      /// Platform native
      /// </summary>
      V4L2 = 200,

      /// <summary>
      /// IEEE 1394 drivers
      /// </summary>
      FIREWARE = 300,
      /// <summary>
      /// IEEE 1394 drivers
      /// </summary>
      FIREWIRE = 300,
      /// <summary>
      /// IEEE 1394 drivers
      /// </summary>
      IEEE1394 = 300,
      /// <summary>
      /// IEEE 1394 drivers
      /// </summary>
      DC1394 = 300,
      /// <summary>
      /// IEEE 1394 drivers
      /// </summary>
      CMU1394 = 300,

      /// <summary>
      /// TYZX proprietary drivers
      /// </summary>
      STEREO = 400,
      /// <summary>
      /// TYZX proprietary drivers
      /// </summary>
      TYZX = 400,
      /// <summary>
      /// TYZX proprietary drivers
      /// </summary>
      LEFT = 400,
      /// <summary>
      /// TYZX proprietary drivers
      /// </summary>
      RIGHT = 401,
      /// <summary>
      /// TYZX proprietary drivers
      /// </summary>
      COLOR = 402,
      /// <summary>
      /// TYZX proprietary drivers
      /// </summary>
      TYZX_Z = 403,

      /// <summary>
      /// QuickTime
      /// </summary>
      QT = 500,

      /// <summary>
      /// Unicap drivers
      /// </summary>
      UNICAP = 600,

      /// <summary>
      /// DirectShow (via videoInput)
      /// </summary>
      DSHOW = 700,

      /// <summary>
      /// PvAPI, Prosilica GigE SDK
      /// </summary>
      PVAPI = 800,

      /// <summary>
      /// OpenNI (for Kinect)
      /// </summary>
      OPENNI = 900,

      /// <summary>
      /// Android
      /// </summary>
      ANDROID = 1000
   }
}
