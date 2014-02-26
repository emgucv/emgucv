//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
   /// <summary>
   /// Type for cvNorm
   /// </summary>
   [Flags]
   public enum NormType
   {
      /// <summary>
      /// if arr2 is NULL, norm = ||arr1||_C = max_I abs(arr1(I));
      /// if arr2 is not NULL, norm = ||arr1-arr2||_C = max_I abs(arr1(I)-arr2(I))
      /// </summary>
      C = 1,
      /// <summary>
      /// if arr2 is NULL, norm = ||arr1||_L1 = sum_I abs(arr1(I));
      /// if arr2 is not NULL, norm = ||arr1-arr2||_L1 = sum_I abs(arr1(I)-arr2(I))
      /// </summary>
      L1 = 2,
      /// <summary>
      /// if arr2 is NULL, norm = ||arr1||_L2 = sqrt( sum_I arr1(I)^2);
      /// if arr2 is not NULL, norm = ||arr1-arr2||_L2 = sqrt( sum_I (arr1(I)-arr2(I))^2 )
      /// </summary>
      L2 = 4,
      /// <summary>
      /// 
      /// </summary>
      NormMask = 7,
      /// <summary>
      /// It is used in combination with either CV_C, CV_L1 or CV_L2
      /// </summary>
      Relative = 8,
      /// <summary>
      /// It is used in combination with either CV_C, CV_L1 or CV_L2
      /// </summary>
      Diff = 16,
      /// <summary>
      /// 
      /// </summary>
      MinMax = 32,
      /// <summary>
      /// 
      /// </summary>
      DiffC = (Diff | C),
      /// <summary>
      /// 
      /// </summary>
      DiffL1 = (Diff | L1),
      /// <summary>
      /// 
      /// </summary>
      DiffL2 = (Diff | L2),
      /// <summary>
      /// norm = ||arr1-arr2||_C/||arr2||_C
      /// </summary>
      RelativeC = (Relative | C),
      /// <summary>
      /// norm = ||arr1-arr2||_L1/||arr2||_L1
      /// </summary>
      RelativeL1 = (Relative | L1),
      /// <summary>
      /// norm = ||arr1-arr2||_L2/||arr2||_L2
      /// </summary>
      RelativeL2 = (Relative | L2)
   }

   /// <summary>
   /// Type used for cvReduce function
   /// </summary>
   public enum ReduceType
   {
      /// <summary>
      /// The output is the sum of all the matrix rows/columns
      /// </summary>
      ReduceSum = 0,
      /// <summary>
      /// The output is the mean vector of all the matrix rows/columns
      /// </summary>
      ReduceAvg = 1,
      /// <summary>
      /// The output is the maximum (column/row-wise) of all the matrix rows/columns
      /// </summary>
      ReduceMax = 2,
      /// <summary>
      /// The output is the minimum (column/row-wise) of all the matrix rows/columns
      /// </summary>
      ReduceMin = 3
   }

   /// <summary>
   /// Type used for cvReduce function
   /// </summary>
   public enum ReduceDimension
   {
      /// <summary>
      /// The matrix is reduced to a single row
      /// </summary>
      SingleRow = 0,
      /// <summary>
      /// The matrix is reduced to a single column
      /// </summary>
      SingleCol = 1,
      /// <summary>
      /// The dimension is chosen automatically by analysing the dst size
      /// </summary>
      Auto = -1,
   }

   /// <summary>
   /// Type used for cvCmp function
   /// </summary>
   public enum CmpType
   {
      /// <summary>
      /// src1(I) "equal to" src2(I)
      /// </summary>
      Equal = 0,
      /// <summary>
      /// src1(I) "greater than" src2(I)
      /// </summary>
      GreaterThan = 1,
      /// <summary>
      /// src1(I) "greater or equal" src2(I)
      /// </summary>
      GreaterEqual = 2,
      /// <summary>
      /// src1(I) "less than" src2(I)
      /// </summary>
      LessThan = 3,
      /// <summary>
      /// src1(I) "less or equal" src2(I)
      /// </summary>
      LessEqual = 4,
      /// <summary>
      /// src1(I) "not equal to" src2(I)
      /// </summary>
      NotEqual = 5
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
      /// Turn the feature off (not controlled manually nor automatically)
      /// </summary>
      CV_CAP_PROP_DC1394_OFF = -4,
      /// <summary>
      /// Set automatically when a value of the feature is set by the user
      /// </summary>
      CV_CAP_PROP_DC1394_MODE_MANUAL = -3,
      /// <summary>
      /// DC1394 mode auto
      /// </summary>
      CV_CAP_PROP_DC1394_MODE_AUTO = -2,
      /// <summary>
      /// DC1394 mode one push auto
      /// </summary>
      CV_CAP_PROP_DC1394_MODE_ONE_PUSH_AUTO = -1,
      /// <summary>
      /// Film current position in milliseconds or video capture timestamp
      /// </summary>
      CV_CAP_PROP_POS_MSEC = 0,
      /// <summary>
      /// 0-based index of the frame to be decoded/captured next
      /// </summary>
      CV_CAP_PROP_POS_FRAMES = 1,
      /// <summary>
      /// Position in relative units (0 - start of the file, 1 - end of the file)
      /// </summary>
      CV_CAP_PROP_POS_AVI_RATIO = 2,
      /// <summary>
      /// Width of frames in the video stream
      /// </summary>
      CV_CAP_PROP_FRAME_WIDTH = 3,
      /// <summary>
      /// Height of frames in the video stream
      /// </summary>
      CV_CAP_PROP_FRAME_HEIGHT = 4,
      /// <summary>
      /// Frame rate 
      /// </summary>
      CV_CAP_PROP_FPS = 5,
      /// <summary>
      /// 4-character code of codec
      /// </summary>
      CV_CAP_PROP_FOURCC = 6,
      /// <summary>
      /// Number of frames in video file
      /// </summary>
      CV_CAP_PROP_FRAME_COUNT = 7,
      /// <summary>
      /// Format
      /// </summary>
      CV_CAP_PROP_FORMAT = 8,
      /// <summary>
      /// Mode
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
      /// Zoom
      /// </summary>
      CV_CAP_PROP_ZOOM = 27,
      /// <summary>
      /// Focus
      /// </summary>
      CV_CAP_PROP_FOCUS = 28,
      /// <summary>
      /// GUID
      /// </summary>
      CV_CAP_PROP_GUID = 29,
      /// <summary>
      /// ISO SPEED
      /// </summary>
      CV_CAP_PROP_ISO_SPEED = 30,
      /// <summary>
      /// MAX DC1394
      /// </summary>
      CV_CAP_PROP_MAX_DC1394 = 31,
      /// <summary>
      /// Backlight
      /// </summary>
      CV_CAP_PROP_BACKLIGHT = 32,
      /// <summary>
      /// Pan
      /// </summary>
      CV_CAP_PROP_PAN = 33,
      /// <summary>
      /// Tilt
      /// </summary>
      CV_CAP_PROP_TILT = 34,
      /// <summary>
      /// Roll
      /// </summary>
      CV_CAP_PROP_ROLL = 35,
      /// <summary>
      /// Iris
      /// </summary>
      CV_CAP_PROP_IRIS = 36,
      /// <summary>
      /// Settings
      /// </summary>
      CV_CAP_PROP_SETTINGS = 37,
      /// <summary>
      /// property for highgui class CvCapture_Android only
      /// </summary>
      CV_CAP_PROP_AUTOGRAB = 1024,
      /// <summary>
      /// readonly, tricky property, returns cpnst char* indeed
      /// </summary>
      CV_CAP_PROP_SUPPORTED_PREVIEW_SIZES_STRING = 1025,
      /// <summary>
      /// readonly, tricky property, returns cpnst char* indeed
      /// </summary>
      CV_CAP_PROP_PREVIEW_FORMAT = 1026,
      /// <summary>
      /// OpenNI map generators
      /// </summary>
      CV_CAP_OPENNI_DEPTH_GENERATOR = 1 << 31,
      /// <summary>
      /// OpenNI map generators
      /// </summary>
      CV_CAP_OPENNI_IMAGE_GENERATOR = 1 << 30,
      /// <summary>
      /// OpenNI map generators
      /// </summary>
      CV_CAP_OPENNI_GENERATORS_MASK = CV_CAP_OPENNI_DEPTH_GENERATOR + CV_CAP_OPENNI_IMAGE_GENERATOR,

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
      /// Flag that synchronizes the remapping depth map to image map
      /// by changing depth generator's view point (if the flag is "on") or
      /// sets this view point to its normal one (if the flag is "off").
      /// </summary>
      CV_CAP_PROP_OPENNI_REGISTRATION = 104,
      /// <summary>
      /// Flag that synchronizes the remapping depth map to image map
      /// by changing depth generator's view point (if the flag is "on") or
      /// sets this view point to its normal one (if the flag is "off").
      /// </summary>
      CV_CAP_PROP_OPENNI_REGISTRATION_ON = CV_CAP_PROP_OPENNI_REGISTRATION,
      /// <summary>
      /// Approx frame sync
      /// </summary>
      CV_CAP_PROP_OPENNI_APPROX_FRAME_SYNC = 105,
      /// <summary>
      /// Max buffer size
      /// </summary>
      CV_CAP_PROP_OPENNI_MAX_BUFFER_SIZE = 106,
      /// <summary>
      /// Circle buffer
      /// </summary>
      CV_CAP_PROP_OPENNI_CIRCLE_BUFFER = 107,
      /// <summary>
      /// Max time duration
      /// </summary>
      CV_CAP_PROP_OPENNI_MAX_TIME_DURATION = 108,
      /// <summary>
      /// Generator present
      /// </summary>
      CV_CAP_PROP_OPENNI_GENERATOR_PRESENT = 109,

      /// <summary>
      /// Openni image generator present
      /// </summary>
      CV_CAP_OPENNI_IMAGE_GENERATOR_PRESENT = CV_CAP_OPENNI_IMAGE_GENERATOR + CV_CAP_PROP_OPENNI_GENERATOR_PRESENT,
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
      /// Openni generator registration
      /// </summary>
      CV_CAP_OPENNI_DEPTH_GENERATOR_REGISTRATION = CV_CAP_OPENNI_DEPTH_GENERATOR + CV_CAP_PROP_OPENNI_REGISTRATION,
      /// <summary>
      /// Openni generator registration on
      /// </summary>
      CV_CAP_OPENNI_DEPTH_GENERATOR_REGISTRATION_ON = CV_CAP_OPENNI_DEPTH_GENERATOR_REGISTRATION,

      /// <summary>
      /// Properties of cameras available through GStreamer interface. Default is 1
      /// </summary>
      CV_CAP_GSTREAMER_QUEUE_LENGTH = 200,
      /// <summary>
      /// Ip for anable multicast master mode. 0 for disable multicast
      /// </summary>
      CV_CAP_PROP_PVAPI_MULTICASTIP = 300,

      /// <summary>
      /// Change image resolution by binning or skipping.
      /// </summary>
      CV_CAP_PROP_XI_DOWNSAMPLING = 400,      
      /// <summary>
      /// Output data format
      /// </summary>
      CV_CAP_PROP_XI_DATA_FORMAT = 401,     
      /// <summary>
      /// Horizontal offset from the origin to the area of interest (in pixels).
      /// </summary>
      CV_CAP_PROP_XI_OFFSET_X = 402,      
      /// <summary>
      /// Vertical offset from the origin to the area of interest (in pixels).
      /// </summary>
      CV_CAP_PROP_XI_OFFSET_Y = 403,      
      /// <summary>
      /// Defines source of trigger.
      /// </summary>
      CV_CAP_PROP_XI_TRG_SOURCE = 404,      
      /// <summary>
      /// Generates an internal trigger. PRM_TRG_SOURCE must be set to TRG_SOFTWARE.
      /// </summary>
      CV_CAP_PROP_XI_TRG_SOFTWARE = 405,      
      /// <summary>
      /// Selects general purpose input
      /// </summary>
      CV_CAP_PROP_XI_GPI_SELECTOR = 406,      
      /// <summary>
      /// Set general purpose input mode
      /// </summary>
      CV_CAP_PROP_XI_GPI_MODE = 407,      
      /// <summary>
      /// Get general purpose level
      /// </summary>
      CV_CAP_PROP_XI_GPI_LEVEL = 408,      
      /// <summary>
      /// Selects general purpose output
      /// </summary>
      CV_CAP_PROP_XI_GPO_SELECTOR = 409,      
      /// <summary>
      /// Set general purpose output mode
      /// </summary>
      CV_CAP_PROP_XI_GPO_MODE = 410,      
      /// <summary>
      /// Selects camera signalling LED
      /// </summary>
      CV_CAP_PROP_XI_LED_SELECTOR = 411,      
      /// <summary>
      /// Define camera signalling LED functionality
      /// </summary>
      CV_CAP_PROP_XI_LED_MODE = 412,      
      /// <summary>
      /// Calculates White Balance(must be called during acquisition)
      /// </summary>
      CV_CAP_PROP_XI_MANUAL_WB = 413,      
      /// <summary>
      /// Automatic white balance
      /// </summary>
      CV_CAP_PROP_XI_AUTO_WB = 414,      
      /// <summary>
      /// Automatic exposure/gain
      /// </summary>
      CV_CAP_PROP_XI_AEAG = 415,      
      /// <summary>
      /// Exposure priority (0.5 - exposure 50%, gain 50%).
      /// </summary>
      CV_CAP_PROP_XI_EXP_PRIORITY = 416,      
      /// <summary>
      /// Maximum limit of exposure in AEAG procedure
      /// </summary>
      CV_CAP_PROP_XI_AE_MAX_LIMIT = 417, 
      /// <summary>
      /// Maximum limit of gain in AEAG procedure
      /// </summary>
      CV_CAP_PROP_XI_AG_MAX_LIMIT = 418,      
      /// <summary>
      /// Average intensity of output signal AEAG should achieve(in %)
      /// </summary>
      CV_CAP_PROP_XI_AEAG_LEVEL = 419,       
      /// <summary>
      /// Image capture timeout in milliseconds
      /// </summary>
      CV_CAP_PROP_XI_TIMEOUT = 420,     

      /// <summary>
      /// Android flash mode
      /// </summary>
      CV_CAP_PROP_ANDROID_FLASH_MODE = 8001,
      /// <summary>
      /// Android focus mode
      /// </summary>
      CV_CAP_PROP_ANDROID_FOCUS_MODE = 8002,
      /// <summary>
      /// Android white balance
      /// </summary>
      CV_CAP_PROP_ANDROID_WHITE_BALANCE = 8003,
      /// <summary>
      /// Android anti banding
      /// </summary>
      CV_CAP_PROP_ANDROID_ANTIBANDING = 8004,
      /// <summary>
      /// Android focal length
      /// </summary>
      CV_CAP_PROP_ANDROID_FOCAL_LENGTH = 8005,
      /// <summary>
      /// Android focus distance near
      /// </summary>
      CV_CAP_PROP_ANDROID_FOCUS_DISTANCE_NEAR = 8006,
      /// <summary>
      /// Android focus distance optimal
      /// </summary>
      CV_CAP_PROP_ANDROID_FOCUS_DISTANCE_OPTIMAL = 8007,
      /// <summary>
      /// Android focus distance far
      /// </summary>
      CV_CAP_PROP_ANDROID_FOCUS_DISTANCE_FAR = 8008,

      /// <summary>
      /// iOS device focus
      /// </summary>
      CV_CAP_PROP_IOS_DEVICE_FOCUS = 9001,
      /// <summary>
      /// iOS device exposure
      /// </summary>
      CV_CAP_PROP_IOS_DEVICE_EXPOSURE = 9002,
      /// <summary>
      /// iOS device flash
      /// </summary>
      CV_CAP_PROP_IOS_DEVICE_FLASH = 9003,
      /// <summary>
      /// iOS device whitebalance 
      /// </summary>
      CV_CAP_PROP_IOS_DEVICE_WHITEBALANCE = 9004,
      /// <summary>
      /// iOS device torch
      /// </summary>
      CV_CAP_PROP_IOS_DEVICE_TORCH = 9005,

      /// <summary>
      /// Smartek Giganetix Ethernet Vision: frame offset X
      /// </summary>
      CV_CAP_PROP_GIGA_FRAME_OFFSET_X = 10001,

      /// <summary>
      /// Smartek Giganetix Ethernet Vision: frame offset Y
      /// </summary>
      CV_CAP_PROP_GIGA_FRAME_OFFSET_Y = 10002,

      /// <summary>
      /// Smartek Giganetix Ethernet Vision: frame width max
      /// </summary>
      CV_CAP_PROP_GIGA_FRAME_WIDTH_MAX = 10003,

      /// <summary>
      /// Smartek Giganetix Ethernet Vision: frame height max
      /// </summary>
      CV_CAP_PROP_GIGA_FRAME_HEIGH_MAX = 10004,

      /// <summary>
      /// Smartek Giganetix Ethernet Vision: frame sens width
      /// </summary>
      CV_CAP_PROP_GIGA_FRAME_SENS_WIDTH = 10005,

      /// <summary>
      /// Smartek Giganetix Ethernet Vision: frame sens height
      /// </summary>
      CV_CAP_PROP_GIGA_FRAME_SENS_HEIGH = 10006

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
   public enum ColorConversion
   {
      ///<summary>
      ///Convert BGR color to BGRA color
      ///</summary>
      BGR2BGRA = 0,
      /// <summary>
      /// Convert RGB color to RGBA color
      /// </summary>
      RGB2RGBA = BGR2BGRA,

      ///<summary>
      ///Convert BGRA color to BGR color
      ///</summary>
      BGRA2BGR = 1,
      /// <summary>
      /// Convert RGBA color to RGB color
      /// </summary>
      RGBA2RGB = BGRA2BGR,
      /// <summary>
      /// Convert BGR color to RGBA color
      /// </summary>
      BGR2RGBA = 2,
      /// <summary>
      /// Convert RGB color to BGRA color
      /// </summary>
      RGB2BGRA = BGR2RGBA,
      /// <summary>
      /// Convert RGBA color to BGR color
      /// </summary>
      RGBA2BGR = 3,
      /// <summary>
      /// Convert BGRA color to RGB color
      /// </summary>
      BGRA2RGB = RGBA2BGR,
      /// <summary>
      /// Convert BGR color to RGB color
      /// </summary>
      BGR2RGB = 4,
      /// <summary>
      /// Convert RGB color to BGR color
      /// </summary>
      RGB2BGR = BGR2RGB,
      /// <summary>
      /// Convert BGRA color to RGBA color
      /// </summary>
      BGRA2RGBA = 5,
      /// <summary>
      /// Convert RGBA color to BGRA color
      /// </summary>
      RGBA2BGRA = BGRA2RGBA,
      ///<summary>
      ///Convert BGR color to GRAY color
      ///</summary>
      BGR2GRAY = 6,
      /// <summary>
      /// Convert RGB color to GRAY color
      /// </summary>
      RGB2GRAY = 7,
      ///<summary>
      ///Convert GRAY color to BGR color
      ///</summary>
      GRAY2BGR = 8,
      /// <summary>
      /// Convert GRAY color to RGB color
      /// </summary>
      GRAY2RGB = GRAY2BGR,
      ///<summary>
      ///Convert GRAY color to BGRA color
      ///</summary>
      GRAY2BGRA = 9,
      /// <summary>
      /// Convert GRAY color to RGBA color
      /// </summary>
      GRAY2RGBA = GRAY2BGRA,
      ///<summary>
      ///Convert BGRA color to GRAY color
      ///</summary>
      BGRA2GRAY = 10,
      /// <summary>
      /// Convert RGBA color to GRAY color
      /// </summary>
      RGBA2GRAY = 11,
      ///<summary>
      ///Convert BGR color to BGR565 color
      ///</summary>
      BGR2BGR565 = 12,
      /// <summary>
      /// Convert RGB color to BGR565 color
      /// </summary>
      RGB2BGR565 = 13,
      ///<summary>
      ///Convert BGR565 color to BGR color
      ///</summary>
      BGR5652BGR = 14,
      /// <summary>
      /// Convert BGR565 color to RGB color
      /// </summary>
      BGR5652RGB = 15,
      ///<summary>
      ///Convert BGRA color to BGR565 color
      ///</summary>
      BGRA2BGR565 = 16,
      /// <summary>
      /// Convert RGBA color to BGR565 color
      /// </summary>
      RGBA2BGR565 = 17,
      ///<summary>
      ///Convert BGR565 color to BGRA color
      ///</summary>
      BGR5652BGRA = 18,
      /// <summary>
      /// Convert BGR565 color to RGBA color
      /// </summary>
      BGR5652RGBA = 19,
      ///<summary>
      ///Convert GRAY color to BGR565 color
      ///</summary>
      GRAY2BGR565 = 20,
      ///<summary>
      ///Convert BGR565 color to GRAY color
      ///</summary>
      BGR5652GRAY = 21,
      ///<summary>
      ///Convert BGR color to BGR555 color
      ///</summary>
      BGR2BGR555 = 22,
      /// <summary>
      /// Convert RGB color to BGR555 color
      /// </summary>
      RGB2BGR555 = 23,
      ///<summary>
      ///Convert BGR555 color to BGR color
      ///</summary>
      BGR5552BGR = 24,
      /// <summary>
      /// Convert BGR555 color to RGB color
      /// </summary>
      BGR5552RGB = 25,
      ///<summary>
      ///Convert BGRA color to BGR555 color
      ///</summary>
      BGRA2BGR555 = 26,
      /// <summary>
      /// Convert RGBA color to BGR555 color
      /// </summary>
      RGBA2BGR555 = 27,
      ///<summary>
      ///Convert BGR555 color to BGRA color
      ///</summary>
      BGR5552BGRA = 28,
      /// <summary>
      /// Convert BGR555 color to RGBA color
      /// </summary>
      BGR5552RGBA = 29,
      ///<summary>
      ///Convert GRAY color to BGR555 color
      ///</summary>
      GRAY2BGR555 = 30,
      ///<summary>
      ///Convert BGR555 color to GRAY color
      ///</summary>
      BGR5552GRAY = 31,
      ///<summary>
      ///Convert BGR color to XYZ color
      ///</summary>
      BGR2XYZ = 32,
      /// <summary>
      /// Convert RGB color to XYZ color
      /// </summary>
      RGB2XYZ = 33,
      ///<summary>
      ///Convert XYZ color to BGR color
      ///</summary>
      XYZ2BGR = 34,
      /// <summary>
      /// Convert XYZ color to RGB color
      /// </summary>
      XYZ2RGB = 35,
      ///<summary>
      ///Convert BGR color to YCrCb color
      ///</summary>
      BGR2YCrCb = 36,
      /// <summary>
      /// Convert RGB color to YCrCb color
      /// </summary>
      RGB2YCrCb = 37,
      ///<summary>
      ///Convert YCrCb color to BGR color
      ///</summary>
      YCrCb2BGR = 38,
      /// <summary>
      /// Convert YCrCb color to RGB color
      /// </summary>
      YCrCb2RGB = 39,
      ///<summary>
      ///Convert BGR color to HSV color
      ///</summary>
      BGR2HSV = 40,
      /// <summary>
      /// Convert RGB colot to HSV color
      /// </summary>
      RGB2HSV = 41,
      ///<summary>
      ///Convert BGR color to Lab color
      ///</summary>
      BGR2Lab = 44,
      /// <summary>
      /// Convert RGB color to Lab color
      /// </summary>
      RGB2Lab = 45,
      ///<summary>
      ///Convert BayerBG color to BGR color
      ///</summary>
      BayerBG2BGR = 46,
      ///<summary>
      ///Convert BayerGB color to BGR color
      ///</summary>
      BayerGB2BGR = 47,
      ///<summary>
      ///Convert BayerRG color to BGR color
      ///</summary>
      BayerRG2BGR = 48,
      ///<summary>
      ///Convert BayerGR color to BGR color
      ///</summary>
      BayerGR2BGR = 49,
      /// <summary>
      /// Convert BayerBG color to BGR color
      /// </summary>
      BayerBG2RGB = BayerRG2BGR,
      /// <summary>
      /// Convert BayerRG color to BGR color
      /// </summary>
      BayerGB2RGB = BayerGR2BGR,
      /// <summary>
      /// Convert BayerRG color to RGB color
      /// </summary>
      BayerRG2RGB = BayerBG2BGR,
      /// <summary>
      /// Convert BayerGR color to RGB color
      /// </summary>
      BayerGR2RGB = BayerGB2BGR,
      ///<summary>
      ///Convert BGR color to Luv color
      ///</summary>
      BGR2Luv = 50,
      /// <summary>
      /// Convert RGB color to Luv color
      /// </summary>
      RGB2Luv = 51,
      ///<summary>
      ///Convert BGR color to HLS color
      ///</summary>
      BGR2HLS = 52,
      /// <summary>
      /// Convert RGB color to HLS color
      /// </summary>
      RGB2HLS = 53,
      ///<summary>
      ///Convert HSV color to BGR color
      ///</summary>
      HSV2BGR = 54,
      /// <summary>
      /// Convert HSV color to RGB color
      /// </summary>
      HSV2RGB = 55,
      ///<summary>
      ///Convert Lab color to BGR color
      ///</summary>
      Lab2BGR = 56,
      /// <summary>
      /// Convert Lab color to RGB color
      /// </summary>
      Lab2RGB = 57,
      ///<summary>
      ///Convert Luv color to BGR color
      ///</summary>
      Luv2BGR = 58,
      /// <summary>
      /// Convert Luv color to RGB color
      /// </summary>
      Luv2RGB = 59,
      ///<summary>
      ///Convert HLS color to BGR color
      ///</summary>
      HLS2BGR = 60,
      /// <summary>
      /// Convert HLS color to RGB color
      /// </summary>
      HLS2RGB = 61,
      /// <summary>
      /// Convert BayerBG pattern to BGR color using VNG
      /// </summary>
      BayerBG2BGR_VNG = 62,
      /// <summary>
      /// Convert BayerGB pattern to BGR color using VNG
      /// </summary>
      BayerGB2BGR_VNG = 63,
      /// <summary>
      /// Convert BayerRG pattern to BGR color using VNG
      /// </summary>
      BayerRG2BGR_VNG = 64,
      /// <summary>
      /// Convert BayerGR pattern to BGR color using VNG
      /// </summary>
      BayerGR2BGR_VNG = 65,
      /// <summary>
      /// Convert BayerBG pattern to RGB color using VNG
      /// </summary>
      BayerBG2RGB_VNG = BayerRG2BGR_VNG,
      /// <summary>
      /// Convert BayerGB pattern to RGB color using VNG
      /// </summary>
      BayerGB2RGB_VNG = BayerGR2BGR_VNG,
      /// <summary>
      /// Convert BayerRG pattern to RGB color using VNG
      /// </summary>
      BayerRG2RGB_VNG = BayerBG2BGR_VNG,
      /// <summary>
      /// Convert BayerGR pattern to RGB color using VNG
      /// </summary>
      BayerGR2RGB_VNG = BayerGB2BGR_VNG,

      /// <summary>
      /// Convert BGR to HSV
      /// </summary>
      BGR2HSV_FULL = 66,
      /// <summary>
      /// Convert RGB to HSV
      /// </summary>
      RGB2HSV_FULL = 67,
      /// <summary>
      /// Convert BGR to HLS
      /// </summary>
      BGR2HLS_FULL = 68,
      /// <summary>
      /// Convert RGB to HLS
      /// </summary>
      RGB2HLS_FULL = 69,

      /// <summary>
      /// Convert HSV color to BGR color
      /// </summary>
      HSV2BGR_FULL = 70,
      /// <summary>
      /// Convert HSV color to RGB color
      /// </summary>
      HSV2RGB_FULL = 71,
      /// <summary>
      /// Convert HLS color to BGR color
      /// </summary>
      HLS2BGR_FULL = 72,
      /// <summary>
      /// Convert HLS color to RGB color
      /// </summary>
      HLS2RGB_FULL = 73,

      /// <summary>
      /// Convert sBGR color to Lab color
      /// </summary>
      LBGR2Lab = 74,
      /// <summary>
      /// Convert sRGB color to Lab color
      /// </summary>
      LRGB2Lab = 75,
      /// <summary>
      /// Convert sBGR color to Luv color
      /// </summary>
      LBGR2Luv = 76,
      /// <summary>
      /// Convert sRGB color to Luv color
      /// </summary>
      LRGB2Luv = 77,

      /// <summary>
      /// Convert Lab color to sBGR color
      /// </summary>
      Lab2LBGR = 78,
      /// <summary>
      /// Convert Lab color to sRGB color
      /// </summary>
      Lab2LRGB = 79,
      /// <summary>
      /// Convert Luv color to sBGR color
      /// </summary>
      Luv2LBGR = 80,
      /// <summary>
      /// Convert Luv color to sRGB color
      /// </summary>
      Luv2LRGB = 81,

      /// <summary>
      /// Convert BGR color to YUV
      /// </summary>
      BGR2YUV = 82,
      /// <summary>
      /// Convert RGB color to YUV
      /// </summary>
      RGB2YUV = 83,
      /// <summary>
      /// Convert YUV color to BGR
      /// </summary>
      YUV2BGR = 84,
      /// <summary>
      /// Convert YUV color to RGB
      /// </summary>
      YUV2RGB = 85,

      /// <summary>
      /// Convert BayerBG to GRAY
      /// </summary>
      BayerBG2GRAY = 86,
      /// <summary>
      /// Convert BayerGB to GRAY
      /// </summary>
      BayerGB2GRAY = 87,
      /// <summary>
      /// Convert BayerRG to GRAY
      /// </summary>
      BayerRG2GRAY = 88,
      /// <summary>
      /// Convert BayerGR to GRAY
      /// </summary>
      BayerGR2GRAY = 89,
      /// <summary>
      /// Convert YUV420i to RGB
      /// </summary>
      YUV420i2RGB = 90,
      /// <summary>
      /// Convert YUV420i to BGR
      /// </summary>
      YUV420i2BGR = 91,
      /// <summary>
      /// Convert YUV420sp to RGB
      /// </summary>
      YUV420sp2RGB = 92,
      /// <summary>
      /// Convert YUV320sp to BGR
      /// </summary>
      YUV420sp2BGR = 93,
      /// <summary>
      /// Convert YUV320i to RGBA
      /// </summary>
      YUV420i2RGBA = 94,
      /// <summary>
      /// Convert YUV420i to BGRA
      /// </summary>
      YUV420i2BGRA = 95,
      /// <summary>
      /// Convert YUV420sp to RGBA
      /// </summary>
      YUV420sp2RGBA = 96,
      /// <summary>
      /// Convert YUV420sp to BGRA
      /// </summary>
      YUV420sp2BGRA = 97,

      /// <summary>
      /// Convert YUV (YV12) to RGB
      /// </summary>
      YUV2RGB_YV12 = 98,
      /// <summary>
      /// Convert YUV (YV12) to BGR
      /// </summary>
      YUV2BGR_YV12 = 99,
      /// <summary>
      /// Convert YUV (iYUV) to RGB
      /// </summary>
      YUV2RGB_IYUV = 100,
      /// <summary>
      /// Convert YUV (iYUV) to BGR
      /// </summary>
      YUV2BGR_IYUV = 101,
      /// <summary>
      /// Convert YUV (i420) to RGB
      /// </summary>
      YUV2RGB_I420 = YUV2RGB_IYUV,
      /// <summary>
      /// Convert YUV (i420) to BGR
      /// </summary>
      YUV2BGR_I420 = YUV2BGR_IYUV,
      /// <summary>
      /// Convert YUV (420p) to RGB
      /// </summary>
      YUV420p2RGB = YUV2RGB_YV12,
      /// <summary>
      /// Convert YUV (420p) to BGR
      /// </summary>
      YUV420p2BGR = YUV2BGR_YV12,

      /// <summary>
      /// Convert YUV (YV12) to RGBA
      /// </summary>
      YUV2RGBA_YV12 = 102,
      /// <summary>
      /// Convert YUV (YV12) to BGRA
      /// </summary>
      YUV2BGRA_YV12 = 103,
      /// <summary>
      /// Convert YUV (iYUV) to RGBA
      /// </summary>
      YUV2RGBA_IYUV = 104,
      /// <summary>
      /// Convert YUV (iYUV) to BGRA
      /// </summary>
      YUV2BGRA_IYUV = 105,
      /// <summary>
      /// Convert YUV (i420) to RGBA
      /// </summary>
      YUV2RGBA_I420 = YUV2RGBA_IYUV,
      /// <summary>
      /// Convert YUV (i420) to BGRA
      /// </summary>
      YUV2BGRA_I420 = YUV2BGRA_IYUV,
      /// <summary>
      /// Convert YUV (420p) to RGBA
      /// </summary>
      YUV420p2RGBA = YUV2RGBA_YV12,
      /// <summary>
      /// Convert YUV (420p) to BGRA
      /// </summary>
      YUV420p2BGRA = YUV2BGRA_YV12,

      /// <summary>
      /// Convert YUV 420 to Gray
      /// </summary>
      YUV2GRAY_420 = 106,
      /// <summary>
      /// Convert YUV NV21 to Gray
      /// </summary>
      YUV2GRAY_NV21 = YUV2GRAY_420,
      /// <summary>
      /// Convert YUV NV12 to Gray
      /// </summary>
      YUV2GRAY_NV12 = YUV2GRAY_420,
      /// <summary>
      /// Convert YUV YV12 to Gray
      /// </summary>
      YUV2GRAY_YV12 = YUV2GRAY_420,
      /// <summary>
      /// Convert YUV (iYUV) to Gray
      /// </summary>
      YUV2GRAY_IYUV = YUV2GRAY_420,
      /// <summary>
      /// Convert YUV (i420) to Gray
      /// </summary>
      YUV2GRAY_I420 = YUV2GRAY_420,
      /// <summary>
      /// Convert YUV (420sp) to Gray
      /// </summary>
      YUV420sp2GRAY = YUV2GRAY_420,
      /// <summary>
      /// Convert YUV (420p) to Gray
      /// </summary>
      YUV420p2GRAY = YUV2GRAY_420,

      //YUV 4:2:2 formats family
      /// <summary>
      /// Convert YUV (UYVY) to RGB
      /// </summary>
      YUV2RGB_UYVY = 107,
      /// <summary>
      /// Convert YUV (UYVY) to BGR
      /// </summary>
      YUV2BGR_UYVY = 108,
      //YUV2RGB_VYUY = 109,
      //YUV2BGR_VYUY = 110,
      /// <summary>
      /// Convert YUV (Y422) to RGB
      /// </summary>
      YUV2RGB_Y422 = YUV2RGB_UYVY,
      /// <summary>
      /// Convert YUV (Y422) to BGR
      /// </summary>
      YUV2BGR_Y422 = YUV2BGR_UYVY,
      /// <summary>
      /// Convert YUV (UYNY) to RGB
      /// </summary>
      YUV2RGB_UYNV = YUV2RGB_UYVY,
      /// <summary>
      /// Convert YUV (UYNV) to BGR
      /// </summary>
      YUV2BGR_UYNV = YUV2BGR_UYVY,

      /// <summary>
      /// Convert YUV (UYVY) to RGBA
      /// </summary>
      YUV2RGBA_UYVY = 111,
      /// <summary>
      /// Convert YUV (VYUY) to BGRA
      /// </summary>
      YUV2BGRA_UYVY = 112,
      //YUV2RGBA_VYUY = 113,
      //YUV2BGRA_VYUY = 114,
      /// <summary>
      /// Convert YUV (Y422) to RGBA
      /// </summary>
      YUV2RGBA_Y422 = YUV2RGBA_UYVY,
      /// <summary>
      /// Convert YUV (Y422) to BGRA
      /// </summary>
      YUV2BGRA_Y422 = YUV2BGRA_UYVY,
      /// <summary>
      /// Convert YUV (UYNV) to RGBA 
      /// </summary>
      YUV2RGBA_UYNV = YUV2RGBA_UYVY,
      /// <summary>
      /// Convert YUV (UYNV) to BGRA
      /// </summary>
      YUV2BGRA_UYNV = YUV2BGRA_UYVY,

      /// <summary>
      /// Convert YUV (YUY2) to RGB
      /// </summary>
      YUV2RGB_YUY2 = 115,
      /// <summary>
      /// Convert YUV (YUY2) to BGR
      /// </summary>
      YUV2BGR_YUY2 = 116,
      /// <summary>
      /// Convert YUV (YVYU) to RGB
      /// </summary>
      YUV2RGB_YVYU = 117,
      /// <summary>
      /// Convert YUV (YVYU) to BGR
      /// </summary>
      YUV2BGR_YVYU = 118,
      /// <summary>
      /// Convert YUV (YUYV) to RGB
      /// </summary>
      YUV2RGB_YUYV = YUV2RGB_YUY2,
      /// <summary>
      /// Convert YUV (YUYV) to BGR 
      /// </summary>
      YUV2BGR_YUYV = YUV2BGR_YUY2,
      /// <summary>
      /// Convert YUV (YUNV) to RGB
      /// </summary>
      YUV2RGB_YUNV = YUV2RGB_YUY2,
      /// <summary>
      /// Convert YUV (YUNV) to BGR
      /// </summary>
      YUV2BGR_YUNV = YUV2BGR_YUY2,

      /// <summary>
      /// Convert YUV (YUY2) to RGBA
      /// </summary>
      YUV2RGBA_YUY2 = 119,
      /// <summary>
      /// Convert YUV (YUY2) to BGRA
      /// </summary>
      YUV2BGRA_YUY2 = 120,
      /// <summary>
      /// Convert YUV (YVYU) to RGBA
      /// </summary>
      YUV2RGBA_YVYU = 121,
      /// <summary>
      /// Convert YUV (YVYU) to BGRA
      /// </summary>
      YUV2BGRA_YVYU = 122,
      /// <summary>
      /// Convert YUV (YUYV) to RGBA
      /// </summary>
      YUV2RGBA_YUYV = YUV2RGBA_YUY2,
      /// <summary>
      /// Convert YUV (YUYV) to BGRA
      /// </summary>
      YUV2BGRA_YUYV = YUV2BGRA_YUY2,
      /// <summary>
      /// Convert YUV (YUNV) to RGBA
      /// </summary>
      YUV2RGBA_YUNV = YUV2RGBA_YUY2,
      /// <summary>
      /// Convert YUV (YUNV) to BGRA
      /// </summary>
      YUV2BGRA_YUNV = YUV2BGRA_YUY2,

      /// <summary>
      /// Convert YUV (UYVY) to Gray
      /// </summary>
      YUV2GRAY_UYVY = 123,
      /// <summary>
      /// Convert YUV (YUY2) to Gray
      /// </summary>
      YUV2GRAY_YUY2 = 124,
      //YUV2GRAY_VYUY = YUV2GRAY_UYVY,
      /// <summary>
      /// Convert YUV (Y422) to Gray
      /// </summary>
      YUV2GRAY_Y422 = YUV2GRAY_UYVY,
      /// <summary>
      /// Convert YUV (UYNV) to Gray
      /// </summary>
      YUV2GRAY_UYNV = YUV2GRAY_UYVY,
      /// <summary>
      /// Convert YUV (YVYU) to Gray
      /// </summary>
      YUV2GRAY_YVYU = YUV2GRAY_YUY2,
      /// <summary>
      /// Convert YUV (YUYV) to Gray
      /// </summary>
      YUV2GRAY_YUYV = YUV2GRAY_YUY2,
      /// <summary>
      /// Convert YUV (YUNV) to Gray
      /// </summary>
      YUV2GRAY_YUNV = YUV2GRAY_YUY2,

      /// <summary>
      /// Alpha premultiplication
      /// </summary>
      RGBA2mRGBA = 125,
      /// <summary>
      /// Alpha premultiplication
      /// </summary>
      mRGBA2RGBA = 126,

      // RGB to YUV 4:2:0 family

      /// <summary>
      /// Convert RGB to YUV_I420
      /// </summary>
      RGB2YUV_I420 = 127,
      /// <summary>
      /// Convert BGR to YUV_I420
      /// </summary>
      BGR2YUV_I420 = 128,
      /// <summary>
      /// Convert RGB to YUV_IYUV
      /// </summary>
      RGB2YUV_IYUV = RGB2YUV_I420,
      /// <summary>
      /// Convert BGR to YUV_IYUV
      /// </summary>
      BGR2YUV_IYUV = BGR2YUV_I420,

      /// <summary>
      /// Convert RGBA to YUV_I420
      /// </summary>
      RGBA2YUV_I420 = 129,
      /// <summary>
      /// Convert BGRA to YUV_I420
      /// </summary>
      BGRA2YUV_I420 = 130,
      /// <summary>
      /// Convert RGBA to YUV_IYUV
      /// </summary>
      RGBA2YUV_IYUV = RGBA2YUV_I420,
      /// <summary>
      /// Convert BGRA to YUV_IYUV
      /// </summary>
      BGRA2YUV_IYUV = BGRA2YUV_I420,
      /// <summary>
      /// Convert RGB to YUV_YV12
      /// </summary>
      RGB2YUV_YV12 = 131,
      /// <summary>
      /// Convert BGR to YUV_YV12
      /// </summary>
      BGR2YUV_YV12 = 132,
      /// <summary>
      /// Convert RGBA to YUV_YV12
      /// </summary>
      RGBA2YUV_YV12 = 133,
      /// <summary>
      /// Convert BGRA to YUV_YV12
      /// </summary>
      BGRA2YUV_YV12 = 134,

      /// <summary>
      /// Convert BayerBG to BGR (Edge-Aware Demosaicing)
      /// </summary>
      BayerBG2BGR_EA = 135,
      /// <summary>
      /// Convert BayerGB to BGR (Edge-Aware Demosaicing)
      /// </summary>
      BayerGB2BGR_EA = 136,
      /// <summary>
      /// Convert BayerRG to BGR (Edge-Aware Demosaicing)
      /// </summary>
      BayerRG2BGR_EA = 137,
      /// <summary>
      /// Convert BayerGR to BGR (Edge-Aware Demosaicing)
      /// </summary>
      BayerGR2BGR_EA = 138,

      /// <summary>
      /// Convert BayerBG to RGB (Edge-Aware Demosaicing)
      /// </summary>
      BayerBG2RGB_EA = BayerRG2BGR_EA,
      /// <summary>
      /// Convert BayerGB to RGB (Edge-Aware Demosaicing)
      /// </summary>
      BayerGB2RGB_EA = BayerGR2BGR_EA,
      /// <summary>
      /// Convert BayerRG to RGB (Edge-Aware Demosaicing)
      /// </summary>
      BayerRG2RGB_EA = BayerBG2BGR_EA,
      /// <summary>
      /// Convert BayerGR to RGB (Edge-Aware Demosaicing)
      /// </summary>
      BayerGR2RGB_EA = BayerGB2BGR_EA,

      /// <summary>
      /// The max number, do not use
      /// </summary>
      COLORCVT_MAX = 139
   }

   /*
   /// <summary>
   /// Type for cvPyrUp(cvPryDown)
   /// </summary>
   public enum FILTER_TYPE
   {
      /// <summary>
      /// 
      /// </summary>
      CV_GAUSSIAN_5x5 = 7
   }*/

   /// <summary>
   /// Fonts
   /// </summary>
   public enum FontFace
   {
      /// <summary>
      /// Hershey simplex
      /// </summary>
      HersheySimplex = 0,
      /// <summary>
      /// Hershey plain
      /// </summary>
      HersheyPlain = 1,
      /// <summary>
      /// Hershey duplex 
      /// </summary>
      HersheyDuplex = 2,
      /// <summary>
      /// Hershey complex
      /// </summary>
      HersheyComplex = 3,
      /// <summary>
      /// Hershey triplex
      /// </summary>
      HersheyTriplex = 4,
      /// <summary>
      /// Hershey complex small
      /// </summary>
      HersheyComplexSmall = 5,
      /// <summary>
      /// Hershey script simplex
      /// </summary>
      HersheyScriptSimplex = 6,
      /// <summary>
      /// Hershey script complex
      /// </summary>
      HersheyScriptComplex = 7
   }

   
   /// <summary>
   /// Flags used for GEMM function
   /// </summary>
   [Flags]
   public enum GemmType
   {
      /// <summary>
      /// Do not apply transpose to neither matrices
      /// </summary>
      Default = 0,
      /// <summary>
      /// transpose src1
      /// </summary>
      Src1Transpose = 1,
      /// <summary>
      /// transpose src2
      /// </summary>
      Src2Transpose = 2,
      /// <summary>
      /// transpose src3
      /// </summary>
      Src3Transpose = 4
   }

   /// <summary>
   /// Hough detection type
   /// </summary>
   public enum HoughType
   {
      /*
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
      CV_HOUGH_MULTI_SCALE = 2,*/
      /// <summary>
      /// 
      /// </summary>
      Gradient = 3
   }

   
   /// <summary>
   /// Inpaint type
   /// </summary>
   public enum InpaintType : int
   {
      /// <summary>
      /// Navier-Stokes based method.
      /// </summary>
      NS = 0,
      /// <summary>
      /// The method by Alexandru Telea 
      /// </summary>
      Telea = 1
   }

   /// <summary>
   /// Edge preserving filter flag
   /// </summary>
   public enum EdgePreservingFilterFlag
   {
      /// <summary>
      /// Recurs filter
      /// </summary>
      RecursFilter = 1,
      /// <summary>
      /// Norm conv filter
      /// </summary>
      NormconvFilter = 2
   }

   /// <summary>
   /// Interpolation types
   /// </summary>
   public enum Inter
   {
      /// <summary>
      /// Nearest-neighbor interpolation
      /// </summary>
      Nearest = 0,
      /// <summary>
      /// Bilinear interpolation
      /// </summary>
      Linear = 1,
      /// <summary>
      /// Resampling using pixel area relation. It is the preferred method for image decimation that gives moire-free results. In case of zooming it is similar to CV_INTER_NN method
      /// </summary>
      Cubic = 2,
      /// <summary>
      /// Bicubic interpolation
      /// </summary>
      Area = 3,
      /// <summary>
      /// LANCZOS 4
      /// </summary>
      Lanczos4 = 4
   }

   /// <summary>
   /// Interpolation type
   /// </summary>
   public enum SmoothType
   {
      /// <summary>
      /// (simple blur with no scaling) - summation over a pixel param1xparam2 neighborhood. If the neighborhood size may vary, one may precompute integral image with cvIntegral function
      /// </summary>
      BlurNoScale = 0,
      /// <summary>
      /// (simple blur) - summation over a pixel param1xparam2 neighborhood with subsequent scaling by 1/(param1xparam2). 
      /// </summary>
      Blur = 1,
      /// <summary>
      /// (gaussian blur) - convolving image with param1xparam2 Gaussian kernel. 
      /// </summary>
      Gaussian = 2,
      /// <summary>
      /// (median blur) - finding median of param1xparam1 neighborhood (i.e. the neighborhood is square). 
      /// </summary>
      Median = 3,
      /// <summary>
      /// (bilateral filter) - applying bilateral 3x3 filtering with color sigma=param1 and space sigma=param2. Information about bilateral filtering can be found 
      /// </summary>
      Bilateral = 4
   }

   /// <summary>
   /// cvLoadImage type
   /// </summary>
   [Flags]
   public enum LoadImageType
   {
      /// <summary>
      /// 8bit, color or not 
      /// </summary>
      Unchanged = -1,

      /// <summary>
      /// 8bit, gray
      /// </summary>
      Grayscale = 0,

      /// <summary>
      /// ?, color
      /// </summary>
      Color = 1,

      /// <summary>
      /// any depth, ?
      /// </summary>
      AnyDepth = 2,

      /// <summary>
      /// ?, any color
      /// </summary>
      AnyColor = 4,
   }

   /// <summary>
   /// OpenCV depth type
   /// </summary>
   public enum DepthType
   {
      /// <summary>
      /// default
      /// </summary>
      Default = -1,
      /// <summary>
      /// Byte
      /// </summary>
      Cv8U = 0,
      /// <summary>
      /// SByte
      /// </summary>
      Cv8S = 1,
      /// <summary>
      /// UInt16
      /// </summary>
      Cv16U = 2,
      /// <summary>
      /// Int16
      /// </summary>
      Cv16S = 3,
      /// <summary>
      /// Int32
      /// </summary>
      Cv32S = 4,
      /// <summary>
      /// float
      /// </summary>
      Cv32F = 5,
      /// <summary>
      /// double
      /// </summary>
      Cv64F = 6
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
   public enum SeqEltype
   {
      ///<summary>
      ///  (x,y) 
      ///</summary>
      Point = (((int)DepthType.Cv32S) + (((2) - 1) << 3)),
      ///<summary>  
      ///freeman code: 0..7 
      ///</summary>
      Code = DepthType.Cv8U + 0 << 3,
      ///<summary>  
      ///unspecified type of sequence elements 
      ///</summary>
      Generic = 0,
      ///<summary>  
      ///=6 
      ///</summary>
      Ptr = 7,
      ///<summary>  
      ///pointer to element of other sequence 
      ///</summary>
      Ppoint = 7,
      ///<summary>  
      ///index of element of some other sequence 
      ///</summary>
      Index = DepthType.Cv32S,
      ///<summary>  
      ///next_o, next_d, vtx_o, vtx_d 
      ///</summary>
      GraphEdge = 0,
      ///<summary>  
      ///first_edge, (x,y) 
      ///</summary>
      GraphVertex = 0,
      ///<summary>  
      ///vertex of the binary tree   
      ///</summary>
      TrainAtr = 0,
      ///<summary>  
      ///connected component  
      ///</summary>
      ConnectedComp = 0,
      ///<summary>  
      ///(x,y,z)  
      ///</summary>
      Point3D = DepthType.Cv32F + 2 << 3

   }

   /// <summary>
   /// The kind of sequence available
   /// </summary>
   public enum SeqKind
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
      CV_SEQ_POINT_SET = (SeqKind.CV_SEQ_KIND_GENERIC | SeqEltype.Point),
      /// <summary>
      /// 
      /// </summary>
      CV_SEQ_POINT3D_SET = (SeqKind.CV_SEQ_KIND_GENERIC | SeqEltype.Point3D),
      /// <summary>
      /// 
      /// </summary>
      CV_SEQ_POLYLINE = (SeqKind.CV_SEQ_KIND_CURVE | SeqEltype.Point),
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
   public enum TermCritType
   {
      /// <summary>
      /// Iteration
      /// </summary>
      Iter = 1,
      /// <summary>
      /// Epsilon
      /// </summary>
      Eps = 2
   }

   /// <summary>
   /// Types of thresholding 
   /// </summary>
   public enum ThresholdType
   {
      ///<summary>
      ///value = value > threshold ? max_value : 0
      ///</summary>
      Binary = 0,
      ///<summary>
      /// value = value > threshold ? 0 : max_value       
      ///</summary>
      BinaryInv = 1,
      ///<summary>
      /// value = value > threshold ? threshold : value   
      ///</summary>
      Trunc = 2,
      ///<summary>
      /// value = value > threshold ? value : 0           
      ///</summary>
      ToZero = 3,
      ///<summary>
      /// value = value > threshold ? 0 : value           
      ///</summary>
      ToZeroInv = 4,
      /// <summary>
      /// 
      /// </summary>
      Mask = 7,
      ///<summary>
      /// use Otsu algorithm to choose the optimal threshold value;
      /// combine the flag with one of the above CV_THRESH_* values 
      ///</summary>
      Otsu = 8

   }

   /// <summary>
   /// Methods for comparing two array
   /// </summary>
   public enum TemplateMatchingType
   {
      /// <summary>
      /// R(x,y)=sumx',y'[T(x',y')-I(x+x',y+y')]2
      /// </summary>
      Sqdiff = 0,
      /// <summary>
      /// R(x,y)=sumx',y'[T(x',y')-I(x+x',y+y')]2/sqrt[sumx',y'T(x',y')2 sumx',y'I(x+x',y+y')2]
      /// </summary>
      SqdiffNormed = 1,
      /// <summary>
      /// R(x,y)=sumx',y'[T(x',y') I(x+x',y+y')]
      /// </summary>
      Ccorr = 2,
      /// <summary>
      /// R(x,y)=sumx',y'[T(x',y') I(x+x',y+y')]/sqrt[sumx',y'T(x',y')2 sumx',y'I(x+x',y+y')2]
      /// </summary>
      CcorrNormed = 3,
      /// <summary>
      /// R(x,y)=sumx',y'[T'(x',y') I'(x+x',y+y')],
      /// where T'(x',y')=T(x',y') - 1/(wxh) sumx",y"T(x",y")
      ///    I'(x+x',y+y')=I(x+x',y+y') - 1/(wxh) sumx",y"I(x+x",y+y")
      /// </summary>
      Ccoeff = 4,
      /// <summary>
      /// R(x,y)=sumx',y'[T'(x',y') I'(x+x',y+y')]/sqrt[sumx',y'T'(x',y')2 sumx',y'I'(x+x',y+y')2]
      /// </summary>
      CcoeffNormed = 5
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
   public enum FlipType
   {
      /// <summary>
      /// No flipping
      /// </summary>
      None = 0,
      /// <summary>
      /// Flip horizontally
      /// </summary>
      Horizontal = 1,
      /// <summary>
      /// Flip vertically
      /// </summary>
      Vertical = 2
   }

   /// <summary>
   /// Enumeration used by cvCheckArr
   /// </summary>
   [Flags]
   public enum CheckType
   {
      /// <summary>
      /// Checks that every element is neigther NaN nor Infinity
      /// </summary>
      NanInfinity = 0,
      /// <summary>
      /// If set, the function checks that every value of array is within [minVal,maxVal) range, otherwise it just checks that every element is neigther NaN nor Infinity
      /// </summary>
      Range = 1,
      /// <summary>
      /// If set, the function does not raises an error if an element is invalid or out of range
      /// </summary>
      Quite = 2
   }

   /// <summary>
   /// Type of floodfill operation
   /// </summary>
   [Flags]
   public enum FloodFillType
   {
      /// <summary>
      /// The default type
      /// </summary>
      Default = 0,
      /// <summary>
      /// If set the difference between the current pixel and seed pixel is considered,
      /// otherwise difference between neighbor pixels is considered (the range is floating).
      /// </summary>
      FixedRange = (1 << 16),
      /// <summary>
      /// If set, the function does not fill the image (new_val is ignored),
      /// but the fills mask (that must be non-NULL in this case).
      /// </summary>
      MaskOnly = (1 << 17)
   }

   /// <summary>
   /// The type for cvSampleLine
   /// </summary>
   public enum Connectivity
   {
      /// <summary>
      /// 8-connected
      /// </summary>
      EightConnected = 8,
      /// <summary>
      /// 4-connected
      /// </summary>
      FourConnected = 4
   }

   /// <summary>
   /// The type of line for drawing
   /// </summary>
   public enum LineType
   {
      /// <summary>
      /// 8-connected
      /// </summary>
      EightConnected = 8,
      /// <summary>
      /// 4-connected
      /// </summary>
      FourConnected = 4,
      /// <summary>
      /// Antialias
      /// </summary>
      AntiAlias = 16
   }

   /// <summary>
   /// Defines for Distance Transform
   /// </summary>
   public enum DistType
   {
      ///<summary>
      ///  User defined distance 
      ///</summary>
      User = -1,

      ///<summary>
      ///  distance = |x1-x2| + |y1-y2| 
      ///</summary>
      L1 = 1,
      ///<summary>
      ///  Simple euclidean distance 
      ///</summary>
      L2 = 2,
      ///<summary>
      ///  distance = max(|x1-x2|,|y1-y2|) 
      ///</summary>
      C = 3,
      ///<summary>
      ///  L1-L2 metric: distance = 2(sqrt(1+x*x/2) - 1)) 
      ///</summary>
      L12 = 4,
      ///<summary>
      ///  distance = c^2(|x|/c-log(1+|x|/c)), c = 1.3998 
      ///</summary>
      Fair = 5,
      ///<summary>
      ///  distance = c^2/2(1-exp(-(x/c)^2)), c = 2.9846 
      ///</summary>
      Welsch = 6,
      ///<summary>
      ///  distance = |x|&lt;c ? x^2/2 : c(|x|-c/2), c=1.345 
      ///</summary>
      Huber = 7,
   }

   /// <summary>
   /// The types for cvMulSpectrums
   /// </summary>
   [Flags]
   public enum MulSpectrumsType
   {
      /// <summary>
      /// The default type
      /// </summary>
      Default = 0,
      /// <summary>
      /// Do forward or inverse transform of every individual row of the input matrix. This flag allows user to transform multiple vectors simultaneously and can be used to decrease the overhead (which is sometimes several times larger than the processing itself), to do 3D and higher-dimensional transforms etc
      /// </summary>
      DxtRows = 4,
      /// <summary>
      /// Conjugate the second argument of cvMulSpectrums
      /// </summary>
      DxtMulConj = 8
   }

   /// <summary>
   /// Flag used for cvDFT
   /// </summary>
   [Flags]
   public enum DxtType
   {
      /// <summary>
      /// Do forward 1D or 2D transform. The result is not scaled
      /// </summary>
      Forward = 0,
      /// <summary>
      /// Do inverse 1D or 2D transform. The result is not scaled. CV_DXT_FORWARD and CV_DXT_INVERSE are mutually exclusive, of course
      /// </summary>
      Inverse = 1,
      /// <summary>
      /// Scale the result: divide it by the number of array elements. Usually, it is combined with CV_DXT_INVERSE, and one may use a shortcut 
      /// </summary>
      Scale = 2,
      /// <summary>
      /// Do forward or inverse transform of every individual row of the input matrix. This flag allows user to transform multiple vectors simultaneously and can be used to decrease the overhead (which is sometimes several times larger than the processing itself), to do 3D and higher-dimensional transforms etc
      /// </summary>
      Rows = 4,
      /// <summary>
      /// Inverse and scale
      /// </summary>
      InvScale = (Scale | Inverse)
   }

   /// <summary>
   /// Flag used for cvDCT
   /// </summary>
   public enum DctType
   {
      /// <summary>
      /// Do forward 1D or 2D transform. The result is not scaled
      /// </summary>
      Forward = 0,
      /// <summary>
      /// Do inverse 1D or 2D transform. The result is not scaled. CV_DXT_FORWARD and CV_DXT_INVERSE are mutually exclusive, of course
      /// </summary>
      Inverse = 1,
      /// <summary>
      /// Do forward or inverse transform of every individual row of the input matrix. This flag allows user to transform multiple vectors simultaneously and can be used to decrease the overhead (which is sometimes several times larger than the processing itself), to do 3D and higher-dimensional transforms etc
      /// </summary>
      Rows = 4
   }

   /// <summary>
   /// Calculates fundamental matrix given a set of corresponding points
   /// </summary>
   [Flags]
   public enum FmType
   {
      /// <summary>
      /// for 7-point algorithm. N == 7
      /// </summary>
      SevenPoint = 1,
      /// <summary>
      /// for 8-point algorithm. N >= 8
      /// </summary>
      EightPoint = 2,
      /// <summary>
      /// for LMedS algorithm. N >= 8
      /// </summary>
      LMedsOnly = 4,
      /// <summary>
      /// for RANSAC algorithm. N >= 8
      /// </summary>
      RansacOnly = 8,
      /// <summary>
      /// CV_FM_LMEDS_ONLY | CV_FM_8POINT
      /// </summary>
      LMeds = (LMedsOnly | EightPoint),
      /// <summary>
      /// CV_FM_RANSAC_ONLY | CV_FM_8POINT
      /// </summary>
      Ransac = (RansacOnly | EightPoint)
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
   public enum ErrorCodes
   {
      /// <summary>
      /// 
      /// </summary>
      STSOK = 0,
      /// <summary>
      /// 
      /// </summary>
      STSBACKTRACE = -1,
      /// <summary>
      /// 
      /// </summary>
      STSERROR = -2,
      /// <summary>
      /// 
      /// </summary>
      STSINTERNAL = -3,
      /// <summary>
      /// 
      /// </summary>
      STSNOMEM = -4,
      /// <summary>
      /// 
      /// </summary>
      STSBADARG = -5,
      /// <summary>
      /// 
      /// </summary>
      STSBADFUNC = -6,
      /// <summary>
      /// 
      /// </summary>
      STSNOCONV = -7,
      /// <summary>
      /// 
      /// </summary>
      STSAUTOTRACE = -8,
      /// <summary>
      /// 
      /// </summary>
      HEADERISNULL = -9,
      /// <summary>
      /// 
      /// </summary>
      BADIMAGESIZE = -10,
      /// <summary>
      /// 
      /// </summary>
      BADOFFSET = -11,
      /// <summary>
      /// 
      /// </summary>
      BADDATAPTR = -12,
      /// <summary>
      /// 
      /// </summary>
      BADSTEP = -13,
      /// <summary>
      /// 
      /// </summary>
      BADMODELORCHSEQ = -14,
      /// <summary>
      /// 
      /// </summary>
      BADNUMCHANNELS = -15,
      /// <summary>
      /// 
      /// </summary>
      BADNUMCHANNEL1U = -16,
      /// <summary>
      /// 
      /// </summary>
      BADDEPTH = -17,
      /// <summary>
      /// 
      /// </summary>
      BADALPHACHANNEL = -18,
      /// <summary>
      /// 
      /// </summary>
      BADORDER = -19,
      /// <summary>
      /// 
      /// </summary>
      BADORIGIN = -20,
      /// <summary>
      /// 
      /// </summary>
      BADALIGN = -21,
      /// <summary>
      /// 
      /// </summary>
      BADCALLBACK = -22,
      /// <summary>
      /// 
      /// </summary>
      BADTILESIZE = -23,
      /// <summary>
      /// 
      /// </summary>
      BADCOI = -24,
      /// <summary>
      /// 
      /// </summary>
      BADROISIZE = -25,
      /// <summary>
      /// 
      /// </summary>
      MASKISTILED = -26,
      /// <summary>
      /// 
      /// </summary>
      STSNULLPTR = -27,
      /// <summary>
      /// 
      /// </summary>
      STSVECLENGTHERR = -28,
      /// <summary>
      /// 
      /// </summary>
      STSFILTERSTRUCTCONTENTERR = -29,
      /// <summary>
      /// 
      /// </summary>
      STSKERNELSTRUCTCONTENTERR = -30,
      /// <summary>
      /// 
      /// </summary>
      STSFILTEROFFSETERR = -31,
      /// <summary>
      /// 
      /// </summary>
      STSBADSIZE = -201,
      /// <summary>
      /// 
      /// </summary>
      STSDIVBYZERO = -202,
      /// <summary>
      /// 
      /// </summary>
      STSINPLACENOTSUPPORTED = -203,
      /// <summary>
      /// 
      /// </summary>
      STSOBJECTNOTFOUND = -204,
      /// <summary>
      /// 
      /// </summary>
      STSUNMATCHEDFORMATS = -205,
      /// <summary>
      /// 
      /// </summary>
      STSBADFLAG = -206,
      /// <summary>
      /// 
      /// </summary>
      STSBADPOINT = -207,
      /// <summary>
      /// 
      /// </summary>
      STSBADMASK = -208,
      /// <summary>
      /// 
      /// </summary>
      STSUNMATCHEDSIZES = -209,
      /// <summary>
      /// 
      /// </summary>
      STSUNSUPPORTEDFORMAT = -210,
      /// <summary>
      /// 
      /// </summary>
      STSOUTOFRANGE = -211,
      /// <summary>
      /// 
      /// </summary>
      STSPARSEERROR = -212,
      /// <summary>
      /// 
      /// </summary>
      STSNOTIMPLEMENTED = -213,
      /// <summary>
      /// 
      /// </summary>
      STSBADMEMBLOCK = -214
   }

   /// <summary>
   /// Types for WarpAffine
   /// </summary>
   public enum Warp
   {
      /// <summary>
      /// Neither FILL_OUTLIERS nor CV_WRAP_INVERSE_MAP
      /// </summary>
      Default = 0,
      /// <summary>
      /// Fill all the destination image pixels. If some of them correspond to outliers in the source image, they are set to fillval.
      /// </summary>
      FillOutliers = 8,
      /// <summary>
      /// Indicates that matrix is inverse transform from destination image to source and, thus, can be used directly for pixel interpolation. Otherwise, the function finds the inverse transform from map_matrix.
      /// </summary>
      InverseMap = 16
   }

   /// <summary>
   /// Types of Adaptive Threshold
   /// </summary>
   public enum AdaptiveThresholdType
   {
      /// <summary>
      /// indicates that "Mean minus C" should be used for adaptive threshold.
      /// </summary>
      MeanC = 0,
      /// <summary>
      /// indicates that "Gaussian minus C" should be used for adaptive threshold.
      /// </summary>
      GaussianC = 1
   }

   
   /// <summary>
   /// Shape of the Structuring Element
   /// </summary>
   public enum ElementShape
   {
      /// <summary>
      /// A rectangular element.
      /// </summary>
      Rectangle = 0,
      /// <summary>
      /// A cross-shaped element.
      /// </summary>
      Cross = 1,
      /// <summary>
      /// An elliptic element.
      /// </summary>
      Ellipse = 2,
      /// <summary>
      /// A user-defined element.
      /// </summary>
      Custom = 100
   }

   /// <summary>
   /// PCA Type
   /// </summary>
   [Flags]
   public enum PcaType
   {
      /// <summary>
      /// the vectors are stored as rows (i.e. all the components of a certain vector are stored continously)
      /// </summary>
      DataAsRow = 0,
      /// <summary>
      ///  the vectors are stored as columns (i.e. values of a certain vector component are stored continuously)
      /// </summary>
      DataAsCol = 1,
      /// <summary>
      /// use pre-computed average vector
      /// </summary>
      UseAvg = 2
   }

   /*
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
   }*/

   
   /// <summary>
   /// cvInvert method
   /// </summary>
   public enum DecompMethod
   {
      /// <summary>
      /// Gaussian elimination with optimal pivot element chose
      /// In case of LU method the function returns src1 determinant (src1 must be square). If it is 0, the matrix is not inverted and src2 is filled with zeros.
      /// </summary>
      LU = 0,
      /// <summary>
      /// Singular value decomposition (SVD) method
      /// In case of SVD methods the function returns the inversed condition number of src1 (ratio of the smallest singular value to the largest singular value) and 0 if src1 is all zeros. The SVD methods calculate a pseudo-inverse matrix if src1 is singular
      /// </summary>
      Svd = 1,
      /// <summary>
      /// Eig
      /// </summary>
      Eig = 2,
      /// <summary>
      /// method for a symmetric positively-defined matrix
      /// </summary>
      Cholesky = 3,
      /// <summary>
      /// QR decomposition
      /// </summary>
      QR = 4,
      /// <summary>
      /// Normal
      /// </summary>
      Normal = 16
   }

   /// <summary>
   /// cvCalcCovarMatrix method types
   /// </summary>
   [Flags]
   public enum CovarMethod
   {
      /// <summary>
      /// Calculates covariation matrix for a set of vectors 
      /// transpose([v1-avg, v2-avg,...]) * [v1-avg,v2-avg,...] 
      /// </summary>
      Scrambled = 0,

      /// <summary>
      /// [v1-avg, v2-avg,...] * transpose([v1-avg,v2-avg,...])
      /// </summary>
      Normal = 1,

      /// <summary>
      /// Do not calc average (i.e. mean vector) - use the input vector instead
      /// (useful for calculating covariance matrix by parts)
      /// </summary>
      UseAvg = 2,

      /// <summary>
      /// Scale the covariance matrix coefficients by number of the vectors
      /// </summary>
      Scale = 4,

      /// <summary>
      /// All the input vectors are stored in a single matrix, as its rows 
      /// </summary>
      Rows = 8,

      /// <summary>
      /// All the input vectors are stored in a single matrix, as its columns
      /// </summary>
      Cols = 16,

   }

   /// <summary>
   /// Type for cvSVD
   /// </summary>
   [Flags]
   public enum SvdType
   {
      /// <summary>
      /// The default type
      /// </summary>
      Default = 0,
      /// <summary>
      /// enables modification of matrix src1 during the operation. It speeds up the processing. 
      /// </summary>
      ModifyA = 1,
      /// <summary>
      /// means that the tranposed matrix U is returned. Specifying the flag speeds up the processing. 
      /// </summary>
      Ut = 2,
      /// <summary>
      /// means that the tranposed matrix V is returned. Specifying the flag speeds up the processing. 
      /// </summary>
      Vt = 4
   }

   /// <summary>
   /// Type for cvCalcOpticalFlowPyrLK
   /// </summary>
   public enum LKFlowFlag
   {
      /// <summary>
      /// The default type
      /// </summary>
      Default = 0,
      /// <summary>
      /// Uses initial estimations, stored in nextPts; if the flag is not set, then prevPts is copied to nextPts and is considered the initial estimate.
      /// </summary>
      UserInitialFlow = 4,
      /// <summary>
      /// use minimum eigen values as an error measure (see minEigThreshold description); if the flag is not set, then L1 distance between patches around the original and a moved point, divided by number of pixels in a window, is used as a error measure.
      /// </summary>
      LKGetMinEigenvals = 8,
   }

   /// <summary>
   /// Various camera calibration flags
   /// </summary>
   [Flags]
   public enum CalibType
   {
      /// <summary>
      /// The default value
      /// </summary>
      Default = 0,
      /// <summary>
      /// intrinsic_matrix contains valid initial values of fx, fy, cx, cy that are optimized further. Otherwise, (cx, cy) is initially set to the image center (image_size is used here), and focal distances are computed in some least-squares fashion
      /// </summary>
      UserIntrinsicGuess = 1,
      /// <summary>
      /// The optimization procedure consider only one of fx and fy as independent variable and keeps the aspect ratio fx/fy the same as it was set initially in intrinsic_matrix. In this case the actual initial values of (fx, fy) are either taken from the matrix (when CV_CALIB_USE_INTRINSIC_GUESS is set) or estimated somehow (in the latter case fx, fy may be set to arbitrary values, only their ratio is used)
      /// </summary>
      FixAspectRatio = 2,
      /// <summary>
      /// The principal point is not changed during the global optimization, it stays at the center and at the other location specified (when CV_CALIB_FIX_FOCAL_LENGTH - Both fx and fy are fixed.
      /// CV_CALIB_USE_INTRINSIC_GUESS is set as well)
      /// </summary>
      FixPrincipalPoint = 4,
      /// <summary>
      /// Tangential distortion coefficients are set to zeros and do not change during the optimization
      /// </summary>
      ZeroTangentDist = 8,
      /// <summary>
      /// The focal length is fixed
      /// </summary>
      FixFocalLength = 16,
      /// <summary>
      /// The 1st distortion coefficient (k1) is fixed to 0 or to the initial passed value if CV_CALIB_USE_INTRINSIC_GUESS is passed
      /// </summary>
      FixK1 = 32,
      /// <summary>
      /// The 2nd distortion coefficient (k2) is fixed to 0 or to the initial passed value if CV_CALIB_USE_INTRINSIC_GUESS is passed
      /// </summary>
      FixK2 = 64,
      /// <summary>
      /// The 3rd distortion coefficient (k3) is fixed to 0 or to the initial passed value if CV_CALIB_USE_INTRINSIC_GUESS is passed
      /// </summary>
      FixK3 = 128,
      /// <summary>
      /// The 4th distortion coefficient (k4) is fixed (see above)
      /// </summary>
      FixK4 = 2048,
      /// <summary>
      /// The 5th distortion coefficient (k5) is fixed to 0 or to the initial passed value if CV_CALIB_USE_INTRINSIC_GUESS is passed
      /// </summary>
      FixK5 = 4096,
      /// <summary>
      /// The 6th distortion coefficient (k6) is fixed to 0 or to the initial passed value if CV_CALIB_USE_INTRINSIC_GUESS is passed
      /// </summary>
      FixK6 = 8192,
      /// <summary>
      /// Rational model
      /// </summary>
      RationalModel = 16384
   }

   /// <summary>
   /// Type of chessboard calibration
   /// </summary>
   [Flags]
   public enum CalibCbType
   {
      /// <summary>
      /// Default type
      /// </summary>
      Default = 0,
      /// <summary>
      /// Use adaptive thresholding to convert the image to black-n-white, rather than a fixed threshold level (computed from the average image brightness)
      /// </summary>
      AdaptiveThresh = 1,
      /// <summary>
      /// Normalize the image using cvNormalizeHist before applying fixed or adaptive thresholding.
      /// </summary>
      NormalizeImage = 2,
      /// <summary>
      /// Use additional criteria (like contour area, perimeter, square-like shape) to filter out false quads that are extracted at the contour retrieval stage
      /// </summary>
      FilterQuads = 4,
      /// <summary>
      /// If it is on, then this check is performed before the main algorithm and if a chessboard is not found, the function returns 0 instead of wasting 0.3-1s on doing the full search.
      /// </summary>
      FastCheck = 8,
   }

   /// <summary>
   /// Type of circles grid calibration
   /// </summary>
   [Flags]
   public enum CalibCgType
   {
      /// <summary>
      /// symmetric grid
      /// </summary>
      SymmetricGrid = 1,
      /// <summary>
      /// asymmetric grid
      /// </summary>
      AsymmetricGrid = 2,
      /// <summary>
      /// Clustering
      /// </summary>
      Clustering = 4,

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
   public enum Orientation
   {
      /// <summary>
      /// clockwise
      /// </summary>
      Clockwise = 1,
      /// <summary>
      /// counter clockwise
      /// </summary>
      CounterClockwise = 2
   }

   /*
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
   }*/

   /// <summary>
   /// Stereo Block Matching Prefilter type
   /// </summary>
   public enum StereoBmPrefilter
   {
      /// <summary>
      /// No prefilter
      /// </summary>
      NormalizedResponse = 0,
      /// <summary>
      /// XSobel
      /// </summary>
      XSobel = 1
   }

   /// <summary>
   /// Type of cvHomography method
   /// </summary>
   public enum HomographyMethod
   {
      /// <summary>
      /// regular method using all the point pairs
      /// </summary>
      Default = 0,
      /// <summary>
      /// Least-Median robust method
      /// </summary>
      LMEDS = 4,
      /// <summary>
      /// RANSAC-based robust method
      /// </summary>
      Ransac = 8
   }

   /// <summary>
   /// Type used by cvMatchShapes
   /// </summary>
   public enum ContoursMatchType
   {
      /// <summary>
      /// I_1(A,B)=sum_{i=1..7} abs(1/m^A_i - 1/m^B_i) where m^A_i=sign(h^A_i) log(h^A_i), m^B_i=sign(h^B_i) log(h^B_i), h^A_i, h^B_i - Hu moments of A and B, respectively
      /// </summary> 
      I1 = 1,
      /// <summary>
      /// I_2(A,B)=sum_{i=1..7} abs(m^A_i - m^B_i) where m^A_i=sign(h^A_i) log(h^A_i), m^B_i=sign(h^B_i) log(h^B_i), h^A_i, h^B_i - Hu moments of A and B, respectively
      /// </summary>
      I2 = 2,
      /// <summary>
      /// I_3(A,B)=sum_{i=1..7} abs(m^A_i - m^B_i)/abs(m^A_i) where m^A_i=sign(h^A_i) log(h^A_i), m^B_i=sign(h^B_i) log(h^B_i), h^A_i, h^B_i - Hu moments of A and B, respectively
      /// </summary>
      I3 = 3
   }

   /// <summary>
   /// The result type of cvSubdiv2DLocate.
   /// </summary>
   public enum Subdiv2DPointLocationType
   {
      /// <summary>
      /// One of input arguments is invalid.
      /// </summary>
      Error = -2,
      /// <summary>
      /// Point is outside the subdivision reference rectangle
      /// </summary>
      OutsideRect = -1,
      /// <summary>
      /// Point falls into some facet
      /// </summary>
      Inside = 0,
      /// <summary>
      /// Point coincides with one of subdivision vertices
      /// </summary>
      Vertex = 1,
      /// <summary>
      /// Point falls onto the edge
      /// </summary>
      OnEdge = 2
   }

   /// <summary>
   /// Type used in cvStereoRectify
   /// </summary>
   public enum StereoRectifyType
   {
      /// <summary>
      /// Shift one of the image in horizontal or vertical direction (depending on the orientation of epipolar lines) in order to maximise the useful image area
      /// </summary>
      Default = 0,
      /// <summary>
      /// Makes the principal points of each camera have the same pixel coordinates in the rectified views
      /// </summary>
      CalibZeroDisparity = 1024
   }

   /// <summary>
   /// The type for CopyMakeBorder function
   /// </summary>
   public enum BorderType
   {
      /// <summary>
      /// Border is filled with the fixed value, passed as last parameter of the function
      /// </summary>
      Constant = 0,
      /// <summary>
      /// The pixels from the top and bottom rows, the left-most and right-most columns are replicated to fill the border
      /// </summary>
      Replicate = 1,
      /// <summary>
      /// Reflect
      /// </summary>
      Reflect = 2,
      /// <summary>
      /// Wrap
      /// </summary>
      Wrap = 3,
      /// <summary>
      /// Reflect 101
      /// </summary>
      Reflect101 = 4,
      /// <summary>
      /// Transparent
      /// </summary>
      Transparent = 5,

      /// <summary>
      /// The default border interpolation type.
      /// </summary>
      Default = Reflect101,
      /// <summary>
      /// do not look outside of ROI
      /// </summary>
      Isolated = 16
   }

   /// <summary>
   /// The types for haar detection
   /// </summary>
   [Flags]
   public enum HaarDetectionType
   {
      /// <summary>
      /// The default type where no optimization is done.
      /// </summary>
      Default = 0,
      /// <summary>
      /// If it is set, the function uses Canny edge detector to reject some image regions that contain too few or too much edges and thus can not contain the searched object. The particular threshold values are tuned for face detection and in this case the pruning speeds up the processing
      /// </summary>
      DoCannyPruning = 1,
      /// <summary>
      /// For each scale factor used the function will downscale the image rather than "zoom" the feature coordinates in the classifier cascade. Currently, the option can only be used alone, i.e. the flag can not be set together with the others
      /// </summary>
      ScaleImage = 2,
      /// <summary>
      /// If it is set, the function finds the largest object (if any) in the image. That is, the output sequence will contain one (or zero) element(s)
      /// </summary>
      FindBiggestObject = 4,
      /// <summary>
      /// It should be used only when CV_HAAR_FIND_BIGGEST_OBJECT is set and min_neighbors &gt; 0. If the flag is set, the function does not look for candidates of a smaller size as soon as it has found the object (with enough neighbor candidates) at the current scale. Typically, when min_neighbors is fixed, the mode yields less accurate (a bit larger) object rectangle than the regular single-object mode (flags=CV_HAAR_FIND_BIGGEST_OBJECT), but it is much faster, up to an order of magnitude. A greater value of min_neighbors may be specified to improve the accuracy
      /// </summary>
      DoRoughSearch = 8
   }

   /// <summary>
   /// Specific if it is back or front
   /// </summary>
   public enum BackOrFront
   {
      /// <summary>
      /// Back
      /// </summary>
      Back,
      /// <summary>
      /// Front
      /// </summary>
      Front
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
   public enum StorageOp
   {
      /// <summary>
      /// The storage is open for reading
      /// </summary>
      Read = 0,
      /// <summary>
      /// The storage is open for writing
      /// </summary>
      Write = 1,
      /// <summary>
      /// The storage is open for append
      /// </summary>
      Append = 2
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
   public enum BlobtrackerMsProfile
   {
      /// <summary>
      /// EPANECHNIKOV
      /// </summary>
      Epanechnikov = 0,
      /// <summary>
      /// DoG
      /// </summary>
      Dog = 1
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
   public enum BlobPostProcessType
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
   public enum HistogramCompMethod
   {
      /// <summary>
      /// Correlation/ 
      /// </summary>
      Correl = 0,
      /// <summary>
      /// Chi-Square
      /// </summary>
      Chisqr = 1,
      /// <summary>
      /// Intersection
      /// </summary>
      Intersect = 2,
      /// <summary>
      /// Bhattacharyya distance
      /// </summary>
      Bhattacharyya = 3,
      /// <summary>
      ///  Synonym for Bhattacharyya
      /// </summary>
      Hellinger = Bhattacharyya,
      /// <summary>
      /// Alternative Chi-Square
      /// </summary>
      ChisqrAlt = 4
   }

   /// <summary>
   /// The type of BGStatModel
   /// </summary>
   public enum BgStatType
   {
      /// <summary>
      /// 
      /// </summary>
      FgdStatModel,
      /// <summary>
      /// Gaussian background model
      /// </summary>
      GaussianBgModel
   }

   /// <summary>
   /// Type of foreground detector
   /// </summary>
   public enum ForgroundDetectorType
   {
      /// <summary>
      /// Latest and greatest algorithm
      /// </summary>
      Fgd = 0,
      /// <summary>
      /// "Mixture of Gaussians", older algorithm
      /// </summary>
      Mog = 1,
      /// <summary>
      ///  A simplified version of FGD
      /// </summary>
      FgdSimple = 2
   }

   /// <summary>
   /// The available flags for farneback optical flow computation
   /// </summary>
   [Flags]
   public enum OpticalflowFarnebackFlag
   {
      /// <summary>
      /// Default
      /// </summary>
      Default = 0,
      /// <summary>
      /// Use the input flow as the initial flow approximation
      /// </summary>
      UseInitialFlow = 4,
      /// <summary>
      /// Use a Gaussian winsize x winsizefilter instead of box
      /// filter of the same size for optical flow estimation. Usually, this option gives more accurate
      /// flow than with a box filter, at the cost of lower speed (and normally winsize for a
      /// Gaussian window should be set to a larger value to achieve the same level of robustness)
      /// </summary>
      FarnebackGaussian = 256
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
   public enum GrabcutInitType
   {
      /// <summary>
      /// Initialize with rectangle
      /// </summary>
      InitWithRect = 0,
      /// <summary>
      /// Initialize with mask
      /// </summary>
      InitWithMask = 1,
      /// <summary>
      /// Eval
      /// </summary>
      Eval = 2
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
      TYZX_LEFT = 400,
      /// <summary>
      /// TYZX proprietary drivers
      /// </summary>
      TYZX_RIGHT = 401,
      /// <summary>
      /// TYZX proprietary drivers
      /// </summary>
      TYZX_COLOR = 402,
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
      /// Microsoft Media Foundation (via videoInput)
      /// </summary>
      MSMF = 1400,

      /// <summary>
      /// PvAPI, Prosilica GigE SDK
      /// </summary>
      PVAPI = 800,

      /// <summary>
      /// OpenNI (for Kinect)
      /// </summary>
      OPENNI = 900,

      /// <summary>
      /// OpenNI (for Asus Xtion)
      /// </summary>
      OPENNI_ASUS = 910,

      /// <summary>
      /// Android
      /// </summary>
      ANDROID = 1000,
      /// <summary>
      /// Android back camera
      /// </summary>
      ANDROID_BACK = ANDROID + 99,
      /// <summary>
      /// // Android front camera
      /// </summary>
      ANDROID_FRONT = ANDROID + 98,
      /// <summary>
      /// XIMEA Camera API
      /// </summary>
      XIAPI = 1100,

      /// <summary>
      /// AVFoundation framework for iOS (OS X Lion will have the same API)
      /// </summary>
      AVFOUNDATION = 1200,

      /// <summary>
      ///  Smartek Giganetix GigEVisionSDK
      /// </summary>
      GIGANETIX = 1300,
   }

   /// <summary>
   /// KMeans initialization type
   /// </summary>
   public enum KMeansInitType
   {
      /// <summary>
      /// Chooses random centers for k-Means initialization
      /// </summary>
      RandomCenters = 0,
      /// <summary>
      /// Uses the user-provided labels for K-Means initialization
      /// </summary>
      UseInitialLabels = 1,
      /// <summary>
      /// Uses k-Means++ algorithm for initialization
      /// </summary>
      PPCenters = 2
   }

   /// <summary>
   /// The type of color map
   /// </summary>
   public enum ColorMapType
   {
      /// <summary>
      /// Autumn
      /// </summary>
      Autumn = 0,
      /// <summary>
      /// Bone
      /// </summary>
      Bone = 1,
      /// <summary>
      /// Jet
      /// </summary>
      Jet = 2,
      /// <summary>
      /// Winter
      /// </summary>
      Winter = 3,
      /// <summary>
      /// Rainbow
      /// </summary>
      Rainbow = 4,
      /// <summary>
      /// Ocean
      /// </summary>
      Ocean = 5,
      /// <summary>
      /// Summer
      /// </summary>
      Summer = 6,
      /// <summary>
      /// Spring
      /// </summary>
      Spring = 7,
      /// <summary>
      /// Cool
      /// </summary>
      Cool = 8,
      /// <summary>
      /// Hsv
      /// </summary>
      Hsv = 9,
      /// <summary>
      /// Pink
      /// </summary>
      Pink = 10,
      /// <summary>
      /// Hot
      /// </summary>
      Hot = 11,
      /*
      /// <summary>
      /// Mkpj1
      /// </summary>
      Mkpj1 = 12,
      /// <summary>
      /// Mpkj2
      /// </summary>
      Mkpj2 = 13*/
   }


   /// <summary>
   /// The return value for solveLP function
   /// </summary>
   public enum SolveLPResult
   {
      /// <summary>
      /// Problem is unbounded (target function can achieve arbitrary high values)
      /// </summary>
      Unbounded = -2,
      /// <summary>
      /// Problem is unfeasible (there are no points that satisfy all the constraints imposed)
      /// </summary>
      Unfeasible = -1,
      /// <summary>
      /// There is only one maximum for target function
      /// </summary>
      Single = 0,
      /// <summary>
      /// there are multiple maxima for target function - the arbitrary one is returned
      /// </summary>
      Multi = 1
   }

   /// <summary>
   /// Morphology operation type
   /// </summary>
   public enum MorphOp
   {
      /// <summary>
      /// Erode
      /// </summary>
      Erode = 0,
      /// <summary>
      /// Dilate
      /// </summary>
      Dilate = 1,
      /// <summary>
      /// Open
      /// </summary>
      Open = 2,
      /// <summary>
      /// Close
      /// </summary>
      Close = 3,
      /// <summary>
      /// Gradient
      /// </summary>
      Gradient = 4,
      /// <summary>
      /// Tophat
      /// </summary>
      Tophat = 5,
      /// <summary>
      /// Blackhat
      /// </summary>
      Blackhat = 6
   }

   /// <summary>
   /// Access type
   /// </summary>
   public enum AccessType
   {
      /// <summary>
      /// Read
      /// </summary>
      Read = 1 << 24,
      /// <summary>
      /// Write
      /// </summary>
      Write = 1 << 25,
      /// <summary>
      /// Read and write
      /// </summary>
      ReadWrite = 3 << 24,
      /// <summary>
      /// Mask
      /// </summary>
      Mask = ReadWrite,
      /// <summary>
      /// Dast
      /// </summary>
      Fast = 1 << 26
   }
}
