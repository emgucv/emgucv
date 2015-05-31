//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
   /// CV Capture property identifier
   /// </summary>
   public enum CapProp
   {
      /// <summary>
      /// Turn the feature off (not controlled manually nor automatically)
      /// </summary>
      DC1394Off = -4,
      /// <summary>
      /// Set automatically when a value of the feature is set by the user
      /// </summary>
      DC1394ModeManual = -3,
      /// <summary>
      /// DC1394 mode auto
      /// </summary>
      DC1394ModeAuto = -2,
      /// <summary>
      /// DC1394 mode one push auto
      /// </summary>
      DC1394ModeOnePushAuto = -1,
      /// <summary>
      /// Film current position in milliseconds or video capture timestamp
      /// </summary>
      PosMsec = 0,
      /// <summary>
      /// 0-based index of the frame to be decoded/captured next
      /// </summary>
      PosFrames = 1,
      /// <summary>
      /// Position in relative units (0 - start of the file, 1 - end of the file)
      /// </summary>
      PosAviRatio = 2,
      /// <summary>
      /// Width of frames in the video stream
      /// </summary>
      FrameWidth = 3,
      /// <summary>
      /// Height of frames in the video stream
      /// </summary>
      FrameHeight = 4,
      /// <summary>
      /// Frame rate 
      /// </summary>
      Fps = 5,
      /// <summary>
      /// 4-character code of codec
      /// </summary>
      FourCC = 6,
      /// <summary>
      /// Number of frames in video file
      /// </summary>
      FrameCount = 7,
      /// <summary>
      /// Format
      /// </summary>
      Format = 8,
      /// <summary>
      /// Mode
      /// </summary>
      Mode = 9,
      /// <summary>
      /// Brightness
      /// </summary>
      Brightness = 10,
      /// <summary>
      /// Contrast
      /// </summary>
      Contrast = 11,
      /// <summary>
      /// Saturation
      /// </summary>
      Staturation = 12,
      /// <summary>
      /// Hue
      /// </summary>
      Hue = 13,
      /// <summary>
      /// Gain
      /// </summary>
      Gain = 14,
      /// <summary>
      /// Exposure
      /// </summary>
      Exposure = 15,
      /// <summary>
      /// Convert RGB
      /// </summary>
      ConvertRgb = 16,
      /// <summary>
      /// White balance blue u
      /// </summary>
      WhiteBalanceBlueU = 17,
      /// <summary>
      /// Rectification
      /// </summary>
      Rectification = 18,
      /// <summary>
      /// Monochrome
      /// </summary>
      Monochrome = 19,
      /// <summary>
      /// Sharpness
      /// </summary>
      Sharpness = 20,
      /// <summary>
      /// Exposure control done by camera, user can adjust reference level using this feature
      /// </summary>
      AutoExposure = 21,
      /// <summary>
      /// Gamma
      /// </summary>
      Gamma = 22,
      /// <summary>
      /// Temperature
      /// </summary>
      Temperature = 23,
      /// <summary>
      /// Trigger
      /// </summary>
      Trigger = 24,
      /// <summary>
      /// Trigger delay
      /// </summary>
      TriggerDelay = 25,
      /// <summary>
      /// White balance red v
      /// </summary>
      WhiteBalanceRedV = 26,
      /// <summary>
      /// Zoom
      /// </summary>
      Zoom = 27,
      /// <summary>
      /// Focus
      /// </summary>
      Focus = 28,
      /// <summary>
      /// GUID
      /// </summary>
      Guid = 29,
      /// <summary>
      /// ISO SPEED
      /// </summary>
      IsoSpeed = 30,
      /// <summary>
      /// MAX DC1394
      /// </summary>
      MaxDC1394 = 31,
      /// <summary>
      /// Backlight
      /// </summary>
      Backlight = 32,
      /// <summary>
      /// Pan
      /// </summary>
      Pan = 33,
      /// <summary>
      /// Tilt
      /// </summary>
      Tilt = 34,
      /// <summary>
      /// Roll
      /// </summary>
      Roll = 35,
      /// <summary>
      /// Iris
      /// </summary>
      Iris = 36,
      /// <summary>
      /// Settings
      /// </summary>
      Settings = 37,
      /// <summary>
      /// property for highgui class CvCapture_Android only
      /// </summary>
      Autograb = 1024,
      /// <summary>
      /// readonly, tricky property, returns cpnst char* indeed
      /// </summary>
      SupportedPreviewSizesString = 1025,
      /// <summary>
      /// readonly, tricky property, returns cpnst char* indeed
      /// </summary>
      PreviewFormat = 1026,
      /// <summary>
      /// OpenNI map generators
      /// </summary>
      OpenniDepthGenerator = 1 << 31,
      /// <summary>
      /// OpenNI map generators
      /// </summary>
      OpenniImageGenerator = 1 << 30,
      /// <summary>
      /// OpenNI map generators
      /// </summary>
      OpenniGeneratorsMask = OpenniDepthGenerator + OpenniImageGenerator,

      /// <summary>
      /// Properties of cameras available through OpenNI interfaces
      /// </summary>
      OpenniOutputMode = 100,
      /// <summary>
      /// Properties of cameras available through OpenNI interfaces, in mm.
      /// </summary>
      OpenniFrameMaxDepth = 101,
      /// <summary>
      /// Properties of cameras available through OpenNI interfaces, in mm.
      /// </summary>
      OpenniBaseline = 102,
      /// <summary>
      /// Properties of cameras available through OpenNI interfaces, in pixels.
      /// </summary>
      OpenniFocalLength = 103,
      /// <summary>
      /// Flag that synchronizes the remapping depth map to image map
      /// by changing depth generator's view point (if the flag is "on") or
      /// sets this view point to its normal one (if the flag is "off").
      /// </summary>
      OpenniRegistration = 104,
      /// <summary>
      /// Flag that synchronizes the remapping depth map to image map
      /// by changing depth generator's view point (if the flag is "on") or
      /// sets this view point to its normal one (if the flag is "off").
      /// </summary>
      OpenniRegistrationOn = OpenniRegistration,
      /// <summary>
      /// Approx frame sync
      /// </summary>
      OpenniApproxFrameSync = 105,
      /// <summary>
      /// Max buffer size
      /// </summary>
      OpenniMaxBufferSize = 106,
      /// <summary>
      /// Circle buffer
      /// </summary>
      OpenniCircleBuffer = 107,
      /// <summary>
      /// Max time duration
      /// </summary>
      OpenniMaxTimeDuration = 108,
      /// <summary>
      /// Generator present
      /// </summary>
      OpenniGeneratorPresent = 109,

      /// <summary>
      /// Openni image generator present
      /// </summary>
      OpenniImageGeneratorPresent = OpenniImageGenerator + OpenniGeneratorPresent,
      /// <summary>
      /// Image generator output mode
      /// </summary>
      OpenniImageGeneratorOutputMode = OpenniImageGenerator + OpenniOutputMode,
      /// <summary>
      /// Depth generator baseline, in mm.
      /// </summary>
      OpenniDepthGeneratorBaseline = OpenniDepthGenerator + OpenniBaseline,
      /// <summary>
      /// Depth generator focal length, in pixels.
      /// </summary>
      OpenniDepthGeneratorFocalLength = OpenniDepthGenerator + OpenniFocalLength,
      /// <summary>
      /// Openni generator registration
      /// </summary>
      OpenniDepthGeneratorRegistration = OpenniDepthGenerator + OpenniRegistration,
      /// <summary>
      /// Openni generator registration on
      /// </summary>
      OpenniDepthGeneratorRegistrationOn = OpenniDepthGeneratorRegistration,

      /// <summary>
      /// Properties of cameras available through GStreamer interface. Default is 1
      /// </summary>
      GstreamerQueueLength = 200,
      /// <summary>
      /// Ip for enable multicast master mode. 0 for disable multicast
      /// </summary>
      PvapiMulticastip = 300,

      /// <summary>
      /// Change image resolution by binning or skipping.
      /// </summary>
      XiDownsampling = 400,
      /// <summary>
      /// Output data format
      /// </summary>
      XiDataFormat = 401,
      /// <summary>
      /// Horizontal offset from the origin to the area of interest (in pixels).
      /// </summary>
      XiOffsetX = 402,
      /// <summary>
      /// Vertical offset from the origin to the area of interest (in pixels).
      /// </summary>
      XiOffsetY = 403,
      /// <summary>
      /// Defines source of trigger.
      /// </summary>
      XiTrgSource = 404,
      /// <summary>
      /// Generates an internal trigger. PRM_TRG_SOURCE must be set to TRG_SOFTWARE.
      /// </summary>
      XiTrgSoftware = 405,
      /// <summary>
      /// Selects general purpose input
      /// </summary>
      XiGpiSelector = 406,
      /// <summary>
      /// Set general purpose input mode
      /// </summary>
      XiGpiMode = 407,
      /// <summary>
      /// Get general purpose level
      /// </summary>
      XiGpiLevel = 408,
      /// <summary>
      /// Selects general purpose output
      /// </summary>
      XiGpoSelector = 409,
      /// <summary>
      /// Set general purpose output mode
      /// </summary>
      XiGpoMode = 410,
      /// <summary>
      /// Selects camera signaling LED
      /// </summary>
      XiLedSelector = 411,
      /// <summary>
      /// Define camera signaling LED functionality
      /// </summary>
      XiLedMode = 412,
      /// <summary>
      /// Calculates White Balance(must be called during acquisition)
      /// </summary>
      XiManualWb = 413,
      /// <summary>
      /// Automatic white balance
      /// </summary>
      XiAutoWb = 414,
      /// <summary>
      /// Automatic exposure/gain
      /// </summary>
      XiAeag = 415,
      /// <summary>
      /// Exposure priority (0.5 - exposure 50%, gain 50%).
      /// </summary>
      XiExpPriority = 416,
      /// <summary>
      /// Maximum limit of exposure in AEAG procedure
      /// </summary>
      XiAeMaxLimit = 417,
      /// <summary>
      /// Maximum limit of gain in AEAG procedure
      /// </summary>
      XiAgMaxLimit = 418,
      /// <summary>
      /// Average intensity of output signal AEAG should achieve(in %)
      /// </summary>
      XiAeagLevel = 419,
      /// <summary>
      /// Image capture timeout in milliseconds
      /// </summary>
      XiTimeout = 420,

      /// <summary>
      /// Android flash mode
      /// </summary>
      AndroidFlashMode = 8001,
      /// <summary>
      /// Android focus mode
      /// </summary>
      AndroidFocusMode = 8002,
      /// <summary>
      /// Android white balance
      /// </summary>
      AndroidWhiteBalance = 8003,
      /// <summary>
      /// Android anti banding
      /// </summary>
      AndroidAntibanding = 8004,
      /// <summary>
      /// Android focal length
      /// </summary>
      AndroidFocalLength = 8005,
      /// <summary>
      /// Android focus distance near
      /// </summary>
      AndroidFocusDistanceNear = 8006,
      /// <summary>
      /// Android focus distance optimal
      /// </summary>
      AndroidFocusDistanceOptimal = 8007,
      /// <summary>
      /// Android focus distance far
      /// </summary>
      AndroidFocusDistanceFar = 8008,

      /// <summary>
      /// iOS device focus
      /// </summary>
      IOSDeviceFocus = 9001,
      /// <summary>
      /// iOS device exposure
      /// </summary>
      IOSDeviceExposure = 9002,
      /// <summary>
      /// iOS device flash
      /// </summary>
      IOSDeviceFlash = 9003,
      /// <summary>
      /// iOS device white-balance 
      /// </summary>
      IOSDeviceWhitebalance = 9004,
      /// <summary>
      /// iOS device torch
      /// </summary>
      IOSDeviceTorch = 9005,

      /// <summary>
      /// Smartek Giganetix Ethernet Vision: frame offset X
      /// </summary>
      GigaFrameOffsetX = 10001,

      /// <summary>
      /// Smartek Giganetix Ethernet Vision: frame offset Y
      /// </summary>
      GigaFrameOffsetY = 10002,

      /// <summary>
      /// Smartek Giganetix Ethernet Vision: frame width max
      /// </summary>
      GigaFrameWidthMax = 10003,

      /// <summary>
      /// Smartek Giganetix Ethernet Vision: frame height max
      /// </summary>
      GigaFrameHeighMax = 10004,

      /// <summary>
      /// Smartek Giganetix Ethernet Vision: frame sens width
      /// </summary>
      GigaFrameSensWidth = 10005,

      /// <summary>
      /// Smartek Giganetix Ethernet Vision: frame sens height
      /// </summary>
      GigaFrameSensHeigh = 10006

   }

   /// <summary>
   /// The named window type
   /// </summary>
   public enum NamedWindowType
   {
      /// <summary>
      /// The user can resize the window (no constraint) / also use to switch a fullscreen window to a normal size
      /// </summary>
      Normal = 0x00000000,
      /// <summary>
      /// The user cannot resize the window, the size is constrainted by the image displayed
      /// </summary>
      AutoSize = 0x00000001,
      /// <summary>
      /// Window with opengl support
      /// </summary>
      Opengl = 0x00001000,
      /// <summary>
      /// Change the window to fullscreen
      /// </summary>
      Fullscreen = 1,
      /// <summary>
      /// The image expends as much as it can (no ratio constraint)
      /// </summary>
      FreeRatio = 0x00000100,
      /// <summary>
      /// the ratio of the image is respected
      /// </summary>
      KeepRatio = 0x00000000
   }

   /// <summary>
   /// contour approximation method
   /// </summary>
   public enum ChainApproxMethod : int
   {
      /// <summary>
      /// output contours in the Freeman chain code. All other methods output polygons (sequences of vertices). 
      /// </summary>
      ChainCode = 0,
      /// <summary>
      /// translate all the points from the chain code into points;
      /// </summary>
      ChainApproxNone = 1,
      /// <summary>
      /// compress horizontal, vertical, and diagonal segments, that is, the function leaves only their ending points; 
      /// </summary>
      ChainApproxSimple = 2,
      /// <summary>
      /// 
      /// </summary>
      ChainApproxTc89L1 = 3,
      /// <summary>
      /// apply one of the flavors of Teh-Chin chain approximation algorithm
      /// </summary>
      ChainApproxTc89Kcos = 4,
      /// <summary>
      /// use completely different contour retrieval algorithm via linking of horizontal segments of 1s. Only LIST retrieval mode can be used with this method
      /// </summary>
      LinkRuns = 5
   }

   /// <summary>
   /// Color Conversion code
   /// </summary>
   public enum ColorConversion
   {
      ///<summary>
      ///Convert BGR color to BGRA color
      ///</summary>
      Bgr2Bgra = 0,
      /// <summary>
      /// Convert RGB color to RGBA color
      /// </summary>
      Rgb2Rgba = Bgr2Bgra,

      ///<summary>
      ///Convert BGRA color to BGR color
      ///</summary>
      Bgra2Bgr = 1,
      /// <summary>
      /// Convert RGBA color to RGB color
      /// </summary>
      Rgba2Rgb = Bgra2Bgr,
      /// <summary>
      /// Convert BGR color to RGBA color
      /// </summary>
      Bgr2Rgba = 2,
      /// <summary>
      /// Convert RGB color to BGRA color
      /// </summary>
      Rgb2Bgra = Bgr2Rgba,
      /// <summary>
      /// Convert RGBA color to BGR color
      /// </summary>
      Rgba2Bgr = 3,
      /// <summary>
      /// Convert BGRA color to RGB color
      /// </summary>
      Bgra2Rgb = Rgba2Bgr,
      /// <summary>
      /// Convert BGR color to RGB color
      /// </summary>
      Bgr2Rgb = 4,
      /// <summary>
      /// Convert RGB color to BGR color
      /// </summary>
      Rgb2Bgr = Bgr2Rgb,
      /// <summary>
      /// Convert BGRA color to RGBA color
      /// </summary>
      Bgra2Rgba = 5,
      /// <summary>
      /// Convert RGBA color to BGRA color
      /// </summary>
      Rgba2Bgra = Bgra2Rgba,
      ///<summary>
      ///Convert BGR color to GRAY color
      ///</summary>
      Bgr2Gray = 6,
      /// <summary>
      /// Convert RGB color to GRAY color
      /// </summary>
      Rgb2Gray = 7,
      ///<summary>
      ///Convert GRAY color to BGR color
      ///</summary>
      Gray2Bgr = 8,
      /// <summary>
      /// Convert GRAY color to RGB color
      /// </summary>
      Gray2Rgb = Gray2Bgr,
      ///<summary>
      ///Convert GRAY color to BGRA color
      ///</summary>
      Gray2Bgra = 9,
      /// <summary>
      /// Convert GRAY color to RGBA color
      /// </summary>
      Gray2Rgba = Gray2Bgra,
      ///<summary>
      ///Convert BGRA color to GRAY color
      ///</summary>
      Bgra2Gray = 10,
      /// <summary>
      /// Convert RGBA color to GRAY color
      /// </summary>
      Rgba2Gray = 11,
      ///<summary>
      ///Convert BGR color to BGR565 color
      ///</summary>
      Bgr2Bgr565 = 12,
      /// <summary>
      /// Convert RGB color to BGR565 color
      /// </summary>
      Rgb2Bgr565 = 13,
      ///<summary>
      ///Convert BGR565 color to BGR color
      ///</summary>
      Bgr5652Bgr = 14,
      /// <summary>
      /// Convert BGR565 color to RGB color
      /// </summary>
      Bgr5652Rgb = 15,
      ///<summary>
      ///Convert BGRA color to BGR565 color
      ///</summary>
      Bgra2Bgr565 = 16,
      /// <summary>
      /// Convert RGBA color to BGR565 color
      /// </summary>
      Rgba2Bgr565 = 17,
      ///<summary>
      ///Convert BGR565 color to BGRA color
      ///</summary>
      Bgr5652Bgra = 18,
      /// <summary>
      /// Convert BGR565 color to RGBA color
      /// </summary>
      Bgr5652Rgba = 19,
      ///<summary>
      ///Convert GRAY color to BGR565 color
      ///</summary>
      Gray2Bgr565 = 20,
      ///<summary>
      ///Convert BGR565 color to GRAY color
      ///</summary>
      Bgr5652Gray = 21,
      ///<summary>
      ///Convert BGR color to BGR555 color
      ///</summary>
      Bgr2Bgr555 = 22,
      /// <summary>
      /// Convert RGB color to BGR555 color
      /// </summary>
      Rgb2Bgr555 = 23,
      ///<summary>
      ///Convert BGR555 color to BGR color
      ///</summary>
      Bgr5552Bgr = 24,
      /// <summary>
      /// Convert BGR555 color to RGB color
      /// </summary>
      Bgr5552Rgb = 25,
      ///<summary>
      ///Convert BGRA color to BGR555 color
      ///</summary>
      Bgra2Bgr555 = 26,
      /// <summary>
      /// Convert RGBA color to BGR555 color
      /// </summary>
      Rgba2Bgr555 = 27,
      ///<summary>
      ///Convert BGR555 color to BGRA color
      ///</summary>
      Bgr5552Bgra = 28,
      /// <summary>
      /// Convert BGR555 color to RGBA color
      /// </summary>
      Bgr5552Rgba = 29,
      ///<summary>
      ///Convert GRAY color to BGR555 color
      ///</summary>
      Gray2Bgr555 = 30,
      ///<summary>
      ///Convert BGR555 color to GRAY color
      ///</summary>
      Bgr5552Gray = 31,
      ///<summary>
      ///Convert BGR color to XYZ color
      ///</summary>
      Bgr2Xyz = 32,
      /// <summary>
      /// Convert RGB color to XYZ color
      /// </summary>
      Rgb2Xyz = 33,
      ///<summary>
      ///Convert XYZ color to BGR color
      ///</summary>
      Xyz2Bgr = 34,
      /// <summary>
      /// Convert XYZ color to RGB color
      /// </summary>
      Xyz2Rgb = 35,
      ///<summary>
      ///Convert BGR color to YCrCb color
      ///</summary>
      Bgr2YCrCb = 36,
      /// <summary>
      /// Convert RGB color to YCrCb color
      /// </summary>
      Rgb2YCrCb = 37,
      ///<summary>
      ///Convert YCrCb color to BGR color
      ///</summary>
      YCrCb2Bgr = 38,
      /// <summary>
      /// Convert YCrCb color to RGB color
      /// </summary>
      YCrCb2Rgb = 39,
      ///<summary>
      ///Convert BGR color to HSV color
      ///</summary>
      Bgr2Hsv = 40,
      /// <summary>
      /// Convert RGB colot to HSV color
      /// </summary>
      Rgb2Hsv = 41,
      ///<summary>
      ///Convert BGR color to Lab color
      ///</summary>
      Bgr2Lab = 44,
      /// <summary>
      /// Convert RGB color to Lab color
      /// </summary>
      Rgb2Lab = 45,
      ///<summary>
      ///Convert BayerBG color to BGR color
      ///</summary>
      BayerBg2Bgr = 46,
      ///<summary>
      ///Convert BayerGB color to BGR color
      ///</summary>
      BayerGb2Bgr = 47,
      ///<summary>
      ///Convert BayerRG color to BGR color
      ///</summary>
      BayerRg2Bgr = 48,
      ///<summary>
      ///Convert BayerGR color to BGR color
      ///</summary>
      BayerGr2Bgr = 49,
      /// <summary>
      /// Convert BayerBG color to BGR color
      /// </summary>
      BayerBg2Rgb = BayerRg2Bgr,
      /// <summary>
      /// Convert BayerRG color to BGR color
      /// </summary>
      BayerGb2Rgb = BayerGr2Bgr,
      /// <summary>
      /// Convert BayerRG color to RGB color
      /// </summary>
      BayerRg2Rgb = BayerBg2Bgr,
      /// <summary>
      /// Convert BayerGR color to RGB color
      /// </summary>
      BayerGr2Rgb = BayerGb2Bgr,
      ///<summary>
      ///Convert BGR color to Luv color
      ///</summary>
      Bgr2Luv = 50,
      /// <summary>
      /// Convert RGB color to Luv color
      /// </summary>
      Rgb2Luv = 51,
      ///<summary>
      ///Convert BGR color to HLS color
      ///</summary>
      Bgr2Hls = 52,
      /// <summary>
      /// Convert RGB color to HLS color
      /// </summary>
      Rgb2Hls = 53,
      ///<summary>
      ///Convert HSV color to BGR color
      ///</summary>
      Hsv2Bgr = 54,
      /// <summary>
      /// Convert HSV color to RGB color
      /// </summary>
      Hsv2Rgb = 55,
      ///<summary>
      ///Convert Lab color to BGR color
      ///</summary>
      Lab2Bgr = 56,
      /// <summary>
      /// Convert Lab color to RGB color
      /// </summary>
      Lab2Rgb = 57,
      ///<summary>
      ///Convert Luv color to BGR color
      ///</summary>
      Luv2Bgr = 58,
      /// <summary>
      /// Convert Luv color to RGB color
      /// </summary>
      Luv2Rgb = 59,
      ///<summary>
      ///Convert HLS color to BGR color
      ///</summary>
      Hls2Bgr = 60,
      /// <summary>
      /// Convert HLS color to RGB color
      /// </summary>
      Hls2Rgb = 61,
      /// <summary>
      /// Convert BayerBG pattern to BGR color using VNG
      /// </summary>
      BayerBg2BgrVng = 62,
      /// <summary>
      /// Convert BayerGB pattern to BGR color using VNG
      /// </summary>
      BayerGb2BgrVng = 63,
      /// <summary>
      /// Convert BayerRG pattern to BGR color using VNG
      /// </summary>
      BayerRg2BgrVng = 64,
      /// <summary>
      /// Convert BayerGR pattern to BGR color using VNG
      /// </summary>
      BayerGr2BgrVng = 65,
      /// <summary>
      /// Convert BayerBG pattern to RGB color using VNG
      /// </summary>
      BayerBg2RgbVng = BayerRg2BgrVng,
      /// <summary>
      /// Convert BayerGB pattern to RGB color using VNG
      /// </summary>
      BayerGb2RgbVng = BayerGr2BgrVng,
      /// <summary>
      /// Convert BayerRG pattern to RGB color using VNG
      /// </summary>
      BayerRg2RgbVng = BayerBg2BgrVng,
      /// <summary>
      /// Convert BayerGR pattern to RGB color using VNG
      /// </summary>
      BayerGr2RgbVng = BayerGb2BgrVng,

      /// <summary>
      /// Convert BGR to HSV
      /// </summary>
      Bgr2HsvFull = 66,
      /// <summary>
      /// Convert RGB to HSV
      /// </summary>
      Rgb2HsvFull = 67,
      /// <summary>
      /// Convert BGR to HLS
      /// </summary>
      Bgr2HlsFull = 68,
      /// <summary>
      /// Convert RGB to HLS
      /// </summary>
      Rgb2HlsFull = 69,

      /// <summary>
      /// Convert HSV color to BGR color
      /// </summary>
      Hsv2BgrFull = 70,
      /// <summary>
      /// Convert HSV color to RGB color
      /// </summary>
      Hsv2RgbFull = 71,
      /// <summary>
      /// Convert HLS color to BGR color
      /// </summary>
      Hls2BgrFull = 72,
      /// <summary>
      /// Convert HLS color to RGB color
      /// </summary>
      Hls2RgbFull = 73,

      /// <summary>
      /// Convert sBGR color to Lab color
      /// </summary>
      Lbgr2Lab = 74,
      /// <summary>
      /// Convert sRGB color to Lab color
      /// </summary>
      Lrgb2Lab = 75,
      /// <summary>
      /// Convert sBGR color to Luv color
      /// </summary>
      Lbgr2Luv = 76,
      /// <summary>
      /// Convert sRGB color to Luv color
      /// </summary>
      Lrgb2Luv = 77,

      /// <summary>
      /// Convert Lab color to sBGR color
      /// </summary>
      Lab2Lbgr = 78,
      /// <summary>
      /// Convert Lab color to sRGB color
      /// </summary>
      Lab2Lrgb = 79,
      /// <summary>
      /// Convert Luv color to sBGR color
      /// </summary>
      Luv2Lbgr = 80,
      /// <summary>
      /// Convert Luv color to sRGB color
      /// </summary>
      Luv2Lrgb = 81,

      /// <summary>
      /// Convert BGR color to YUV
      /// </summary>
      Bgr2Yuv = 82,
      /// <summary>
      /// Convert RGB color to YUV
      /// </summary>
      Rgb2Yuv = 83,
      /// <summary>
      /// Convert YUV color to BGR
      /// </summary>
      Yuv2Bgr = 84,
      /// <summary>
      /// Convert YUV color to RGB
      /// </summary>
      Yuv2Rgb = 85,

      /// <summary>
      /// Convert BayerBG to GRAY
      /// </summary>
      BayerBg2Gray = 86,
      /// <summary>
      /// Convert BayerGB to GRAY
      /// </summary>
      BayerGb2Gray = 87,
      /// <summary>
      /// Convert BayerRG to GRAY
      /// </summary>
      BayerRg2Gray = 88,
      /// <summary>
      /// Convert BayerGR to GRAY
      /// </summary>
      BayerGr2Gray = 89,
      /// <summary>
      /// Convert YUV420i to RGB
      /// </summary>
      Yuv420I2Rgb = 90,
      /// <summary>
      /// Convert YUV420i to BGR
      /// </summary>
      Yuv420I2Bgr = 91,
      /// <summary>
      /// Convert YUV420sp to RGB
      /// </summary>
      Yuv420Sp2Rgb = 92,
      /// <summary>
      /// Convert YUV320sp to BGR
      /// </summary>
      Yuv420Sp2Bgr = 93,
      /// <summary>
      /// Convert YUV320i to RGBA
      /// </summary>
      Yuv420I2Rgba = 94,
      /// <summary>
      /// Convert YUV420i to BGRA
      /// </summary>
      Yuv420I2Bgra = 95,
      /// <summary>
      /// Convert YUV420sp to RGBA
      /// </summary>
      Yuv420Sp2Rgba = 96,
      /// <summary>
      /// Convert YUV420sp to BGRA
      /// </summary>
      Yuv420Sp2Bgra = 97,

      /// <summary>
      /// Convert YUV (YV12) to RGB
      /// </summary>
      Yuv2RgbYv12 = 98,
      /// <summary>
      /// Convert YUV (YV12) to BGR
      /// </summary>
      Yuv2BgrYv12 = 99,
      /// <summary>
      /// Convert YUV (iYUV) to RGB
      /// </summary>
      Yuv2RgbIyuv = 100,
      /// <summary>
      /// Convert YUV (iYUV) to BGR
      /// </summary>
      Yuv2BgrIyuv = 101,
      /// <summary>
      /// Convert YUV (i420) to RGB
      /// </summary>
      Yuv2RgbI420 = Yuv2RgbIyuv,
      /// <summary>
      /// Convert YUV (i420) to BGR
      /// </summary>
      Yuv2BgrI420 = Yuv2BgrIyuv,
      /// <summary>
      /// Convert YUV (420p) to RGB
      /// </summary>
      Yuv420P2Rgb = Yuv2RgbYv12,
      /// <summary>
      /// Convert YUV (420p) to BGR
      /// </summary>
      Yuv420P2Bgr = Yuv2BgrYv12,

      /// <summary>
      /// Convert YUV (YV12) to RGBA
      /// </summary>
      Yuv2RgbaYv12 = 102,
      /// <summary>
      /// Convert YUV (YV12) to BGRA
      /// </summary>
      Yuv2BgraYv12 = 103,
      /// <summary>
      /// Convert YUV (iYUV) to RGBA
      /// </summary>
      Yuv2RgbaIyuv = 104,
      /// <summary>
      /// Convert YUV (iYUV) to BGRA
      /// </summary>
      Yuv2BgraIyuv = 105,
      /// <summary>
      /// Convert YUV (i420) to RGBA
      /// </summary>
      Yuv2RgbaI420 = Yuv2RgbaIyuv,
      /// <summary>
      /// Convert YUV (i420) to BGRA
      /// </summary>
      Yuv2BgraI420 = Yuv2BgraIyuv,
      /// <summary>
      /// Convert YUV (420p) to RGBA
      /// </summary>
      Yuv420P2Rgba = Yuv2RgbaYv12,
      /// <summary>
      /// Convert YUV (420p) to BGRA
      /// </summary>
      Yuv420P2Bgra = Yuv2BgraYv12,

      /// <summary>
      /// Convert YUV 420 to Gray
      /// </summary>
      Yuv2Gray420 = 106,
      /// <summary>
      /// Convert YUV NV21 to Gray
      /// </summary>
      Yuv2GrayNv21 = Yuv2Gray420,
      /// <summary>
      /// Convert YUV NV12 to Gray
      /// </summary>
      Yuv2GrayNv12 = Yuv2Gray420,
      /// <summary>
      /// Convert YUV YV12 to Gray
      /// </summary>
      Yuv2GrayYv12 = Yuv2Gray420,
      /// <summary>
      /// Convert YUV (iYUV) to Gray
      /// </summary>
      Yuv2GrayIyuv = Yuv2Gray420,
      /// <summary>
      /// Convert YUV (i420) to Gray
      /// </summary>
      Yuv2GrayI420 = Yuv2Gray420,
      /// <summary>
      /// Convert YUV (420sp) to Gray
      /// </summary>
      Yuv420Sp2Gray = Yuv2Gray420,
      /// <summary>
      /// Convert YUV (420p) to Gray
      /// </summary>
      Yuv420P2Gray = Yuv2Gray420,

      //YUV 4:2:2 formats family
      /// <summary>
      /// Convert YUV (UYVY) to RGB
      /// </summary>
      Yuv2RgbUyvy = 107,
      /// <summary>
      /// Convert YUV (UYVY) to BGR
      /// </summary>
      Yuv2BgrUyvy = 108,
      //YUV2RGB_VYUY = 109,
      //YUV2BGR_VYUY = 110,
      /// <summary>
      /// Convert YUV (Y422) to RGB
      /// </summary>
      Yuv2RgbY422 = Yuv2RgbUyvy,
      /// <summary>
      /// Convert YUV (Y422) to BGR
      /// </summary>
      Yuv2BgrY422 = Yuv2BgrUyvy,
      /// <summary>
      /// Convert YUV (UYNY) to RGB
      /// </summary>
      Yuv2RgbUynv = Yuv2RgbUyvy,
      /// <summary>
      /// Convert YUV (UYNV) to BGR
      /// </summary>
      Yuv2BgrUynv = Yuv2BgrUyvy,

      /// <summary>
      /// Convert YUV (UYVY) to RGBA
      /// </summary>
      Yuv2RgbaUyvy = 111,
      /// <summary>
      /// Convert YUV (VYUY) to BGRA
      /// </summary>
      Yuv2BgraUyvy = 112,
      //YUV2RGBA_VYUY = 113,
      //YUV2BGRA_VYUY = 114,
      /// <summary>
      /// Convert YUV (Y422) to RGBA
      /// </summary>
      Yuv2RgbaY422 = Yuv2RgbaUyvy,
      /// <summary>
      /// Convert YUV (Y422) to BGRA
      /// </summary>
      Yuv2BgraY422 = Yuv2BgraUyvy,
      /// <summary>
      /// Convert YUV (UYNV) to RGBA 
      /// </summary>
      Yuv2RgbaUynv = Yuv2RgbaUyvy,
      /// <summary>
      /// Convert YUV (UYNV) to BGRA
      /// </summary>
      Yuv2BgraUynv = Yuv2BgraUyvy,

      /// <summary>
      /// Convert YUV (YUY2) to RGB
      /// </summary>
      Yuv2RgbYuy2 = 115,
      /// <summary>
      /// Convert YUV (YUY2) to BGR
      /// </summary>
      Yuv2BgrYuy2 = 116,
      /// <summary>
      /// Convert YUV (YVYU) to RGB
      /// </summary>
      Yuv2RgbYvyu = 117,
      /// <summary>
      /// Convert YUV (YVYU) to BGR
      /// </summary>
      Yuv2BgrYvyu = 118,
      /// <summary>
      /// Convert YUV (YUYV) to RGB
      /// </summary>
      Yuv2RgbYuyv = Yuv2RgbYuy2,
      /// <summary>
      /// Convert YUV (YUYV) to BGR 
      /// </summary>
      Yuv2BgrYuyv = Yuv2BgrYuy2,
      /// <summary>
      /// Convert YUV (YUNV) to RGB
      /// </summary>
      Yuv2RgbYunv = Yuv2RgbYuy2,
      /// <summary>
      /// Convert YUV (YUNV) to BGR
      /// </summary>
      Yuv2BgrYunv = Yuv2BgrYuy2,

      /// <summary>
      /// Convert YUV (YUY2) to RGBA
      /// </summary>
      Yuv2RgbaYuy2 = 119,
      /// <summary>
      /// Convert YUV (YUY2) to BGRA
      /// </summary>
      Yuv2BgraYuy2 = 120,
      /// <summary>
      /// Convert YUV (YVYU) to RGBA
      /// </summary>
      Yuv2RgbaYvyu = 121,
      /// <summary>
      /// Convert YUV (YVYU) to BGRA
      /// </summary>
      Yuv2BgraYvyu = 122,
      /// <summary>
      /// Convert YUV (YUYV) to RGBA
      /// </summary>
      Yuv2RgbaYuyv = Yuv2RgbaYuy2,
      /// <summary>
      /// Convert YUV (YUYV) to BGRA
      /// </summary>
      Yuv2BgraYuyv = Yuv2BgraYuy2,
      /// <summary>
      /// Convert YUV (YUNV) to RGBA
      /// </summary>
      Yuv2RgbaYunv = Yuv2RgbaYuy2,
      /// <summary>
      /// Convert YUV (YUNV) to BGRA
      /// </summary>
      Yuv2BgraYunv = Yuv2BgraYuy2,

      /// <summary>
      /// Convert YUV (UYVY) to Gray
      /// </summary>
      Yuv2GrayUyvy = 123,
      /// <summary>
      /// Convert YUV (YUY2) to Gray
      /// </summary>
      Yuv2GrayYuy2 = 124,
      //YUV2GRAY_VYUY = YUV2GRAY_UYVY,
      /// <summary>
      /// Convert YUV (Y422) to Gray
      /// </summary>
      Yuv2GrayY422 = Yuv2GrayUyvy,
      /// <summary>
      /// Convert YUV (UYNV) to Gray
      /// </summary>
      Yuv2GrayUynv = Yuv2GrayUyvy,
      /// <summary>
      /// Convert YUV (YVYU) to Gray
      /// </summary>
      Yuv2GrayYvyu = Yuv2GrayYuy2,
      /// <summary>
      /// Convert YUV (YUYV) to Gray
      /// </summary>
      Yuv2GrayYuyv = Yuv2GrayYuy2,
      /// <summary>
      /// Convert YUV (YUNV) to Gray
      /// </summary>
      Yuv2GrayYunv = Yuv2GrayYuy2,

      /// <summary>
      /// Alpha premultiplication
      /// </summary>
      Rgba2MRgba = 125,
      /// <summary>
      /// Alpha premultiplication
      /// </summary>
      MRgba2Rgba = 126,

      // RGB to YUV 4:2:0 family

      /// <summary>
      /// Convert RGB to YUV_I420
      /// </summary>
      Rgb2YuvI420 = 127,
      /// <summary>
      /// Convert BGR to YUV_I420
      /// </summary>
      Bgr2YuvI420 = 128,
      /// <summary>
      /// Convert RGB to YUV_IYUV
      /// </summary>
      Rgb2YuvIyuv = Rgb2YuvI420,
      /// <summary>
      /// Convert BGR to YUV_IYUV
      /// </summary>
      Bgr2YuvIyuv = Bgr2YuvI420,

      /// <summary>
      /// Convert RGBA to YUV_I420
      /// </summary>
      Rgba2YuvI420 = 129,
      /// <summary>
      /// Convert BGRA to YUV_I420
      /// </summary>
      Bgra2YuvI420 = 130,
      /// <summary>
      /// Convert RGBA to YUV_IYUV
      /// </summary>
      Rgba2YuvIyuv = Rgba2YuvI420,
      /// <summary>
      /// Convert BGRA to YUV_IYUV
      /// </summary>
      Bgra2YuvIyuv = Bgra2YuvI420,
      /// <summary>
      /// Convert RGB to YUV_YV12
      /// </summary>
      Rgb2YuvYv12 = 131,
      /// <summary>
      /// Convert BGR to YUV_YV12
      /// </summary>
      Bgr2YuvYv12 = 132,
      /// <summary>
      /// Convert RGBA to YUV_YV12
      /// </summary>
      Rgba2YuvYv12 = 133,
      /// <summary>
      /// Convert BGRA to YUV_YV12
      /// </summary>
      Bgra2YuvYv12 = 134,

      /// <summary>
      /// Convert BayerBG to BGR (Edge-Aware Demosaicing)
      /// </summary>
      BayerBg2BgrEa = 135,
      /// <summary>
      /// Convert BayerGB to BGR (Edge-Aware Demosaicing)
      /// </summary>
      BayerGb2BgrEa = 136,
      /// <summary>
      /// Convert BayerRG to BGR (Edge-Aware Demosaicing)
      /// </summary>
      BayerRg2BgrEa = 137,
      /// <summary>
      /// Convert BayerGR to BGR (Edge-Aware Demosaicing)
      /// </summary>
      BayerGr2BgrEa = 138,

      /// <summary>
      /// Convert BayerBG to RGB (Edge-Aware Demosaicing)
      /// </summary>
      BayerBg2RgbEa = BayerRg2BgrEa,
      /// <summary>
      /// Convert BayerGB to RGB (Edge-Aware Demosaicing)
      /// </summary>
      BayerGb2RgbEa = BayerGr2BgrEa,
      /// <summary>
      /// Convert BayerRG to RGB (Edge-Aware Demosaicing)
      /// </summary>
      BayerRg2RgbEa = BayerBg2BgrEa,
      /// <summary>
      /// Convert BayerGR to RGB (Edge-Aware Demosaicing)
      /// </summary>
      BayerGr2RgbEa = BayerGb2BgrEa,

      /// <summary>
      /// The max number, do not use
      /// </summary>
      ColorcvtMax = 139
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
   public enum InpaintType
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
      /// (Gaussian blur) - convolving image with param1xparam2 Gaussian kernel. 
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

   /*
   /// <summary>
   /// CV RAND TYPE
   /// </summary>
   public enum RandType
   {
      /// <summary>
      /// Uniform distribution
      /// </summary>
      Uni = 0,
      /// <summary>
      /// Normal distribution
      /// </summary>
      Normal = 1
   }*/

   /// <summary>
   /// contour retrieval mode
   /// </summary>
   public enum RetrType
   {
      /// <summary>
      /// retrieve only the extreme outer contours 
      /// </summary>
      External = 0,
      /// <summary>
      ///  retrieve all the contours and puts them in the list 
      /// </summary>
      List = 1,
      /// <summary>
      /// retrieve all the contours and organizes them into two-level hierarchy: top level are external boundaries of the components, second level are bounda boundaries of the holes 
      /// </summary>
      Ccomp = 2,
      /// <summary>
      /// retrieve all the contours and reconstructs the full hierarchy of nested contours 
      /// </summary>
      Tree = 3
   }

   internal static class SeqConst
   {
      /// <summary>
      /// The bit to shift for SEQ_ELTYPE
      /// </summary>
      public const int EltypeBits = 12;

      /// <summary>
      /// The mask of CV_SEQ_ELTYPE
      /// </summary>
      public const int EltypeMask = ((1 << EltypeBits) - 1);

      /// <summary>
      /// The bits to shift for SEQ_KIND
      /// </summary>
      public const int KindBits = 2;
      /// <summary>
      /// The bits to shift for SEQ_FLAG
      /// </summary>
      public const int Shift = KindBits + EltypeBits;
   }

   /// <summary>
   /// Sequence element type
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
      Generic = (0 << SeqConst.EltypeBits),
      /// <summary>
      /// dense sequence subtypes 
      /// </summary>
      Curve = (1 << SeqConst.EltypeBits),
      /// <summary>
      /// dense sequence subtypes 
      /// </summary>
      BinTree = (2 << SeqConst.EltypeBits),
      /// <summary>
      /// sparse sequence (or set) subtypes 
      /// </summary>
      Graph = (1 << SeqConst.EltypeBits),
      /// <summary>
      /// sparse sequence (or set) subtypes 
      /// </summary>
      Subdiv2D = (2 << SeqConst.EltypeBits)
   }

   /// <summary>
   /// Sequence flag
   /// </summary>
   public enum SeqFlag
   {
      /// <summary>
      /// close sequence
      /// </summary>
      Closed = (1 << SeqConst.Shift),
      /// <summary>
      /// 
      /// </summary>
      Simple = (2 << SeqConst.Shift),
      /// <summary>
      /// 
      /// </summary>
      Convex = (4 << SeqConst.Shift),
      /// <summary>
      /// 
      /// </summary>
      Hole = (8 << SeqConst.Shift)
   }

   /// <summary>
   /// Sequence type for point sets
   /// </summary>
   public enum SeqType
   {
      /// <summary>
      /// 
      /// </summary>
      PointSet = (SeqKind.Generic | SeqEltype.Point),
      /// <summary>
      /// 
      /// </summary>
      Point3DSet = (SeqKind.Generic | SeqEltype.Point3D),
      /// <summary>
      /// 
      /// </summary>
      Polyline = (SeqKind.Curve | SeqEltype.Point),
      /// <summary>
      /// 
      /// </summary>
      Polygon = (SeqFlag.Closed | Polyline),
      //CV_SEQ_CONTOUR         =POLYGON,
      /// <summary>
      /// 
      /// </summary>
      SimplePolygon = (SeqFlag.Simple | Polygon)
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
   public enum IplDepth : uint
   {
      /// <summary>
      /// indicates if the value is signed
      /// </summary>
      IplDepthSign = 0x80000000,
      /// <summary>
      /// 1bit unsigned
      /// </summary>
      IplDepth_1U = 1,
      /// <summary>
      /// 8bit unsigned (Byte)
      /// </summary>
      IplDepth_8U = 8,
      /// <summary>
      /// 16bit unsigned
      /// </summary>
      IplDepth16U = 16,
      /// <summary>
      /// 32bit float (Single)
      /// </summary>
      IplDepth32F = 32,
      /// <summary>
      /// 8bit signed
      /// </summary>
      IplDepth_8S = (IplDepthSign | 8),
      /// <summary>
      /// 16bit signed
      /// </summary>
      IplDepth16S = (IplDepthSign | 16),
      /// <summary>
      /// 32bit signed 
      /// </summary>
      IplDepth32S = (IplDepthSign | 32),
      /// <summary>
      /// double
      /// </summary>
      IplDepth64F = 64
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
      /// Checks that every element is neither NaN nor Infinity
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
      /// Anti-alias
      /// </summary>
      AntiAlias = 16
   }

   /// <summary>
   /// Distance transform algorithm flags
   /// </summary>
   public enum DistLabelType
   {
      /// <summary>
      /// Connected component
      /// </summary>
      CComp = 0,
      /// <summary>
      /// The pixel
      /// </summary>
      Pixel = 1
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
   public enum General
   {
      /// <summary>
      /// 
      /// </summary>
      MaxDim = 32,
      /// <summary>
      /// 
      /// </summary>
      SeqMagicVal = 0x42990000,
      /// <summary>
      /// 
      /// </summary>
      SetMagicVal = 0x42980000
   }

   ///<summary>
   /// Error codes
   /// </summary>
   public enum ErrorCodes
   {
      /// <summary>
      /// 
      /// </summary>
      StsOk = 0,
      /// <summary>
      /// 
      /// </summary>
      StsBacktrace = -1,
      /// <summary>
      /// 
      /// </summary>
      StsError = -2,
      /// <summary>
      /// 
      /// </summary>
      StsInternal = -3,
      /// <summary>
      /// 
      /// </summary>
      StsNoMem = -4,
      /// <summary>
      /// 
      /// </summary>
      StsBadArg = -5,
      /// <summary>
      /// 
      /// </summary>
      StsBadFunc = -6,
      /// <summary>
      /// 
      /// </summary>
      StsNoConv = -7,
      /// <summary>
      /// 
      /// </summary>
      StsAutoTrace = -8,
      /// <summary>
      /// 
      /// </summary>
      HeaderIsNull = -9,
      /// <summary>
      /// 
      /// </summary>
      BadImageSize = -10,
      /// <summary>
      /// 
      /// </summary>
      BadOffset = -11,
      /// <summary>
      /// 
      /// </summary>
      BadDataPtr = -12,
      /// <summary>
      /// 
      /// </summary>
      Badstep = -13,
      /// <summary>
      /// 
      /// </summary>
      BadModelOrChseq = -14,
      /// <summary>
      /// 
      /// </summary>
      BadNumChannels = -15,
      /// <summary>
      /// 
      /// </summary>
      BadNumChannel1U = -16,
      /// <summary>
      /// 
      /// </summary>
      BadDepth = -17,
      /// <summary>
      /// 
      /// </summary>
      BadAlphaChannel = -18,
      /// <summary>
      /// 
      /// </summary>
      BadOrder = -19,
      /// <summary>
      /// 
      /// </summary>
      BadOrigin = -20,
      /// <summary>
      /// 
      /// </summary>
      BadAlign = -21,
      /// <summary>
      /// 
      /// </summary>
      BadCallback = -22,
      /// <summary>
      /// 
      /// </summary>
      BadTileSize = -23,
      /// <summary>
      /// 
      /// </summary>
      BadCoi = -24,
      /// <summary>
      /// 
      /// </summary>
      BadRoiSize = -25,
      /// <summary>
      /// 
      /// </summary>
      MaskIsTiled = -26,
      /// <summary>
      /// 
      /// </summary>
      StsNullPtr = -27,
      /// <summary>
      /// 
      /// </summary>
      StsVecLengthErr = -28,
      /// <summary>
      /// 
      /// </summary>
      StsFilterStructContenterr = -29,
      /// <summary>
      /// 
      /// </summary>
      StsKernelStructContenterr = -30,
      /// <summary>
      /// 
      /// </summary>
      StsFilterOffSetErr = -31,
      /// <summary>
      /// 
      /// </summary>
      StsBadSize = -201,
      /// <summary>
      /// 
      /// </summary>
      StsDivByZero = -202,
      /// <summary>
      /// 
      /// </summary>
      StsInplaceNotSupported = -203,
      /// <summary>
      /// 
      /// </summary>
      StsObjectNotFound = -204,
      /// <summary>
      /// 
      /// </summary>
      StsUnmatchedFormats = -205,
      /// <summary>
      /// 
      /// </summary>
      StsBadFlag = -206,
      /// <summary>
      /// 
      /// </summary>
      StsBadPoint = -207,
      /// <summary>
      /// 
      /// </summary>
      StsBadMask = -208,
      /// <summary>
      /// 
      /// </summary>
      StsUnmatchedSizes = -209,
      /// <summary>
      /// 
      /// </summary>
      StsUnsupportedFormat = -210,
      /// <summary>
      /// 
      /// </summary>
      StsOutOfRange = -211,
      /// <summary>
      /// 
      /// </summary>
      StsParseError = -212,
      /// <summary>
      /// 
      /// </summary>
      StsNotImplemented = -213,
      /// <summary>
      /// 
      /// </summary>
      StsBadMemBlock = -214
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
   public enum SvdFlag
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
      /// indicates that only a vector of singular values `w` is to be processed, while u and vt will be set to empty matrices
      /// </summary>
      NoUV = 2,
      /// <summary>
      /// when the matrix is not square, by default the algorithm produces u and vt matrices of
      /// sufficiently large size for the further A reconstruction; if, however, FULL_UV flag is
      /// specified, u and vt will be full-size square orthogonal matrices.
      /// </summary>
      FullUV = 4
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
   public enum EigobjType
   {
      /// <summary>
      /// No callback
      /// </summary>
      NoCallback = 0,
      /// <summary>
      /// input callback
      /// </summary>
      InputCallback = 1,
      /// <summary>
      /// output callback
      /// </summary>
      OutputCallback = 2,
      /// <summary>
      /// both callback
      /// </summary>
      BothCallback = 3
   }

   /// <summary>
   /// CvNextEdgeType
   /// </summary>
   public enum NextEdgeType
   {
      /// <summary>
      /// next around the edge origin (eOnext)
      /// </summary>
      NextAroundOrg = 0x00,
      /// <summary>
      /// next around the edge vertex (eDnext) 
      /// </summary>
      NextAroundDst = 0x22,
      /// <summary>
      /// previous around the edge origin (reversed eRnext)
      /// </summary>
      PrevAroundOrg = 0x11,
      /// <summary>
      /// previous around the edge destination (reversed eLnext) 
      /// </summary>
      PreAroundDst = 0x33,
      /// <summary>
      /// next around the left facet (eLnext) 
      /// </summary>
      NextAroundLeft = 0x13,
      /// <summary>
      /// next around the right facet (eRnext)
      /// </summary>
      NextAroundRight = 0x31,
      /// <summary>
      /// previous around the left facet (reversed eOnext)
      /// </summary>
      PrevAroundLeft = 0x20,
      /// <summary>
      /// previous around the right facet (reversed eDnext) 
      /// </summary>
      PrevAroundRight = 0x02
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
      /// Used by some cuda methods, will pass the value -1 to the function
      /// </summary>
      NegativeOne = -1,

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

   /*
   /// <summary>
   /// The method for matching contour tree
   /// </summary>
   public enum MatchContourTreeMethod
   {
      /// <summary>
      /// 
      /// </summary>
      ContourTreesMatchI1 = 1
   }*/

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

   /*
   /// <summary>
   /// The type of blob detector
   /// </summary>
   public enum BlobDetectorType
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
   public enum BlobTrackerType
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
   }*/

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

   /*
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
   }*/

   /// <summary>
   /// The available flags for Farneback optical flow computation
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
      /// Auto detect
      /// </summary>
      Any = 0,

      /// <summary>
      /// MIL proprietary drivers
      /// </summary>
      Mil = 100,

      /// <summary>
      /// Platform native
      /// </summary>
      Vfw = 200,
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
      Firewire = 300,
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
      Stereo = 400,
      /// <summary>
      /// TYZX proprietary drivers
      /// </summary>
      Tyzx = 400,
      /// <summary>
      /// TYZX proprietary drivers
      /// </summary>
      TyzxLeft = 400,
      /// <summary>
      /// TYZX proprietary drivers
      /// </summary>
      TyzxRight = 401,
      /// <summary>
      /// TYZX proprietary drivers
      /// </summary>
      TyzxColor = 402,
      /// <summary>
      /// TYZX proprietary drivers
      /// </summary>
      TyzxZ = 403,

      /// <summary>
      /// QuickTime
      /// </summary>
      QT = 500,

      /// <summary>
      /// Unicap drivers
      /// </summary>
      Unicap = 600,

      /// <summary>
      /// DirectShow (via videoInput)
      /// </summary>
      DShow = 700,

      /// <summary>
      /// Microsoft Media Foundation (via videoInput)
      /// </summary>
      Msmf = 1400,

      /// <summary>
      /// PvAPI, Prosilica GigE SDK
      /// </summary>
      Pvapi = 800,

      /// <summary>
      /// OpenNI (for Kinect)
      /// </summary>
      OpenNI = 900,

      /// <summary>
      /// OpenNI (for Asus Xtion)
      /// </summary>
      OpenNIAsus = 910,

      /// <summary>
      /// Android
      /// </summary>
      Android = 1000,
      /// <summary>
      /// Android back camera
      /// </summary>
      AndroidBack = Android + 99,
      /// <summary>
      /// // Android front camera
      /// </summary>
      AndroidFront = Android + 98,
      /// <summary>
      /// XIMEA Camera API
      /// </summary>
      XiApi = 1100,

      /// <summary>
      /// AVFoundation framework for iOS (OS X Lion will have the same API)
      /// </summary>
      AVFoundation = 1200,

      /// <summary>
      ///  Smartek Giganetix GigEVisionSDK
      /// </summary>
      Giganetix = 1300,
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

   /// <summary>
   /// Rectangle intersect type
   /// </summary>
   public enum RectIntersectType
   {
      /// <summary>
      /// No intersection
      /// </summary>
      None = 0,
      /// <summary>
      /// There is a partial intersection
      /// </summary>
      Partial = 1,
      /// <summary>
      /// One of the rectangle is fully enclosed in the other
      /// </summary>
      Full = 2
   }

   /// <summary>
   /// Method for solving a PnP problem
   /// </summary>
   public enum SolvePnpMethod
   {
      /// <summary>
      /// Iterative
      /// </summary>
      Iterative = 0,
      /// <summary>
      /// F.Moreno-Noguer, V.Lepetit and P.Fua "EPnP: Efficient Perspective-n-Point Camera Pose Estimation"
      /// </summary>
      EPnP = 1,
      /// <summary>
      /// X.S. Gao, X.-R. Hou, J. Tang, H.-F. Chang; "Complete Solution Classification for the Perspective-Three-Point Problem"
      /// </summary>
      P3P = 2
   }

   /// <summary>
   /// White balance algorithms
   /// </summary>
   public enum WhiteBalanceMethod
   {
      /// <summary>
      /// Simple
      /// </summary>
      Simple = 0,
      /// <summary>
      /// Grayworld
      /// </summary>
      Grayworld = 1
   };
}
